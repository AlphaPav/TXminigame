using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
    public  float v = 0.0f;/*vertical*/
    public float h = 0.0f;/*horizontal*/
    public GameObject highlightPosCube;
    Vector3 Pos = new Vector3(0,0,0);
    Vector3 Dir= new Vector3(0, 0, 0);
    // Use this for initialization

    public bool transparent = false;

    void Start () {
        highlightPosCube.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        ShowSkill();
        if (this.GetComponent<InfoControl>().getState() == PEOPLE.FREE)
        {
            SkillUpdate();
        }
        MovementUpdate();
        HandleClick();
    }
    void HandleClick()
    {
        if (Input.GetMouseButton(0))//鼠标左键
        {
            Debug.Log("MouseDown");
            //创建射线;从摄像机发射一条经过鼠标当前位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //发射射线
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                //获取碰撞点的位置
                if (hitInfo.collider.tag.Equals("GoldenPage")|| hitInfo.collider.tag.Equals("BlackPage") || hitInfo.collider.tag.Equals("WhitePage"))
                {
                    Debug.Log("unlock_time_left:"+ hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left);
                    hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left -= this.GetComponent<InfoControl>().unlock_page_speed * Time.deltaTime;                   
            
                    if (hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left <= 0)
                    {
                        hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left = 0;
                        acquirePageSkill(hitInfo.collider);
                    }
                }
            }
        }
    }


    Vector3 GetSkillTgtPos()
    {
        Vector3 targetPos = Pos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //发射射线
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag.Equals("Walkable"))
            {
                targetPos = hitInfo.point;
            }
        }
        
        return targetPos;
    }


 
    void ShowSkill()
    { 
        //this.GetComponent<InfoControl>().basicSkill的冷却时间如果不为零，显示剩余冷却时间，技能图标颜色为灰色
        //按顺序展示 this.GetComponent<InfoControl>().pageSkills的内容


    }
    



    void SkillUpdate()
    {
       
        // UI 接收基本技能的位置，这里匹配到键盘按键数字0
        if (this.GetComponent<InfoControl>().basicSkill.cd_time_left <= 0)// 判断基本技能的冷却时间
        {
            
            SKILL basicSkill = this.GetComponent<InfoControl>().basicSkill;
            if (Input.GetKey(KeyCode.Alpha0))//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey(KeyCode.Alpha0)");
                HandleGetKeyEvent(basicSkill);
                return;
            }
            if (Input.GetKeyUp(KeyCode.Alpha0))//如果用户放开了按键,表示触发？ 
            {
                Debug.Log("Input.GetKeyUp(KeyCode.Alpha0)");
                HandleGetKeyUpEvent(basicSkill);
                return;
            }
        }

        // UI上 第一个纸张技能的位置，这里匹配到键盘按键数字1
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 1)  //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[0];
            if (Input.GetKey(KeyCode.Alpha1))//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey(KeyCode.Alpha1)");
                HandleGetKeyEvent(_skill);
                return;
            }
            if (Input.GetKeyUp(KeyCode.Alpha1))//如果用户放开了按键,表示触发？ 
            {
                Debug.Log("Input.GetKeyUp(KeyCode.Alpha1)");
                HandleGetKeyUpEvent(_skill);
                return;
            }
        }


        // UI上 第二个纸张技能的位置 ，这里匹配到键盘按键数字2
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 2) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[1];
            if (Input.GetKey(KeyCode.Alpha2))//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey(KeyCode.Alpha2)");
                HandleGetKeyEvent(_skill);
                return;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))//如果用户放开了按键,表示触发？ 
            {
                Debug.Log("Input.GetKeyUp(KeyCode.Alpha2)");
                HandleGetKeyUpEvent(_skill);
                return;
            }
        }

        // UI上 第三个纸张技能的位置 ，这里匹配到键盘按键数字3
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 3) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[2];
            if (Input.GetKey(KeyCode.Alpha3))//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey(KeyCode.Alpha3)");
                HandleGetKeyEvent(_skill);
                return;
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))//如果用户放开了按键,表示触发？ 
            {
                Debug.Log("Input.GetKeyUp(KeyCode.Alpha3)");
                HandleGetKeyUpEvent(_skill);
                return;
            }
        }
        // UI上 第四个纸张技能的位置 ，这里匹配到键盘按键数字4
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 4) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[3];
            if (Input.GetKey(KeyCode.Alpha4))//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey(KeyCode.Alpha4)");
                HandleGetKeyEvent(_skill);
                return;
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))//如果用户放开了按键,表示触发？ 
            {
                Debug.Log("Input.GetKeyUp(KeyCode.Alpha4)");
                HandleGetKeyUpEvent(_skill);
                return;
            }
        }
    }
    void HandleGetKeyEvent(SKILL _skill)
    {
        Debug.Log(_skill.getSkillName());
        if (_skill.need_dir)
        {
            //从UI实时接收方向信息
            //UI显示方向
            Dir = new Vector3(1.0f, 0.0f, 1.0f);// 应该用户输入，方向y轴分量应该是0 
        }
        if (_skill.need_pos)
        {
            //从UI实时接收位置信息
            Pos = GetSkillTgtPos();
            if (_skill.IsPosValid(Pos))
            {
                Debug.Log("PosValid");
                Debug.Log(Pos);
                //UI显示位置
                highlightPosCube.SetActive(true);
                highlightPosCube.transform.position = Pos;
            }
        }

    }
    void HandleGetKeyUpEvent(SKILL _skill)
    {
        if (_skill.need_dir)
        {
            _skill.SetDirection(Dir);
        }
        if (_skill.need_pos)
        {
            if (_skill.IsPosValid(Pos))
            {
                Debug.Log("PosValid");
                Debug.Log(Pos);
                _skill.SetTargetPos(Pos);
            }
            else
            {
                Debug.Log("PosInValid");
                Debug.Log(Pos);
                //位置不在距离范围内，做异常处理 放在主角所在位置？  纸张还是要被使用
                _skill.SetTargetPos(this.transform.position);
            }
            Debug.Log("Already SetTargetPos:");
            Debug.Log(Pos);
            //UI显示位置
            highlightPosCube.SetActive(true);
            highlightPosCube.transform.position = Pos;
        }
        this.GetComponent<InfoControl>().next_skill_to_begin = _skill;
        highlightPosCube.SetActive(false);
    }

    void MovementUpdate()
    {
        v = 0.0f;
        h = 0.0f;
        bool move = false;
        //w键前进  
        if (Input.GetKey(KeyCode.W))
        {
            v += Time.deltaTime * 100;
            move = true;
        }
        //s键后退  
        if (Input.GetKey(KeyCode.S))
        {
            v -= Time.deltaTime * 100;
            move = true;
        }
        //a键左
        if (Input.GetKey(KeyCode.A))
        {
            h -= Time.deltaTime * 100;
            move = true;
        }
        //d键右
        if (Input.GetKey(KeyCode.D))
        {
            h += Time.deltaTime * 100;
            move = true;
        }
        if (move && transparent==true)
        {
            GameObject model = GameObject.Find("model");
            Material[] _material = model.GetComponent<SkinnedMeshRenderer>().materials;
            Color temp1 = _material[0].color;
            Color temp2 = _material[1].color;
            _material[0].SetColor("_Color", new Color(temp1[0], temp1[1], temp1[2], 1.0f));
            _material[1].SetColor("_Color", new Color(temp2[0], temp2[1], temp2[2], 1.0f));
            transparent = false;
        }
    }

    private void acquirePageSkill(Collider other)
    {
        if (other.tag.Equals("GoldenPage"))
        {
            Debug.Log("acquireGoldenPageSkill");
            this.GetComponent<InfoControl>().addPageSkill("GoldenPage");
        }
        else if (other.tag.Equals("BlackPage"))
        {
            Debug.Log("acquireBlackPageSkill");
            this.GetComponent<InfoControl>().addPageSkill("BlackPage");
        }
        else if (other.tag.Equals("WhitePage"))
        {
            Debug.Log("acquireWhitePageSkill");
            this.GetComponent<InfoControl>().addPageSkill("WhitePage");
        }

        //从游戏场景中消失，或者这里先播放一个纸张消失的动画再消失
        other.gameObject.SetActive(false);
      
    }


}
