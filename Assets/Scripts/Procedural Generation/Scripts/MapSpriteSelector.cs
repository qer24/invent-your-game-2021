using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
    public class MapSpriteSelector : MonoBehaviour
    {
        public RoomTypes type; // 0: normal, 1: enter, 2: boss
        public Color normalColor, enterColor, bossColor;

        Color mainColor;
        SpriteRenderer rend;

        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            mainColor = normalColor;
            PickColor();
        }

        //changes color based on what type the room is
        void PickColor()
        {
            switch (type)
            {
                case RoomTypes.Normal:
                    mainColor = normalColor;
                    break;
                case RoomTypes.Start:
                    mainColor = enterColor;
                    break;
                case RoomTypes.Boss:
                    mainColor = bossColor;
                    break;
            }

            rend.color = mainColor;
        }
    }
}