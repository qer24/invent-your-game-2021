using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAoeWeapon : Weapon
{
    Camera mainCam = null;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public override void Attack(string enemyTag)
    {
        Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.y = 0;
        GameObject go = LeanPool.Spawn(aoePrefab, pos, Quaternion.identity);
        go.GetComponent<Aoe>().Init(FinalDamage, 0.25f, FinalSize, enemyTag);
    }
}
