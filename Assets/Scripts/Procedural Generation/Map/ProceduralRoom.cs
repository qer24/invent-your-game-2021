using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
    public enum RoomTypes
    {
        Normal = 0,
        Start,
        Boss
    }

    public class ProceduralRoom
    {
        public Vector2 gridPos;
        public RoomTypes type;
        public RoomContentGenerator worldRoom;

        public ProceduralRoom(Vector2 _gridPos, RoomTypes _type)
        {
            gridPos = _gridPos;
            type = _type;
        }
    }
}
