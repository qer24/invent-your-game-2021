using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcLightningWeapon : Weapon, IDamager
{
    private Vector3 zapStart;
    private Vector3 target;
    public LineRenderer lineRend;
    public float arcLength = 1.0f;
    public float arcVariation = 1.0f;
    public float inaccuracy = 0.5f;
    public float timeOfZap = 0.25f;
    private float zapTimer;

    public float zapRange = 15f;
    public float checkRadius = 5f;

    public int chainCount = 3;
    public float chainDistance = 25f;

    HashSet<Collider> alreadyHitEnemies = new HashSet<Collider>();
    Collider lastHitEnemy = null;

    public Action<float, Transform> OnDealDamage { get; set; }

    void Start()
    {
        lineRend.transform.SetParent(null);
        DontDestroyOnLoad(lineRend.gameObject);

        lineRend.gameObject.SetActive(true);
        lineRend.transform.position = Vector3.up;
        lineRend.transform.localScale = Vector3.one;
        zapTimer = 0;
        lineRend.positionCount = 1;
    }

    public override void Attack(string enemyTag)
    {
        StartCoroutine(ZapCoroutine());
    }

    IEnumerator ZapCoroutine()
    {
        alreadyHitEnemies = new HashSet<Collider>();
        lastHitEnemy = null;

        Ray ray = new Ray(playerShooter.shootPoint.position, playerShooter.shootPoint.transform.forward);
        if(Physics.Raycast(ray, out var hit, zapRange))
        {
            alreadyHitEnemies.Add(hit.collider);
            lastHitEnemy = hit.collider;

            ZapTarget(hit.collider.transform.position, playerShooter.shootPoint.position, hit.transform);
        }
        else
        {
            var closest = GetClosestEnemyToRay(ray);
            if(closest != null)
            {
                alreadyHitEnemies.Add(closest);
                lastHitEnemy = closest;

                ZapTarget(closest.transform.position, playerShooter.shootPoint.position, closest.transform);
            }else
            {
                ZapTarget(ray.GetPoint(zapRange), playerShooter.shootPoint.position, null);
                yield break;
            }
        }

        yield return new WaitForSeconds(zapTimer + 0.01f);

        for (int i = 0; i < chainCount; i++)
        {
            if (lastHitEnemy == null) yield break;

            var colInRadius = Physics.OverlapSphere(lastHitEnemy.transform.position, chainDistance);
            if (colInRadius.Length > 0)
            {
                Collider closestEnemy = null;
                float closestDistance = Mathf.Infinity;
                foreach (var col in colInRadius)
                {
                    if (alreadyHitEnemies.Contains(col) || !col.CompareTag("Enemy")) continue;

                    float dist = Vector3.Distance(col.transform.position, playerShooter.transform.position);
                    if (dist < closestDistance)
                    {
                        closestEnemy = col;
                        closestDistance = dist;
                    }
                }

                if (closestEnemy == null) yield break;

                ZapTarget(closestEnemy.transform.position, lastHitEnemy.transform.position, closestEnemy.transform);

                alreadyHitEnemies.Add(closestEnemy);
                lastHitEnemy = closestEnemy;
            }
            else
            {
                yield break;
            }

            yield return new WaitForSeconds(zapTimer + 0.01f);
        }
    }

    void Update()
    {
        if (zapTimer > 0)
        {
            Vector3 lastPoint = zapStart;
            int i = 1;
            lineRend.SetPosition(0, zapStart);//make the origin of the LR the same as the transform
            while (Vector3.Distance(target, lastPoint) > 3.0f)
            {//was the last arc not touching the target?
                lineRend.positionCount = i + 1;//then we need a new vertex in our line renderer
                Vector3 fwd = target - lastPoint;//gives the direction to our target from the end of the last arc
                fwd.Normalize();//makes the direction to scale
                fwd = Randomize(fwd, inaccuracy);//we don't want a straight line to the target though
                fwd *= Random.Range(arcLength * arcVariation, arcLength);//nature is never too uniform
                fwd += lastPoint;//point + distance * direction = new point. this is where our new arc ends
                lineRend.SetPosition(i, fwd);//this tells the line renderer where to draw to
                i++;
                lastPoint = fwd;//so we know where we are starting from for the next arc
            }
            lineRend.positionCount = i + 1;
            lineRend.SetPosition(i, target);
            zapTimer = zapTimer - Time.deltaTime;
        }
        else
            lineRend.positionCount = 1;

    }

    private Vector3 Randomize(Vector3 newVector, float devation)
    {
        newVector += new Vector3(Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f)) * devation;
        newVector.Normalize();
        return newVector;
    }

    public void ZapTarget(Vector3 newTarget, Vector3 newStart, Transform damagable)
    {
        //print ("zap called");
        target = newTarget;
        zapStart = newStart;
        zapTimer = timeOfZap;

        foreach (var behaviour in OnDamageBehaviours)
        {
            if(!transform.Find($"{behaviour.name}(Clone)"))
                Instantiate(behaviour, transform).SetActive(true);
        }

        if (damagable != null)
        {
            if(damagable.TryGetComponent<IDamagable>(out var idamagable))
            {
                idamagable.TakeDamage(FinalDamage);
                OnDealDamage?.Invoke(FinalDamage, damagable);
            }
        }
    }

    Collider GetClosestEnemyToRay(Ray ray)
    {
        Vector3[] points = 
            {
            ray.GetPoint(zapRange * 0.2f),
            ray.GetPoint(zapRange * 0.4f),
            ray.GetPoint(zapRange * 0.6f),
            ray.GetPoint(zapRange * 0.8f),
            ray.GetPoint(zapRange)
            };

        HashSet<Collider> enemiesInRange = new HashSet<Collider>();
        foreach (var point in points)
        {
            var colliders = Physics.OverlapSphere(point, checkRadius);
            foreach (var col in colliders)
            {
                if(col.CompareTag("Enemy"))
                {
                    enemiesInRange.Add(col);
                }
            }
        }
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in enemiesInRange)
        {
            float dist = Vector3.Distance(enemy.transform.position, playerShooter.transform.position);
            if (dist < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = dist;
            }
        }

        if(closestEnemy != null)
        {
            return closestEnemy;
        }

        return null;
    }

    private void OnDestroy()
    {
        if(lineRend != null)
            Destroy(lineRend.gameObject);
    }

    //private void OnDrawGizmos()
    //{
    //    if (playerShooter == null) return;
    //    Ray ray = new Ray(playerShooter.shootPoint.position, playerShooter.shootPoint.transform.forward);

    //    Vector3[] points =
    //        {
    //        ray.GetPoint(zapRange * 0.2f),
    //        ray.GetPoint(zapRange * 0.4f),
    //        ray.GetPoint(zapRange * 0.6f),
    //        ray.GetPoint(zapRange * 0.8f),
    //        ray.GetPoint(zapRange)
    //        };

    //    foreach (var item in points)
    //    {
    //        Gizmos.DrawWireSphere(item, checkRadius);
    //    }
    //}
}

