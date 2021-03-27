using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinteringProjectileBehaviour : ProjectileBehaviour
{
    // DOESNT WORK I GAVE UP

    //public override void Start()
    //{
    //    base.Start();

    //    projectile.OnDespawn += Splinter;
    //}

    //private void OnDestroy()
    //{
    //    if(projectile != null)
    //        projectile.OnDespawn -= Splinter;
    //}

    //void Splinter()
    //{
    //    Quaternion randomRot = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        GameObject go = LeanPool.Spawn(projectile.gameObject, projectile.transform.position, projectile.transform.rotation * Quaternion.AngleAxis(i * 90, Vector3.up) * randomRot);

    //        Destroy(go.GetComponent<Projectile>());
    //        var proj = go.AddComponent(projectile);

    //        Destroy(proj.GetComponent<SplinteringProjectileBehaviour>());

    //        proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * projectile.velocity);
    //        proj.GetComponentInChildren<Renderer>().material = projectile.GetComponentInChildren<Renderer>().material;

    //        proj.damage = projectile.damage * 0.4f;
    //        proj.velocity = projectile.velocity * 0.4f;
    //        proj.totalLifeTime = projectile.totalLifeTime * 0.2f;
    //        proj.lifeTimeLeft = proj.totalLifeTime;

    //        proj.startScale = projectile.transform.localScale;
    //        proj.scaleThreshold = proj.totalLifeTime * 0.6f;
    //        proj.disableDamageThreshold = proj.totalLifeTime * 0.2f;

    //        proj.ingoreCollisionTime = 0.5f;

    //        proj.despawnOnCollision = true;
    //    }
    //}
}
