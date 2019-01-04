using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BossMoveControl : NetworkBehaviour{
    [SerializeField] private float m_moveSpeedStandard = 1f;
    [SerializeField] private float m_moveSpeed = 1f;
    //[SerializeField] private Animator m_animator;
    bool isNowRun = false;
    bool isLastRun = false;
 
    

    public GameObject remain;
    float v = 0.0f;
    float h = 0.0f;
    //传送
    private Vector3 m_lastPos = new Vector3(0, 0, 0);
    private float transfer_unlock_lefttime = 3;
    private float transfer1_unlock_lefttime = 3;
    private float transfer_unlock_speed = 1;
    public Vector3 transfer_pos;
    public Vector3 transfer1_pos ;

    private float slow_cd =45;
    private float slow_cd_speed = 4;

    private void Start()
    {
        transfer1_pos = new Vector3(-14.277f, 0f, -10.8f);
        transfer_pos = new Vector3(6f, 0f, 2.6f);
        //  m_animator = this.GetComponent<Animator>();
       
        isNowRun = false;
        isLastRun = false;
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
            //Debug.Log("slow");
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
                this.GetComponent<BossUIControl>().showMsg("技能被打断");
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
                this.GetComponent<BossUIControl>().showMsg("踩到冰冻陷阱");
                other.gameObject.SetActive(false);
                CmdDelete_trap(other.gameObject);//同步trap消失
                break;
            case "BlindTrap":
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.BLIND);
                this.GetComponent<BossUIControl>().showMsg("踩到致盲陷阱");
                other.gameObject.SetActive(false);
                CmdDelete_trap(other.gameObject);//同步trap消失
                break;
            case "SlowTrap":
                this.GetComponent<BossInfoControl>().changeState(PEOPLE.SLOW);
                this.GetComponent<BossUIControl>().showMsg("踩到减速陷阱");
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
            m_lastPos = this.transform.position;
            TransferUnlock(other.gameObject);
        }else if (other.tag.Equals("TransferPos1"))
        {
            m_lastPos = this.transform.position;
            Transfer1Unlock(other.gameObject);
        }
    }
    private void MovementUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        h = this.GetComponent<BossInfoControl>().getHorizontalMove();
        v = this.GetComponent<BossInfoControl>().getVerticalMove();

        float dis = (new Vector3(h, 0, v)).magnitude;
        if (dis > 0)
        {
            isNowRun = true;
        }
        else
        {
            isNowRun = false;
        }
        if (isNowRun != isLastRun)
        {
            if(isNowRun) CmdPlayAnimation(this.gameObject, "run");
            else CmdPlayAnimation(this.gameObject, "idle");
        }
        isLastRun = isNowRun;
  

        transform.position = this.transform.position + new Vector3(h, 0, v) * m_moveSpeed * Time.deltaTime * 1.5f;
        Debug.Log((new Vector3(h, 0, v) * m_moveSpeed * Time.deltaTime * 10).magnitude);
        if (h != 0 || v != 0)
        {
            Rotating(h, v);
        }
      
       
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
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, m_moveSpeed * Time.deltaTime * 4);
        this.transform.rotation = newRotation;

    }
    
    /*
     * 传送启动 移动会打断
     */
    public void TransferUnlock(GameObject transObj)
    {
        if (!isLocalPlayer) return;
        GameObject light = transObj.transform.Find("SpotLight").gameObject;
        if (m_lastPos == this.transform.position)
        {
            transfer_unlock_lefttime -= transfer_unlock_speed * Time.deltaTime;
            //传送点附近添加动画
           
            light.SetActive(true);

            Debug.Log(transfer_unlock_lefttime);
            this.GetComponent<BossUIControl>().showMsg("正在传送中");
        }
        else
        {
            this.GetComponent<BossUIControl>().showMsg("传送中断");
            light.SetActive(false);
            Debug.Log("传送打断");
        }


        if (transfer_unlock_lefttime <= 0)
        {
            light.SetActive(false);
            this.transform.position = new Vector3(transfer1_pos[0], transform.position.y, transfer1_pos[2]);
            Debug.Log(transform.position);
            transfer_unlock_lefttime = 3;    
        }
        m_lastPos = this.transform.position;

    }
    public void Transfer1Unlock(GameObject transObj)
    {
        if (!isLocalPlayer) return;
        GameObject light = transObj.transform.Find("SpotLight").gameObject;
        if (m_lastPos == this.transform.position)
        {
            transfer1_unlock_lefttime -= transfer_unlock_speed * Time.deltaTime;
            //传送点附近添加动画
            light.SetActive(true);
            this.GetComponent<BossUIControl>().showMsg("正在传送中");
            Debug.Log(transfer1_unlock_lefttime);
        }
        else
        {
            this.GetComponent<BossUIControl>().showMsg("传送中断");
            light.SetActive(false);
            Debug.Log("传送打断");
        }

        if (transfer1_unlock_lefttime <= 0)
        {
            this.transform.position = new Vector3(transfer_pos[0], transform.position.y, transfer_pos[2]);
            Debug.Log(transform.position);
            transfer1_unlock_lefttime = 3;
            light.SetActive(false);
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
        mark_pos.y -= 0.1f;
        var _henji = (GameObject)Instantiate(remain, mark_pos, pos.rotation);
        NetworkServer.Spawn(_henji);
        obj.SetActive(false);
    }
    [Command]
    void CmdPlayAnimation(GameObject obj, string _aniName)
    {
        Debug.Log(obj.tag + " CmdPlayAnimation: "+ _aniName);
         RpcPlayAnimation(obj, _aniName);
        //m_animation.Play(_aniName);
    }
    [ClientRpc]
    void RpcPlayAnimation(GameObject obj, string _aniName)
    {
        Debug.Log(obj.tag + " RpcPlayAnimation: " +_aniName);
        Animation _ani;
        _ani = obj.GetComponent<Animation>();

        _ani.Play(_aniName);
    }
}
