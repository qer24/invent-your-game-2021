using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public enum TestEnemyState
{
    Idle,
    Reloading,
    Escaping
}

public class TestEnemy : MonoBehaviour
{
    [SerializeField] float moveForce = 25;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    public float timeBetweenActions = 2f;
    public float rotationSpeed = 10f;

    [Header("BulletStats")]
    public float bulletSpeed;
    public float bulletLifetime = 2f;

    static string playerTag = "Player";

    Rigidbody player;
    Rigidbody rb;
    float randomAngle;
    int randomSign;
    Material enemyMaterial;

    TestEnemyState currentState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>();
        currentState = TestEnemyState.Idle;

        enemyMaterial = GetComponentInChildren<Renderer>().material;
        rb = GetComponent<Rigidbody>();

        //once spawned outside of the map go towards the center for x seconds - done
        Vector3 dir = Vector3.zero.normalized - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;

        StartCoroutine(SearchAndDestroy(timeBetweenActions));
    }

    IEnumerator SearchAndDestroy(float waitTime)
    {
        rb.AddForce(transform.forward * moveForce * 2f);
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            currentState = TestEnemyState.Reloading;
            yield return new WaitForSeconds(waitTime);
            Shoot();
            yield return new WaitForSeconds(waitTime * 0.125f);
            Shoot();
            yield return new WaitForSeconds(waitTime * 0.125f);
            Shoot();
            currentState = TestEnemyState.Idle;


            randomAngle = Random.Range(60, 100);
            randomSign = Random.value < 0.5f ? 1 : -1;

            yield return new WaitForSeconds(waitTime * 0.5f);

            currentState = TestEnemyState.Escaping;
            yield return new WaitForSeconds(waitTime);
            currentState = TestEnemyState.Idle;
        }
    }
    void Update()
    {
        if (currentState == TestEnemyState.Idle) return;

        Vector3 dirToPlayer = PredictedPosition(player.position, player.velocity, bulletSpeed) - transform.position;
        float angle = Mathf.Atan2(dirToPlayer.x, dirToPlayer.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        switch (currentState)
        {
            case TestEnemyState.Idle:
                return;

            case TestEnemyState.Reloading:
                //rotate towards player for 1-2sec - done
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                rb.AddForce(transform.forward * moveForce * 0.0025f);
                break;

            case TestEnemyState.Escaping:
                //rotate in a random direction away from the player and escape - done
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * Quaternion.AngleAxis(randomAngle * randomSign, Vector3.up), rotationSpeed * Time.deltaTime);
                rb.AddForce(transform.forward * moveForce * 0.02f);
                break;
        }
    }

    void Shoot()
    {
        GameObject go = LeanPool.Spawn(bulletPrefab, shootPoint.position, transform.rotation);
        go.GetComponent<Renderer>().material = enemyMaterial;
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * bulletSpeed);
        go.GetComponent<Projectile>().Init(bulletSpeed, bulletLifetime, gameObject.tag, playerTag, 0, 0);
    }

    private Vector3 PredictedPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        //calculate distance to player with pythagoras
        float distance = Vector3.Distance(transform.position, targetPosition);
        //calculate traveltime
        float travelTime = distance / projectileSpeed;
        return targetPosition + targetVelocity * travelTime;
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.DrawWireSphere(PredictedPosition(player.position, player.velocity, bulletSpeed), 0.5f);
    }
}
