using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillControl : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
            // Debug.Log("basicSkill.cd_time_left: "+ this.GetComponent<BossInfoControl>().basicSkill.cd_speed * Time.deltaTime;);
            this.GetComponent<BossInfoControl>().basicSkill.cd_time_left -= this.GetComponent<BossInfoControl>().basicSkill.cd_speed * Time.deltaTime;
        }



	}

    

}
