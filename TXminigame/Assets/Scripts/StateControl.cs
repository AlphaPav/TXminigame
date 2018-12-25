using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//人物状态的转变与同步必须在服务器上完成。

public class StateControl : NetworkBehaviour {

    [SyncVar]
    public int state;
    private int temp_catch_count;
    // Use this for initialization
    void Start () {
        state = PEOPLE.FREE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer) return;
        if (state == PEOPLE.FREE)
        {
            
            if (this.GetComponent<InfoControl>().blood_num == 0)
            {
                CmdFlatten(this.gameObject);
                
                CmdtransStateTo(PEOPLE.SEALED);
            }
            return;
        } 
        if (state == PEOPLE.END_SKILL)
        {
            Debug.Log("END_SKILL");
            SKILL current_skill = this.GetComponent<InfoControl>().current_skill;
            SKILL basicSkill = this.GetComponent<InfoControl>().basicSkill;
            if (current_skill!=null && current_skill.getSkillID() == basicSkill.getSkillID())
            {
                //回复技能冷却时间和引导时间
                basicSkill.cd_time_left = basicSkill.cd_time;
                basicSkill.boot_time_left = basicSkill.boot_time;
            }
            else
            {
                this.GetComponent<InfoControl>().deletePageSkill(current_skill);
            }
            CmdtransStateTo(PEOPLE.FREE);
            current_skill = null;
            return;
        }

        if (state == PEOPLE.SEALED)
        {
            Debug.Log("PEOPLE.SEALED");
            this.GetComponent<InfoControl>().seal_time += Time.deltaTime;
            if (this.GetComponent<InfoControl>().seal_time >=120)
            {
                Debug.Log("state= die");
                this.GetComponent<InfoControl>().seal_time = 0;
                CmdtransStateTo(PEOPLE.DIE);
            }
            return;
        }
        if (state == PEOPLE.CATCHED)
        {
            this.GetComponent<UIControl>().showMsg("不好，被作者抓住了，灵识数量减一！");
            CmdBeCatched();
        }


        //if (state == PEOPLE.CATCHED && temp_catch_count == 0)
        //{
        //    Debug.Log("PEOPLE.CATCHED");
        //    temp_catch_count++;
        //    int _blood_num = this.GetComponent<InfoControl>().blood_num;
        //    this.GetComponent<InfoControl>().CmdBloodChange(-1);
        //    Debug.Log(this.gameObject.tag + "CmdtransStateTo(PEOPLE.FREE)");
        //    CmdtransStateTo(PEOPLE.FREE);

        //    if (_blood_num ==1) //现在应该是0了
        //    {
        //        Debug.Log(this.gameObject.tag + "CmdtransStateTo(PEOPLE.SEALED)");
        //        CmdFlatten(this.gameObject);
        //        CmdtransStateTo(PEOPLE.SEALED); 
        //    }   
        //}
        //else if(state == PEOPLE.CATCHED)
        //{
        //    Debug.Log("temp catch count: " + temp_catch_count);
        //    CmdJudgeState();
        //}
        
    }
    [Command]
    public void CmdBeCatched()
    {
        if (state != PEOPLE.CATCHED)
        {
            return;
        }
        Debug.Log("PEOPLE.CATCHED");
        this.GetComponent<InfoControl>().CmdBloodChange(-1);
        Debug.Log(this.gameObject.tag + "CmdtransStateTo(PEOPLE.FREE)");
        CmdtransStateTo(PEOPLE.FREE);
    }


    [Command]
    public void CmdJudgeState(){
        if(this.GetComponent<InfoControl>().blood_num == 0){
            CmdtransStateTo(PEOPLE.SEALED); 
        }
        else{
            CmdtransStateTo(PEOPLE.FREE); 
        }
    }

    [Command]
    public void CmdFlatten(GameObject obj)
    {
        Debug.Log("CmdFlatten");
        RpcFlatten(obj);
    }
    [ClientRpc]
    public void RpcFlatten(GameObject obj)
    {
        this.GetComponent<UIControl>().showMsg(obj.tag+"灵识数量等于0，被封印了，等待队友救助！");
        Vector3 _scale = obj.transform.localScale;
        _scale.z = 0.1f;
        obj.transform.localScale = _scale;
        obj.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
      
        obj.transform.position = this.gameObject.GetComponent<InfoControl>().original_book_pos;
    }
    [Command]
    public void CmdtransStateTo(int targetState)
    {
        Debug.Log(this.gameObject.tag+ "from"+state+ "CmdtanslateStateTo" + targetState);
        state = targetState;
        //RpcChangeState(targetState);
    }
    [ClientRpc]
    public void RpcChangeState(int targetState)
    {
        Debug.Log(this.gameObject.tag + "from" + state + "RPCChangeStaet" + targetState);
        state = targetState;
    }




}
