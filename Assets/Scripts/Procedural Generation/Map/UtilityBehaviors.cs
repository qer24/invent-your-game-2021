using ProcGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityBehaviors : MonoBehaviour
{
    public RoomManager roomManager;

	void Update ()
    {
		if (Input.GetKeyDown("r")){//reload scene, for testing purposes
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
        if(Input.GetKeyDown("t"))
        {
            foreach (var room in roomManager.allRoomsInLevel)
            {
                if (room.mapRoom.worldRoom.roomState == RoomState.Invisible)
                    room.mapRoom.worldRoom.Reveal();
            }
        }
        if(Input.GetKeyDown("y"))
        {
            DropManager.Instance.DropWeapon();
        }
        if (Input.GetKeyDown("u"))
        {
            DropManager.Instance.DropMod();
        }
        if (Input.GetKeyDown("i"))
        {
            PlayerPersistencyMenager.Instance.GetComponent<PlayerUpgradeManager>().AddExp(99);
            PlayerPersistencyMenager.Instance.GetComponent<PlayerUpgradeManager>().EnableButton();
        }
    }
}
