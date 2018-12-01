using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// State Control 是维护人物状态的脚本

public class BossInfoControl : NetworkBehaviour{
    /* skill 有关的变量，由UIControl和SkillControl共同维护*/
    public SKILL basicSkill;
    public SKILL next_skill_to_begin = null;
    public SKILL current_skill;
    public string basic_skill_name ="";
    int id_count=0;
    public float unlock_page_speed = 5.0f;
    public float blind_time = 0.0f;
    public float ice_time = 0.0f;
    public Vector3 original_book_pos = new Vector3(-8, 0, 4);


    // Use this for initialization
    void Start () {
        basic_skill_name = "SKILL_CATCH";
        basicSkill = new SKILL_CATCH(this.gameObject, id_count++);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (next_skill_to_begin!=null) //UI里已经判断过是否可以使用 这技能是一定可以使用的
        {
            Debug.Log("next_skill_to_begin!=null");
            current_skill = next_skill_to_begin;
            //更新人物状态：EXECUTE_SKILL
            Debug.Log("changeState(PEOPLE.EXECUTE_SKILL)");
            changeState(PEOPLE.EXECUTE_SKILL);
            next_skill_to_begin = null;
        }

        
	}


    public int getState()
    {
      
        return this.GetComponent<BossStateControl>().state;        
    }


    public float getHorizontalMove() {
        return this.GetComponent<BossUIControl>().h;
    }
    public float getVerticalMove()
    {
        return this.GetComponent<BossUIControl>().v;
    }
    public void changeState( int tgtState)
    {
        Debug.Log("change state");
        this.GetComponent<BossStateControl>().CmdtransStateTo(tgtState);
        Debug.Log("change state end");
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(this.transform);
    }

}
