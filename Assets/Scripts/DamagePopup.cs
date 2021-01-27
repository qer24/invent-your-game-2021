using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public Rigidbody rb;
    public TextMeshPro text;

    public Vector3 forceMin;
    public Vector3 forceMax;

    public LeanTweenType scaleDownType;
    public float timeToScale = 0.5f;

    public void Init(int damage)
    {
        Vector3 force = new Vector3(Random.Range(forceMin.x, forceMax.x), Random.Range(forceMin.y, forceMax.y), Random.Range(forceMin.z, forceMax.z));
        rb.AddForce(force, ForceMode.VelocityChange);
        text.text = damage.ToString();
        Lean.Pool.LeanPool.Despawn(gameObject, 1f);

        LeanTween.scale(gameObject, Vector3.zero, timeToScale).setEase(scaleDownType);
    }
}
