using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineProjectileBehaviour : ProjectileBehaviour
{
    public float sineFrequency = 1f;
    public float sineAmplitude = 1f;

    float time = 0;

    private void LateUpdate()
    {
        transform.position += transform.right * (Mathf.Sin(time * sineFrequency) * sineAmplitude);
        time += Time.deltaTime;
    }
}
