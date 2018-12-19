using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BossUIControl : NetworkBehaviour
{
    public  float v = 0.0f;/*vertical*/
    public float h = 0.0f;/*horizontal*/

    public Image blindImage;
    private float flashSpeed = 5;
    // Use this for initialization
    private MoveJoystick moveJoystick;
    private GameObject baseSkillUI;
    private GameObject skill1UI;
    private GameObject skill2UI;
    private GameObject skill3UI;
    private GameObject skill4UI;
    public GameObject msgText;
    Sprite sp_catch;
    Sprite sp_catch_colding;
    private float show_text_time=0;
    Text Msg_text = null;
    Text Time_text = null;
    float GameTimeLeft = 899;
    int temp_minute_left = 15;
    void Start () {
        if (!isLocalPlayer) return;
        Time_text = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
        moveJoystick = GameObject.FindGameObjectWithTag("UIMove").GetComponent<MoveJoystick>();
        baseSkillUI = GameObject.FindGameObjectWithTag("UIBaseSkill");
        msgText = GameObject.FindGameObjectWithTag("MsgText");
        Msg_text= msgText.GetComponent<Text>();
        
        skill1UI = GameObject.FindGameObjectWithTag("UISkill1");
        skill2UI = GameObject.FindGameObjectWithTag("UISkill2");
        skill3UI = GameObject.FindGameObjectWithTag("UISkill3");
        skill4UI = GameObject.FindGameObjectWithTag("UISkill4");
        skill1UI.SetActive(false);
        skill2UI.SetActive(false);
        skill3UI.SetActive(false);
        skill4UI.SetActive(false);

        LoadUIResources();
        blindImage = GameObject.FindGameObjectWithTag("BlindImage").GetComponent<Image>();
        blindImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }
    void LoadUIResources()
    {
        sp_catch= Resources.Load("Textrues/SKILL_CATCH", typeof(Sprite)) as Sprite;
        sp_catch_colding= Resources.Load("Textrues/SKILL_CATCH_COLDING", typeof(Sprite)) as Sprite;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (this.GetComponent<BossInfoControl>().getState() == PEOPLE.FREE)
        {
            SkillUpdate();
        }
        MovementUpdate();
        SkillUIUpdate();
        MsgUpdate();
    }
    void MsgUpdate()
    {
        GameTimeLeft -= Time.deltaTime;
        
        int minute = (int)Mathf.Ceil(GameTimeLeft / 60) ;
        if (minute < temp_minute_left)
        {
            Debug.Log(minute);
            temp_minute_left = minute;
            Time_text.text = "游戏剩余时间： " + temp_minute_left.ToString()+ "分钟";
        }


        if (show_text_time > 0)
        {
            show_text_time -= Time.deltaTime;
        }
        if (show_text_time < 0)
        {
            show_text_time = 0;
            Msg_text.text = "";
        }
    }

    public void showMsg(string msg)
    {
        show_text_time = 1.0f;
        Msg_text.text= msg;
    }

    void SkillUIUpdate()
    {
        SKILL _basicSkill = this.GetComponent<BossInfoControl>().basicSkill;
        Image baseSkillImg = baseSkillUI.GetComponent<Image>();
        string skill_name = _basicSkill.getSkillName();
        if (_basicSkill.cd_time_left > 0)
        {
            Text cd_text = baseSkillUI.transform.Find("cd_time_left_text").gameObject.GetComponent<Text>();
            cd_text.text = _basicSkill.cd_time_left.ToString();
            baseSkillImg.sprite = sp_catch_colding;
        }
        else
        {
            baseSkillImg.sprite = sp_catch;
        }
    }


    void SkillUpdate()
    {
        SKILL _basicSkill = this.GetComponent<BossInfoControl>().basicSkill;
        SkillJoystick skillJoystick = baseSkillUI.GetComponent<SkillJoystick>();
        if (_basicSkill.cd_time_left <= 0)
        {
            if (skillJoystick.isPressed())//在UI上接收点击信息 
            {
                Debug.Log("Input.GetKey");
                this.GetComponent<BossInfoControl>().next_skill_to_begin = _basicSkill;
                return;
            }
        }
    }
    
    void MovementUpdate()
    {
        v = 0.0f;
        h = 0.0f;

        h = moveJoystick.Horizontal();
        v = moveJoystick.Vertical();
     
    }
    public void BlindMask(bool b)
    {
        if (b)
        {
            blindImage.color = Color.Lerp(blindImage.color, new Color(0.1f, 0.1f, 0.1f, 0.9f), flashSpeed * Time.deltaTime);
            Debug.Log("Blind");
        }
        else
        {
            blindImage.color = Color.clear;
        }
    }

}
