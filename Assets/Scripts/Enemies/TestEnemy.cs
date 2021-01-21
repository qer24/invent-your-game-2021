using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestEnemy : MonoBehaviour
{
    public float reloadTime = 5f;
    public float rotateTime = 5f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed;
    public float bulletLifetime = 2f;
    public float bulletRotationSpeed = 1f;
    public float bulletSeekDistance = 4f;

    string playerTag = "Player";
    float cur_reloadTime;

    private void Start()
    {
        cur_reloadTime = reloadTime;
    }
    void Update()
    {
        cur_reloadTime -= 0.01f;
        if (cur_reloadTime < 0)
        {
            SeekAndDestroy();
            Shoot();
            Escape();
        }
    }
    private void SeekAndDestroy()
    {
        cur_reloadTime = reloadTime;
        //rotate towards player for 1-2sec
    }
    private void Escape()
    {
        //rotate in a random direction away from the player and escape
        //dirToPlayer* Quaternion.AngleAxis(-randomAngle, Vector3.up)
    }
    private void Shoot()
    {
        GameObject go = LeanPool.Spawn(bulletPrefab, shootPoint.position, transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * bulletSpeed);
        go.GetComponent<Projectile>().Init(bulletSpeed, bulletLifetime, gameObject.tag, "Enemy", bulletRotationSpeed, bulletSeekDistance);
    }
}
