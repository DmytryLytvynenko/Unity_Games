using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    //Oсновные параметры
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canExplode;
    [SerializeField] private float ExplosionJumpHeight = 6f;




    //Ссылки на компоненты
    private MobileController mController;
    private Animator ch_animator;
    private Rigidbody rb;
    private Explosion explosion;

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
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        SetCanExplode();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //перемещение персонажа
        
        if (moveVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveVector * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void SetCanExplode()// Need to finish
    {
        if (!isGrounded && Physics.Raycast(transform.position, -Vector3.up, ExplosionJumpHeight))
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
}