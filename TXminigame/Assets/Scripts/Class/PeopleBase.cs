using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	
	人的要素：
		人的类型（type） [boss or 故事人物]
		人的状态
		状态持续时间
*/

class PEOPLE
{
    /*
	人物状态定义
	*/

    public const int FREE = 0;   //
    public const int BEGIN_SKILL = 1;    // 引导技能阶段
    public const int EXECUTE_SKILL = 2;  //执行技能阶段
    public const int END_SKILL = 3;  // 结束技能

    public const int ICE = 4;    // 被冰冻
    public const int SLOW = 5; //被减速
    public const int BLIND = 6; //被黑色蒙版遮住
    public const int SEALED = 7; //被封印
    public const int DIE = 8;
    public bool transparent = false;
                                 // ....

}



public class PeopleBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
