using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossStateControl : NetworkBehaviour{
    [SyncVar]
    public int state;

	// Use this for initialization
	void Start () {
        state = PEOPLE.FREE;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if(state == PEOPLE.SLOW)
        {
            Debug.Log("SLOW");
        }
        if (state == PEOPLE.END_SKILL)
        {
           
            SKILL basicSkill = this.GetComponent<BossInfoControl>().basicSkill;
           
            //回复技能冷却时间和引导时间
            basicSkill.cd_time_left = basicSkill.cd_time;
            basicSkill.boot_time_left = basicSkill.boot_time;
            
            CmdBeSlowDown();

            return;
        }
        //state为 slow 已经在movecontrol中写了

        if (state == PEOPLE.BLIND)
        {
            Debug.Log("state == PEOPLE.BLIND"+ this.GetComponent<BossInfoControl>().blind_time);
            this.GetComponent<BossInfoControl>().blind_time += Time.deltaTime;
            if (isLocalPlayer)
            {
                this.GetComponent<BossUIControl>().BlindMask(true);
            }

            if (this.GetComponent<BossInfoControl>().blind_time >= 5)
            {
                this.GetComponent<BossInfoControl>().blind_time = 0;
                state = PEOPLE.FREE;
                if (isLocalPlayer)
                {
                    this.GetComponent<BossUIControl>().BlindMask(false);
                }
            }
                return;
        }
        if (state == PEOPLE.ICE)
        {
            Debug.Log("state == PEOPLE.ICE"+ this.GetComponent<BossInfoControl>().ice_time);
            this.GetComponent<BossInfoControl>().ice_time += Time.deltaTime;
            if (this.GetComponent<BossInfoControl>().ice_time >= 2)
            {
                this.GetComponent<BossInfoControl>().ice_time = 0;
                state= PEOPLE.FREE;

            }
            return;
        }
    }

    [Command]
    public void CmdBeSlowDown()
    {
        if(state != PEOPLE.END_SKILL){
            return;
        }
        //如果抓到人
        if (this.GetComponent<BossSkillControl>().BeSlowAfterEndSkill)
        {
            CmdtransStateTo(PEOPLE.SLOW);
            Debug.Log("SLOWING");
            this.GetComponent<BossSkillControl>().BeSlowAfterEndSkill = false;
        }
        else
        {
            CmdtransStateTo(PEOPLE.FREE);
        }
    }

    [Command]
    public void CmdtransStateTo(int targetState)
    {
        state = targetState;
        Debug.Log("BOSS  CmdtanslateStateTo"+ state);
    }
    [ClientRpc]
    public void RpcChangeState(int targetState)
    {
        Debug.Log(this.gameObject.tag+"from"+state+ "RPCChangeState to "+ targetState);
        state = targetState;
       
    }


}
