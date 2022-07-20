using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float followSpeed = 5;

    private Transform t;

    private void Start()
    {
        target = GameObject.Find("Player").transform;

        t = transform;
    }

    private void FixedUpdate()
    {
        var targetPos = target.position + new Vector3(0, 0, -10);
        t.position = Vector3.Lerp(t.position, targetPos, Time.fixedDeltaTime * followSpeed);
    }
}
