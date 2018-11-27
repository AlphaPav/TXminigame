using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillControl : NetworkBehaviour {
    
	// Use this for initialization
	void Start () {
		
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
    public void CmdSetTramp(Vector3 targetPos)
    {
        Debug.Log("Here@");
        UnityEngine.Object trapPreb = Resources.Load("Prefabs/slow_trap", typeof(GameObject));
        var my_trap = (GameObject)Instantiate(trapPreb, targetPos, Quaternion.identity);
        NetworkServer.Spawn(my_trap);
    }
   

}
