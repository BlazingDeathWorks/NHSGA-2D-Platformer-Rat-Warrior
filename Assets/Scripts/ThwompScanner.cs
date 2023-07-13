using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompScanner : MonoBehaviour
{
    private Thwomp thwomp;

    private void Awake()
    {
        thwomp = GetComponentInParent<Thwomp>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (thwomp == null) return;
        Debug.Log(collision.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            thwomp.Smash();
        }
    }
}
