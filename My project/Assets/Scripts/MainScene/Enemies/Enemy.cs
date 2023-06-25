using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform target;
    protected Rigidbody rb;
    [SerializeField] protected int contactDamage;

    protected float moveSpeed;
    protected float maxSpeed;
    protected float rotationSpeed;

    protected Vector3 rotationVector// �����������  ������������
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
        }
    }
    protected void GiveContactDamage(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (!collision.gameObject.GetComponent<HealthControll>())
        {
            return;
        }
        else
        {
            collision.gameObject.GetComponent<HealthControll>().ChangeHealth(-contactDamage);
        }
    }
    protected void Die()
    {
        Destroy(this.gameObject);
    }
    protected virtual void Move()
    {
        //����������� ���������

        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//����� ������������ 
        }
    }
}
