using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomSpawnPoints
{
    Top = 0,
    TopMiddleRight,
    TopRight,
    RightTop,
    RightMiddleTop,
    Right,
    RightMiddleBottom,
    RightBottom,
    BottomRight,
    BottomMiddleRight,
    Bottom,
    BottomMiddleLeft,
    BottomLeft,
    LeftBottom,
    LeftMiddleBottom,
    Left,
    LeftMiddleTop,
    LeftTop,
    TopLeft,
    TopMiddleLeft
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] Vector2[] spawnPointViewportPositions = null;
    static Vector3[] spawnPointPositions;

    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        ConvertScreenSpawnpointsToWorld();
    }

    void ConvertScreenSpawnpointsToWorld()
    {
        spawnPointPositions = new Vector3[spawnPointViewportPositions.Length];

        for (int i = 0; i < spawnPointViewportPositions.Length; i++)
        {
            Vector2 viewportPos = spawnPointViewportPositions[i];
            Vector3 worldPos = new Vector3(viewportPos.x, 0, viewportPos.y);
            //Debug.Log(worldPos);
            spawnPointPositions[i] = mainCam.ViewportToWorldPoint(viewportPos);
            spawnPointPositions[i].y = 0;
        }
    }

    public static Vector3 WorldPositionFromSpawnPoint(RoomSpawnPoints spawnPoint)
    {
        return spawnPointPositions[(int)spawnPoint];
    }

    // TODO: obliterate, cease this function, let it be gone.
    private void OnDrawGizmosSelected()
    {
        if (spawnPointViewportPositions == null) return;
        if (spawnPointViewportPositions.Length < 1) return;

        mainCam = Camera.main;
        ConvertScreenSpawnpointsToWorld();

        Gizmos.color = Color.cyan;
        foreach (var position in spawnPointPositions)
        {
            Gizmos.DrawWireSphere(position, 5f);
        }
    }
}