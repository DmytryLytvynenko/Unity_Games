using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigBoss : MonoBehaviour
{
    // Main characteristics
    [Header("Main")]
    [SerializeField] private float moveSpeed;
    private float tempMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int explosionDamage;
    [SerializeField] private int contactDamage;
    private Transform target;
    private Rigidbody rb;
    [SerializeField] private GameObject pointer;
    private GameObject _pointer;
    private float prepareTimer = 0f;
    private float waitTillTime;
    private Action currentAction;
    private float chooseActionTimer = 0f;
    [SerializeField] private float chooseActionTime;
    private bool isGrounded;  



    [Header("Shoot")]
    [SerializeField] private float waitTillShootTime;
    [SerializeField] private float shootDistnace;
    [SerializeField] private float xShootAngle;
    [SerializeField] private float numberOfBullets;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject bullet;
    private Transform ShootPos; // ������ ��������
    private bool isShooting;
    private bool canShoot;


    [Header("Jump")]
    [SerializeField] private float jumpDistnace;
    private bool canJump;
    [SerializeField] private float waitTilljumpTime;
    [SerializeField] private float jumpHeight = 7;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool debugPath;


    [Header("Kick")]
    private Transform kickArea;
    [SerializeField] private float kickDistnace;
    [SerializeField] private float waitTillKickTime;
    private bool canKick;
    [SerializeField] private float xKickSize;
    [SerializeField] private float yKickSize;
    [SerializeField] private float zKickSize;
    [SerializeField] private float yKickAngle;
    [SerializeField] private float kickForce;
    [SerializeField] private int kickDamage;

    private enum Action
    {
        Shoot,
        Jump,
        Kick,
        None
    }
    private Vector3 rotationVector// �����������  ������������
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
        }
    }
    void Start()
    {
        /*pointer = Resources.Load<GameObject>("Prefabs/BigBossPointer");*/
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
        ShootPos = this.gameObject.transform.GetChild(0);
        tempMoveSpeed = moveSpeed;
        kickArea = transform.GetChild(2).transform;
        currentAction = Action.None;
    }
    void Update()
    {
        Move();
        ChooseAction();
        Jump();
        Kick();
        ControllJumpDistance();
        ControllShootDistance();
        ControllKickDistance();
        VisualizeBox.DisplayBox(kickArea.position, new Vector3(xKickSize / 2, yKickSize / 2, zKickSize / 2), kickArea.rotation);
    }

    private void ChooseAction()
    {
        if (!isGrounded)
        {
            return;
        }
        if (currentAction != Action.None)
        {
            //���� ��������� �������� �� ��������� �����, �� ��� ������������(currentAction = Action.None) � � ��������� ��� ��������� ������
/*            if (chooseActionTimer >= chooseActionTime)
            {
                currentAction = Action.None;
            }
            chooseActionTimer += Time.deltaTime;*/
            return;
        }
        int randomDigit = Random.Range(1,4);
        switch (randomDigit)
        {
            case (1):
                currentAction = Action.Shoot;
                break;
            case (2):
                currentAction = Action.Jump;
                break;
            case (3):
                currentAction = Action.Kick;
                break;
            default:
                break;
        }
    }
    private void Move()
    {
        //����������� ���������
        if (canJump)
        {
            return;
        }

        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }

        if (rb.velocity.magnitude < maxMoveSpeed)
        {
            Vector3 offset = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + offset);//����� ������������ 
        }
    }
    private void ControllKickDistance()
    {
        if (currentAction != Action.Kick)
        {
            return;
        }
        if (rotationVector.magnitude <= kickDistnace)
        {
            if (!isGrounded)
            {
                return;
            }
            moveSpeed = 0f;
            waitTillTime = waitTillKickTime;
            IndicateAction(Action.Kick);
            WaitForTime(Action.Kick);
        }
        else
        {
            if (!isGrounded)
            {
                return;
            }
            if (canKick)
            {
                return;
            }
            if (moveSpeed != 0)
            {
                return;
            }
            DeIndicateAction(Action.Jump);
            moveSpeed = tempMoveSpeed;
        }
    }
    public void Kick()
    {
        if (currentAction != Action.Kick)
        {
            return;
        }
        if (!canKick)
        {
            return;
        }
        Collider[] overlappedColiders = Physics.OverlapBox(kickArea.position, new Vector3(xKickSize / 2, yKickSize / 2, zKickSize / 2), kickArea.rotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !overlappedColiders[i].gameObject.CompareTag("Boss"))
            {
                Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x, (rigitbody.transform.position.y - transform.position.y) + yKickAngle, rigitbody.transform.position.z - transform.position.z);
                Debug.DrawLine(transform.position, new Vector3(rigitbody.transform.position.x, rigitbody.transform.position.y + yKickAngle, rigitbody.transform.position.z), Color.red, 100);
                rigitbody.AddForce(enemyDir.normalized * kickForce, ForceMode.Impulse);
                Debug.Log("Kick!");
                if (!rigitbody.gameObject.GetComponent<HealthControll>())
                {
                    continue;
                }
                else
                {
                    rigitbody.GetComponent<HealthControll>().ChangeHealth(-kickDamage);
                }
            }
        }
        canKick = false;
        currentAction = Action.None;
    }
    private void Jump()
    {
        if (!canJump)
        {
            return;
        }
        /*Logic*/
        DrawPointer();
        Launch();
        canJump = false;
        currentAction = Action.None;
    }
    private void ControllJumpDistance()
    {
        if (currentAction != Action.Jump)
        {
            return;
        }
        if (rotationVector.magnitude <= jumpDistnace)
        {
            if (!isGrounded)
            {
                return;
            }
            moveSpeed = 0f;
            waitTillTime = waitTilljumpTime;
            IndicateAction(Action.Jump);
            WaitForTime(Action.Jump);
        }
        else
        {
            if (!isGrounded)
            {
                return;
            }
            if (canJump)
            {
                return;
            }
            if (moveSpeed != 0)
            {
                return;
            }
            DeIndicateAction(Action.Jump);
            moveSpeed = tempMoveSpeed;
        }
    }
    private IEnumerator shootingCoroutine()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
            bullet.GetComponent<Bullet>().targetPoint = target;

            yield return new WaitForSeconds(timeBetweenShots);
        }
        canShoot = false;
        isShooting = false;
        currentAction = Action.None;
    }
    private void ControllShootDistance()
    {
        if (currentAction != Action.Shoot)
        {
            return;
        }
        if (rotationVector.magnitude <= shootDistnace)
        {
            if (canShoot)
            {
                return;
            }
            moveSpeed = 0f;
            waitTillTime = waitTillShootTime;
            IndicateAction(Action.Shoot);
            WaitForTime(Action.Shoot);
            if (!canShoot)
            {
                return;
            }
            if (isShooting)
            {
                return;
            }
            else
            {
                isShooting = true;
                StartCoroutine(shootingCoroutine());
            }
        }
        else
        {
            if (canShoot)
            {
                return;
            }
            if (moveSpeed != 0)
            {
                return;
            }
            DeIndicateAction(Action.Shoot);
            moveSpeed = tempMoveSpeed;
        }
    }
    private void IndicateAction(Action action)
    {
        switch (action)
        {
            case Action.Shoot:
                GetComponent<Renderer>().material.color = Color.blue;
                break;

            case Action.Jump:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;

            case Action.Kick:
                GetComponent<Renderer>().material.color = Color.green;
                break;

            default:
                break;
        }
    }
    private void DeIndicateAction(Action action)
    {
        switch (action)
        {
            case Action.Shoot:
                GetComponent<Renderer>().material.color = Color.red;
                break;

            case Action.Jump:
                GetComponent<Renderer>().material.color = Color.red;
                break;

            case Action.Kick:
                GetComponent<Renderer>().material.color = Color.red;
                break;

            default:
                break;
        }
    }
    private void WaitForTime(Action action)
    {
        if (canShoot)
        {
            return;
        }
        if (prepareTimer <= waitTillTime)
        {
            prepareTimer += Time.deltaTime;
        }
        else
        {
            prepareTimer = 0f;
            switch (action)
            {
                case Action.Shoot:
                    canShoot = true;
                    break;

                case Action.Jump:
                    canJump = true;
                    break;

                case Action.Kick:
                    canKick = true;
                    break;

                default:
                    break;
            }
        }
    }
    private void DrawPointer()
    {
        Ray ray = new Ray(target.position, -Vector3.up * 10);
        Physics.Raycast(ray, out RaycastHit hit);
        Vector3 drawPoint = hit.point;
        _pointer = Instantiate(pointer, drawPoint, Quaternion.identity);
    }
    private void Launch()
    {
        rb.velocity = CalculateLaunchData().initialVelocity;
    }
    LaunchData CalculateLaunchData()
    {
        if (target.position.y >= rb.position.y)
        {
            jumpHeight = target.position.y + 1;
        }
        else
        {
            jumpHeight = rb.position.y + 1;
        }

        float displacementY = target.position.y - rb.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - rb.position.x, 0, target.position.z - rb.position.z);
        float time = Mathf.Sqrt(-2 * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded)
        {
            gameObject.GetComponent<Explosion>().BossExplode(explosionDamage);
        }
        IsGroundedUpate(collision, true);
        if (_pointer != null)
        {
            Destroy(_pointer);
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

