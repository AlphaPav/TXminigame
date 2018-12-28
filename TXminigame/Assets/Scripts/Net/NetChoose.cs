using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetChoose : MonoBehaviour {
    public static int chosenID = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //选择角色按钮0
    public void Btn0()
    {
        chosenID = 0;
        
    }

    //选择角色按钮1
    public void Btn1()
    {
        chosenID = 1;
    }

    //选择角色按钮2
    public void Btn2()
    {
        chosenID = 2;
    }

    //选择角色按钮3
    public void Btn3()
    {
        chosenID = 3;
    }

    
    public void StartPlay()
    {
        SceneManager.LoadScene("Main");

    }
}
