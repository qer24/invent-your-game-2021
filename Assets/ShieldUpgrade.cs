using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : PlayerUpgrade
{
    public PlayerShield shield;

    public override void Upgrade()
    {
        base.Upgrade();

        shield.transform.parent.SetParent(null);
        shield.transform.parent.position = playerController.transform.position;

        shield.transform.parent.gameObject.SetActive(true);
        DontDestroyOnLoad(shield.transform.parent.gameObject);

        shield.playerTransform = playerController.transform;
    }

    private void OnDestroy()
    {
        if (shield == null) return;
        if (shield.transform.parent.gameObject != null)
            Destroy(shield.transform.parent.gameObject);
    }
}
