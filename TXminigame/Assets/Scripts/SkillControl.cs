using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
   

}
