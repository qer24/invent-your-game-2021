using Lean.Pool;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using FMODUnity;

public class Boss : Enemy
{
    public float moveForce = 25;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;

    public Slider healthSlider;

    public Transform shootPoint;
    public ParticleSystem reloadParticles, shootParticles;

    public GameObject warningSign;

    [EventRef] public string music;

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
    public ProjectileBossOrb orbProjectile;
    public Aoe orbAoePrefab;
    public float orbAoeDamage = 1f;
    public Vector2 orbAoeSize = Vector2.one * 3f;
    public float orbAoeScaleTime = 0.25f;
    public float orbSpeed = 25f;
    public float orbChargeTime = 2f;
    public float orbProjectileSize = 3f;
    public int orbTicksPerDamage = 5;

    [Header("Shield")]
    public BossShield shield;
    public float shieldScaleTime = 1f;
    public LeanTweenType shieldScaleEaseType;

    [Header("Multishot attack")]
    public Projectile bullet;
    public float bulletSpeed;
    public float bulletLifetime;

    [Header("Audio")]
    [EventRef] public string warningAudio;
    [EventRef] public string waveAttackAudio;
    [EventRef] public string beamAttackAudio;
    [EventRef] public string beamAttackAudioEnd;
    [EventRef] public string spawnAddsAudio;
    [EventRef] public string orbAttackAudio;
    [EventRef] public string shieldAudioStart;
    [EventRef] public string shieldAudioEnd;
    [EventRef] public string bulletAttackAudio;

    FMOD.Studio.EventInstance beamAudioInstance;

    Material enemyMaterial;

    Transform beamTransform;
    Transform orbTransform = null;

    bool moving = true;
    Vector3 shieldSize;
    Collider mainCol;

    public static Action OnBossDeath;

    public override void Start()
    {
        base.Start();

        timeBetweenActions = (timeBetweenActions * 0.5f) + (timeBetweenActions * 0.5f - (0.025f * DifficultyManager.Instance.currentDifficulty));
        moveForce *= (1 + 0.05f * DifficultyManager.Instance.currentDifficulty);
        rotationSpeed *= (1 + 0.05f * DifficultyManager.Instance.currentDifficulty);

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

        shieldSize = shield.transform.localScale;
        mainCol = GetComponent<Collider>();
        shield.enemyMaterial = enemyMaterial;

        beamAudioInstance = RuntimeManager.CreateInstance(beamAttackAudio);

        transform.position = RoomManager.CurrentRoom.roomManager.WorldPositionFromSpawnPoint(RoomSpawnPoints.Right);

        StartCoroutine(Behaviour(timeBetweenActions));
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateUI;
    }

    private void Update()
    {
        if (PauseManager.paused)
        {
            beamAudioInstance.getPaused(out var pause);
            if (!pause)
            {
                beamAudioInstance.setPaused(true);
            }
            return;
        }

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (moving)
            rb.AddForce(transform.forward * moveForce * 2f * Time.deltaTime);

        if (beamTransform != null)
        {
            beamAudioInstance.setPaused(false);
            beamTransform.position = transform.position;
            beamTransform.rotation = transform.rotation;
            Physics.SyncTransforms();
        }
        if (orbTransform != null)
        {
            orbTransform.position = shootPoint.position;
            orbTransform.rotation = shootPoint.rotation;
        }
    }

    IEnumerator Behaviour(float waitTime)
    {
        MusicManager.Stop();
        moving = false;
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 3; i++)
        {
            AudioManager.Play(warningAudio, true);
            warningSign.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            warningSign.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        healthSlider.gameObject.SetActive(true);        
        moving = true;

        MusicManager.Play(music);
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(transform.forward * distance * distance * 0.5f);
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
            beamAudioInstance.start();
            beamIndicatorLine.widthMultiplier = startWidth;
            beamIndicatorLine.enabled = false;

            moving = false;

            yield return new WaitForSeconds(beamLifetime);
            beamAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.Play(beamAttackAudioEnd, true);
            yield return new WaitForSeconds(waitTime);

            // spawn pusher bois or shooty bois

            if (Random.value > 0.5f)
                StartCoroutine(SpawnPushers());
            else
                StartCoroutine(SpawnShooters());

            yield return new WaitForSeconds(waitTime * 2f);

            moving = true;

            yield return new WaitForSeconds(waitTime);

            // random attack either laser (5%) or spawn orb (55%) or shield phase (45%)
            float randomValue = Random.value;
            if (randomValue <= 0.05f)
            {
                //beam

                beamIndicatorLine.enabled = true;
                beamIndicatorLine.widthMultiplier = 0;
                LeanTween.value(gameObject, 0, beamSize.x * 0.6f, waitTime).setOnUpdate((float val) => beamIndicatorLine.widthMultiplier = val).setEase(LeanTweenType.easeInCubic);

                yield return new WaitForSeconds(waitTime);

                ShootBeam();
                beamAudioInstance.start();
                beamIndicatorLine.widthMultiplier = startWidth;
                beamIndicatorLine.enabled = false;

                yield return new WaitForSeconds(beamLifetime);
                beamAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                AudioManager.Play(beamAttackAudioEnd, true);
                yield return new WaitForSeconds(waitTime);
            }
            else if (randomValue > 0.05f && randomValue <= 0.55f)
            {
                //orb


                Vector3 dirToPlayer = (player.position - shootPoint.position).normalized;
                float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
                var orb = Instantiate(orbProjectile, shootPoint.position, targetRotation);

                orb.gameObject.SetActive(true);
                var rend = orb.GetComponentInChildren<Renderer>();
                orb.InitOrb(
                    orbAoeDamage,
                    orbAoePrefab.gameObject,
                    new Vector3(orbAoeSize.x, orb.transform.localScale.y, orbAoeSize.y),
                    orbAoeScaleTime,
                    orbSpeed,
                    999,
                    "Player",
                    orbTicksPerDamage
                );
                orb.transform.localScale = Vector3.zero;
                AudioManager.Play(orbAttackAudio, true);
                LeanTween.scale(orb.gameObject, new Vector3(orbProjectileSize, orbProjectileSize, orbProjectileSize * 0.825f), orbChargeTime * 0.9f);

                orbTransform = orb.transform;

                yield return new WaitForSeconds(orbChargeTime);

                orb.FinishOrb();
                orbTransform = null;
            }
            else
            {
                //shield

                moving = false;

                shield.gameObject.SetActive(true);
                mainCol.enabled = false;

                shield.transform.localScale = Vector3.zero;

                LeanTween.scale(shield.gameObject, shieldSize, shieldScaleTime).setEase(shieldScaleEaseType);
                AudioManager.Play(shieldAudioStart, true);

                yield return new WaitForSeconds(shieldScaleTime + waitTime * 4f);

                LeanTween.scale(shield.gameObject, Vector3.zero, shieldScaleTime * 0.25f).setOnComplete(() => shield.gameObject.SetActive(false));

                mainCol.enabled = true;
                yield return new WaitForSeconds(shieldScaleTime * 0.25f);

                moving = true;
            }

            yield return new WaitForSeconds(waitTime);

            reloadParticles.Play();
            yield return new WaitForSeconds(waitTime);
            AudioManager.Play(bulletAttackAudio, true);
            ShootBullets();

            yield return new WaitForSeconds(waitTime);
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
        AudioManager.Play(waveAttackAudio, true);

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
        var aoe = LeanPool.Spawn(beamPrefab, transform.position, transform.rotation, null);
        aoe.gameObject.SetActive(true);
        var rend = aoe.GetComponentInChildren<Renderer>();
        rend.material = enemyMaterial;
        rend.material.SetFloat("_NoiseScale", 0f);
        aoe.Init(beamDamage, beamLifetime, new Vector3(beamSize.x, 1, beamSize.y), "Player");
        aoe.constantDamage = true;
        aoe.ticksPerDamage = beamTicksPerDamage;

        beamTransform = aoe.transform;
    }

    IEnumerator SpawnPushers()
    {
        yield return null;
        for (int i = 0; i < 2; i++)
        {
            AudioManager.Play(spawnAddsAudio, true);

            shootParticles.transform.position = spawnPositions[i].position;
            shootParticles.Play();

            Enemy enemy = Instantiate(pusherCard.prefab, spawnPositions[i].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.enemyCard = pusherCard;
            enemy.expValue = 1;
            Room.enemiesAlive.Add(enemy.gameObject);

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnShooters()
    {
        yield return null;
        for (int i = 0; i < 2; i++)
        {
            AudioManager.Play(spawnAddsAudio, true);

            shootParticles.transform.position = spawnPositions[i].position;
            shootParticles.Play();

            Enemy enemy = Instantiate(shooterCard.prefab, spawnPositions[i].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.enemyCard = shooterCard;
            enemy.expValue = 1;
            Room.enemiesAlive.Add(enemy.gameObject);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void ShootBullets()
    {
        shootParticles.transform.position = shootPoint.position;
        shootParticles.Play();

        for (int i = -2; i <= 2; i++)
        {
            float angle = i * 20;

            var spawnedBullet = LeanPool.Spawn(bullet, shootPoint.position, shootPoint.rotation * Quaternion.AngleAxis(angle, Vector3.up));
            spawnedBullet.GetComponent<Renderer>().material = enemyMaterial;
            spawnedBullet.GetComponent<Rigidbody>().AddForce(spawnedBullet.transform.forward * enemyCard.projectileSpeed);
            spawnedBullet.transform.localScale *= 1.6f;
            spawnedBullet.GetComponent<Projectile>().Init(
                enemyCard.projectileDamage,
                bulletSpeed,
                bulletLifetime,
                playerTag
            );
        }
    }

    private void OnDestroy()
    {
        MusicManager.Stop();

        OnBossDeath?.Invoke();
        foreach (Action a in OnBossDeath.GetInvocationList())
        {
            if(a != null)
                OnBossDeath -= a;
        }

        beamAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if(beamTransform != null)
        {
            Destroy(beamTransform.gameObject);
        }
        if(orbTransform != null)
        {
            Destroy(orbTransform.gameObject);
        }
    }

    void UpdateUI(float currentHealth)
    {
        LeanTween.value(gameObject, healthSlider.value, currentHealth / FinalHealth, 0.4f).setOnUpdate((float val) => healthSlider.value = val).setEase(LeanTweenType.easeOutCubic);
    }
}
