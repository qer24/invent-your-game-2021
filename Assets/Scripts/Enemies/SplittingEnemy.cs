using Lean.Pool;
using ProcGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingEnemy : Enemy
{
    [Header("Splitting enemy")]
    public float moveForce = 25;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;

    public ParticleSystem reloadParticles, shootParticles;

    float randomAngle;
    int randomSign;
    Material enemyMaterial;

    TestEnemyState currentState;

    public int splitCount = 0;
    public float splitMultiplier = 1;

    public override void Start()
    {
        base.Start();

        currentState = TestEnemyState.Idle;

        enemyMaterial = GetComponentInChildren<Renderer>().material;

        if (splitCount == 0)
        {
            screenConfiner.enabled = false;

            //once spawned outside of the map go towards the center for x seconds - done
            Vector3 dir = Vector3.zero.normalized - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = targetRotation;
        }else
        {
            screenConfiner.enabled = true;
        }

        var main = reloadParticles.main;
        main.duration = timeBetweenActions;
        main.startLifetime = timeBetweenActions;

        health.currentHealth *= splitMultiplier;

        StartCoroutine(Behaviour(timeBetweenActions));
    }

    IEnumerator Behaviour(float waitTime)
    {
        if (splitCount == 0)
        {
            float distance = Vector3.Distance(transform.position, Vector3.zero);
            rb.AddForce(transform.forward * distance * distance * 0.5f);
            yield return new WaitForSeconds(waitTime);
            screenConfiner.enabled = true;
        }else
        {
            yield return new WaitForSeconds(waitTime);
        }
        while (true)
        {
            currentState = TestEnemyState.Reloading;
            reloadParticles.Play();
            yield return new WaitForSeconds(waitTime);
            Shoot();
            if (splitCount > 0)
            {
                for (int i = 0; i < splitCount; i++)
                {
                    yield return new WaitForSeconds(waitTime * 0.25f);
                    Shoot();
                }
            }
            currentState = TestEnemyState.Idle;


            randomAngle = Random.Range(60, 100);
            randomSign = Random.value < 0.5f ? 1 : -1;

            yield return new WaitForSeconds(waitTime * 0.5f);

            currentState = TestEnemyState.Escaping;
            yield return new WaitForSeconds(waitTime);
            currentState = TestEnemyState.Idle;
        }
    }
    void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        switch (currentState)
        {
            case TestEnemyState.Idle:
                return;
            case TestEnemyState.Reloading:
                rb.AddForce(transform.forward * moveForce * 0.0025f);
                break;
            case TestEnemyState.Escaping:
                rb.AddForce(transform.forward * moveForce * 0.02f);
                break;
        }
    }

    void Shoot()
    {
        shootParticles.Play();

        GameObject go = LeanPool.Spawn(bulletPrefab, shootPoint.position, transform.rotation);
        go.GetComponent<Renderer>().material = enemyMaterial;
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * enemyCard.projectileSpeed);
        go.transform.localScale *= 2f / (1 + splitCount * 0.5f);
        go.GetComponent<Projectile>().Init(
            enemyCard.projectileDamage * splitMultiplier,
            enemyCard.projectileSpeed * (1 + splitCount * 0.25f),
            enemyCard.projectileLifetime,
            gameObject.tag,
            playerTag
        );
    }

    public override void OnDeath()
    {
        if (splitCount < 3)
        {
            for (int i = 0; i < 2; i++)
            {
                SplittingEnemy enemy = Instantiate(gameObject, transform.position + Vector3.one * i, Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.up)).GetComponent<SplittingEnemy>();
                enemy.enemyCard = enemyCard;
                enemy.expValue = 1;
                enemy.splitCount = splitCount + 1;
                enemy.splitMultiplier = splitMultiplier * 0.5f;

                enemy.transform.localScale *= 0.75f;
                enemy.transform.position = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
                var dir = Quaternion.Euler(0, -90 * i, 0) * -transform.forward * 50 / (i+1);
                var enemyRb = enemy.GetComponent<Rigidbody>();
                enemyRb.mass = rb.mass * 0.75f;
                enemyRb.AddForce(dir * 25f);
                Random.InitState((int)Time.time);


                Room.enemiesAlive.Add(enemy.gameObject);
            }
        }
        
        base.OnDeath();
    }
}
