using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    //相机距离人物高度
    float m_Height;
    //相机距离人物距离
    float m_Distance;
    //相机跟随速度
    public float m_Speed = 5f;
    //目标位置
    Vector3 m_TargetPosition;
    //要跟随的人物
    public Transform hero;

    // Use this for initialization
    void Start () {
        m_Height = this.transform.position.y - hero.position.y;
        //hero初始朝着z轴正方向
        m_Distance =  hero.position.z - this.transform.position.z;

    }
	
	// Update is called once per frame
	void Update () {
        //得到目标位置
        m_TargetPosition = hero.position + Vector3.up * m_Height - hero.forward * m_Distance;
        //相机位置插值过渡
        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_Speed * Time.deltaTime);
        //相机时刻看着人物
        transform.LookAt(hero);
      
	}
}
