using Lean.Pool;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    public float moveForce = 25;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;

    public Slider healthSlider;

    public Transform shootPoint;
    public ParticleSystem reloadParticles, shootParticles;

    [Header("Wave attack")]
    public Projectile waveProjectile;
    public Transform[] waveShootPoints;

    [Header("Beam attack")]
    public LineRenderer beamIndicatorLine;
    public Aoe beamPrefab;
    public float beamDamage;
    public Vector2 beamSize;
    public float beamLifetime;
    public int beamTicksPerDamage = 30;

    [Header("Spawn adds")]
    public Transform[] spawnPositions;
    public EnemyCard pusherCard;
    public EnemyCard shooterCard;

    [Header("Orb attack")]
    public Projectile orbProjectile;
    public Aoe orbAoePrefab;
    public float orbAoeDamage = 1;
    public Vector3 orbAoeSize = Vector3.one * 3f;

    [Header("Shield")]
    public GameObject shield;

    [Header("Multishot attack")]
    public Projectile bullet;

    Material enemyMaterial;

    Transform beamTransform;

    bool moving = true;

    public override void Start()
    {
        base.Start();

        Vector3 dir = Vector3.zero.normalized - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;

        enemyMaterial = GetComponentInChildren<Renderer>().material;

        screenConfiner.enabled = false;

        var main = reloadParticles.main;
        main.duration = timeBetweenActions;
        main.startLifetime = timeBetweenActions;

        beamIndicatorLine.enabled = false;

        health.OnHealthChanged += UpdateUI;

        StartCoroutine(Behaviour(timeBetweenActions));
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateUI;
    }

    private void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if(moving)
            rb.AddForce(transform.forward * moveForce * 0.02f);

        if (beamTransform != null)
        {
            beamTransform.position = transform.position;
            beamTransform.rotation = transform.rotation;
            Physics.SyncTransforms();
        }
    }

    IEnumerator Behaviour(float waitTime)
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(transform.forward * distance * distance * 0.25f);
        yield return new WaitForSeconds(waitTime);
        screenConfiner.enabled = true;
        //wave attack
        StartCoroutine(RepeatedWaveAttack(waitTime));
        while (true)
        {
            yield return new WaitForSeconds(waitTime * 2f);

            // beam attack
            beamIndicatorLine.enabled = true;
            float startWidth = beamIndicatorLine.widthMultiplier;
            beamIndicatorLine.widthMultiplier = 0;
            LeanTween.value(gameObject, 0, beamSize.x * 0.6f, waitTime).setOnUpdate((float val) => beamIndicatorLine.widthMultiplier = val).setEase(LeanTweenType.easeInCubic);

            yield return new WaitForSeconds(waitTime);

            ShootBeam();
            beamIndicatorLine.widthMultiplier = startWidth;
            beamIndicatorLine.enabled = false;

            moving = false;

            yield return new WaitForSeconds(beamLifetime);

            // spawn pusher bois or shooty bois

            if(Random.value > 0.5f)
                SpawnPushers();
            else
                SpawnShooters();

            yield return new WaitForSeconds(beamLifetime);

            moving = true;

            yield return new WaitForSeconds(waitTime);

            // random attack either laser (5%) or spawn orbs (35%) or shield phase (60%)
            float randomValue = Random.value;
            if(randomValue <= 0.05f)
            {
                //beam
                beamIndicatorLine.enabled = true;
                beamIndicatorLine.widthMultiplier = 0;
                LeanTween.value(gameObject, 0, beamSize.x * 0.6f, waitTime).setOnUpdate((float val) => beamIndicatorLine.widthMultiplier = val).setEase(LeanTweenType.easeInCubic);

                yield return new WaitForSeconds(waitTime);

                ShootBeam();
                beamIndicatorLine.widthMultiplier = startWidth;
                beamIndicatorLine.enabled = false;
            }else if(randomValue > 0.05f && randomValue <= 0.35f)
            {
                //orbs
            }else
            {
                //shield
            }

            // shoot 5 bullets
        }
    }


    IEnumerator RepeatedWaveAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            // wave attack
            shootParticles.transform.position = waveShootPoints[0].position;
            ShootWave(45, waveShootPoints[0].position);
            yield return new WaitForSeconds(waitTime * 0.1f);
            shootParticles.transform.position = waveShootPoints[1].position;
            ShootWave(-45, waveShootPoints[1].position);

            yield return new WaitForSeconds(waitTime * Random.Range(1.2f, 2.5f));
        }
    }

    void ShootWave(float angle, Vector3 pos)
    {
        shootParticles.Play();
        //AudioManager.Play("event:/SFX/Enemies/EnemyShoot", true);

        var proj = LeanPool.Spawn(waveProjectile, pos, shootPoint.rotation);
        var rend = proj.GetComponentInChildren<Renderer>();
        rend.material = enemyMaterial;
        rend.material.SetFloat("_NoiseScale", 0f);
        proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * enemyCard.projectileSpeed);
        proj.transform.Rotate(0, angle, 0);
        proj.transform.localScale = new Vector3(1.3f, 1f, 1.1f);
        proj.Init(
            enemyCard.projectileDamage,
            enemyCard.projectileSpeed,
            enemyCard.projectileLifetime,
            playerTag
        );
    }

    void ShootBeam()
    {
        var aoe = LeanPool.Spawn(beamPrefab, transform.position, transform.rotation);
        var rend = aoe.GetComponentInChildren<Renderer>();
        rend.material = enemyMaterial;
        rend.material.SetFloat("_NoiseScale", 0f);
        aoe.Init(beamDamage, beamLifetime, new Vector3(beamSize.x, 1, beamSize.y), "Player");
        aoe.constantDamage = true;
        aoe.ticksPerDamage = beamTicksPerDamage;

        beamTransform = aoe.transform;
    }

    void SpawnPushers()
    {
        for (int i = 0; i < 2; i++)
        {
            shootParticles.transform.position = spawnPositions[i].position;
            shootParticles.Play();

            Enemy enemy = Instantiate(pusherCard.prefab, spawnPositions[i].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.enemyCard = pusherCard;
            enemy.expValue = 1;
            Room.enemiesAlive.Add(enemy.gameObject);
        }
    }

    void SpawnShooters()
    {
        for (int i = 0; i < 2; i++)
        {
            shootParticles.transform.position = spawnPositions[i].position;
            shootParticles.Play();

            Enemy enemy = Instantiate(shooterCard.prefab, spawnPositions[i].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.enemyCard = shooterCard;
            enemy.expValue = 1;
            Room.enemiesAlive.Add(enemy.gameObject);
        }
    }

    void UpdateUI(float currentHealth)
    {
        LeanTween.value(gameObject, healthSlider.value, currentHealth / enemyCard.maxHealth, 0.4f).setOnUpdate((float val) => healthSlider.value = val).setEase(LeanTweenType.easeOutCubic);
    }
}
