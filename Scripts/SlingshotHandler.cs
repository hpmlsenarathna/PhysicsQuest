using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingshotHandler : MonoBehaviour
{
    [Header("Line Renders")]
    [SerializeField] private LineRenderer lineRendererLeft;
    [SerializeField] private LineRenderer lineRendererRight;

    [Header("Transforms/Position")]
    [SerializeField] private Transform lineRendererLeftTransform;
    [SerializeField] private Transform lineRendererRightTransform;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform idlePos;
    [SerializeField] private Transform elasticTransform;

    [Header("Distance")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce = 9f;
    [SerializeField] private float timeBetweenRespawns = 2f;
    [SerializeField] private float elasticDivider = 1.2f;

    [Header("Other")]
    [SerializeField] private SlingshotArea SlingshotAreaInstance;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private AnimationCurve passingCurve;
    [SerializeField] private float maxAniTime = 1f;

    private Vector2 slingshotLinePos;
    private Vector2 direction;
    private Vector2 directionNormalized;

    private bool clickedWithinArea;

    [Header("Bird")]
    [SerializeField] private AngryBird birdPrefab;
    [SerializeField] private float birdOffset = .275f;
    private AngryBird birdPrefabSpawned;

    //need to fix this
    [SerializeField] private bool _birdOnSlingShot;

    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPullCip;
    [SerializeField] private AudioClip[] elasticPullCips;

    private AudioSource AudioSource;


    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();

        lineRendererLeft.enabled = false;
        lineRendererRight.enabled = false;

        SpawnAngryBird();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.WasLeftMosueButtonPressed && SlingshotAreaInstance.isWithinSlingshotArea())
        {
            clickedWithinArea = true;

            if (_birdOnSlingShot)
            {
                SoundManager.instance.PlayClip(elasticPullCip, AudioSource);
                cameraManager.SwitchFollowCam(birdPrefabSpawned.transform);
            }
        }

        if (InputManager.IsLeftMousePressed && clickedWithinArea)
        {
            Debug.Log("Left Mouse pressed");
            DrawSlingshot();
            PositionRotationBird();
        }

        if (InputManager.WasLeftMosueButtonReleased && clickedWithinArea)
        {
            if (GameManager.Instance.hasEnoughShots())
            {
                clickedWithinArea = false;
                _birdOnSlingShot = false;

                birdPrefabSpawned.LaunchBird(direction, shotForce);
                GameManager.Instance.useShot();
                SoundManager.instance.PlayRandomClip(elasticPullCips, AudioSource);
                AnimateSlingshot();
                

                if (GameManager.Instance.hasEnoughShots())
                {
                    StartCoroutine(SpawnAngryBirdTime());
                }
            }
        }
    }

    #region Slingshot method

    //draws slingshot based on mouse position
    private void DrawSlingshot()
    {
        //gets the screen poistion of the mouse
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePos);

        //takes the center of the slingshot and makes it where it can only but in a certain magnitude/certain positions
        slingshotLinePos = centerPos.position + Vector3.ClampMagnitude(touchPosition - centerPos.position, maxDistance);

        SetLines(slingshotLinePos);

        direction = (Vector2)centerPos.position - slingshotLinePos;
        directionNormalized = direction.normalized;
    }

    //draws the lines
    private void SetLines(Vector2 position)
    {
        if (!lineRendererLeft.enabled && !lineRendererRight.enabled)
        {
            lineRendererRight.enabled = true;
            lineRendererLeft.enabled = true;
        }

        lineRendererLeft.SetPosition(0, position);
        lineRendererLeft.SetPosition(1, lineRendererLeftTransform.position);

        lineRendererRight.SetPosition(0, position);
        lineRendererRight.SetPosition(1, lineRendererRightTransform.position);
    }
    #endregion

    #region Angry Bird methods
    private void SpawnAngryBird()
    {
        elasticTransform.DOComplete();
        SetLines(idlePos.position);
        Vector2 dir = (centerPos.position - idlePos.position).normalized;
        Vector2 spawnPos = (Vector2) idlePos.position + dir * birdOffset;

        birdPrefabSpawned = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
        birdPrefabSpawned.transform.right = dir;

        _birdOnSlingShot = true;
    }

    private void PositionRotationBird()
    {
        birdPrefabSpawned.transform.position = slingshotLinePos + directionNormalized * birdOffset;
        birdPrefabSpawned.transform.right = directionNormalized;   
    }

    private IEnumerator SpawnAngryBirdTime()
    {
        yield return new WaitForSeconds(timeBetweenRespawns);

        SpawnAngryBird();

        cameraManager.SwitchIdleCam();
    }
    #endregion

    #region Animate Slingshot
    private void AnimateSlingshot()
    {
        elasticTransform.position = lineRendererLeft.GetPosition(0);
        float dis = Vector2.Distance(elasticTransform.position, centerPos.position);

        float time = dis / 1.2f;

        elasticTransform.DOMove(centerPos.position, time).SetEase(passingCurve);
        StartCoroutine(SlingshotLines(elasticTransform, time));
    }

    private IEnumerator SlingshotLines(Transform trans, float timeBetweenRespawns)
    {
        float elpasedTime = 0f;
        while (elpasedTime < timeBetweenRespawns && elpasedTime < maxAniTime)
        {
            elpasedTime += Time.deltaTime;

            SetLines(trans.position);

            yield return null;
        }
    }
    #endregion
}
