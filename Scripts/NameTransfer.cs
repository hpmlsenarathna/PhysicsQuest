using UnityEngine;

public class Cat : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed = 5f, dirx;
    [SerializeField] GameObject codePanel, closedSafe, openedSafe;
    public static bool isSafeOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        codePanel.SetActive(false);
        closedSafe.SetActive(true);
        openedSafe.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        dirx = Input.GetAxis("Horizontal") * moveSpeed;

        if (isSafeOpened)
        {
            codePanel.SetActive(false);
            closedSafe.SetActive(false);
            openedSafe.SetActive(true);
        }
    }

    // FixedUpdate is called at a fixed time interval
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirx, rb.velocity.y);
    }

    // When an object enters the trigger collider
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Safe") && !isSafeOpened)
        {
            codePanel.SetActive(true);
        }
    }

    // When an object exits the trigger collider
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Safe"))
        {
            codePanel.SetActive(false);
        }
    }
}
