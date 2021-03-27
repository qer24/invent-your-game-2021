using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStack : MonoBehaviour
{
    public int maxStacks = 5;
    int stacks = 1;

    IDamagable damagable;

    private void Start()
    {
        stacks = 1;

        damagable = GetComponentInParent<IDamagable>();
        InvokeRepeating(nameof(Damage), 1f, 1f);
    }

    void Damage()
    {
        if (stacks <= 0) return;

        damagable.TakeDamage(stacks);
        stacks--;
    }

    private void OnDisable()
    {
        Destroy(gameObject);
        CancelInvoke();
    }

    public void AddStack()
    {
        if (stacks >= maxStacks) return;

        stacks++;
    }
}
