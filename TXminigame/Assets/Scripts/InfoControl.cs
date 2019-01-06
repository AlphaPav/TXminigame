using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// State Control 是维护人物状态的脚本

public class InfoControl : NetworkBehaviour
{
    /* skill 有关的变量，由UIControl和SkillControl共同维护*/
    public SKILL basicSkill;
    public List<SKILL> pageSkills = new List<SKILL>();
   
    public SKILL next_skill_to_begin = null;//UI传递过来，在update判断是否合法
    public SKILL current_skill;
    public string basic_skill_name = "";
    int id_count=0;
    public float unlock_page_speed =3.0f;
    public float seal_time = 0;//已经被封印的时间

    public Vector3 original_book_pos = new Vector3(-1.66f, 3, -12.85f);
    [SyncVar]
    public int blood_num = 2;
    public List<SkinnedMeshRenderer> myMeshRenderer = new List<SkinnedMeshRenderer>();

    // Use this for initialization
    void Start () {
        unlock_page_speed = 3.0f;
        original_book_pos = new Vector3(-1.6f, 2, -12.85f);
        if (!isLocalPlayer) return;

        GameObject model = this.transform.Find("model").gameObject;
        if (model == null) Debug.Log("NULL");
        foreach (Transform child in model.transform)
        {
            GameObject childObj = child.gameObject;
            myMeshRenderer.Add(childObj.GetComponent<SkinnedMeshRenderer>());
        }


        if (this.gameObject.tag.Equals("Hero1"))
        {
            basic_skill_name = "SKILL_TRAP_SLOW";
            Debug.Log("tag.Equals Hero1");
            basicSkill = new SKILL_TRAP_SLOW(this.gameObject, id_count++);
           
        }
        else if (this.gameObject.tag.Equals("Hero2"))
        {
            basic_skill_name = "SKILL_TRAP_ICE";
            Debug.Log("tag.Equals Hero2");
            basicSkill = new SKILL_TRAP_ICE(this.gameObject, id_count++);
        
        }
        else if (this.gameObject.tag.Equals("Hero3"))
        {
            basic_skill_name = "SKILL_TRAP_BLIND";
            Debug.Log("tag.Equals Hero3");
            basicSkill = new SKILL_TRAP_BLIND(this.gameObject, id_count++);
        
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer) return;
        
        if (next_skill_to_begin!=null) //UI里已经判断过是否可以使用 这技能是一定可以使用的
        {
            Debug.Log("next_skill_to_begin!=null");
            current_skill = next_skill_to_begin;
            //更新人物状态：BEGIN_SKILL
            Debug.Log("changeState(PEOPLE.BEGIN_SKILL)");
            changeState(PEOPLE.BEGIN_SKILL);
            next_skill_to_begin = null;
            Debug.Log("END");
        }

      
    }




    public void addPageSkill(string _page_name)
    {
        if (pageSkills.Count >= 4) return;
        SKILL _skill=null;
        switch (_page_name)
        {
            case "GoldenPage":
                _skill = new SKILL_FLASH(this.gameObject, id_count++);
                break;
            case "WhitePage":
                _skill = new SKILL_REVIVE(this.gameObject, id_count++);
                break;
            case "BlackPage":
                _skill = new SKILL_HIDE(this.gameObject, id_count++);
                break;
            default :
                Debug.Log("Error! Invalid _page_name!");
                break;
        }
        if (_skill != null)
        {
            pageSkills.Add(_skill);
            CmdBloodChange(1);

        }

        //print for test 
        outputPageSkillsForTest();
    }
    public void deletePageSkill(SKILL _skill)
    {
        for (int i = 0; i < pageSkills.Count; i++)
        {
            if (pageSkills[i].getSkillID() == _skill.getSkillID())
            {
                pageSkills.RemoveAt(i);//从列表里删除，使用的情况: 1.释放完全结束 2. 引导时被中断  
                //还要写一下内存释放？
                CmdBloodChange(-1);

            }
        }

        //print for test 
        outputPageSkillsForTest();
    }
    public void outputPageSkillsForTest()
    {
        print("temp Page Skill num:" + pageSkills.Count);
        for (int i = 0; i < pageSkills.Count; i++)
        {
            Debug.Log(pageSkills[i].getSkillName());
        }
    }

    public int getState()
    {
        // State Control 是维护人物状态的脚本
        return this.GetComponent<StateControl>().state;        
    }


    public float getHorizontalMove() {
        return this.GetComponent<UIControl>().h;
    }
    public float getVerticalMove()
    {
        return this.GetComponent<UIControl>().v;
    }
    public void changeState( int tgtState)
    {
        Debug.Log("change state TO "+ tgtState);
        this.GetComponent<StateControl>().CmdtransStateTo(tgtState);
       
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(this.transform);
    }

    [Command]
    public void CmdBloodChange(int _change_num)
    {
        Debug.Log("onBloodNum"+_change_num);
        blood_num += _change_num;

    }
    

}
