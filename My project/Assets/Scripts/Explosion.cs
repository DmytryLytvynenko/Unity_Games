using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius;
    public float explosionForce;

    public GameObject explosionEffect;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Explode();
        }
    }

    public void Explode()
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Player"))
            {
                Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
                int explosionDamage =Convert.ToInt32(((radius - distanceToTarget.magnitude) / radius) * explosionForce);// Чем ближе к игроку протимник тем больше damage

                rigitbody.AddExplosionForce(explosionForce, transform.position, radius);
                if (!rigitbody.gameObject.GetComponent<HealthControll>())
                {
                    continue;
                }
                else
                {
                    rigitbody.GetComponent<HealthControll>().ChangeHealth(-explosionDamage);
                }
                print($"Explosion Damage:{explosionDamage}");
            }
        }

        print("explosion");
        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
    public void NoDamageExplode()
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Player"))
            {
                Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
                rigitbody.AddExplosionForce(explosionForce, transform.position, radius);
            }
        }

        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
}
