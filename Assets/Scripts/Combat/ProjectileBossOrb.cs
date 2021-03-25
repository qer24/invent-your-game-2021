using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossOrb : Projectile
{
    [FMODUnity.EventRef]
    public string explosionAudio;
    public float explodingTime;

    GameObject aoePrefab;
    float aoeScaleTime;
    Vector3 aoeSize;
    float aoeDamage = 1;
    int aoeTicksPerDamage = 5;

    float velocityAfterCreation;
    string enemyTagAfterCreation;

    Transform gfx;

    bool exploding = false;

    public void InitOrb(float _aoeDamage, GameObject _aoePrefab, Vector3 _aoeSize, float _aoeScaleTime, float _velocity, float _lifetime, string _enemyTag, int _aoeTicksPerDamage)
    {
        Init(0, 0, _lifetime, "Finish");
        aoeDamage = _aoeDamage;
        aoePrefab = _aoePrefab;
        aoeSize = _aoeSize;
        aoeScaleTime = _aoeScaleTime;

        velocityAfterCreation = _velocity;
        enemyTagAfterCreation = _enemyTag;

        aoeTicksPerDamage = _aoeTicksPerDamage;

        despawnOnCollision = false;

        gfx = GetComponentInChildren<Renderer>().transform;

        Boss.OnBossDeath += () => 
        {
            if (gameObject == null) return;
            Destroy(gameObject);
        };
    }

    protected override void Update()
    {
        if (exploding) return;

        base.Update();

        gfx.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public void FinishOrb()
    {
        velocity = velocityAfterCreation;
        enemyTag = enemyTagAfterCreation;

        GetComponent<HomingProjectileBehaviour>().enemyTag = enemyTag;
    }

    private void Start()
    {
        OnCollision += CreateAoe;
    }

    private void OnDestroy()
    {
        OnCollision -= CreateAoe;
    }

    public void CreateAoe(Collider col)
    {
        OnCollision -= CreateAoe;

        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine()
    {
        exploding = true;

        AudioManager.Play(explosionAudio, true);

        LeanTween.scale(gameObject, transform.localScale * 1.5f, explodingTime * 0.95f).setOnComplete(() => LeanTween.scale(gameObject, Vector3.zero, explodingTime * 0.05f));

        velocity = 0;
        GetComponent<HomingProjectileBehaviour>().rotationSpeed = 0;

        yield return new WaitForSeconds(explodingTime);

        Aoe aoe = Lean.Pool.LeanPool.Spawn(aoePrefab, transform.position, Quaternion.identity).GetComponent<Aoe>();
        aoe.gameObject.SetActive(true);
        aoe.Init(aoeDamage, 999f, aoeSize, enemyTag, () =>
        {
            aoe.transform.localScale = Vector3.zero;
            LeanTween.scale(aoe.gameObject, aoeSize, aoeScaleTime).setEase(aoe.inType);
        }
        );
        aoe.constantDamage = true;
        aoe.ticksPerDamage = aoeTicksPerDamage;

        Boss.OnBossDeath += () => LeanTween.scale(aoe.gameObject, Vector3.zero, 0.5f).setOnComplete(() => Destroy(aoe.gameObject));
        Destroy(gameObject);
    }
}
