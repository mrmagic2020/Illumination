using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandglassRotate : MonoBehaviour
{
    private Transform sandglassTransform;
    private Quaternion target;
    private float rotation;
    private float r_speed;

    // Start is called before the first frame update
    void Start()
    {
        sandglassTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        rotation = sandglassTransform.rotation.z;
        float dot = Vector3.Dot(transform.up, Vector3.up);
        dot = (dot + 2) * 10;

        r_speed = dot;

        target = Quaternion.Euler(0, 0, rotation + r_speed);

        sandglassTransform.rotation = Quaternion.Lerp(sandglassTransform.rotation, target, r_speed*Time.deltaTime);

        Debug.Log(r_speed);
    }

    private Vector3 QuaternionToVector3(Quaternion quaternion)
    {
        return new Vector3(quaternion.x, quaternion.y, quaternion.z);
    }
}
