using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingEnemy : Enemy
{
    public float rotationSpeed = 10f;
    public float moveForce = 25;
    public float boostForce = 2400;

    public override void Start()
    {
        base.Start();

        InvokeRepeating("Boost", 1f, 2f);
    }

    void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.forward * moveForce * 0.02f);
    }

    void Boost()
    {
        rb.AddForce(transform.forward * boostForce * 0.02f, ForceMode.Impulse);
    }
}
