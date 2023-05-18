using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float destroyHeight;
    private float pos;

    private void Start()
    {
        pos = transform.position.y;   
    }
    void Update()
    {
        pos -= fallingSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, pos, transform.position.z);
        transform.Rotate(new Vector3(rotationSpeed, rotationSpeed + 0.25f, rotationSpeed + 0.5f));
        if (transform.position.y < destroyHeight)
        {
            Destroy(this.gameObject);
        }
    }
}
