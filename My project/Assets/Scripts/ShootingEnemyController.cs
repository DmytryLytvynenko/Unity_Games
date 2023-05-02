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
    public float timeToLive;
    public float shootDistnace;
    public float xShootAngle;
    public float shootCooldown;
    private float timer = 0;
    [SerializeField] float Y; 

    // Components

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
    // Start is called before the first frame update
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        Invoke("Die", timeToLive);
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
        ShootPos = this.gameObject.transform.GetChild(0);

        temporaryMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
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
    private void Die()
    {
        Destroy(this.gameObject);
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
        Y = transform.localEulerAngles.y;
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;
    }
}
