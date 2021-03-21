using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcGen
{
    public enum RoomState
    {
        Visited,
        Unvisited,
        Invisible
    }

    public class RoomContentGenerator : MonoBehaviour
    {
        public RoomTypes type; // 0: normal, 1: enter, 2: boss
        public Color normalColor, enterColor, bossColor;
        public int id = 0;

        public RoomState roomState = RoomState.Invisible;

        Room thisRoom;

        Color mainColor;
        Image img;
        Button button;

        void Start()
        {
            thisRoom = GetComponent<Room>();
            img = GetComponent<Image>();
            button = GetComponent<Button>();
            mainColor = normalColor;
            id = transform.GetSiblingIndex();

            PickColor();
            PickWaves();

            if (type == RoomTypes.Start)
            {
                roomState = RoomState.Visited;
            }
            else
            {
                roomState = RoomState.Invisible;
            }

            SetAlpha();
        }

        public void Reveal()
        {
            roomState = RoomState.Unvisited;
            SetAlpha();
        }

        public void Visit()
        {
            roomState = RoomState.Visited;
            SetAlpha();
        }

        private void SetAlpha()
        {
            float alpha = 255;
            bool isInteractible = false;

            switch (roomState)
            {
                case RoomState.Unvisited:
                    isInteractible = true;
                    alpha = 40;
                    break;
                case RoomState.Invisible:
                    alpha = 0;
                    break;
            }

            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha/255);
            button.interactable = isInteractible;
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

        private void PickWaves()
        {
            switch (type)
            {
                case RoomTypes.Normal:
                    break;
                case RoomTypes.Start:
                    thisRoom.waveCount = 0;
                    break;
                case RoomTypes.Boss:
                    thisRoom.waveCount = 0;
                    thisRoom.waves = new List<ProceduralWave>
                    { new ProceduralWave
                        (
                            DifficultyManager.Instance.currentDifficulty,
                            LevelManager.Instance.currentLevel.bossCards,
                        1)
                    };
                    break;
            }
        }
    }
}