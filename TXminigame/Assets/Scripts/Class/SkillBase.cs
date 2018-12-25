using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*SKILL 基类
 * 关键要素：冷却时间、技能引导时间、技能释放对象、技能效果
 * 默认参数：无冷却时间、无引导时间、无引导动画（但是有构造动画的构造函数）
*/


public class SKILL
{
    public float boot_time = 2;        // 技能引导时间（引导可以是念咒之类的）
    public float cd_time = 0;          // 技能冷却时间，单位 Seconds
    public bool is_page_skill = false;  //是否为一次性的纸张技能
    public string page_name = "";
    public string skill_name = "";
    private string boot_animotion_name = null;
    private int id;

    public string active_icon_path = "";//UI中 技能被选中时的icon
    public string usable_icon_path = ""; //UI中 技能可以使用时的icon
    public string unusable_icon_path = ""; //UI中 技能不可以使用，被冷却时的icon


    public float boot_time_left = 0;    // 技能引导剩余时间
    public float cd_time_left = 0;
    public float boot_speed = 5.0f;
    public float cd_speed = 5.0f;
    public Animator m_animator;

    private DateTime last_skill_use_time;

    /*技能的参数*/
    protected GameObject skill_owner = null;
    protected Vector3 direction;
    protected Vector3 targetPos;
    public bool need_dir = false;
    public bool need_pos = false;
    public float pos_radius = 10f;


    /*构造函数*/

    public SKILL(GameObject _skill_owner, int _id)
    {
        skill_owner = _skill_owner;
        boot_time_left = boot_time;
        id = _id;
        m_animator = skill_owner.GetComponent<Animator>();
    }
    public int getSkillID()
    {
        return id;
    }
    public string getSkillName()
    {
        return skill_name;
    }

    //不用重载
    public void Boot()
    {
        Debug.Log("In boot");
        if (skill_owner.GetComponent<InfoControl>().getState() == PEOPLE.BEGIN_SKILL)  /*技能引导阶段*/
        {
            if (boot_time_left > 0)
            {
                boot_time_left -= boot_speed * Time.deltaTime;
                //更改一下动画，这里随便用了一个动画
                m_animator.SetBool("Grounded", false);
                return;
            }
            else {
                //更改一下动画，这里随便用了一个动画
                m_animator.SetBool("Grounded", true);
                /*进入执行技能阶段*/
                skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.EXECUTE_SKILL);
                /*恢复引导时间*/
                boot_time_left = boot_time;
            }
        }

    }

    // 重点重载函数
    virtual public void Attack()
    {
        return;
    }

    //设置技能参数

    public bool NeedDirection()
    {
        return need_dir;
    }
    public bool NeedTargetPos()
    {
        return need_pos;
    }
    public bool IsPosValid(Vector3 _targetPos)
    {
        _targetPos.y = skill_owner.transform.position.y;
        if ((_targetPos - skill_owner.transform.position).magnitude <= pos_radius) return true;
        else return false;
    }
    public void SetDirection(Vector3 _direction)
    {
        direction = _direction;
    }
    public void SetTargetPos(Vector3 _targetPos)
    {
        targetPos = _targetPos;
    }
}



/*闪现：金色纸张技能*/
//只需要同步人物的位置即可
public class SKILL_FLASH : SKILL
{

    private float distance = 3.0f;
    public SKILL_FLASH(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        need_dir = true; //需要方向
        is_page_skill = true;  //是否为一次性的纸张技能
        page_name = "GoldenPage";
        skill_name = "SKILL_FLASH";
    }
    // 重载

    override public void Attack()
    {
        skill_owner.transform.Translate(direction * distance);
        skill_owner.GetComponent<StateControl>().state = PEOPLE.END_SKILL;
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        Debug.Log("Flash on!!!!!!!");
        return;
    }
}

/*隐身：黑色纸张技能*/
public class SKILL_HIDE : SKILL
{
    public SKILL_HIDE(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
         is_page_skill = true;
         need_dir = false;
         page_name = "BlackPage";
         skill_name = "SKILL_HIDE";
    }

    public override void Attack()
    {
        skill_owner.GetComponent<UIControl>().CmdBecomeTransparent();
        skill_owner.GetComponent<StateControl>().state = PEOPLE.END_SKILL;
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        return;
    }
}
/*复活：白色纸张技能*/

public class SKILL_REVIVE : SKILL
{
   
    public SKILL_REVIVE(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        is_page_skill = true; 
        page_name = "WhitePage";
        skill_name = "SKILL_REVIVE";
    }
    override public void Attack()
    {
        skill_owner.GetComponent<SkillControl>().CmdRevive();
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        return;
    }

}


/*变慢的陷阱：基本技能*/
public class SKILL_TRAP_SLOW : SKILL
{
    public SKILL_TRAP_SLOW(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        cd_time = 30;   // 5min
        need_pos = true; //需要位置
        skill_name = "SKILL_TRAP_SLOW";
    }
    // 重载
    override public void Attack()
    {
        //Debug.Log(" SKILL_TRAP_SLOW Attack()");
        skill_owner.GetComponent<SkillControl>().CmdSetTrap(targetPos);
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        return;
    }
}

/*冻住的陷阱：基本技能*/
public class SKILL_TRAP_ICE : SKILL
{
    public SKILL_TRAP_ICE(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        cd_time = 30;   // 5min
        need_pos = true; //需要位置
        skill_name = "SKILL_TRAP_ICE";
    }
    // 重载
    override public void Attack()
    {
        //Debug.Log(" SKILL_TRAP_ICE Attack()");
        skill_owner.GetComponent<SkillControl>().CmdSetTrap(targetPos);
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        return;
    }
}

/*变漆黑的陷阱：基本技能*/
public class SKILL_TRAP_BLIND : SKILL
{
    public SKILL_TRAP_BLIND(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        cd_time = 30;   // 30 second
        need_pos = true; //需要位置
        skill_name = "SKILL_TRAP_BLIND";
    }
    // 重载
    override public void Attack()
    {
        skill_owner.GetComponent<SkillControl>().CmdSetTrap(targetPos);
        skill_owner.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
        return;
    }
}

/*Boss抓人：基本技能*/
public class SKILL_CATCH: SKILL
{
    public SKILL_CATCH(GameObject _skill_owner, int _id) : base(_skill_owner, _id)
    {
        cd_time = 30;   // 30second
        boot_time = 0;
        skill_name = "SKILL_CATCH";
        need_pos = false;
        need_dir = false;
    }
    // 重载
    override public void Attack()
    {   //目前是按照hero1 ,hero2, hero3的顺序来判断位置  只抓其中一个
        //Debug.Log(" SKILL_CATCH Attack()");
        //skill_owner.GetComponent<BossSkillControl>().CmdCatchAttack();
        //skill_owner.GetComponent<BossStateControl>().CmdtransStateTo(PEOPLE.END_SKILL);
        return;
    }
}



public class SkillBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
