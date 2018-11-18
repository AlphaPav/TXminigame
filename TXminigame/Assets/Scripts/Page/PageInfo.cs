using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageInfo : MonoBehaviour {
    public float unlock_time_total = 10.0f;
    public float unlock_time_left = 10.0f; //可以被人物的SkillControl脚本修改
    public float old_value=10.0f;
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*纸张上面加一个Canvas显示解锁进度？*/

        if (old_value == unlock_time_left&&old_value!=10.0f)
        {
            unlock_time_left = 10.0f;
            GameObject _canvas = this.transform.Find("Canvas").gameObject;
            GameObject _hp = _canvas.transform.Find("hp").gameObject;
            Slider _slider = _hp.GetComponent<Slider>();
            _slider.value = unlock_time_left;
        }
        if ((unlock_time_left < 10.0f && unlock_time_left >= 0.0f))
        {
            GameObject _canvas = this.transform.Find("Canvas").gameObject;
            GameObject _hp = _canvas.transform.Find("hp").gameObject;
            Slider _slider = _hp.GetComponent<Slider>();
            _slider.value = unlock_time_left;
          
        }
        old_value = unlock_time_left;
        
    }
}
