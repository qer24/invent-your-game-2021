using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : PlayerUpgrade
{
    public float moveForceAmount = 3000;

    public override void Upgrade()
    {
        base.Upgrade();
        playerController.moveForce += moveForceAmount;
    }
}
