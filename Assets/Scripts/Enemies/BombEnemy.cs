using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{
    public float moveForce = 25;
    public int projectileCount = 8;

    Material enemyMaterial;

    public override void Start()
    {
        base.Start();

        enemyMaterial = GetComponentInChildren<Renderer>().material;

        screenConfiner.enabled = false;
        StartCoroutine(Behaviour());
    }

    IEnumerator Behaviour()
    {
        Vector3 dir = (Vector3.zero - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(dir * distance * distance * 0.25f);

        yield return new WaitForSeconds(2f);

        screenConfiner.enabled = true;
    }
    
    void Explode()
    {
        CinemachineShake.Instance.ShakeCamera(20, 0.6f);

        float anglePerProjectile = 360 / projectileCount;
        float angle = Random.Range(90, 180);
        for (int i = 0; i < projectileCount; i++)
        {
            ShootBullet(Quaternion.AngleAxis(angle, Vector3.up));
            angle += anglePerProjectile;
        }
    }

    void ShootBullet(Quaternion rotation)
    {
        GameObject go = LeanPool.Spawn(enemyCard.projectile, transform.position, rotation);
        go.GetComponent<Renderer>().material = enemyMaterial;
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * enemyCard.projectileSpeed);
        go.GetComponent<Projectile>().Init(
            enemyCard.projectileDamage,
            enemyCard.projectileSpeed,
            enemyCard.projectileLifetime,
            playerTag
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Damagable>().TakeDamage(health.currentHealth);
        }
    }

    public override void OnDeath()
    {
        if (!string.IsNullOrEmpty(enemyCard.onDeathAudio))
        {
            AudioManager.Play(enemyCard.onDeathAudio, true);
        }

        Explode();

        PlayerUpgradeManager.Instance.AddExp(expValue);
        EndScreen.EnemiesDestroyed++;
        Destroy(gameObject);
    }
}
