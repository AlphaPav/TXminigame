using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniControl : MonoBehaviour {
    int lastState = PEOPLE.FREE;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int tempState = this.GetComponent<InfoControl>().getState();
        if (lastState == tempState)
            return;

        if (tempState == PEOPLE.BEGIN_SKILL)
        {
            //根据技能播放动画...
        }
        lastState = tempState;
	}
}
