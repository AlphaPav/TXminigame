using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform m_TargetTransform; // 镜头要跟踪的目标
    private float depth = -2f;          // 镜头相对于角色的前后位置，负数代表位于角色后方；
    [SerializeField]
    private float m_Speed = 5f; // 控制镜头跟踪时的速度，用于调整镜头额平滑移动，如果速度过大，极限情况下直接把目标位置赋给镜头，那么对于闪现一类的角色瞬移效果，将会带来不利的视觉影像
    Vector3 position1 = new Vector3(7.82f, 5.74f, -18f);
    Vector3 position2 = new Vector3(-9.14f, 5.74f, -18f);
    Vector3 position3 = new Vector3(-9.14f, 5.74f, -5f);

    bool reach_1 = false;
    bool reach_2 = false;
    bool reach_3 = false;


    private void Start()
    {

    }
    void Update()
    {
        GameObject manager = GameObject.Find("Network Manager");
        int id = manager.GetComponent<MyNetManager>().chosenCharacter;
        //Debug.Log(id);
        if (id == 0)
        {
            GameObject[] blind_trap = GameObject.FindGameObjectsWithTag("BlindTrap");
            GameObject[] ice_trap = GameObject.FindGameObjectsWithTag("IceTrap");
            GameObject[] slow_trap = GameObject.FindGameObjectsWithTag("SlowTrap");
            foreach (GameObject trap in blind_trap)
            {
                trap.layer = LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh01").gameObject.layer= LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh02").gameObject.layer = LayerMask.NameToLayer("trap");
            }
            foreach (GameObject trap in ice_trap)
            {
                trap.layer = LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh01").gameObject.layer = LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh02").gameObject.layer = LayerMask.NameToLayer("trap");
            }
            foreach (GameObject trap in slow_trap)
            {
                trap.layer = LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh01").gameObject.layer = LayerMask.NameToLayer("trap");
                trap.transform.Find("Mesh02").gameObject.layer = LayerMask.NameToLayer("trap");
            }
        }
        if (m_TargetTransform != null)
        {
            //var targetposition = m_TargetTransform.position + new Vector3(0, 2, depth);
            //transform.position = Vector3.MoveTowards(transform.position, targetposition, m_Speed * Time.deltaTime);

            if (reach_1 == false)
            {
                var targetpos = position1;
                transform.position = Vector3.MoveTowards(transform.position, targetpos, m_Speed * Time.deltaTime);
                if (transform.position == position1) reach_1 = true;
            }
            else if (reach_2 == false)
            {
                var targetpos = position2;
                transform.position = Vector3.MoveTowards(transform.position, targetpos, m_Speed * Time.deltaTime);
                if (transform.position == position2) reach_2 = true;
            }
            else if (reach_3 == false)
            {
                var targetpos = position3;
                transform.position = Vector3.MoveTowards(transform.position, targetpos, m_Speed * Time.deltaTime);
                if (transform.position == position3) reach_3 = true;
            }
            else
            {
                var targetposition = m_TargetTransform.position + new Vector3(0, 2, depth);
                transform.position = Vector3.MoveTowards(transform.position, targetposition, m_Speed * Time.deltaTime);
            }
        }

    }

    public void SetTarget(Transform target)
    {

        m_TargetTransform = target;
    }
}
