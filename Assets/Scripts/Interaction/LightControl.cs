using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LightControl : MonoBehaviour {

    private Light _light;
    [Header("Размер света min/max")]
    public float RangeLight_min = 0.5f;
    public float RangeLight_max = 1.0f;
    [Header("Время изменения min/max")]
    public float RangeTime_min = 1.0f;
    public float RangeTime_max = 2.0f;

    private void Start()
    {
        _light = GetComponent<Light>();

        StartCoroutine(Lights());
    }
    private IEnumerator Lights()
    {
        float range1 = Random.Range(RangeLight_min, RangeLight_max);
        float range2 = Random.Range(RangeTime_min, RangeTime_max);
        _light.range = range1;
        yield return new WaitForSeconds(range2);
        StartCoroutine(Lights());
    }
}
