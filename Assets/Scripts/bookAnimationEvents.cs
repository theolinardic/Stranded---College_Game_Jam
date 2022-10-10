using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void animDone()
    {
        if (GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().isJournalOpen == true)
        {
            GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().isJournalOpen = false;
        }
        else
        {
            GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().isJournalOpen = true; 
        }
    }
}
