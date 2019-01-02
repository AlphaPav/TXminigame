using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillControl : NetworkBehaviour {
    private float ReviveRadius=4;
	// Use this for initialization
	void Start () {
        ReviveRadius = 4;

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

        int state = this.GetComponent<InfoControl>().getState();
        if (state == PEOPLE.BEGIN_SKILL)
        {
            SKILL current_skill = this.GetComponent<InfoControl>().current_skill;
            Debug.Log("current_skill.Boot();");
            current_skill.Boot();
            return;
        }
        else if (state == PEOPLE.EXECUTE_SKILL)
        {
            SKILL current_skill = this.GetComponent<InfoControl>().current_skill;
            Debug.Log("Attack()");
            current_skill.Attack();
            Debug.Log(state);
            return;
        }
        //剩余情况下 为基础技能更新冷却时间
        if (this.GetComponent<InfoControl>().basicSkill.cd_time_left > 0)
        {
           // Debug.Log("basicSkill.cd_time_left: "+ this.GetComponent<InfoControl>().basicSkill.cd_time_left);
            this.GetComponent<InfoControl>().basicSkill.cd_time_left -= this.GetComponent<InfoControl>().basicSkill.cd_speed * Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
       
    }
    private void OnTriggerStay(Collider other)
    { 
    }
    private void OnTriggerExit(Collider other)
    {
     

    }

    [Command]
    public void CmdSetTrap(Vector3 targetPos)
    {
        Debug.Log("Here set trap");
        UnityEngine.Object trapPreb = Resources.Load("Prefabs/slow_trap", typeof(GameObject)); ;
        if (this.gameObject.tag.Equals("Hero1"))
        {
            trapPreb = Resources.Load("Prefabs/slow_trap", typeof(GameObject));
        }
        else if (this.gameObject.tag.Equals("Hero2"))
        {
            trapPreb = Resources.Load("Prefabs/ice_trap", typeof(GameObject));
        }
        else if (this.gameObject.tag.Equals("Hero3"))
        {
            trapPreb = Resources.Load("Prefabs/blind_trap", typeof(GameObject));
        }
        var my_trap = (GameObject)Instantiate(trapPreb, targetPos, Quaternion.identity);
        NetworkServer.Spawn(my_trap);
    }
    [Command]
    public void CmdRevive()
    {
        GameObject hero1 = GameObject.FindGameObjectWithTag("Hero1");
        GameObject hero2 = GameObject.FindGameObjectWithTag("Hero2");
        GameObject hero3 = GameObject.FindGameObjectWithTag("Hero3");
        if (hero1 != null&& (!hero1.tag.Equals(this.gameObject.tag)) 
            && hero1.GetComponent<StateControl>().state== PEOPLE.SEALED)//被封印
        {
            if ((transform.position - hero1.transform.position).magnitude < ReviveRadius)//合理半径内
            {
                //hero1改成自由状态，血量+1
                CmdUnflatten(hero1);
                hero1.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.FREE);
                hero1.GetComponent<InfoControl>().CmdBloodChange(1);
                return;
            }
        }
        if (hero2 != null && (!hero2.tag.Equals(this.gameObject.tag))
           && hero2.GetComponent<StateControl>().state == PEOPLE.SEALED)//被封印
        {
            if ((transform.position - hero2.transform.position).magnitude < ReviveRadius)//合理半径内
            {
                //hero2改成自由状态，血量+1
                CmdUnflatten(hero2);
                hero2.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.FREE);
                hero2.GetComponent<InfoControl>().CmdBloodChange(1);
                return;
            }
        }

        if (hero3 != null && (!hero3.tag.Equals(this.gameObject.tag))
            && hero3.GetComponent<StateControl>().state == PEOPLE.SEALED)//被封印
        {
            if ((transform.position - hero3.transform.position).magnitude < ReviveRadius)//合理半径内
            {
                //hero3改成自由状态，血量+1
                CmdUnflatten(hero3);
                hero3.GetComponent<StateControl>().CmdtransStateTo(PEOPLE.FREE);
                hero3.GetComponent<InfoControl>().CmdBloodChange(1);
                return;
            }
        }
    }
    [Command]
    public void CmdUnflatten(GameObject obj)
    {
        Debug.Log("CmdUnflatten");
        RpcUnflatten(obj);
    }
    [ClientRpc]
    public void RpcUnflatten(GameObject obj)
    {
        obj.transform.localScale = new Vector3(7f, 7f, 7f); //TODO: 根据之后的模型要改
        obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

}
