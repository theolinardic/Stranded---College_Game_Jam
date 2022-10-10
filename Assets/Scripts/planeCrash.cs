using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeCrash : MonoBehaviour
{
    public AudioSource crash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void crashPlay()
    {
        crash.Play(0);
    }
}
