using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BossSkillControl : NetworkBehaviour
{
   
    public bool BeSlowAfterEndSkill = false;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        int state = this.GetComponent<BossInfoControl>().getState();
     
        if (state == PEOPLE.EXECUTE_SKILL)
        {
            SKILL current_skill = this.GetComponent<BossInfoControl>().current_skill;
            Debug.Log("Attack()");
            current_skill.Attack();
            return;
        }
        //剩余情况下 为基础技能更新冷却时间
        if (this.GetComponent<BossInfoControl>().basicSkill.cd_time_left > 0)
        {
            Debug.Log("cd_time_left update");
            this.GetComponent<BossInfoControl>().basicSkill.cd_time_left -= this.GetComponent<BossInfoControl>().basicSkill.cd_speed * Time.deltaTime;
        }

	}

    [Command]
    public void CmdCatchAttack()
    {
        Debug.Log("Boss Catch Attack");
        GameObject hero1 = GameObject.FindGameObjectWithTag("Hero1");
        GameObject hero2 = GameObject.FindGameObjectWithTag("Hero2");
        GameObject hero3 = GameObject.FindGameObjectWithTag("Hero3");

        if (hero1 != null)
        {
            if ((transform.position - hero1.transform.position).magnitude < 5)
            {
                CmdFlatten(hero1);//扁平化
                //hero1改成封印状态
                hero1.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.SEALED);
                //Boss 减速
                BeSlowAfterEndSkill = true;
                return;
            }
        }
        if (hero2 != null)
        {
            if ((transform.position - hero2.transform.position).magnitude < 5)
            {
                //hero2改成封印状态
                CmdFlatten(hero2);
                hero2.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.SEALED);
                //Boss 减速
                BeSlowAfterEndSkill = true;
                return;
            }
        }
        if (hero3 != null)
        {
            if ((transform.position - hero3.transform.position).magnitude < 5)
            {
                //hero3改成封印状态
                CmdFlatten(hero3);
                hero3.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.SEALED);
                //Boss 减速
                BeSlowAfterEndSkill = true;
                return;
            }
        }
        Debug.Log("out of the scope of attacking");
    }

    [Command]
    public void CmdFlatten(GameObject obj)
    {
        Debug.Log("CmdFlatten");
        RpcFlatten(obj);
    }
    [ClientRpc]
    public void RpcFlatten(GameObject obj)//无论写在哪个脚本里都可以 所以只要BossSkillControl写,SkillControl不用再写 
    {
        Vector3 _scale = obj.transform.localScale;
        _scale.z = 0.1f;
        obj.transform.localScale = _scale;
        obj.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        obj.transform.position = this.gameObject.GetComponent<BossInfoControl>().original_book_pos;
    }


}
