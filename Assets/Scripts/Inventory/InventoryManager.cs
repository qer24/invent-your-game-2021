using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Weapon[] weapons = null;
    public Weapon CurrentWeapon { get => weapons[0];}
}
