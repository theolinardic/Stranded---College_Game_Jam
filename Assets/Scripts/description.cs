using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class description : MonoBehaviour
{
    GameObject text, desc;
    public float time;
    public static bool shouldShow;
    // Start is called before the first frame update
    void Start()
    {
        shouldShow = true;
        time = 10;
        text = this.transform.GetChild(0).gameObject;
        text = text.transform.GetChild(0).gameObject;
        desc = text.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if(shouldShow == true)
        {
            if (time > 2)
            {
                text.GetComponent<Renderer>().enabled = false;
                desc.GetComponent<Renderer>().enabled = false;
            }
        }

    }

    public void showDescription()
    {
        text.GetComponent<Renderer>().enabled = true;
        desc.GetComponent<Renderer>().enabled = true;
        time = 0f;
    }

    public void turnFalse()
    {
        shouldShow = false;
        text.GetComponent<Renderer>().enabled = false;
        desc.GetComponent<Renderer>().enabled = false;
    }


}
