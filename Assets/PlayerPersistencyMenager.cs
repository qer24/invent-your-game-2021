using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistencyMenager : MonoBehaviour
{
    public static PlayerPersistencyMenager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
