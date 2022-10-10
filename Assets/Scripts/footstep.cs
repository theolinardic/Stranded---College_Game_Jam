using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstep : MonoBehaviour
{

    CharacterController cc;
    public AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(GameObject.Find("Player").GetComponent<playerMovement>().velocity.x);
        if(GameObject.Find("Player").GetComponent<playerMovement>().isGrounded == true && audio.isPlaying == false && (GameObject.Find("Player").GetComponent<playerMovement>().x != 0 || GameObject.Find("Player").GetComponent<playerMovement>().z != 0)
            && GameObject.Find("Player").GetComponent<playerMovement>().itemPickedUp == false)
        {
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play(0);
        }
        else
        {
            if (audio.isPlaying == true && (GameObject.Find("Player").GetComponent<playerMovement>().x == 0 && GameObject.Find("Player").GetComponent<playerMovement>().z == 0))
                audio.Stop();
        }
        
    }
}
