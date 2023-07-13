using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Positions must be childrens of the platform gameobject to force organized hierarchies and reduce clutter
public class Platform : MonoBehaviour
{
    [SerializeField] [Range(0, 30)] private float speed = 1;
    private List<Vector2> positions = new List<Vector2>();
    private int index;

    private void Awake()
    {
        positions.Add(transform.position);
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            positions.Add(child.position);
            child.parent = null;
        }
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, positions[index], speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, positions[index]) != 0) return;

        if (++index >= positions.Count)
        {
            index = 0;
        }
    }
}
