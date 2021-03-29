using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnemy : Enemy
{
    [Header("TrailEnemy")]
    public float moveForce = 400;
    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;
    public float playerFollowPointAnglePerSecond = 5;
    public float playerFollowPointRadius = 3;

    Material enemyMaterial;

    float degrees;
    float hitCooldown = 0f;

    public override void Start()
    {
        base.Start();

        GetComponentInChildren<DamagingTrailParticles>().damage = enemyCard.nonProjectileDamage;
        enemyMaterial = GetComponentInChildren<Renderer>().material;

        screenConfiner.enabled = false;

        //once spawned outside of the map go towards the center for x seconds - done
        Vector3 dir = Vector3.zero.normalized - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;
        StartCoroutine(Behaviour(timeBetweenActions));

    }

    IEnumerator Behaviour(float waitTime)
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(transform.forward * distance * distance * 0.125f);
        yield return new WaitForSeconds(waitTime);
        screenConfiner.enabled = true;

        float oldRadius = playerFollowPointRadius;
        float oldRotationSpeed = rotationSpeed;
        float oldSpeed = moveForce;
        while(true)
        {
            yield return new WaitForSeconds(waitTime * 3f);
            playerFollowPointRadius = 1;
            rotationSpeed = 20;
            moveForce *= 2;
            yield return new WaitForSeconds(waitTime);
            playerFollowPointRadius = oldRadius;
            rotationSpeed = oldRotationSpeed;
            moveForce = oldSpeed;
        }
    }
    void Update()
    {
        if (!screenConfiner.enabled) return;

        degrees += Time.deltaTime * playerFollowPointAnglePerSecond;
        var radians = degrees * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);
        var pos = player.position + (new Vector3(x, 0, y) * playerFollowPointRadius);

        var dirToPlayer = pos - transform.position;
        var angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.forward * moveForce * 2f * Time.deltaTime);

        hitCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if (hitCooldown > 0) return;

        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            //other.GetComponent<Rigidbody>().AddForce(transform.forward * playerKnockbackForce);
            damagable.TakeDamage(enemyCard.nonProjectileDamage);
            hitCooldown = 0.5f;
        }
    }
}
