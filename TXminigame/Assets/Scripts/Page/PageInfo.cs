using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PageInfo : NetworkBehaviour
{
    public float unlock_time_total = 10.0f;

    [SyncVar]
    public float unlock_time_left = 10.0f; //可以被人物的SkillControl脚本修改
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*纸张上面加一个Canvas显示解锁进度？*/
            GameObject _canvas = this.transform.Find("Canvas").gameObject;
            GameObject _hp = _canvas.transform.Find("hp").gameObject;
            Slider _slider = _hp.GetComponent<Slider>();
            _slider.value = unlock_time_left;
    }
}
