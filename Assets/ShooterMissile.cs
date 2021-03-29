using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMissile : Enemy
{
    [Header("Test enemy")]
    public float moveForce = 25;
    public Transform shootPoint;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;

    public ParticleSystem reloadParticles, shootParticles;

    public GameObject aoePrefab;
    public Vector2 aoeSize;
    public float aoeLifeTime;

    Vector3 aoeConvertedSize { get => new Vector3(aoeSize.x, 1, aoeSize.y); }

    float randomAngle;
    int randomSign;
    Material enemyMaterial;

    TestEnemyState currentState;

    public override void Start()
    {
        base.Start();

        currentState = TestEnemyState.Idle;

        enemyMaterial = GetComponentInChildren<Renderer>().material;

        screenConfiner.enabled = false;

        //once spawned outside of the map go towards the center for x seconds - done
        Vector3 dir = Vector3.zero.normalized - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;

        var main = reloadParticles.main;
        main.duration = timeBetweenActions;
        main.startLifetime = timeBetweenActions;

        StartCoroutine(Behaviour(timeBetweenActions));
    }

    IEnumerator Behaviour(float waitTime)
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(transform.forward * distance * distance * 0.25f);
        yield return new WaitForSeconds(waitTime);
        screenConfiner.enabled = true;
        yield return new WaitForSeconds(waitTime * Random.Range(0.1f, 0.8f));
        while (true)
        {
            currentState = TestEnemyState.Reloading;
            reloadParticles.Play();
            yield return new WaitForSeconds(waitTime);
            Shoot();
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
        if (PauseManager.paused || player == null) return;
        if (currentState == TestEnemyState.Idle) return;

        Vector3 dirToPlayer = player.position - transform.position;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        switch (currentState)
        {
            case TestEnemyState.Idle:
                return;

            case TestEnemyState.Reloading:
                //rotate towards player for 1-2sec - done
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                rb.AddForce(transform.forward * moveForce * 0.25f * Time.deltaTime);
                break;

            case TestEnemyState.Escaping:
                //rotate in a random direction away from the player and escape - done
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * Quaternion.AngleAxis(randomAngle * randomSign, Vector3.up), rotationSpeed * Time.deltaTime);
                rb.AddForce(transform.forward * moveForce * 2f * Time.deltaTime);
                break;
        }
    }

    void Shoot()
    {
        shootParticles.Play();
        AudioManager.Play("event:/SFX/Enemies/EnemyShoot", true);

        GameObject go = LeanPool.Spawn(enemyCard.projectile, shootPoint.position, shootPoint.rotation);
        go.GetComponentInChildren<Renderer>().material = enemyMaterial;
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * enemyCard.projectileSpeed);
        go.GetComponent<ProjectileEnemyMissile>().InitMissile(
            enemyCard.projectileDamage,
            enemyCard.projectileSpeed,
            enemyCard.projectileLifetime,
            playerTag,
            aoePrefab,
            aoeConvertedSize,
            aoeLifeTime
        );
    }
}
