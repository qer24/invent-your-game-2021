using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectileBehaviour : ProjectileBehaviour
{
    public override void Start()
    {
        base.Start();

        projectile.OnCollision += Bounce;
    }

    private void OnDestroy()
    {
        projectile.OnCollision -= Bounce;
    }

    void Bounce(Collider col)
    {
        projectile.despawnOnCollision = false;

        Vector3 rot = projectile.transform.rotation.eulerAngles;
        float randomRot = Random.Range(135, 225);
        rot = new Vector3(rot.x, rot.y + randomRot, rot.z);
        projectile.transform.rotation = Quaternion.Euler(rot);
    }
}
