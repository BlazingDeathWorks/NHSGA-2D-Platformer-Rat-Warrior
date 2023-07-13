using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform shootPos;

    public void Fire()
    {
        Bullet instance = Instantiate(bullet, shootPos.position, Quaternion.identity);
        instance.transform.localScale = transform.localScale;
        instance.SetParent(transform);
    }
}
