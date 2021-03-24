using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();

    [SerializeField] LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] float tweenDuration = 0.2f;

    [SerializeField] Transform mouseFollowPoint = null;
    [SerializeField] float pickupRadius = 0.5f;
    [SerializeField] List<GameObject> keyTooltipsUI = null;
    [SerializeField] List<GameObject> weaponSlots = null;

    public Weapon CurrentWeapon { get => weapons[0];}
    public Action OnWeaponSwap;

    Canvas dropsCanvas = null;

    float switchCooldownTimer;
    WeaponPickup currentClosestWeaponPickup;

    Camera mainCam;

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) =>
        {
            mainCam = Camera.main;
            dropsCanvas = DropManager.Instance.GetComponent<Canvas>();
        };
        mainCam = Camera.main;
        switchCooldownTimer = tweenDuration;
    }

    public void Update()
    {
        if (PauseManager.paused) return;

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
        OnWeaponSwap?.Invoke();

        LeanTween.move(currentClosestWeaponPickup.GetComponent<RectTransform>(), Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuart);
        if (weapons[slot] != null)
        {
            weapons[slot].transform.SetParent(dropsCanvas.transform);
            Vector3 pos = mainCam.WorldToScreenPoint(new Vector3(Random.Range(-6, 6f), 0, Random.Range(-6f, 6f)));
            LeanTween.move(weapons[slot].gameObject, pos, 0.5f).setEase(LeanTweenType.easeOutQuart);
            weapons[slot].GetComponent<WeaponPickup>().isInWorld = true;
            if(ProcGen.RoomManager.CurrentRoom != null)
                ProcGen.RoomManager.CurrentRoom.dropsInThisRoom.Add(weapons[slot].gameObject);
        }
        currentClosestWeaponPickup.transform.SetParent(weaponSlots[slot].transform);
        currentClosestWeaponPickup.isInWorld = false;

        Weapon pickedUpWeapon = currentClosestWeaponPickup.GetComponent<Weapon>();
        pickedUpWeapon.OnEquip?.Invoke();

        weapons[slot] = pickedUpWeapon;
    }

    WeaponPickup GetClosestWeaponPickup()
    {
        if (ModDrop.DraggingMod) return null;

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

        Vector3 firstWeaponPos = weaponSlots[0].transform.position;
        Vector3 secondWeaponPos = weaponSlots[1].transform.position;

        LeanTween.moveX(weaponSlots[0].gameObject, secondWeaponPos.x, tweenDuration).setEase(tweenType);
        LeanTween.moveX(weaponSlots[1].gameObject, firstWeaponPos.x, tweenDuration).setEase(tweenType);

        weapons.Reverse();
        keyTooltipsUI.Reverse();
        weaponSlots.Reverse();
    }
}
