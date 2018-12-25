using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

//UIcontrol 中一切设备输入都必须限制在local player上才能执行，但是ui界面的同步可能会有bug？

public class UIControl : NetworkBehaviour {
    public  float v = 0.0f;/*vertical*/
    public float h = 0.0f;/*horizontal*/
    public GameObject highlightPosCube;
    Vector3 Pos = new Vector3(0,0,0);
    Vector3 Dir= new Vector3(0, 0, 0);
    
     //屏幕红色蒙版变量
    public Image warnImage;
    private float flashSpeed = 5;
    [SyncVar]
    public bool transparent = false;

    [SyncVar]
    public bool GETING_SKILL = false;
    private MoveJoystick moveJoystick;
    
    private GameObject baseSkillUI;
    private GameObject skill1UI;
    private GameObject skill2UI;
    private GameObject skill3UI;
    private GameObject skill4UI;
  
    Sprite sp_trap_blind;
    Sprite sp_trap_blind_colding;
    Sprite sp_trap_slow;
    Sprite sp_trap_slow_colding;
    Sprite sp_trap_ice;
    Sprite sp_trap_ice_colding;
    Sprite sp_flash;
    Sprite sp_hide;
    Sprite sp_revive;
    Sprite sp_null;
    public GameObject msgText;
    private float show_text_time = 0;
    Text Msg_text = null;
    Text Time_text = null;
    private GameObject Msg_ImgObj = null;
    //float GameTimeLeft = 899;
    float GameTimeLeft = 599;//10min
    int temp_minute_left = 15;
    private GameObject bossUI;
    private GameObject hero1UI;
    private GameObject hero2UI;
    private GameObject hero3UI;
    private GameObject boss;
    private GameObject hero1;
    private GameObject hero2;
    private GameObject hero3;
    Text blood1_text;
    Text blood2_text;
    Text blood3_text;
    

    //人物解锁 地面光环
    public GameObject groundGlow;

    // Use this for initialization
    void Start () {
        groundGlow = this.transform.Find("groundGlow").gameObject;
        if (!isLocalPlayer) return;
        msgText = GameObject.FindGameObjectWithTag("MsgText");
        Msg_text = msgText.GetComponent<Text>();
        Time_text = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
        Msg_ImgObj = GameObject.FindGameObjectWithTag("MsgImg");
        Msg_ImgObj.SetActive(false);
        moveJoystick = GameObject.FindGameObjectWithTag("UIMove").GetComponent<MoveJoystick>();
        baseSkillUI = GameObject.FindGameObjectWithTag("UIBaseSkill");
        skill1UI = GameObject.FindGameObjectWithTag("UISkill1");
        skill2UI = GameObject.FindGameObjectWithTag("UISkill2");
        skill3UI = GameObject.FindGameObjectWithTag("UISkill3");
        skill4UI = GameObject.FindGameObjectWithTag("UISkill4");
        bossUI = GameObject.FindGameObjectWithTag("BossImg");
        hero1UI = GameObject.FindGameObjectWithTag("Hero1Img");
        hero2UI = GameObject.FindGameObjectWithTag("Hero2Img");
        hero3UI = GameObject.FindGameObjectWithTag("Hero3Img");

        boss = GameObject.FindGameObjectWithTag("Boss");
        hero1 = GameObject.FindGameObjectWithTag("Hero1");
        hero2 = GameObject.FindGameObjectWithTag("Hero2");
        hero3 = GameObject.FindGameObjectWithTag("Hero3");
        blood1_text = hero1UI.transform.Find("BloodText").gameObject.GetComponent<Text>();
        blood2_text = hero2UI.transform.Find("BloodText").gameObject.GetComponent<Text>();
        blood3_text = hero3UI.transform.Find("BloodText").gameObject.GetComponent<Text>();
        
        //UI 里找到warnImage
        warnImage = GameObject.FindGameObjectWithTag("WarnImage").GetComponent<Image>();
        warnImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        LoadUIResources();
    }
    void LoadUIResources()
    {
        sp_trap_blind = Resources.Load("Textrues/SKILL_TRAP_BLIND", typeof(Sprite)) as Sprite;
        sp_trap_blind_colding= Resources.Load("Textrues/SKILL_TRAP_BLIND_COLDING", typeof(Sprite)) as Sprite;
        sp_trap_slow= Resources.Load("Textrues/SKILL_TRAP_SLOW", typeof(Sprite)) as Sprite;
        sp_trap_slow_colding= Resources.Load("Textrues/SKILL_TRAP_SLOW_COLDING", typeof(Sprite)) as Sprite;
        sp_trap_ice= Resources.Load("Textrues/SKILL_TRAP_ICE", typeof(Sprite)) as Sprite;
        sp_trap_ice_colding= Resources.Load("Textrues/SKILL_TRAP_ICE_COLDING", typeof(Sprite)) as Sprite;
        sp_flash= Resources.Load("Textrues/SKILL_FLASH", typeof(Sprite)) as Sprite;
        sp_hide= Resources.Load("Textrues/SKILL_HIDE", typeof(Sprite)) as Sprite;
        sp_revive= Resources.Load("Textrues/SKILL_REVIVE", typeof(Sprite)) as Sprite;
        sp_null = Resources.Load("Textrues/NULL", typeof(Sprite)) as Sprite;
    }
	
	// Update is called once per frame
    //人物一切的ui输入必须是localplayer才能操作，但是ui界面的同步？
	void Update ()
    {
        glowEffect();
        if (!isLocalPlayer)
        {
            return;
        }
        
        UIUpdateBloodNum();
        if (this.GetComponent<InfoControl>().getState() == PEOPLE.FREE)
        {
            SkillUpdate();
        }
        MovementUpdate();
        HandleClick();
        SkillUIUpdate();
        WarnEffect();
        MsgUpdate();
       

    }
    private void glowEffect()
    {
        if (GETING_SKILL)
        {
            groundGlow.SetActive(true);
        }
        else {
            groundGlow.SetActive(false);
        }
    }

    public void UIUpdateBloodNum()
    {
        if (hero1 == null||hero2==null|| hero3==null)
        {
            hero1 = GameObject.FindGameObjectWithTag("Hero1");
            hero2 = GameObject.FindGameObjectWithTag("Hero2");
            hero3 = GameObject.FindGameObjectWithTag("Hero3");
        }
        if (hero1 != null)
        {
            int hero1_bloodnum = hero1.GetComponent<InfoControl>().blood_num;
            //Debug.Log(blood1_text.text);
            blood1_text.text = "灵识数量:" + hero1_bloodnum.ToString();
        }

        if (hero2 != null)
        {
            int hero2_bloodnum = hero2.GetComponent<InfoControl>().blood_num;
            blood2_text.text = "灵识数量:" + hero2_bloodnum.ToString();
        }

        if (hero3 != null)
        {
            int hero3_bloodnum = hero3.GetComponent<InfoControl>().blood_num;
            blood3_text.text = "灵识数量:" + hero3_bloodnum.ToString();
        }
       
    }

    void gameOver()
    {
        int num1 = 2;
        if (hero1 != null) num1 = hero1.GetComponent<InfoControl>().blood_num;
        int num2 = 2;
        if (hero2 != null) num2 = hero2.GetComponent<InfoControl>().blood_num;
        int num3 = 2;
        if (hero3 != null) num3 = hero3.GetComponent<InfoControl>().blood_num;
        if (num1 + num2 + num3 > 6)
        {
            Debug.Log(this.gameObject.tag + "Win");
            SceneManager.LoadScene(this.gameObject.tag + "Win");
        }
        else {
            SceneManager.LoadScene(this.gameObject.tag + "Lose");
        }
    }


    void MsgUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GameTimeLeft -= Time.deltaTime;
        if (GameTimeLeft < 0)
        {
            gameOver();
        }
        int minute = (int)Mathf.Ceil(GameTimeLeft / 60);
        if (minute < temp_minute_left)
        {
            Debug.Log(minute);
            temp_minute_left = minute;
            Time_text.text = "游戏剩余时间： " + temp_minute_left.ToString() + "分钟";
        }

        if (show_text_time > 0)
        {
            show_text_time -= Time.deltaTime;
        }
        if (show_text_time <= 0)
        {
            show_text_time = 0;
            Msg_text.text = "";
            Msg_ImgObj.SetActive(false);
        }
    }

    public void showMsg(string msg)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Msg_text == null)
        {
            msgText = GameObject.FindGameObjectWithTag("MsgText");
            Msg_text = msgText.GetComponent<Text>();
        }

        show_text_time = 2.0f;
        Msg_text.text = msg;
        Msg_ImgObj.SetActive(true);
    }


    private void WarnEffect()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss == null) return;
        if ((this.transform.position - boss.transform.position).magnitude > 5)
            warnImage.color = Color.Lerp(warnImage.color, Color.clear, flashSpeed * Time.deltaTime);
        else
        {
            Color c = new Color(0.5f, 0, 0, 0.30f);
            warnImage.color = Color.Lerp(warnImage.color, c, flashSpeed * Time.deltaTime);
            warnImage.color = c;
        }
    }

    void HandleClick()
    {
        if (!isLocalPlayer) return;
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
                if (hitInfo.collider.tag.Equals("GoldenPage") || hitInfo.collider.tag.Equals("BlackPage") || hitInfo.collider.tag.Equals("WhitePage"))
                {
                    if (this.GetComponent<InfoControl>().pageSkills.Count >= 4)
                    {
                        showMsg("已经获取四个纸张，无法再解锁！");
                        return;
                    }
                    Debug.Log("unlock_time_left:" + hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left);
                    CmdChangeTime(hitInfo.collider.gameObject);
                    CmdGettingSkill(true);

                    if (hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left <= 0)
                    {
                        Debug.Log("Acccqqqqurore!");
                        hitInfo.collider.gameObject.GetComponent<PageInfo>().unlock_time_left = 0;
                        acquirePageSkill(hitInfo.collider);

                    }
                }

            }
            else if (GETING_SKILL == true) //点击位置错误
            {

                CmdGettingSkill(false);
            }
        } //没点击
        else if (GETING_SKILL == true)
        {

            CmdGettingSkill(false);
        }
    }
    [Command]
    void CmdGettingSkill( bool state)
    {
        GETING_SKILL = state;
    }

  

    [Command]
    void CmdChangeTime(GameObject obj)
    {
        RpcChangeTime(obj);
    }
    [ClientRpc]
    void RpcChangeTime(GameObject obj)
    {
        obj.GetComponent<PageInfo>().unlock_time_left -= this.GetComponent<InfoControl>().unlock_page_speed * Time.deltaTime;
    }


    Vector3 GetSkillTgtPos()
    {
        //原地留下陷阱
        Vector3 targetPos = this.transform.position;
        
        return targetPos;
    }

    void PageSkillUIUpdate(SKILL _skill, GameObject _skillUI)
    {
        if (!isLocalPlayer) return;
        Image _skillImg = _skillUI.GetComponent<Image>();
        string skill_name = _skill.getSkillName();
        switch (skill_name)
        {
            case "SKILL_FLASH":
                _skillImg.sprite = sp_flash;
                break;
            case "SKILL_REVIVE":
                _skillImg.sprite = sp_revive;
                break;
            case "SKILL_HIDE":
                _skillImg.sprite = sp_hide;
                break;
            default:
                Debug.Log("Error! Invalid PAGE skill_name!");
                break;
        }

    }
    void setPageSkillUINULL()
    {
        skill1UI.GetComponent<Image>().sprite = sp_null;
        skill2UI.GetComponent<Image>().sprite = sp_null;
        skill3UI.GetComponent<Image>().sprite = sp_null;
        skill4UI.GetComponent<Image>().sprite = sp_null;
    }
    void SkillUIUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        SKILL _basicSkill = this.GetComponent<InfoControl>().basicSkill;
        Image baseSkillImg = baseSkillUI.GetComponent<Image>();
        string skill_name = _basicSkill.getSkillName();
        if (_basicSkill.cd_time_left > 0)
        {
            Text cd_text = baseSkillUI.transform.Find("cd_time_left_text").gameObject.GetComponent<Text>();
            cd_text.text = _basicSkill.cd_time_left.ToString();
            switch (skill_name)
            {
                case "SKILL_TRAP_BLIND":
                    baseSkillImg.sprite = sp_trap_blind_colding;
                    break;
                case "SKILL_TRAP_ICE":
                    baseSkillImg.sprite = sp_trap_ice_colding;
                    break;
                case "SKILL_TRAP_SLOW":
                    baseSkillImg.sprite = sp_trap_slow_colding;
                    break;
                default:
                    Debug.Log("Error! Invalid skill_name!");
                    break;
            }

        }
        else
        {
            switch (skill_name)
            {
                case "SKILL_TRAP_BLIND":
                    baseSkillImg.sprite = sp_trap_blind;
                    break;
                case "SKILL_TRAP_ICE":
                    baseSkillImg.sprite = sp_trap_ice;
                    break;
                case "SKILL_TRAP_SLOW":
                    baseSkillImg.sprite = sp_trap_slow;
                    break;
                default:
                    Debug.Log("Error! Invalid BASE skill_name!");
                    break;
            }
        }
        setPageSkillUINULL();
        // UI上 第一个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 1)  //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[0];
            PageSkillUIUpdate(_skill, skill1UI);
        }
        // UI上 第二个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 2) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[1];
            PageSkillUIUpdate(_skill, skill2UI);
        }

        // UI上 第三个纸张技能的位置 
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 3) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[2];
            PageSkillUIUpdate(_skill, skill3UI);
        }
        // UI上 第四个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 4) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[3];
            PageSkillUIUpdate(_skill, skill4UI);
        }


    }
 
    bool HandleBasicSkillEvent()
    {
        if (!isLocalPlayer) return false;
        SKILL _basicSkill = this.GetComponent<InfoControl>().basicSkill;
        SkillJoystick skillJoystick = baseSkillUI.GetComponent<SkillJoystick>();
        if (_basicSkill.cd_time_left > 0)
        {

        }
        else {
          
            if (skillJoystick.isPressed())//在UI上接收点击信息 实时获取方向/位置信息
            {
                Debug.Log("Input.GetKey");
                skillJoystick.waspressed = true;
                HandleGetKeyEvent(_basicSkill);
                return true;
            }
            else if (skillJoystick.waspressed)//如果用户现在没按下但是曾经按下，表示放开了按键
            {
                Debug.Log("Input.GetKeyUp");
                HandleGetKeyUpEvent(_basicSkill);
                skillJoystick.waspressed = false;
                return true;
            }
        }
        return false;
 
    }

    bool HandlePageSkillEvent(SKILL _skill,GameObject _skillUI)
    {
        if (!isLocalPlayer) return false;
        SkillJoystick skillJoystick= _skillUI.GetComponent<SkillJoystick>();

        if (skillJoystick.isPressed())//在UI上接收点击信息 实时获取方向/位置信息
        {
            Debug.Log("Input.GetKey");
            skillJoystick.waspressed = true;
            HandleGetKeyEvent(_skill);
            return true;
        }
        else if (skillJoystick.waspressed)//如果用户现在没按下但是曾经按下，表示放开了按键
        {
            Debug.Log("Input.GetKeyUp");
            HandleGetKeyUpEvent(_skill);
            skillJoystick.waspressed  = false;
            return true;
        }
        return false;
    }

    void SkillUpdate()
    {
        if (!isLocalPlayer) return;

        if (HandleBasicSkillEvent()) //true表示接收到按键事件，此次不需要再检查别的按键了
        {
            return;
        }
      
        // UI上 第一个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 1)  //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[0];
            if (HandlePageSkillEvent(_skill, skill1UI))
            {
                return;
            }

        }
        // UI上 第二个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 2) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[1];
            if (HandlePageSkillEvent(_skill, skill2UI))
            {
                return;
            }
        }
        // UI上 第三个纸张技能的位置 
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 3) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[2];
            if (HandlePageSkillEvent(_skill, skill3UI))
            {
                return;
            }
        }
        // UI上 第四个纸张技能的位置
        if (this.GetComponent<InfoControl>().pageSkills.Count >= 4) //如果这个位置有技能
        {
            SKILL _skill = this.GetComponent<InfoControl>().pageSkills[3];
            if (HandlePageSkillEvent(_skill, skill4UI))
            {
                return;
            }
        }
    }
    void HandleGetKeyEvent(SKILL _skill)
    {
        if (!isLocalPlayer) return;
        Debug.Log(_skill.getSkillName());
        if (_skill.need_dir)
        {
            //从UI实时接收方向信息
            //UI显示方向
            Dir = Vector3.forward;// 用户面朝的方向
        }
        if (_skill.need_pos)
        {
            //接收位置信息
            Pos = GetSkillTgtPos();
            if (_skill.IsPosValid(Pos))
            {
                Debug.Log("PosValid");
                Debug.Log(Pos);
                //UI显示位置
            }
        }

    }
    void HandleGetKeyUpEvent(SKILL _skill)
    {
        if (!isLocalPlayer) return;
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
               
                _skill.SetTargetPos(this.transform.position);
            }
            Debug.Log("Already SetTargetPos:");
            Debug.Log(Pos);
            //UI显示位置

        }
        this.GetComponent<InfoControl>().next_skill_to_begin = _skill;
    }
 
    void MovementUpdate()
    {
        if (!isLocalPlayer) return;

        v = 0.0f;
        h = 0.0f;
        bool move = false;
        //w键前进  
        h = moveJoystick.Horizontal();
        v = moveJoystick.Vertical();
        if (v != 0.0f || h != 0.0f) move = true;

        if (move && transparent==true)
        {
            transparent = false;
            CmdCancelTransparent();
        }
    }


    private void acquirePageSkill(Collider other)
    {
        if (!isLocalPlayer) return;
        if (other.tag.Equals("GoldenPage"))
        {
            Debug.Log("acquireGoldenPageSkill");
            showMsg("获得金色纸张 闪现技能");
            this.GetComponent<InfoControl>().addPageSkill("GoldenPage");
        }
        else if (other.tag.Equals("BlackPage"))
        {
            Debug.Log("acquireBlackPageSkill");
            showMsg("获得黑色纸张 隐身技能");
            this.GetComponent<InfoControl>().addPageSkill("BlackPage");
        }
        else if (other.tag.Equals("WhitePage"))
        {
            Debug.Log("acquireWhitePageSkill");
            showMsg("获得白色纸张 复活技能");
            this.GetComponent<InfoControl>().addPageSkill("WhitePage");
        }

        //从游戏场景中消失，或者这里先播放一个纸张消失的动画再消失
        other.gameObject.SetActive(false);
        CmdDelete_page(other.gameObject);
        //NetworkServer.Destroy(other.gameObject);
        //other.gameObject.SetActive(false);
      
    }

    [Command]
    void CmdDelete_page(GameObject obj)
    {
        Debug.Log("CmdDelete_page");
        RpcDelete_page(obj);
    }
    [ClientRpc]
    void RpcDelete_page(GameObject obj)
    {
        Debug.Log("RpcDelete_page");
        obj.SetActive(false);
    }


    [Command]
    public void CmdBecomeTransparent()
    {
        transparent = true;
        RpcBecomeTransparent();
    }

    [Command]
    void CmdCancelTransparent()
    {
        RpcCancelTransparent();
    }

    [ClientRpc]
    void RpcBecomeTransparent()
    {
        GameObject model = this.transform.Find("model").gameObject;
        if (model == null) Debug.Log("NULL");
        Material[] _material = model.GetComponent<SkinnedMeshRenderer>().materials;
        Color temp1 = _material[0].color;
        Color temp2 = _material[1].color;
        _material[0].SetColor("_Color", new Color(temp1[0], temp1[1], temp1[2], 0f));
        _material[1].SetColor("_Color", new Color(temp2[0], temp2[1], temp2[2], 0f));
    }

    [ClientRpc]
    void RpcCancelTransparent()
    {
        GameObject model = this.transform.Find("model").gameObject;
        if (model == null) Debug.Log("NULL");
        Material[] _material = model.GetComponent<SkinnedMeshRenderer>().materials;
        Color temp1 = _material[0].color;
        Color temp2 = _material[1].color;
        _material[0].SetColor("_Color", new Color(temp1[0], temp1[1], temp1[2], 1.0f));
        _material[1].SetColor("_Color", new Color(temp2[0], temp2[1], temp2[2], 1.0f));
    }


}
