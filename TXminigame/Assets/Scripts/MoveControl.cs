using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveControl : NetworkBehaviour {
    //[SerializeField] 作用：私有化变量也可以在面板上显示和赋值
    [SerializeField] private float m_moveSpeedStandard = 2;
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private Animator m_animator;
   
    float v = 0.0f;
    float h = 0.0f;

    public GameObject footprint;
    public Transform footfalls;
    public float total_time=0;
    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        bool transparent = this.GetComponent<UIControl>().transparent;
        total_time += Time.deltaTime;
        if (total_time>0.1&&transparent==false)
        {
            CmdsetFootPrint();
            //Instantiate(footfalls, GetComponent<Transform>().position, footfalls.rotation);
            total_time = 0;
        }

        int tempState = this.GetComponent<InfoControl>().getState();
        if (tempState == PEOPLE.EXECUTE_SKILL|| tempState == PEOPLE.END_SKILL||tempState==PEOPLE.SEALED|| tempState == PEOPLE.DIE)
            return;

        
        MovementUpdate();
     

        if (tempState == PEOPLE.BEGIN_SKILL)
        {
            //如果这时候movement改变，则技能被打断
            if (h != 0.0f || v != 0.0f)
            {
                Debug.Log("changeState(PEOPLE.END_SKILL)");
                this.GetComponent<InfoControl>().changeState(PEOPLE.END_SKILL);
            }
            
        }
    }
    [Command]
    private void CmdsetFootPrint()
    {
        var foot =(GameObject)Instantiate(footprint, GetComponent<Transform>().position, footfalls.rotation);
        NetworkServer.Spawn(foot);
        Destroy(foot, 2.0f);
    }



    private void MovementUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        m_animator.SetBool("Grounded", true);
        h = this.GetComponent<InfoControl>().getHorizontalMove();
        v = this.GetComponent<InfoControl>().getVerticalMove();

       // Debug.Log(new Vector2(h, v));

        float dis = (new Vector3(h, 0, v)).magnitude;

        transform.position = this.transform.position + new Vector3(h, 0, v) * m_moveSpeed * Time.deltaTime;
        if (h != 0 || v != 0)
        {

            Rotating(h, v);
        }
        m_animator.SetFloat("MoveSpeed", dis);

    }
    void Rotating(float horizontal, float vertical)
    {
        if (!isLocalPlayer) return;

       // Debug.Log("rotating");
        // 创建角色目标方向的向量
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        // 创建目标旋转值 并假设Y轴正方向为"上"方向
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); //函数参数解释: LookRotation(目标方向为"前方向", 定义声明"上方向")
        // 创建新旋转值 并根据转向速度平滑转至目标旋转值
        //  Lerp(角色刚体当前旋转值, 目标旋转值, 根据旋转速度平滑转向)
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, m_moveSpeed * Time.deltaTime);
        // 更新刚体旋转值为 新旋转值
        this.transform.rotation = newRotation;

    }


}
