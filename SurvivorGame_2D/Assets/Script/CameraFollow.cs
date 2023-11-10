using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
        Mathf.Clamp(followTransform.position.x,minX,maxX),
        Mathf.Clamp(followTransform.position.y,minY,maxY),
        transform.position.z);
    }
}
