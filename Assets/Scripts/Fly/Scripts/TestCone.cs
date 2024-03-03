using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCone : MonoBehaviour
{
    public float radius = 3;

    public float angle = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnDrawGizmos()
    {
        if (BoidManager.Instance == null || BoidManager.Instance.Boids == null)
            return;
        foreach (var boid in BoidManager.Instance.Boids)
        {
            if (IsTargetInCone(transform.position, transform.forward, radius, angle, boid.transform.position))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(boid.transform.position, 0.5f);
            }
        }
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * radius);
    }

    public static float AngleInDegrees(Vector3 vectorA, Vector3 vectorB)
    {
        float angleInRadians = Mathf.Acos(Vector3.Dot(vectorA.normalized, vectorB.normalized));
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }

    public static bool IsTargetInCone(Vector3 coneCenter, Vector3 coneDirection, float coneRadius, float coneAngle, Vector3 targetPosition)
    {
        // 计算目标到圆锥中心的距离
        float distanceToTarget = Vector3.Distance(coneCenter, targetPosition);

        // 首先检查目标是否在半径范围内
        if (distanceToTarget > coneRadius)
        {
            return false;
        }

        // 计算从圆锥中心指向目标的方向
        Vector3 directionToTarget = (targetPosition - coneCenter).normalized;

        // 计算圆锥中心方向和目标方向之间的夹角
        float angleBetweenDirections = AngleInDegrees(coneDirection, directionToTarget);

        // 如果两个方向间的夹角小于或等于圆锥顶角除以2，则目标在圆锥体范围内
        return angleBetweenDirections <= coneAngle / 2;
    }
}
