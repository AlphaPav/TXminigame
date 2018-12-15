using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmove : MonoBehaviour {
    private int Speed = 2;
    Rigidbody rigBody;
    private MoveJoystick moveJoystick;
    private SkillJoystick baseSkillJoystick;
    [SerializeField] private Animator m_animator;
    private bool m_wasGrounded;
    private bool m_isGrounded = true;
    private List<Collider> m_collisions = new List<Collider>();
    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
        moveJoystick = GameObject.FindGameObjectWithTag("UIMove").GetComponent<MoveJoystick>();
      
    }
    void Update()
    {
        m_animator.SetBool("Grounded",true);
        float h = moveJoystick.Horizontal();
        float v = moveJoystick.Vertical();
       
        Debug.Log(new Vector2(h,v));
        
        float dis = (new Vector3(h, 0, v) ).magnitude;
     
        transform.position=  this.transform.position + new Vector3(h, 0, v) * Speed * Time.deltaTime;
        if (h != 0 || v != 0)
        {
            
            Rotating(h, v);
        }
        m_animator.SetFloat("MoveSpeed", dis);
    
    }
    void Rotating(float horizontal, float vertical)
    {
        Debug.Log("rotating");
        // 创建角色目标方向的向量
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        // 创建目标旋转值 并假设Y轴正方向为"上"方向
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); //函数参数解释: LookRotation(目标方向为"前方向", 定义声明"上方向")
        // 创建新旋转值 并根据转向速度平滑转至目标旋转值
        //  Lerp(角色刚体当前旋转值, 目标旋转值, 根据旋转速度平滑转向)
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Speed * Time.deltaTime);
        // 更新刚体旋转值为 新旋转值
        this.transform.rotation = newRotation;
      
    }

}
