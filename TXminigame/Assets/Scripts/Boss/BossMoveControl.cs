using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMoveControl : MonoBehaviour {
    [SerializeField] private float m_moveSpeedStandard = 5;
    [SerializeField] private float m_moveSpeed = 5;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] public GameObject TransButtton;

    private float m_currentV = 0;
    private float m_currentH = 0;

    //传送
    private Vector3 m_lastPos = new Vector3(0, 0, 0);
    private float transfer_unlock_lefttime = 10;
    private float transfer_unlock_speed = 1;

    //攻击
    private float slow_cd = 10;
    private float slow_cd_speed = 2;

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
        //BossAttack();
        //OnTrap();
    }

    /*
     * 触碰到特定陷阱或传送点
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision..." + other.gameObject.name);
    }

    private void OnTriggerStay (Collider other)
    {
        //触碰到陷阱
        if (other.tag.Contains("Trap"))
        {
            OnTrap();
        }
        //触碰到传送点
        if (other.tag.Equals("TransferPos"))
        {
            TransferUnlock();
        }
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

        ////判断boss 状态
        //if(this.GetComponent<StateControl>().state == PEOPLE.SLOW)
        //{
        //    m_moveSpeed = 0.25f * m_moveSpeedStandard;
        //    // slow cd 判断
        //    slow_cd -= slow_cd_speed * Time.deltaTime;
        //}
        ////恢复正常状态
        //if (slow_cd <= 0)
        //{
        //    slow_cd = 10;
        //    this.GetComponent<StateControl>().transStateTo(PEOPLE.FREE);
        //    m_moveSpeed = Mathf.Lerp(m_moveSpeedStandard, m_moveSpeed, Time.deltaTime * 1);
        //}

    }

    /*
     * boss 落入陷阱 
     */
    private void OnTrap()
    {
        Debug.Log("Trapped");   //boss产生具体状态改变 有待改写
    }

    /*
     * 传送启动 移动会打断
     */
    public void TransferUnlock()
    {
        if (m_lastPos == this.transform.position)
        {
            transfer_unlock_lefttime -= transfer_unlock_speed * Time.deltaTime;
            Debug.Log(transfer_unlock_lefttime);
        }
        else
        {
            Debug.Log("传送打断");
        }


        if (transfer_unlock_lefttime <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            transfer_unlock_lefttime = 10;
        }
        m_lastPos = this.transform.position;

    }

    private bool OnTrsPos()
    {
        GameObject trsobj = GameObject.Find("TransferPos");
        if ((this.transform.position - trsobj.transform.position).magnitude < 1)
            return true;
        return false;
    }

    /*
     * boss点击人物 抓会书本
     */
    private void BossAttack()
    {
        //创建射线;从摄像机发射一条经过鼠标当前位置的射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //发射射线
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo))
        {
            //获取碰撞点的物体
            GameObject hitObj = hitInfo.collider.gameObject;
            if (hitObj.name.Equals("hero") && Input.GetMouseButton(0)) 
            {
                Debug.Log("Attacking " + hitObj.name);
                
                // 判断攻击距离
                if ((this.transform.position - hitObj.transform.position).magnitude < 5)
                {
                    //被攻击者，大小、位置变化
                    hitObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    hitObj.transform.position = new Vector3(1, 1, 1);
                    //人物冰冻
                    hitObj.GetComponent<StateControl>().transStateTo(PEOPLE.ICE);
                    //Boss 减速
                    this.GetComponent<StateControl>().transStateTo(PEOPLE.SLOW);
                }
                else
                {
                    Debug.Log("out of the scope of attacking");
                }

            }
        }
    }
}
