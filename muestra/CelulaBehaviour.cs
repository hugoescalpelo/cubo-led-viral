using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelulaBehaviour : MonoBehaviour
{
    public bool live;
    int lifePeriod;
    float nextActionTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        live = true;
        lifePeriod = Random.Range(10, 20);
        nextActionTime = Mathf.FloorToInt(Time.time) + (lifePeriod * 600);
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > nextActionTime) {
            live = false;
        }
    }
}
