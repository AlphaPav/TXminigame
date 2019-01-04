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
    Sprite sp_empty;
    Sprite sp_lingshi;
    Sprite sp_msg;
    private float show_text_time=0;
    Text Msg_text = null;
    Text Time_text = null;
    private GameObject Msg_ImgObj = null;
    //float GameTimeLeft = 899;//15min
    float GameTimeLeft = 479;//8min
    int temp_minute_left = 15;

    private GameObject bossUI;
    private GameObject hero1UI;
    private GameObject hero2UI;
    private GameObject hero3UI;
    private GameObject[] hero1_lingshi;
    private GameObject[] hero2_lingshi;
    private GameObject[] hero3_lingshi;
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
        LoadUIResources();

        Time_text = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
        moveJoystick = GameObject.FindGameObjectWithTag("UIMove").GetComponent<MoveJoystick>();
        baseSkillUI = GameObject.FindGameObjectWithTag("UIBaseSkill");
        msgText = GameObject.FindGameObjectWithTag("MsgText");
        Msg_text= msgText.GetComponent<Text>();
        Msg_ImgObj = GameObject.FindGameObjectWithTag("MsgImg");
        Msg_ImgObj.GetComponent<Image>().sprite = sp_empty;
        skill1UI = GameObject.FindGameObjectWithTag("UISkill1");
        skill2UI = GameObject.FindGameObjectWithTag("UISkill2");
        skill3UI = GameObject.FindGameObjectWithTag("UISkill3");
        skill4UI = GameObject.FindGameObjectWithTag("UISkill4");
        skill1UI.GetComponent<Image>().sprite = sp_empty;
        skill2UI.GetComponent<Image>().sprite = sp_empty;
        skill3UI.GetComponent<Image>().sprite = sp_empty;
        skill4UI.GetComponent<Image>().sprite = sp_empty;

        bossUI = GameObject.FindGameObjectWithTag("BossImg");
        hero1UI = GameObject.FindGameObjectWithTag("Hero1Img");
        hero2UI = GameObject.FindGameObjectWithTag("Hero2Img");
        hero3UI = GameObject.FindGameObjectWithTag("Hero3Img");
        hero1_lingshi = new GameObject[6];
        hero2_lingshi = new GameObject[6];
        hero3_lingshi = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            
            hero1_lingshi[i] = hero1UI.transform.Find("Image" + i).gameObject;
            hero2_lingshi[i] = hero2UI.transform.Find("Image" + i).gameObject;
            hero3_lingshi[i] = hero3UI.transform.Find("Image" + i).gameObject;

            hero1_lingshi[i].GetComponent<Image>().sprite = sp_empty;
            hero2_lingshi[i].GetComponent<Image>().sprite = sp_empty;
            hero3_lingshi[i].GetComponent<Image>().sprite = sp_empty;
        }
        boss = GameObject.FindGameObjectWithTag("Boss");
        hero1 = GameObject.FindGameObjectWithTag("Hero1");
        hero2 = GameObject.FindGameObjectWithTag("Hero2");
        hero3 = GameObject.FindGameObjectWithTag("Hero3");
        //blood1_text = hero1UI.transform.Find("BloodText").gameObject.GetComponent<Text>();
        //blood2_text = hero2UI.transform.Find("BloodText").gameObject.GetComponent<Text>();
        //blood3_text = hero3UI.transform.Find("BloodText").gameObject.GetComponent<Text>();

        cd_text = baseSkillUI.transform.Find("cd_time_left_text").gameObject.GetComponent<Text>();
        baseSkillImg = baseSkillUI.GetComponent<Image>();
       
        blindImage = GameObject.FindGameObjectWithTag("BlindImage").GetComponent<Image>();
        blindImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 2.5f, Screen.height * 2.5f);



    }
    void LoadUIResources()
    {
        sp_catch= Resources.Load("Textrues/SKILL_CATCH", typeof(Sprite)) as Sprite;
        sp_catch_colding= Resources.Load("Textrues/SKILL_CATCH_COLDING", typeof(Sprite)) as Sprite;
        sp_empty = Resources.Load("Textrues/empty", typeof(Sprite)) as Sprite;
        sp_lingshi=Resources.Load("Textrues/LINGSHI", typeof(Sprite)) as Sprite;
        sp_msg= Resources.Load("Textrues/msgImg", typeof(Sprite)) as Sprite;
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
        isAllSealed();
    }

    void isAllSealed()
    {
        if ((hero1 == null) || (hero2 == null) || (hero3 == null)) return;
        bool ishero1sealed = false;
        bool ishero2sealed = false;
        bool ishero3sealed = false;

        if (hero1 != null && (hero1.GetComponent<StateControl>().state == PEOPLE.SEALED)) ishero1sealed = true;
        if (hero2 != null &&( hero2.GetComponent<StateControl>().state == PEOPLE.SEALED)) ishero2sealed = true;
        if (hero3 != null&& (hero3.GetComponent<StateControl>().state == PEOPLE.SEALED)) ishero3sealed = true;

        if (ishero1sealed && ishero2sealed && ishero3sealed)
        {
            SceneManager.LoadScene(this.gameObject.tag + "Win");
        }

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
            //Debug.Log(blood1_text.text);
           // blood1_text.text = "灵识数量:" + hero1_bloodnum.ToString();
            for (int i = 0; i < 6; i++)
            {
                if (i < hero1_bloodnum)
                {
                    hero1_lingshi[i].GetComponent<Image>().sprite = sp_lingshi;
                }
                else
                {
                    hero1_lingshi[i].GetComponent<Image>().sprite = sp_empty;
                }
            }
        }

        if (hero2 != null)
        {
            int hero2_bloodnum = hero2.GetComponent<InfoControl>().blood_num;
           // blood2_text.text = "灵识数量:" + hero2_bloodnum.ToString();
            for (int i = 0; i < 6; i++)
            {
                if (i < hero2_bloodnum)
                {
                    hero2_lingshi[i].GetComponent<Image>().sprite = sp_lingshi;
                }
                else
                {
                    hero2_lingshi[i].GetComponent<Image>().sprite = sp_empty;
                }
            }

        }

        if (hero3 != null)
        {
            int hero3_bloodnum = hero3.GetComponent<InfoControl>().blood_num;
           // blood3_text.text = "灵识数量:" + hero3_bloodnum.ToString();
            for (int i = 0; i < 6; i++)
            {
                if (i < hero3_bloodnum)
                {
                    hero3_lingshi[i].GetComponent<Image>().sprite = sp_lingshi;
                }
                else
                {
                    hero3_lingshi[i].GetComponent<Image>().sprite = sp_empty;
                }
            }
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
            Time_text.text =  temp_minute_left.ToString()+ " Minutes";
        }


        if (show_text_time > 0)
        {
            show_text_time -= Time.deltaTime;
        }
        if (show_text_time <= 0)
        {
            show_text_time = 0;
            Msg_text.text = "";
            if (Msg_ImgObj == null)
            {
                Msg_ImgObj = GameObject.FindGameObjectWithTag("MsgImg");
            }
            Msg_ImgObj.GetComponent<Image>().sprite = sp_empty;
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
        Msg_ImgObj.GetComponent<Image>().sprite = sp_msg;
    }

    void SkillUIUpdate()
    {
        if (cd_text == null) {
            cd_text = baseSkillUI.transform.Find("cd_time_left_text").gameObject.GetComponent<Text>();
        }
        if (baseSkillImg == null)
        {
            baseSkillImg = baseSkillUI.GetComponent<Image>();
        }
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
            blindImage.color = Color.Lerp(blindImage.color, new Color(0.05f, 0.05f, 0.05f, 1.0f), flashSpeed * Time.deltaTime);
            Debug.Log("Blind");
        }
        else
        {
            blindImage.color = Color.clear;
        }
    }

}
