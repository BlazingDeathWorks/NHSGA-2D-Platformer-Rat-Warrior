using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] [Range(0, 15)] private float radius = 1;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] [Range(0, 15)] private float combatRange = 1;
    private Vector2 target;
    private Vector2 originalPosition;
    private bool inRadius;
    private bool outOfBounds;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (Player.Instance == null) return;

        if (Vector2.Distance(transform.position, originalPosition) > combatRange)
        {
            inRadius = false;
            outOfBounds = true;
            StartCoroutine(EnableBounds());
        }

        if (!inRadius)
        {
            if ((Vector2)transform.position == originalPosition) return;
            target = originalPosition;
            FollowTarget();
            return;
        }

        target = Player.Instance.transform.position;
        FollowTarget();
    }

    private void FixedUpdate()
    {
        if (outOfBounds) return;

        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, whatIsPlayer);
        inRadius = collider;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(originalPosition, combatRange);
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, combatRange);
        }
    }

    private IEnumerator EnableBounds()
    {
        yield return new WaitForSecondsRealtime(3f);
        outOfBounds = false;
    }

    private void FollowTarget()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(transform.position.x - target.x), transform.localScale.y, transform.localScale.z);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
