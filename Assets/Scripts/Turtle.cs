using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.left * speed;
    }

    public void FlipTurtle()
    {
        speed *= -1;
        sr.flipX = !sr.flipX;
    }
}
