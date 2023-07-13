using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject swordContainer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotatingRadius = 1.1f;
    [SerializeField] private float swordRotationOffset = -90;
    private Vector2 direction;

    private void Update()
    {
        direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        swordContainer.transform.localPosition = FindSwordPos();
        swordContainer.transform.localEulerAngles = new Vector3(swordContainer.transform.localRotation.x, swordContainer.transform.localRotation.y, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + swordRotationOffset);
    }

    private Vector2 FindSwordPos()
    {
        float hyp = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        float ratio = rotatingRadius / hyp;
        return (Vector2)transform.position + new Vector2(direction.x * ratio, direction.y * ratio);
    }
}
