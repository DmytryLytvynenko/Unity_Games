using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    //Oсновные параметры
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
/*    [SerializeField] private float maxSpeed;*/
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canExplode = false;
    [SerializeField] private float ExplosionJumpHeight = 4f;
    private float velocityToExplode = -6.5f;


    //Ссылки на компоненты
    private MobileController mController;
    private Animator ch_animator;
    private Rigidbody rb;
    private Explosion explosion;
    private Transform throwArea;

    [Header("Throw variables")]
    [SerializeField] private float xThrowSize;
    [SerializeField] private float yThrowSize;
    [SerializeField] private float zThrowSize;
    [SerializeField] private float yThrowAngle;
    [SerializeField] private float throwForce;
    [SerializeField] private int throwDamage;

    // Параметры геймплея
    private Vector3 moveVector// направление  передвижения
    {
        get
        {
            var horizontal = mController.Horizontal();
            var vertical = mController.Vertical();

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        /*ch_animator = GetComponent<Animator>();*/
        mController = GameObject.FindGameObjectsWithTag("Joystick")[0].GetComponent<MobileController>();
        throwArea = transform.GetChild(2).transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Move();
        VisualizeBox.DisplayBox(throwArea.position, new Vector3(xThrowSize/2, yThrowSize/2, zThrowSize/2), throwArea.rotation);
        /*        Throw();*/
    }
    private void FixedUpdate()
    {
        SetCanExplode();
    }

    private void Move()
    {
        //вращение персонажа
        if (moveVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        //перемещение персонажа
        // без инерции
        Vector3 offset = moveVector * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + offset);

        // с инерцией
/*        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveVector * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }*/
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void Throw()
    {
        Collider[] overlappedColiders = Physics.OverlapBox(throwArea.position, new Vector3(xThrowSize, yThrowSize, zThrowSize), throwArea.rotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Player"))
            {
                Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x, (rigitbody.transform.position.y - transform.position.y) + yThrowAngle, rigitbody.transform.position.z - transform.position.z);
                Debug.DrawLine(transform.position, new Vector3(rigitbody.transform.position.x, rigitbody.transform.position.y + yThrowAngle, rigitbody.transform.position.z), Color.red, 100);
                rigitbody.AddForce(enemyDir.normalized * throwForce, ForceMode.Impulse);
                Debug.Log("Throw!");
                if (!rigitbody.gameObject.GetComponent<HealthControll>())
                {
                    continue;
                }
                else
                {
                    rigitbody.GetComponent<HealthControll>().ChangeHealth(-throwDamage);
                }
            }
        }
    }

    private void SetCanExplode()
    {
        if (isGrounded)
        {
            return;
        }

        Ray ray = new Ray(transform.position, -100*Vector3.up);
        Debug.DrawRay(transform.position, -100 * Vector3.up, Color.red);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.distance >= ExplosionJumpHeight && rb.velocity.y < velocityToExplode)
        {
            canExplode = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
        if (collision.gameObject.CompareTag("Ground") && canExplode)
        {
            explosion.Explode();
            canExplode = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void OnCollisionStay(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground") || collision.gameObject.tag == ("Platform"))
        {
            isGrounded = value;
        }
    }
    private void OnDrawGizmos()
    {

    }

}