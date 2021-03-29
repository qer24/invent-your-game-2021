using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public EnemyCard[] allEnemyCardsInLevel;
    public EnemyCard[] bossCards;

    [EventRef] public string backgroundMusic;

    private void Start()
    {
        MusicManager.Play(backgroundMusic);
        PlayerPersistencyMenager.Instance.GetComponent<PlayerHealth>().OnDeath += () => gameObject.SetActive(false);
    }
}
