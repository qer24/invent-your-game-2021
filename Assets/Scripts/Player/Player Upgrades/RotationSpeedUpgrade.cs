using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSpeedUpgrade : PlayerUpgrade
{
    public override void Upgrade()
    {
        base.Upgrade();
        playerController.rotationSpeed *= 2;
    }
}
