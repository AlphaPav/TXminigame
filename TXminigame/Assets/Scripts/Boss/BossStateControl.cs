using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateControl : MonoBehaviour {
    public int state;

	// Use this for initialization
	void Start () {
        state = PEOPLE.FREE;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == PEOPLE.END_SKILL)
        {
            SKILL current_skill = this.GetComponent<BossInfoControl>().current_skill;
            SKILL basicSkill = this.GetComponent<BossInfoControl>().basicSkill;
            if (current_skill.getSkillID() == basicSkill.getSkillID())
            {
                //回复技能冷却时间和引导时间
                basicSkill.cd_time_left = basicSkill.cd_time;
                basicSkill.boot_time_left = basicSkill.boot_time;
            }
            transStateTo(PEOPLE.FREE);
            current_skill = null;
            return;
        }
        //state为 slow 已经在movecontrol中写了

        if (state == PEOPLE.BLIND)
        {
            Debug.Log("state == PEOPLE.BLIND"+ this.GetComponent<BossInfoControl>().blind_time);
            this.GetComponent<BossInfoControl>().blind_time += Time.deltaTime;
            //致盲蒙版
            this.GetComponent<BossUIControl>().BlindMask(true);
            if (this.GetComponent<BossInfoControl>().blind_time >= 2)
            {
                this.GetComponent<BossInfoControl>().blind_time = 0;
                state = PEOPLE.FREE;
                this.GetComponent<BossUIControl>().BlindMask(false);
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

    public void transStateTo(int targetState)
    {
        state = targetState;
    }

   

}
