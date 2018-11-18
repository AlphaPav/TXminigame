using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour {
    //[SerializeField] 作用：私有化变量也可以在面板上显示和赋值
    [SerializeField] private float m_moveSpeedStandard = 5;
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private Animator m_animator;
   


    float v = 0.0f;
    float h = 0.0f;
    float last_v = 0.0f;
    float last_h = 0.0f;


    public Transform footfalls;
    public float total_time=0;
    private void Start()
    {
        m_animator = this.GetComponent<Animator>();


    }

    void Update()
    {
        bool transparent = this.GetComponent<UIControl>().transparent;
        total_time += Time.deltaTime;
        if (total_time>0.1&&transparent==false)
        {
            Instantiate(footfalls, GetComponent<Transform>().position, footfalls.rotation);
            total_time = 0;
        }

        int tempState = this.GetComponent<InfoControl>().getState();
        if (tempState == PEOPLE.ICE|| tempState == PEOPLE.EXECUTE_SKILL|| tempState == PEOPLE.END_SKILL)
            return;

        if (tempState == PEOPLE.FREE)
        {
            m_moveSpeed = m_moveSpeedStandard;
        }
        else if (tempState == PEOPLE.SLOW)
        {
            m_moveSpeed =  m_moveSpeedStandard * (float)0.5;
        }
        
        m_animator.SetBool("Grounded", true);
        MovementUpdate();
     

        if (tempState == PEOPLE.BEGIN_SKILL)
        {
            //如果这时候movement改变，则技能被打断
            if (h != 0.0f || v != 0.0f)
            {
                Debug.Log("changeState(PEOPLE.END_SKILL)");
                this.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
            }
            
        }

        //last_v = v;
        //last_h = h;

    }

    private void MovementUpdate()
    {

        m_animator.SetBool("Grounded", true);
        h = this.GetComponent<InfoControl>().getHorizontalMove();
        v = this.GetComponent<InfoControl>().getVerticalMove();

       // Debug.Log(new Vector2(h, v));

        float dis = (new Vector3(h, 0, v)).magnitude;

        transform.position = this.transform.position + new Vector3(h, 0, v) * m_moveSpeed * Time.deltaTime;
        if (h != 0 || v != 0)
        {

            Rotating(h, v);
        }
        m_animator.SetFloat("MoveSpeed", dis);

    }
    void Rotating(float horizontal, float vertical)
    {
        Debug.Log("rotating");
        // 创建角色目标方向的向量
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        // 创建目标旋转值 并假设Y轴正方向为"上"方向
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); //函数参数解释: LookRotation(目标方向为"前方向", 定义声明"上方向")
        // 创建新旋转值 并根据转向速度平滑转至目标旋转值
        //  Lerp(角色刚体当前旋转值, 目标旋转值, 根据旋转速度平滑转向)
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, m_moveSpeed * Time.deltaTime);
        // 更新刚体旋转值为 新旋转值
        this.transform.rotation = newRotation;

    }


    //[SerializeField] private Rigidbody m_rigidBody;
    //[SerializeField] private float m_turnSpeed = 200;
    //[SerializeField] private float m_jumpForce = 4;



    //private float m_currentV = 0;
    //private float m_currentH = 0;

    //private readonly float m_interpolation = 10;
    //private readonly float m_walkScale = 0.33f;
    //private readonly float m_backwardsWalkScale = 0.16f;
    //private readonly float m_backwardRunScale = 0.66f;

    //private bool m_wasGrounded;
    //private Vector3 m_currentDirection = Vector3.zero;

    //private float m_jumpTimeStamp = 0;
    //private float m_minJumpInterval = 0.25f;

    //private bool m_isGrounded = true;
    //private List<Collider> m_collisions = new List<Collider>();

    //private void JumpingAndLanding()
    //{
    //    bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

    //    if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
    //    {
    //        m_jumpTimeStamp = Time.time;
    //        m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
    //    }

    //    if (!m_wasGrounded && m_isGrounded)
    //    {
    //        m_animator.SetTrigger("Land");
    //    }

    //    if (!m_isGrounded && m_wasGrounded)
    //    {
    //        m_animator.SetTrigger("Jump");
    //    }
    //}





    //private void OnCollisionEnter(Collision collision)
    //{
    //    ContactPoint[] contactPoints = collision.contacts;
    //    for (int i = 0; i < contactPoints.Length; i++)
    //    {
    //        if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
    //        {
    //            if (!m_collisions.Contains(collision.collider))
    //            {
    //                m_collisions.Add(collision.collider);
    //            }
    //            m_isGrounded = true;
    //        }
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    ContactPoint[] contactPoints = collision.contacts;
    //    bool validSurfaceNormal = false;
    //    for (int i = 0; i < contactPoints.Length; i++)
    //    {
    //        if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
    //        {
    //            validSurfaceNormal = true; break;
    //        }
    //    }

    //    if (validSurfaceNormal)
    //    {
    //        m_isGrounded = true;
    //        if (!m_collisions.Contains(collision.collider))
    //        {
    //            m_collisions.Add(collision.collider);
    //        }
    //    }
    //    else
    //    {
    //        if (m_collisions.Contains(collision.collider))
    //        {
    //            m_collisions.Remove(collision.collider);
    //        }
    //        if (m_collisions.Count == 0) { m_isGrounded = false; }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (m_collisions.Contains(collision.collider))
    //    {
    //        m_collisions.Remove(collision.collider);
    //    }
    //    if (m_collisions.Count == 0) { m_isGrounded = false; }
    //}



}
