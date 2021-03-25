using Lean.Pool;
using ProcGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : Enemy
{
    [Header("Snake enemy")]
    public float moveForce = 25;
    public Transform shootPoint;
    public Transform barrel;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;
    public float barrelRotationSpeed = 10f;

    public ParticleSystem reloadParticles, shootParticles;

    Material enemyMaterial;

    bool ready;

    public SnakeEnemy parent;
    public float minDistance = 4f;
    public float followSpeed = 10f;
    public float playerFollowPointAnglePerSecond = 5;
    public float playerFollowPointRadius = 3;

    public SnakeEnemy[] children;

    float degrees;

    public override void Start()
    {
        base.Start();

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

        if(parent == null)
        {
            int segmentCount = Random.Range(2, 5);
            for (int i = children.Length - 1; i >= segmentCount; i--)
            {
                Destroy(children[i].gameObject);
            }
        }else
        {
            Room.enemiesAlive.Add(gameObject);
            transform.parent = null;
            expValue = 1;
        }


        ready = false;
        StartCoroutine(Behaviour(timeBetweenActions));
    }

    IEnumerator Behaviour(float waitTime)
    {
        if (parent == null)
        {
            float distance = Vector3.Distance(transform.position, Vector3.zero);
            rb.AddForce(transform.forward * distance * distance * 0.125f);
            yield return new WaitForSeconds(waitTime);
            screenConfiner.enabled = true;
        }
        yield return new WaitForSeconds(Random.Range(waitTime * 0.25f, waitTime * 2f));
        ready = true;
        while (true)
        {
            reloadParticles.Play();
            yield return new WaitForSeconds(waitTime);
            Shoot();

            yield return new WaitForSeconds(waitTime);
        }
    }
    void Update()
    {
        if (PauseManager.paused || player == null) return;

        Vector3 dirToPlayer = PredictedPosition(player.position, player.velocity, enemyCard.projectileSpeed) - transform.position;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        barrel.rotation = Quaternion.Slerp(barrel.rotation, targetRotation, barrelRotationSpeed * Time.deltaTime);
        if(parent != null)
        {
            var dist = Vector3.Distance(parent.transform.position, transform.position);

            Vector3 newpos = parent.transform.position;


            float T = Time.deltaTime * dist / minDistance * followSpeed;

            if (T > 0.5f)
                T = 0.5f;
            transform.position = Vector3.Slerp(transform.position, newpos, T);
            Physics.SyncTransforms();
            //curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, T);


            Vector3 dirToParent = parent.transform.position - transform.position;
            angle = Mathf.Atan2(dirToParent.x, dirToParent.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 8f);
            //var distance = Vector3.Distance(transform.position, parent.transform.position);
            //if (distance > minDistance)
            //    rb.AddForce(transform.forward * moveForce * 0.01f * distance);
        }else
        {
            if (!screenConfiner.enabled && ready) screenConfiner.enabled = true;

            degrees += Time.deltaTime * playerFollowPointAnglePerSecond;
            var radians = degrees * Mathf.Deg2Rad;
            var x = Mathf.Cos(radians);
            var y = Mathf.Sin(radians);
            var pos = player.position + (new Vector3(x, 0, y) * playerFollowPointRadius);

            dirToPlayer = pos - transform.position;
            angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.AddForce(transform.forward * moveForce * 2f * Time.deltaTime);
        }
    }

    private Vector3 PredictedPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        //calculate distance to player with pythagoras
        float distance = Vector3.Distance(transform.position, targetPosition);
        //calculate traveltime
        float travelTime = distance / projectileSpeed;
        return targetPosition + targetVelocity * travelTime;
    }

    void Shoot()
    {
        shootParticles.Play();
        AudioManager.Play("event:/SFX/Enemies/EnemyShoot", true);

        GameObject go = LeanPool.Spawn(enemyCard.projectile, shootPoint.position, shootPoint.rotation);
        go.GetComponent<Renderer>().material = enemyMaterial;
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * enemyCard.projectileSpeed);
        go.GetComponent<Projectile>().Init(
            enemyCard.projectileDamage,
            enemyCard.projectileSpeed,
            enemyCard.projectileLifetime,
            playerTag
        );
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        var radians = degrees * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);
        var pos = player.position + new Vector3(x, 0, y) * playerFollowPointRadius;

        Gizmos.DrawWireSphere(pos, 0.5f);
    }
}
