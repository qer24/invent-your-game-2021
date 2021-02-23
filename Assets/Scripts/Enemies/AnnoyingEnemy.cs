using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingEnemy : Enemy
{
    public float rotationSpeed = 10f;
    public float moveForce = 25;
    void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.forward * moveForce * 0.02f);
    }
}
