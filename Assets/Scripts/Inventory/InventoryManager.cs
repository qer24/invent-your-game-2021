using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();

    [SerializeField] LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] float tweenDuration = 0.2f;

    public Weapon CurrentWeapon { get => weapons[0];}
    public Action OnWeaponSwap;

    float switchCooldownTimer;

    private void Start()
    {
        switchCooldownTimer = tweenDuration;
    }

    public void Update()
    {
        if(switchCooldownTimer > 0)
        {
            switchCooldownTimer -= Time.deltaTime;
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && weapons[1] != null)
        {
            switchCooldownTimer = tweenDuration * 4f;
            SwapWeapon();
        }
    }

    void SwapWeapon()
    {
        OnWeaponSwap?.Invoke();

        Vector3 firstWeaponPos = weapons[0].transform.parent.position;
        Vector3 secondWeaponPos = weapons[1].transform.parent.position;

        LeanTween.moveX(weapons[0].transform.parent.gameObject, secondWeaponPos.x, tweenDuration).setEase(tweenType);
        LeanTween.moveX(weapons[1].transform.parent.gameObject, firstWeaponPos.x, tweenDuration).setEase(tweenType);

        weapons.Reverse();
    }
}
