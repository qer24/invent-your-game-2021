using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingWeapon : Weapon
{
    public float shootForce = 500f;

    List<Transform> rings = new List<Transform>();

    private void Update()
    {
        if (rings.Count > 0)
        {
            for (int i = 0; i < rings.Count; i++)
            {
                if (rings[i] == null)
                {
                    rings.RemoveAt(i);
                }
                else
                {
                    rings[i].rotation = Quaternion.Euler(-90, 0, 0);
                }
            }
        }
    }

    public override void Attack(string enemyTag)
    {
        GameObject go = Lean.Pool.LeanPool.Spawn(aoePrefab, playerShooter.transform.position, playerShooter.transform.rotation);
        go.GetComponent<Aoe>().Init(FinalDamage, aoeLifeTime, FinalSize, enemyTag);
        go.GetComponent<Rigidbody>().AddForce(playerShooter.transform.forward.normalized * shootForce, ForceMode.Impulse);

        foreach (var behaviour in OnDamageBehaviours)
        {
            Instantiate(behaviour, go.transform).SetActive(true);
        }

        rings.Add(go.GetComponentInChildren<Renderer>().transform);
    }
}
