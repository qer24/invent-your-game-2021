using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static int DamageDealt = 0;
    public static int DamageTaken = 0;
    public static int EnemiesDestroyed = 0;
    public static int RoomsCleared = 0;
    public static int ExpGained = 0;
    public static int LevelsGained = 0;
    public static int TotalShots = 0;
    public static int BossesDestroyed = 0;

    public static void ResetStats()
    {
        DamageDealt = 0;
        DamageTaken = 0;
        EnemiesDestroyed = 0;
        RoomsCleared = 0;
        ExpGained = 0;
        LevelsGained = 0;
        TotalShots = 0;
        BossesDestroyed = 0;
    }

    private void Start()
    {
        MusicManager.Stop();
    }

    public void Quit()
    {
        Destroy(LevelManager.Instance.gameObject);
        if (PlayerPersistencyMenager.Instance != null)
            Destroy(PlayerPersistencyMenager.Instance.gameObject);

        MusicManager.Stop();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
