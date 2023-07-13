using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    [SerializeField] private float smashDownSpeed = 1;
    private bool isSmashing;
    private bool isLifting;
    private Vector2 originalPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        transform.GetChild(0).parent = null;
        isLifting = false;
    }

    private void Update()
    {
        if (!isLifting) return;
        
        if (Vector2.Distance(transform.position, originalPosition) <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            isLifting = false;
        }
    }

    private void FixedUpdate()
    {
        if (isSmashing)
        {
            rb.velocity = Vector2.down * smashDownSpeed;
        }
        if (isLifting)
        {
            rb.velocity = Vector2.up * smashDownSpeed / 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isSmashing = false;
        isLifting = true;
    }

    public void Smash()
    {
        if (isLifting) return;
        isSmashing = true;
    }
}
