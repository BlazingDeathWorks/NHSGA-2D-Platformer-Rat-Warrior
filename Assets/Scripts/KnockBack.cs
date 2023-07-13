using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float movementDisableTime = 0.7f;
    private float knockBackForce = 30;
    private float timeSinceLastHit;
    private Movement playerMovement;

    private void Update()
    {
        if (playerMovement == null || playerMovement.enabled) return;

        timeSinceLastHit += Time.deltaTime;
        if (timeSinceLastHit >= movementDisableTime)
        {
            playerMovement.enabled = true;
            timeSinceLastHit = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement = collision.GetComponent<Movement>();
            playerMovement.TriggerHit(knockBackForce, transform.position.x);
        }
    }
}
