using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [SerializeField] Weapon[] weaponPool = null;
    [SerializeField] Mod[] modPool = null;

    public static DropManager Instance;

    Camera mainCam;

    public static bool FirstWeaponDropped = false;
    public static bool FirstModDropped = false;
    MonoBehaviour lastDrop = null;

    public static Action OnFirstModDrop;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
            return;
        }

        mainCam = CameraManager.Instance.mainCam;
    }

    public GameObject RandomDrop()
    {
        if(!FirstWeaponDropped)
        {
            FirstWeaponDropped = true;
            return DropWeapon();
        }

        if (UnityEngine.Random.value > 0.5f)
        {
            return DropWeapon();
        }
        else
        {
            return DropMod();
        }
    }

    public GameObject DropWeapon()
    {
        GameObject go = InstantiateRandomItemFromArray(weaponPool, 5);
        go.GetComponent<Weapon>().GenerateWeapon();

        return go;
    }

    public GameObject DropMod()
    {
        if(!FirstModDropped)
        {
            FirstModDropped = true;
            OnFirstModDrop?.Invoke();
        }
        return InstantiateRandomItemFromArray(modPool);
    }

    GameObject InstantiateRandomItemFromArray(MonoBehaviour[] array, float zOffset = 0)
    {
        MonoBehaviour[] exceptArray = {lastDrop};
        var newlist = array.Except(exceptArray).ToList();

        int random = UnityEngine.Random.Range(0, newlist.Count);
        Vector3 pos = mainCam.WorldToScreenPoint(new Vector3(UnityEngine.Random.Range(-6, 6f), 0, zOffset + UnityEngine.Random.Range(-6f, 6f)));
        //pos.y = 0;
        GameObject go = Instantiate(newlist[random], pos, Quaternion.identity, transform).gameObject;
        go.transform.localScale = Vector3.zero;
        LeanTween.scale(go, Vector3.one, 0.4f).setEase(LeanTweenType.easeOutBack);
        go.name = newlist[random].gameObject.name;

        lastDrop = newlist[random];
        return go;
    }

    public static void ResetDrops()
    {
        FirstWeaponDropped = false;
    }
}
