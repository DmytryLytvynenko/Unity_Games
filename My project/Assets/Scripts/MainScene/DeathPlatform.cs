using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
