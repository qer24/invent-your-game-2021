using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] AnimationCurve scalingCurve = null;
    [SerializeField] GameObject impactPrefab = null;

    float velocity = 500;
    float totalLifeTime = 5f;
    float lifeTimeLeft = 5f;

    float scaleThreshold = 2f;
    float disableDamageThreshold = 1f;

    float rotationSpeed = 5;
    float seekDistance = 4f;

    string alliedTag = "Player";
    string enemyTag = "Enemy";

    Vector3 startScale;

    public void Init(float _velocity, float _lifetime, string _alliedTag, string _enemyTag, float _rotationSpeed, float _seekDistance)
    {
        velocity = _velocity;
        totalLifeTime = _lifetime;
        alliedTag = _alliedTag;
        enemyTag = _enemyTag;
        rotationSpeed = _rotationSpeed;
        seekDistance = _seekDistance;

        lifeTimeLeft = totalLifeTime;

        startScale = transform.localScale;
        scaleThreshold = totalLifeTime * 0.6f;
        disableDamageThreshold = totalLifeTime * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward.normalized * velocity;

        lifeTimeLeft -= Time.deltaTime;
        if (lifeTimeLeft < 0)
        {
            Despawn();
            return;
        }

        Transform target = FindClosestTarget();
        if (target != null)
        {
            RotateToTarget(target);
        }
        if (lifeTimeLeft < scaleThreshold)
        {
            Vector3 scale = Vector3.Lerp(startScale, Vector3.zero, 1 - scalingCurve.Evaluate((scaleThreshold - lifeTimeLeft) / scaleThreshold));
            transform.localScale = scale;
        }
    }

    Transform FindClosestTarget()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(enemyTag);

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (var target in allTargets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > seekDistance)
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closest = target;
                closestDistance = distance;
            }
        }

        if (closest == null) return null;

        Debug.DrawLine(transform.position, closest.transform.position, Color.red);
        return closest.transform;
    }

    private void RotateToTarget(Transform target)
    {
        Vector3 direction = target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.forward).y;
        rb.angularVelocity = new Vector3(0, -rotationSpeed * rotateAmount, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lifeTimeLeft < disableDamageThreshold) return;
        if (other.CompareTag(enemyTag))
        {
            LeanPool.Spawn(impactPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
            Despawn();
        }
    }

    void Despawn()
    {
        LeanPool.Despawn(gameObject);
    }
}
