using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectileBehaviour : ProjectileBehaviour
{
    public float rotationSpeed = 8f;
    public float seekDistance = 25f;
    public string enemyTag = "Enemy";

    public override void Start()
    {
        base.Start();

        enemyTag = projectile.enemyTag;
    }

    private void LateUpdate()
    {
        if (seekDistance <= 0) return;

        Transform target = FindClosestTarget();
        if (target != null)
        {
            RotateToTarget(target);
        }
    }

    Transform FindClosestTarget()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(enemyTag);

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (var target in allTargets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > seekDistance)
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closest = target;
                closestDistance = distance;
            }
        }

        if (closest == null) return null;

        Debug.DrawLine(transform.position, closest.transform.position, Color.red);
        return closest.transform;
    }

    private void RotateToTarget(Transform target)
    {
        Vector3 direction = target.position - projectile.rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.forward).y;
        projectile.rb.angularVelocity = new Vector3(0, -rotationSpeed * rotateAmount, 0);
    }
}
