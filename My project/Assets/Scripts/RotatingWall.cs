using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    [SerializeField]private float rotatingSpeed;

    float yAngle;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        yAngle += rotatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
    }
}
