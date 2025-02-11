using System.Collections;
using TMPro;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    public TMP_Text speedText;
    public GameObject angleInputField;
    public GameObject submitButton;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private Rigidbody2D carRigidbody;
    private bool isInBend = false;
    private Vector2 frozenVelocity;
    private Vector2 frozenPosition;
    private float frozenRotation;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        carRigidbody.velocity = Vector2.zero;
        carRigidbody.angularVelocity = 0f;
        carRigidbody.drag = 5f;
        carRigidbody.gravityScale = 0f;
    }

    public void MoveUp() => accelerationInput = 1;
    public void MoveDown() => accelerationInput = -1;
    public void TurnLeft() => steeringInput = -1;
    public void TurnRight() => steeringInput = 1;
    public void StopAcceleration() => accelerationInput = 0;
    public void StopSteering() => steeringInput = 0;

    private void ApplyEngineForce()
    {
        float velocityVsUp = Vector2.Dot(transform.up, carRigidbody.velocity);
        if (velocityVsUp > maxSpeed && accelerationInput > 0) return;
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) return;
        if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;

        carRigidbody.drag = (accelerationInput == 0) ? Mathf.Lerp(carRigidbody.drag, 3.0f, Time.fixedDeltaTime * 3) : 0;
        carRigidbody.AddForce(transform.up * accelerationInput * accelerationFactor, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float turnFactorAdjusted = Mathf.Clamp01(carRigidbody.velocity.magnitude / 8);
        rotationAngle -= steeringInput * turnFactor * turnFactorAdjusted;
        carRigidbody.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.velocity, transform.right);
        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void FixedUpdate()
    {
        if (!isInBend)
        {
            ApplyEngineForce();
            KillOrthogonalVelocity();
            ApplySteering();

            speedText.text = "Speed: " + carRigidbody.velocity.magnitude.ToString("F1") + " km/h";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bend") && !isInBend)
        {
            Debug.Log("Entered Bend Trigger");

            isInBend = true;

            // Freeze the current state
            frozenVelocity = carRigidbody.velocity;
            frozenPosition = carRigidbody.position;
            frozenRotation = carRigidbody.rotation;

            carRigidbody.bodyType = RigidbodyType2D.Static;

            float currentSpeed = frozenVelocity.magnitude;
            Debug.Log("Current Speed Before Freeze: " + currentSpeed);

            // ✅ Display speed with a slight delay for UI refresh
            StartCoroutine(ShowSpeedAfterDelay(currentSpeed));

            // Activate UI elements
            angleInputField.SetActive(true);
            submitButton.SetActive(true);

            // Ensure UI is positioned correctly
            RectTransform inputRect = angleInputField.GetComponent<RectTransform>();
            RectTransform buttonRect = submitButton.GetComponent<RectTransform>();

            inputRect.anchoredPosition = new Vector2(0, 0);       // Center
            buttonRect.anchoredPosition = new Vector2(0, -50);    // Below the input field

            inputRect.localScale = Vector3.one;
            buttonRect.localScale = Vector3.one;
            inputRect.localRotation = Quaternion.identity;
            buttonRect.localRotation = Quaternion.identity;

            Canvas canvas = angleInputField.GetComponentInParent<Canvas>();
            canvas.sortingOrder = 9999;  // Bring to front

            Debug.Log("Input Field Position: " + inputRect.anchoredPosition);
            Debug.Log("Submit Button Position: " + buttonRect.anchoredPosition);
        }
    }

    // 🕰️ Coroutine to delay UI update slightly
    private IEnumerator ShowSpeedAfterDelay(float speed)
    {
        yield return new WaitForEndOfFrame(); // Small delay to allow UI refresh

        speedText.text = "Paused at bend. Speed: " + speed.ToString("F2") + " m/s";
        Canvas.ForceUpdateCanvases();         // Force canvas to refresh
        Debug.Log("Displayed Speed: " + speed);
    }



    public void OnSubmitAngle()
    {
        if (float.TryParse(angleInputField.GetComponent<TMP_InputField>().text, out float cornerAngle))
        {
            // Re-enable physics
            carRigidbody.bodyType = RigidbodyType2D.Dynamic;

            // Restore state
            carRigidbody.position = frozenPosition;
            carRigidbody.rotation = frozenRotation;
            carRigidbody.velocity = frozenVelocity;

            speedText.text = "Car resumed. Speed: " + frozenVelocity.magnitude.ToString("F2") + " m/s";
            Invoke("HideSpeedText", 2f);

            angleInputField.SetActive(false);
            submitButton.SetActive(false);
            isInBend = false;
        }
        else
        {
            speedText.text = "Invalid angle. Try again.";
        }
    }

    private void HideSpeedText()
    {
        speedText.text = "";
    }
}
