using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcGen
{
    public class RoomContentGenerator : MonoBehaviour
    {
        public RoomTypes type; // 0: normal, 1: enter, 2: boss
        public Color normalColor, enterColor, bossColor;
        public int id = 0;

        Room thisRoom;

        Color mainColor;
        Image img;

        void Start()
        {
            thisRoom = GetComponent<Room>();
            img = GetComponent<Image>();
            mainColor = normalColor;
            id = transform.GetSiblingIndex();

            PickColor();
            PickWaves();
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

            img.color = mainColor;
        }

        //TODO: Proc gen
        private void PickWaves()
        {
            switch (type)
            {
                case RoomTypes.Normal:
                    break;
                case RoomTypes.Start:
                    thisRoom.waves = new List<Wave>();
                    break;
                case RoomTypes.Boss:
                    thisRoom.waves = new List<Wave>();
                    break;
            }
        }
    }
}