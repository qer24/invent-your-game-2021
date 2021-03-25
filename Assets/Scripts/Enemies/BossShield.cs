using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [FMODUnity.EventRef] public string bounceAudio;
    [HideInInspector] public Material enemyMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.enemyTag == "Player") return;

            AudioManager.Play(bounceAudio, true);

            Vector3 rot = proj.transform.rotation.eulerAngles;
            float randomRot = Random.Range(160, 200);
            rot = new Vector3(rot.x, rot.y + randomRot, rot.z);
            proj.transform.rotation = Quaternion.Euler(rot);

            proj.Init(proj.damage * 0.25f, proj.velocity, proj.totalLifeTime, "Player");
            proj.GetComponentInChildren<Renderer>().material = enemyMaterial;
        }
    }
}
