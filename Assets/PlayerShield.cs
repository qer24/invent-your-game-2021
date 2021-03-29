using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [FMODUnity.EventRef] public string bounceAudio;
    public float rotationSpeed = 5f;

    public Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.enemyTag == "Enemy") return;

            AudioManager.Play(bounceAudio, true);
            Lean.Pool.LeanPool.Despawn(proj.gameObject);
        }
    }

    private void LateUpdate()
    {
        transform.parent.position = playerTransform.position;
        transform.parent.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        Physics.SyncTransforms();
    }
}
