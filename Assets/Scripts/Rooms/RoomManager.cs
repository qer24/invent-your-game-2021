using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace ProcGen
{
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

        public ProceduralRoom[,] roomMap;
        Room[] allRoomsInLevel;
        Room currentRoom = null;

        Camera mainCam;

        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main;
            ConvertScreenSpawnpointsToWorld();

            allRoomsInLevel = GetComponentsInChildren<Room>();
            GoToRoom(0);
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

        public Vector3 WorldPositionFromSpawnPoint(RoomSpawnPoints spawnPoint)
        {
            ConvertScreenSpawnpointsToWorld();

            return spawnPointPositions[(int)spawnPoint];
        }

        public void RevealNeighbours(int id)
        {
            ProceduralRoom mapRoom = allRoomsInLevel[id].mapRoom;

            Vector2Int pos = Vector2Int.zero;
            for (int x = 0; x < roomMap.GetLength(0); x++)
            {
                for (int y = 0; y < roomMap.GetLength(1); y++)
                {
                    if (roomMap[x,y] == mapRoom)
                    {
                        pos = new Vector2Int(x, y);
                    }
                }
            }

            List<RoomContentGenerator> roomsToReveal = new List<RoomContentGenerator>();
            if (pos.x + 1 < roomMap.GetLength(0))
            {
                if(roomMap[pos.x + 1, pos.y] != null)
                {
                    roomsToReveal.Add(roomMap[pos.x + 1, pos.y].worldRoom);
                }
            }
            if (pos.x - 1 >= 0)
            {
                if (roomMap[pos.x - 1, pos.y] != null)
                {
                    roomsToReveal.Add(roomMap[pos.x - 1, pos.y].worldRoom);
                }
            }
            if (pos.y + 1 < roomMap.GetLength(1))
            {
                if (roomMap[pos.x, pos.y + 1] != null)
                {
                    roomsToReveal.Add(roomMap[pos.x, pos.y + 1].worldRoom);
                }
            }
            if (pos.y - 1 >= 0)
            {
                if (roomMap[pos.x, pos.y - 1] != null)
                {
                    roomsToReveal.Add(roomMap[pos.x, pos.y - 1].worldRoom);
                }
            }

            foreach (var room in roomsToReveal)
            {
                if(room.roomState == RoomState.Invisible)
                    room.Reveal();
            }
        }

        public void GoToRoom(int id)
        {
            if (currentRoom != null)
                currentRoom.enabled = false;
            allRoomsInLevel[id].enabled = true;

            currentRoom = allRoomsInLevel[id];

            allRoomsInLevel[id].GetComponent<RoomContentGenerator>().Visit();
            RevealNeighbours(id);
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

            if (roomMap == null) return;

            Gizmos.color = Color.white;
            foreach (ProceduralRoom room in roomMap)
            {
                if (room == null) continue;

                var pos = new Vector3(room.gridPos.x, 0, room.gridPos.y);

                Gizmos.DrawCube(pos, Vector3.one * 0.9f);
            }
        }
    }
}
