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

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
            return;
        }

        mainCam = Camera.main;
    }

    public GameObject DropWeapon()
    {
        return InstantiateRandomItemFromArray(weaponPool);
    }

    public GameObject DropMod()
    {
        return InstantiateRandomItemFromArray(modPool);
    }

    GameObject InstantiateRandomItemFromArray(MonoBehaviour[] array)
    {
        int random = Random.Range(0, array.Length);
        Vector3 pos = mainCam.WorldToScreenPoint(new Vector3(Random.Range(-6, 6f), 0, Random.Range(-6f, 6f)));
        //pos.y = 0;
        GameObject go = Instantiate(array[random], pos, Quaternion.identity, transform).gameObject;
        go.transform.localScale = Vector3.zero;
        LeanTween.scale(go, Vector3.one, 0.4f).setEase(LeanTweenType.easeOutBack);

        return go;
    }
}
