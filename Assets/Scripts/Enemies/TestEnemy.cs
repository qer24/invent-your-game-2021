using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] float moveForce = 25;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float rotateSeconds = 2f;
    public float rotationSpeed = 10f;
    public float bulletSpeed;
    public float bulletLifetime = 2f;
    public float bulletRotationSpeed = 1f;
    public float bulletSeekDistance = 4f;
    Rigidbody rb;
    float randomAngle;

    string playerTag = "Player";
    private IEnumerator coroutine;
    private bool reload = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 dir = Vector3.zero - transform.position;

        //once spawned outside of the map go towards the center for x seconds - done
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation;
        coroutine = SearchAndDestroy(rotateSeconds);
        StartCoroutine(coroutine);
    }
    IEnumerator SearchAndDestroy(float waitTime)
    {
        rb.AddForce(transform.forward * moveForce * 10f);
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            reload = true;
            yield return new WaitForSeconds(waitTime);
            reload = false;
            Shoot();
            Escape();
            yield return new WaitForSeconds(waitTime * 2);
        }

    }
    void Update()
    {
        randomAngle = Random.Range(0.1f, 1f);
        if (reload == true)
        {
            //rotate towards player for 1-2sec - done
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            Vector3 dir = player.GetComponent<Transform>().position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.AddForce(transform.forward * moveForce * 0.01f);
        }
    }

    private void Escape()
    {
        //rotate in a random direction away from the player and escape - malfunction
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        Vector3 dir = player.GetComponent<Transform>().position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = targetRotation * Quaternion.AngleAxis(-randomAngle, Vector3.up);
        rb.AddForce(transform.forward * moveForce * 25f);
    }
    private void Shoot()
    {
        GameObject go = LeanPool.Spawn(bulletPrefab, shootPoint.position, transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * bulletSpeed);
        go.GetComponent<Projectile>().Init(bulletSpeed, bulletLifetime, gameObject.tag, "Enemy", bulletRotationSpeed, bulletSeekDistance);
    }
}
