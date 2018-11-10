using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveControl : MonoBehaviour {
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



    void Update()
    {

        MovementUpdate();
        
    }

    private void MovementUpdate()
    {
        /*float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");*/
        float v = 0.0f;
        float h = 0.0f;
        //w键前进  
        if (Input.GetKey(KeyCode.UpArrow))
        {
            v += Time.deltaTime * 100;
        }
        //s键后退  
        if (Input.GetKey(KeyCode.DownArrow))
        {
            v -= Time.deltaTime * 100;
        }
        //a键左
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            h -= Time.deltaTime * 100;
        }
        //d键右
        if (Input.GetKey(KeyCode.RightArrow))
        {
            h += Time.deltaTime * 100;
        }

        if (v < 0)
        {

            v *= m_backwardRunScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);


    }
    
}
