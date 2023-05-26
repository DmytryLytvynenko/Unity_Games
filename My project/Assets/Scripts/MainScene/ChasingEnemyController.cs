using UnityEngine;

public class ChasingEnemyController : MonoBehaviour
{
    private Transform target;

    public float moveSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public int damage;

    private Rigidbody rb;

    private Vector3 rotationVector// направление  передвижения
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z).normalized;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
    }
    void Update()
    {
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
