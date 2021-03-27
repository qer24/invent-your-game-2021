using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaWeapon : Weapon
{
    List<Transform> novas = new List<Transform>();

    private void Update()
    {
        if(novas.Count > 0)
        {
            for (int i = 0; i < novas.Count; i++)
            {
                if(novas[i] == null)
                {
                    novas.RemoveAt(i);
                }else
                {
                    novas[i].position = playerShooter.transform.position;
                }
            }
        }
    }

    public override void Attack(string enemyTag)
    {
        GameObject go = Lean.Pool.LeanPool.Spawn(aoePrefab, playerShooter.transform.position, playerShooter.transform.rotation);
        go.GetComponent<Aoe>().Init(FinalDamage, aoeLifeTime, FinalSize, enemyTag);

        foreach (var behaviour in OnDamageBehaviours)
        {
            Instantiate(behaviour, go.transform).SetActive(true);
        }

        novas.Add(go.transform);
    }
}
