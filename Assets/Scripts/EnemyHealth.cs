using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;

    public void Damage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
