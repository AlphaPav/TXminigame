using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicSlide : MonoBehaviour {

    public GameObject Img, Text;
    const int slide_speed = 2;

    bool slideFlag = false;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (slideFlag)
        {
            SlidePlay();
        }
    }

    public void SlidePlay()
    {
        Vector3 targetpos = new Vector3(-Img.GetComponent<RectTransform>().sizeDelta.x + Screen.width, 0, 0);

        Img.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(Img.GetComponent<RectTransform>().anchoredPosition, targetpos, slide_speed);

        if(Img.GetComponent<RectTransform>().anchoredPosition.x<= -Img.GetComponent<RectTransform>().sizeDelta.x + Screen.width)
        {
            slideFlag = false;

            Text.SetActive(true);
        }
    }
    public void Flag()
    {
        slideFlag = true;
    }

}
