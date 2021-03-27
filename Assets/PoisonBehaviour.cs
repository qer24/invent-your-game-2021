using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBehaviour : OnDamageBehaviour
{
    public GameObject poisonStackGameObject;

    private void Start()
    {
        damager.OnDealDamage += AddPoison;
    }

    private void OnDestroy()
    {
        if (damager != null)
        {
            damager.OnDealDamage -= AddPoison;
        }
    }

    private void AddPoison(float damage, Transform damagable)
    {
        if (damagable.TryGetComponent(out Enemy enemy))
        {
            var poisonStack = enemy.GetComponentInChildren<PoisonStack>();

            if (poisonStack == null)
            {
                Instantiate(poisonStackGameObject, enemy.transform).SetActive(true);
            }
            else
            {
                poisonStack.AddStack();
            }
        }
    }
}
