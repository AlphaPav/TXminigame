using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Image Img;
    private bool press = false;
    public bool waspressed = false;
	// Use this for initialization
	void Start () {
        Img = GetComponent<Image>();
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("press");

        press = true;
       
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        press = false;
      
    }
    
    public bool isPressed()
    {
        //Debug.Log("press");
        return press;
       
    }
}
