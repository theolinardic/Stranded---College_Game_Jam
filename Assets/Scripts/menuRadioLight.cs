using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuRadioLight : MonoBehaviour
{
    public GameObject radioLight;
    float timePassed;
    bool lightOn;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0f;
        lightOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = timePassed + Time.deltaTime;

        if(timePassed > 2)
        {
            timePassed = 0;
            if(lightOn == true)
            {
                radioLight.GetComponent<Light>().intensity = 0f;
            }
            if(lightOn == false)
            {
                radioLight.GetComponent<Light>().intensity = 0.09f;
            }
            lightOn = !lightOn;
        }
    }
}
