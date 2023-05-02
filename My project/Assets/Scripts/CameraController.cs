using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 posOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x + posOffset.x, posOffset.y, playerTransform.position.z + posOffset.z);
    }
}
