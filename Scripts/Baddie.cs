using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float damageThreshold = 0.2f;
    [SerializeField] private GameObject baddieDeathParticle;
    private float currentHealth;

    [SerializeField] private AudioClip Audioclip;
    private AudioSource AudioSource;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DamageBaddie(float damaageAmt)
    {
        currentHealth -= damaageAmt;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVel = collision.relativeVelocity.magnitude;
        if(impactVel > damageThreshold)
        {
            DamageBaddie(impactVel);
        }
    }

    private void Die()
    {
        GameManager.Instance.removeBaddie(this);

        Instantiate(baddieDeathParticle, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint( Audioclip, transform.position);
        Destroy(gameObject);
    }
}
