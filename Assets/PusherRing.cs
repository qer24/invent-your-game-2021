using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherRing : Enemy
{
    [Header("Pusher")]
    public float rotationSpeed = 10f;
    public float moveForce = 25;
    public float boostForce = 2400;
    //public float playerKnockbackForce = 400f;

    public Aoe ringAoe;
    public Vector2 aoeSize;

    Vector3 convertedAoeSize { get => new Vector3(aoeSize.x, 1, aoeSize.y); }

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

        ringAoe.gameObject.SetActive(true);
        ringAoe.Init(enemyCard.nonProjectileDamage, 9999, convertedAoeSize, playerTag, () =>
        {
            ringAoe.transform.localScale = Vector3.zero;
            LeanTween.scale(ringAoe.gameObject, convertedAoeSize, 1f).setEase(ringAoe.inType);
        });

        InvokeRepeating("Boost", 1f, 2f);
    }

    void Update()
    {
        if (PauseManager.paused || player == null) return;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.forward * moveForce * 2f * Time.deltaTime);

        hitCooldown -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        ringAoe.transform.rotation = Quaternion.identity;
    }

    void Boost()
    {
        rb.AddForce(transform.forward * boostForce * 0.02f, ForceMode.Impulse);
    }
}
