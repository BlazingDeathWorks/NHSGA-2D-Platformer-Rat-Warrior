using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private AudioClip swordSlash;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(swordSlash);
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.Damage();
                return;
            }
            Destroy(collision.gameObject);
        }
    }
}
