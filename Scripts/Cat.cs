using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private AudioClip Audioclip;
    private AudioSource AudioSource;
    public Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool hasBeenLaunched;
    private bool shouldFaceVelcityDirect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rb.isKinematic = true;
        circleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!hasBeenLaunched && shouldFaceVelcityDirect)
        {
            //has the bird face the direction of the launch
            transform.right = rb.velocity;
        }
    }

    public void LaunchCat(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        circleCollider.enabled = true;

        //apply force
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
        shouldFaceVelcityDirect = true;
    }

    //automatically called when a 2D collider is hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        shouldFaceVelcityDirect = false;
        SoundManager.instance.PlayClip(Audioclip, AudioSource);
    }
}
