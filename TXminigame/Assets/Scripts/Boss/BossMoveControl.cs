using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BossMoveControl : NetworkBehaviour{
    [SerializeField] private float m_moveSpeedStandard = 3;
    [SerializeField] private float m_moveSpeed = 3;
    [SerializeField] private Animator m_animator;
    public GameObject remain;
    float v = 0.0f;
    float h = 0.0f;
    //传送
    private Vector3 m_lastPos = new Vector3(0, 0, 0);
    private float transfer_unlock_lefttime = 3;
    private float transfer_unlock_speed = 1;

  
    private float slow_cd =30;
    private float slow_cd_speed = 2;

    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
    }


    void Update()
    {
        if (!isLocalPlayer) return;

        int tempState = this.GetComponent<BossInfoControl>().getState();
        if (tempState == PEOPLE.ICE || tempState == PEOPLE.EXECUTE_SKILL || tempState == PEOPLE.END_SKILL)
            return;

        if (tempState == PEOPLE.FREE)
        {
            m_moveSpeed = m_moveSpeedStandard;
        }
        else if (tempState == PEOPLE.SLOW) 
        {
            Debug.Log("slow");
            m_moveSpeed = m_moveSpeedStandard * (float)0.15;
            // slow cd 判断
            slow_cd -= slow_cd_speed * Time.deltaTime;

            //恢复正常状态
            if (slow_cd <= 0)
            {
                slow_cd = 30;
                this.GetComponent<BossStateControl>().CmdtransStateTo(PEOPLE.FREE);
                m_moveSpeed = Mathf.Lerp(m_moveSpeedStandard, m_moveSpeed, Time.deltaTime * 1);
            }
        }


        MovementUpdate();

        if (tempState == PEOPLE.BEGIN_SKILL)
        {
            //如果这时候movement改变，则技能被打断
            if (h != 0.0f || v != 0.0f)
            {
                Debug.Log("changeState(PEOPLE.END_SKILL)");
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.END_SKILL);
            }

        }
        //BossAttack()写成 SKILL_CATCH 在SkillBase里；触发在BossUIControl
        //BossAttack();
        
    }

    /*
     * 触碰到特定陷阱或传送点
     */
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer) return;
        Debug.Log("Collision..." + other.gameObject.name);
        switch (other.tag)
        {
            case "IceTrap":
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.ICE);
                other.gameObject.SetActive(false);
                CmdDelete_trap(other.gameObject);//同步trap消失
                break;
            case "BlindTrap":
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.BLIND);
                other.gameObject.SetActive(false);
                CmdDelete_trap(other.gameObject);//同步trap消失
                break;
            case "SlowTrap":
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.SLOW);
                other.gameObject.SetActive(false);
                CmdDelete_trap(other.gameObject);//同步trap消失
                break;
            default:
                break;
        }
        
        

    }

    private void OnTriggerStay (Collider other)
    {
        if (!isLocalPlayer) return;
        //触碰到传送点
        if (other.tag.Equals("TransferPos"))
        {
            TransferUnlock();
        }
    }
    private void MovementUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        m_animator.SetBool("Grounded", true);
        h = this.GetComponent<BossInfoControl>().getHorizontalMove();
        v = this.GetComponent<BossInfoControl>().getVerticalMove();

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
        if (!isLocalPlayer)
        {
            return;
        }

        //行走时 朝向改变
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); //函数参数解释: LookRotation(目标方向为"前方向", 定义声明"上方向")
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, m_moveSpeed * Time.deltaTime);
        this.transform.rotation = newRotation;

    }
    
    /*
     * 传送启动 移动会打断
     */
    public void TransferUnlock()
    {
        if (!isLocalPlayer) return;

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
            transfer_unlock_lefttime = 3;
        }
        m_lastPos = this.transform.position;

    }

    [Command]
    void CmdDelete_trap(GameObject obj)
    {
        Debug.Log("Delete trap");
        RpcDelete_trap(obj);
    }
    [ClientRpc]
    void RpcDelete_trap(GameObject obj)
    {
        Transform pos = obj.transform;
        Vector3 mark_pos = pos.position;
        mark_pos.y -= 0.33f;
        var _henji = (GameObject)Instantiate(remain, mark_pos, pos.rotation);
        NetworkServer.Spawn(_henji);
        obj.SetActive(false);
    }

    
}
