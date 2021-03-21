using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingEnemy : Enemy
{
    [Header("Pusher")]
    public float rotationSpeed = 10f;
    public float moveForce = 25;
    public float boostForce = 2400;
    //public float playerKnockbackForce = 400f;

    float hitCooldown = 0f;

    public override void Start()
    {
        base.Start();

        screenConfiner.enabled = false;
        StartCoroutine(Behaviour());
    }
    
    IEnumerator Behaviour()
    {
        Vector3 dir = Vector3.zero.normalized - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;

        float distance = Vector3.Distance(transform.position, Vector3.zero);
        rb.AddForce(transform.forward * distance * distance * 0.25f);

        yield return new WaitForSeconds(2f);
        screenConfiner.enabled = true;

        InvokeRepeating("Boost", 1f, 2f);
    }

    void Update()
    {
        if (PauseManager.paused) return;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.forward * moveForce * 0.02f);

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

    void Boost()
    {
        rb.AddForce(transform.forward * boostForce * 0.02f, ForceMode.Impulse);
    }
}
