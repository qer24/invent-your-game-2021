using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsTabs : MonoBehaviour
{
    public GameObject[] tabs;

    public void OpenTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if(i == index)
            {
                tabs[i].SetActive(true);
            }else
            {
                tabs[i].SetActive(false);
            }
        }
    }
}
