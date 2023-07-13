using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleScanner : MonoBehaviour
{
    private Turtle turtle;

    private void Awake()
    {
        turtle = GetComponentInParent<Turtle>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        turtle.FlipTurtle();
    }
}
