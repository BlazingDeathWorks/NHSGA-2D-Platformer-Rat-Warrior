using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Transform parent;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    private void FixedUpdate()
    {
        if (parent == null)
        {
            Destroy(gameObject);
            return;
        }
        rb.velocity = Vector2.left * speed * parent.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }
}
