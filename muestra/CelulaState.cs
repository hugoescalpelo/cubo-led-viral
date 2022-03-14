using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelulaState : MonoBehaviour
{

    public bool vivo = true;
    private bool lastState = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((!lastState) && vivo) {
            gameObject.SetActive(true);
            lastState = vivo;
        } else if (lastState && !vivo) {
            gameObject.SetActive(false);
            lastState = vivo;
        }
    }
}
