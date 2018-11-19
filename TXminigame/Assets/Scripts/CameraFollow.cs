using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform m_TargetTransform; // 镜头要跟踪的目标
    private float depth = -3f;          // 镜头相对于角色的前后位置，负数代表位于角色后方；
    [SerializeField]
    private float m_Speed = 5f; // 控制镜头跟踪时的速度，用于调整镜头额平滑移动，如果速度过大，极限情况下直接把目标位置赋给镜头，那么对于闪现一类的角色瞬移效果，将会带来不利的视觉影像

    void Update()
    {
        // m_TargetTransform = GameObject.FindWithTag("Hero1").transform;
        m_TargetTransform = GameObject.FindWithTag("Boss").transform;
        if (m_TargetTransform != null)
        {
            var targetposition = m_TargetTransform.position + new Vector3(0, 3, depth);
            transform.position = Vector3.MoveTowards(transform.position, targetposition, m_Speed * Time.deltaTime);
        }

    }

    public void SetTarget(Transform target)
    {

        m_TargetTransform = target;
    }
}
