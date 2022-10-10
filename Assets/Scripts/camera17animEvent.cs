using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class camera17animEvent : MonoBehaviour
{
    public AudioSource transitionSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unparentObjects()
    {
        GameObject.Find("OpenJournal").transform.parent = GameObject.Find("camTest").transform;
    }

    public void changeRadio()
    {
        GameObject.Find("Line (2)").GetComponent<line>().heightTest = 1f;
        GameObject.Find("Line (2)").GetComponent<line>().testVar = 0.85f;
        GameObject.Find("signalStrength").GetComponent<TextMesh>().text = "CONNECTION FOUND";
    }

    public void displayEnd()
    {
        GameObject.Find("theEnd").GetComponent<Image>().enabled = true;
    }

    public void loadMenu()
    {
        GameObject.Find("theEnd").GetComponent<Image>().enabled = false;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Menu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void playRadioSound()
    {
        transitionSound.Play(0);
    }
}
