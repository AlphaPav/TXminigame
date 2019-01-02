using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class change : MonoBehaviour {
    public Sprite change_sprite;
    public Sprite initial_sprite;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => { OnClickChange(); });
		
	}

    void OnClickChange()
    {
        GameObject btn0 = GameObject.Find("Button0");
        GameObject btn1 = GameObject.Find("Button1");
        GameObject btn2 = GameObject.Find("Button2");
        GameObject btn3 = GameObject.Find("Button3");

        Image img0 = btn0.GetComponent<Image>() as Image; change change0 = btn0.GetComponent<change>();
        Image img1 = btn1.GetComponent<Image>() as Image; change change1 = btn1.GetComponent<change>();
        Image img2 = btn2.GetComponent<Image>() as Image; change change2 = btn2.GetComponent<change>();
        Image img3 = btn3.GetComponent<Image>() as Image; change change3 = btn3.GetComponent<change>();

        img0.sprite = change0.initial_sprite;
        img1.sprite = change1.initial_sprite;
        img2.sprite = change2.initial_sprite;
        img3.sprite = change3.initial_sprite;

        Image back_ground = GetComponent<Image>() as Image;
        back_ground.sprite = change_sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
