using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour
{
    // Main characteristics
    public float moveSpeed;
    private float temporaryMoveSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float shootDistnace;
    public float xShootAngle;
    public float shootCooldown;
    private float timer = 0;
    [SerializeField] int damage; 

    //links
    private Transform target;
    private Rigidbody rb;
    private Transform ShootPos; // откуда стреляем
    public GameObject bullet;


    private Vector3 rotationVector// направление  передвижения
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
        }
    }
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
        ShootPos = this.gameObject.transform.GetChild(0);

        temporaryMoveSpeed = moveSpeed;
    }
    void Update()
    {
        ControllDistance();
        Move();
    }
    private void Move()
    {
        //перемещение персонажа

        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    private void ControllDistance()
    {
        if (rotationVector.magnitude <= shootDistnace)
        {
            Stop();
            if (timer >= shootCooldown)
            {
                Shoot();
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            Go();
        }
    }
    private void Stop()
    {
        moveSpeed = 0;
    }
    private void Go()
    {
        moveSpeed = temporaryMoveSpeed;
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<HealthControll>())
        {
            return;
        }
        else
        {
            collision.gameObject.GetComponent<HealthControll>().ChangeHealth(-damage);
        }
    }
}
