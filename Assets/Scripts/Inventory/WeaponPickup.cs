using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Image pickupIndicator = null;
    [SerializeField] Color highlightColor = Color.white;
    public bool isInWorld;

    Color startingColor;

    private void Start()
    {
        startingColor = pickupIndicator.color;
    }

    public void Select()
    {
        pickupIndicator.color = highlightColor;
    }

    public void Deselect()
    {
        pickupIndicator.color = startingColor;
    }
}
