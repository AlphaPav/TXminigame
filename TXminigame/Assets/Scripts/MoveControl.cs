using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour {
    // [SerializeField]作用：私有化变量也可以在面板上显示和赋值
    [SerializeField] private float m_moveSpeedStandard = 5;
    [SerializeField] private float m_moveSpeed = 5;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;




    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded = true;
    private List<Collider> m_collisions = new List<Collider>();


    float v = 0.0f;
    float h = 0.0f;
    float last_v = 0.0f;
    float last_h = 0.0f;

   
    public Transform footfalls;
    public float total_time=0;
   

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
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
        
        m_animator.SetBool("Grounded", m_isGrounded);
        MovementUpdate();
        m_wasGrounded = m_isGrounded;

        if (tempState == PEOPLE.BEGIN_SKILL)
        {
            //如果这时候movement改变，则技能被打断
            if (last_v != v || last_h != h)
            {
                this.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
            }
            
        }

        last_v = v;
        last_h = h;

    }

    private void MovementUpdate()
    {

        float v = this.GetComponent<InfoControl>().getVerticalMove();
        float h = this.GetComponent<InfoControl>().getHorizontalMove();
        if (v < 0)
        {

            v *= m_backwardRunScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }


    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }
}
