using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    Vector3 startPosition, startRotation;

    public Text pickedUpLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickUp()
    {
        startPosition = this.transform.position;
        startRotation = this.transform.eulerAngles;
        pickedUpLabel.text = this.name;
    }
}
