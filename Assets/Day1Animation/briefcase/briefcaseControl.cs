using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class briefcaseControl : MonoBehaviour
{
    public AudioSource zipper, opening;
    // Start is called before the first frame update
    void Start()
    {
        //var aSources = GetComponents<AudioSource>();
        //zipper = aSources[0];
        //opening = aSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playZipper()
    {
        zipper.Play(0);
    }
    public void stopZipper()
    {
        zipper.Stop();
    }

    public void playOpening()
    {
        opening.Play(0);
    }

    void enableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void disableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
