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

    [SerializeField] Transform mouseFollowPoint = null;
    [SerializeField] float pickupRadius = 0.5f;
    [SerializeField] GameObject[] keyTooltipsUI = null;
    [SerializeField] GameObject[] weaponSlots = null;
    [SerializeField] Canvas dropsCanvas = null;

    public Weapon CurrentWeapon { get => weapons[0];}
    public Action OnWeaponSwap;

    float switchCooldownTimer;
    WeaponPickup currentClosestWeaponPickup;

    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        switchCooldownTimer = tweenDuration;
    }

    public void Update()
    {
        currentClosestWeaponPickup = GetClosestWeaponPickup();
        if (currentClosestWeaponPickup != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SwitchWeapon(0);
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                SwitchWeapon(1);
            }

            if (!keyTooltipsUI[0].activeSelf)
            {
                foreach (var tooltip in keyTooltipsUI)
                {
                    tooltip.SetActive(true);
                }
            }
        }else if(keyTooltipsUI[0].activeSelf)
        {
            foreach (var tooltip in keyTooltipsUI)
            {
                tooltip.SetActive(false);
            }
        }

        if (switchCooldownTimer > 0)
        {
            switchCooldownTimer -= Time.deltaTime;
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && weapons[1] != null)
        {
            switchCooldownTimer = tweenDuration * 4f;
            SwapWeapons();
        }
    }

    void SwitchWeapon(int slot)
    {
        LeanTween.move(currentClosestWeaponPickup.GetComponent<RectTransform>(), Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuart);
        if (weapons[slot] != null)
        {
            weapons[slot].transform.SetParent(dropsCanvas.transform);
            Vector3 zero = mainCam.WorldToScreenPoint(new Vector3(0, 0, 0));
            zero.z = 0;
            weapons[slot].transform.position = zero;
            weapons[slot].GetComponent<WeaponPickup>().isInWorld = true;
        }
        currentClosestWeaponPickup.transform.SetParent(weaponSlots[slot].transform.parent);
        currentClosestWeaponPickup.isInWorld = false;
        weapons[slot] = currentClosestWeaponPickup.GetComponent<Weapon>();
        weaponSlots[slot].transform.parent.GetComponent<WeaponSlotTooltip>().ConnectWeapon(weapons[slot]);
    }

    WeaponPickup GetClosestWeaponPickup()
    {
        WeaponPickup closestPickup = null;

        float minDistance = Mathf.Infinity;
        foreach (var pickup in FindObjectsOfType<WeaponPickup>())
        {
            if (!pickup.isInWorld) continue;

            Vector3 worldPos = mainCam.ScreenToWorldPoint(pickup.transform.position);
            worldPos.y = 0;
            Debug.DrawLine(mouseFollowPoint.transform.position, worldPos);
            float distance = Vector3.Distance(worldPos, mouseFollowPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPickup = pickup;
            }
        }

        if (minDistance <= pickupRadius && closestPickup != null)
        {
            if (closestPickup != currentClosestWeaponPickup && currentClosestWeaponPickup != null)
            {
                currentClosestWeaponPickup.Deselect();
            }
            closestPickup.Select();

            return closestPickup;
        }

        if (currentClosestWeaponPickup != null)
        {
            currentClosestWeaponPickup.Deselect();
        }

        return null;
    }

    void SwapWeapons()
    {
        OnWeaponSwap?.Invoke();

        Vector3 firstWeaponPos = weaponSlots[0].transform.parent.position;
        Vector3 secondWeaponPos = weaponSlots[1].transform.parent.position;

        LeanTween.moveX(weaponSlots[0].transform.parent.gameObject, secondWeaponPos.x, tweenDuration).setEase(tweenType);
        LeanTween.moveX(weaponSlots[1].transform.parent.gameObject, firstWeaponPos.x, tweenDuration).setEase(tweenType);

        weapons.Reverse();
    }
}
