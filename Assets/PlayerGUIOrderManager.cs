using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUIOrderManager : MonoBehaviour
{
    public Canvas canvas;
    public static PlayerGUIOrderManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    public void ShowOnTop(bool onTop)
    {
        if (canvas == null) return;

        canvas.sortingOrder = onTop ? 10 : 0;
    }
}
