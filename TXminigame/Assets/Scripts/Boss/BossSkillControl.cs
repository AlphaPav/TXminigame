using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BossSkillControl : NetworkBehaviour
{
   
    [SyncVar]
    public bool BeSlowAfterEndSkill = false;

    public float CatchRadius = 2;
	// Use this for initialization
	void Start () {
        CatchRadius = 2;

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        int state = this.GetComponent<BossInfoControl>().getState();
     
        if (state == PEOPLE.EXECUTE_SKILL)
        {
            CmdCatchAttack();
            return;
        }
        //剩余情况下 为基础技能更新冷却时间
        if (this.GetComponent<BossInfoControl>().basicSkill.cd_time_left > 0)
        {
            this.GetComponent<BossInfoControl>().basicSkill.cd_time_left -= this.GetComponent<BossInfoControl>().basicSkill.cd_speed * Time.deltaTime;
        }

	}

    [Command]
    public void CmdCatchAttack()
    {
        if(this.GetComponent<BossInfoControl>().getState() != PEOPLE.EXECUTE_SKILL){
            return;
        }
        Debug.Log(this.gameObject.tag + "Boss Catch Attack");

        GameObject hero1 = GameObject.FindGameObjectWithTag("Hero1");
        GameObject hero2 = GameObject.FindGameObjectWithTag("Hero2");
        GameObject hero3 = GameObject.FindGameObjectWithTag("Hero3");

        if (hero1 != null)
        {
            Debug.Log("Hero 1 exist");
            if (((transform.position - hero1.transform.position).magnitude < CatchRadius) 
                && hero1.GetComponent<StateControl>().state!=PEOPLE.SEALED )
            {
                Debug.Log("Boss Catch Attack hero1");
                //hero1改成封印状态
                hero1.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.CATCHED);
                //Boss 减速
                Debug.Log("Boss Catch Attack end");
                BeSlowAfterEndSkill = true;
            }
        }
        if (hero2 != null)
        {
            if (((transform.position - hero2.transform.position).magnitude < CatchRadius)
                && hero2.GetComponent<StateControl>().state != PEOPLE.SEALED)
            {
                
                hero2.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.CATCHED);
                //Boss 减速
                BeSlowAfterEndSkill = true;
            }
        }
        if (hero3 != null)
        {
            if (((transform.position - hero3.transform.position).magnitude < CatchRadius)
                && hero3.GetComponent<StateControl>().state != PEOPLE.SEALED)
            {
                
                hero3.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.CATCHED);
                //Boss 减速
                BeSlowAfterEndSkill = true;
            }
        }
        Debug.Log("out of the scope of attacking");
        this.GetComponent<BossStateControl>().CmdtransStateTo(PEOPLE.END_SKILL);
        Debug.Log("Boss Catch Attack() done"); 
    }

    


}
