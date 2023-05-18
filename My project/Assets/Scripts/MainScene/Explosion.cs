﻿using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius;
    public float explosionForce;
    [SerializeField] private float noDamageExplosionForce;
    [SerializeField] private float noDamageExplosionRadius;

    public GameObject explosionEffect;

    public void Explode(int explosionDamage)
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Player"))
            {
                Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
                int damage =Convert.ToInt32(((explosionRadius - distanceToTarget.magnitude) / explosionRadius) * explosionDamage);// Чем ближе к игроку протимник тем больше damage
                if (rigitbody.gameObject.CompareTag("Bomb") && damage >= (explosionDamage/2))
                {
                    rigitbody.gameObject.GetComponent<Bomb>().SetActive(true);
                }
                rigitbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                if (!rigitbody.gameObject.GetComponent<HealthControll>())
                {
                    continue;
                }
                else
                {
                    rigitbody.GetComponent<HealthControll>().ChangeHealth(-damage);
                }
                print($"Explosion Damage:{damage}");
            }
        }

        print("explosion");
        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
    public void NoDamageExplode()
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, noDamageExplosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Player"))
            {
                Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
                rigitbody.AddExplosionForce(noDamageExplosionForce, transform.position, noDamageExplosionRadius);
            }
        }

        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
    public void BossExplode(int explosionDamage)
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Boss"))
            {
                rigitbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
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
}
