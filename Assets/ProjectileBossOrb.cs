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

    public void InitOrb(float _aoeDamage, GameObject _aoePrefab, Vector3 _aoeSize, float _aoeScaleTime, float _velocity, float _lifetime, string _enemyTag)
    {
        Init(0, _velocity, _lifetime, _enemyTag);
        aoeDamage = _aoeDamage;
        aoePrefab = _aoePrefab;
        aoeSize = _aoeSize;
        aoeScaleTime = _aoeScaleTime;
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
        AudioManager.Play(explosionAudio, true);

        LeanTween.scale(gameObject, transform.localScale * 1.3f, explodingTime * 0.95f).setOnComplete(() => LeanTween.scale(gameObject, Vector3.zero, explodingTime * 0.05f));

        yield return new WaitForSeconds(explodingTime);

        Aoe aoe = Lean.Pool.LeanPool.Spawn(aoePrefab, transform.position, Quaternion.identity).GetComponent<Aoe>();
        aoe.Init(aoeDamage, 999f, aoeSize, enemyTag, () =>
        {
            aoe.transform.localScale = Vector3.zero;
            LeanTween.scale(aoe.gameObject, aoeSize, aoeScaleTime).setEase(aoe.inType);
        }
        );
        aoe.constantDamage = true;
    }
}
