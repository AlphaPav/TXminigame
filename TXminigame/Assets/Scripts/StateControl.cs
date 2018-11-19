using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoBehaviour {
    public int state;

	// Use this for initialization
	void Start () {
        state = PEOPLE.FREE;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (state == PEOPLE.END_SKILL)
        {
            SKILL current_skill = this.GetComponent<InfoControl>().current_skill;
            SKILL basicSkill = this.GetComponent<InfoControl>().basicSkill;
            if (current_skill.getSkillID() == basicSkill.getSkillID())
            {
                //回复技能冷却时间和引导时间
                basicSkill.cd_time_left = basicSkill.cd_time;
                basicSkill.boot_time_left = basicSkill.boot_time;
            }
            else
            {
                this.GetComponent<InfoControl>().deletePageSkill(current_skill);
            }
            transStateTo(PEOPLE.FREE);
            current_skill = null;
            return;
        }

        if (state == PEOPLE.SEALED)
        {
            this.GetComponent<InfoControl>().seal_time += Time.deltaTime;
            if (this.GetComponent<InfoControl>().seal_time >=120)
            {
                Debug.Log("state= die");
                this.GetComponent<InfoControl>().seal_time = 0;
                state = PEOPLE.DIE;
            }
            return;
        }

    }

    public void transStateTo(int targetState)
    {
        state = targetState;
    }

   

}
