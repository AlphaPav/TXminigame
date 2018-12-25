using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    private GameObject Msg_ImgObj = null;
    //float GameTimeLeft = 899;//15min
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
    Text cd_text;
    Image baseSkillImg;
    void Start () {
        if (!isLocalPlayer) return;
        Time_text = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
        moveJoystick = GameObject.FindGameObjectWithTag("UIMove").GetComponent<MoveJoystick>();
        baseSkillUI = GameObject.FindGameObjectWithTag("UIBaseSkill");
        msgText = GameObject.FindGameObjectWithTag("MsgText");
        Msg_text= msgText.GetComponent<Text>();
        Msg_ImgObj = GameObject.FindGameObjectWithTag("MsgImg");
        Msg_ImgObj.SetActive(false);
        skill1UI = GameObject.FindGameObjectWithTag("UISkill1");
        skill2UI = GameObject.FindGameObjectWithTag("UISkill2");
        skill3UI = GameObject.FindGameObjectWithTag("UISkill3");
        skill4UI = GameObject.FindGameObjectWithTag("UISkill4");
        skill1UI.SetActive(false);
        skill2UI.SetActive(false);
        skill3UI.SetActive(false);
        skill4UI.SetActive(false);

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

        cd_text = baseSkillUI.transform.Find("cd_time_left_text").gameObject.GetComponent<Text>();
        baseSkillImg = baseSkillUI.GetComponent<Image>();
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
        BossUIUpdateBloodNum();
        if (this.GetComponent<BossInfoControl>().getState() == PEOPLE.FREE)
        {
            SkillUpdate();
        }
        MovementUpdate();
        SkillUIUpdate();
        MsgUpdate();
    }

    public void BossUIUpdateBloodNum()
    {

        if (hero1 == null || hero2 == null || hero3 == null)
        {
            hero1 = GameObject.FindGameObjectWithTag("Hero1");
            hero2 = GameObject.FindGameObjectWithTag("Hero2");
            hero3 = GameObject.FindGameObjectWithTag("Hero3");
        }
        if (hero1 != null)
        {
            int hero1_bloodnum = hero1.GetComponent<InfoControl>().blood_num;

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
        if(hero1!=null) num1= hero1.GetComponent<InfoControl>().blood_num;
        int num2 = 2;
        if (hero2 != null) num2 = hero2.GetComponent<InfoControl>().blood_num;
        int num3 = 2;
        if (hero3 != null) num3 = hero3.GetComponent<InfoControl>().blood_num;
        if (num1 + num2 + num3 > 6)
        {
            Debug.Log(this.gameObject.tag + "Lose");
            SceneManager.LoadScene(this.gameObject.tag + "Lose");
        }
        else
        {
            SceneManager.LoadScene(this.gameObject.tag + "Win");
        }
    }

    void MsgUpdate()
    {
        GameTimeLeft -= Time.deltaTime;
        if (GameTimeLeft < 0)
        {
            gameOver();
        }
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

        show_text_time = 1.0f;
        Msg_text.text= msg;
        Msg_ImgObj.SetActive(true);
    }

    void SkillUIUpdate()
    {
        SKILL _basicSkill = this.GetComponent<BossInfoControl>().basicSkill;
  
        if (_basicSkill.cd_time_left > 0)
        {
           
            cd_text.text = _basicSkill.cd_time_left.ToString();
            baseSkillImg.sprite = sp_catch_colding;
        }
        else
        {
            cd_text.text = "";
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
                this.GetComponent<BossInfoControl>().basicSkill.cd_time_left = this.GetComponent<BossInfoControl>().basicSkill.cd_time;
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
