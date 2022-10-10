using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screwBehavior : MonoBehaviour
{
    public bool isRaised, shouldRun;
    public int endLocation = 0;
    GameObject[] endLocations;
    float lerpPos = 0f;
    float lerpPos2;
    Vector3 startPosition;
    Quaternion startRotation;
    bool hasRun = false;

    public string attachedObject;
    // Start is called before the first frame update
    void Start()
    {
        isRaised = false;
        endLocations = new GameObject[8];
        endLocations[0] = GameObject.Find("ScrewEndPoint");
        endLocations[1] = GameObject.Find("ScrewEndPoint2");
        endLocations[2] = GameObject.Find("ScrewEndPoint3");
        endLocations[3] = GameObject.Find("ScrewEndPoint4");
        endLocations[4] = GameObject.Find("ScrewEndPoint5");
        endLocations[5] = GameObject.Find("ScrewEndPoint6");
        endLocations[6] = GameObject.Find("ScrewEndPoint7");
        endLocations[7] = GameObject.Find("ScrewEndPoint8");
        startPosition = this.transform.position;
        startPosition.y += 0.5f;
        startRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaised == true && shouldRun == true)
        {

            if (lerpPos > 1)
            {
                if (hasRun == false)
                {
                    lerpPos2 = 0;
                    startPosition = this.transform.position;
                    startRotation = this.transform.rotation;
                    hasRun = true;
                }

                
            }
            if (lerpPos < 1)
            {
                this.transform.position = Vector3.Lerp(startPosition, GameObject.Find("ScrewMiddlePoint").transform.position, lerpPos);
                this.transform.rotation = Quaternion.Lerp(startRotation, GameObject.Find("ScrewMiddlePoint").transform.rotation, lerpPos);
                lerpPos = lerpPos + (1.5f * Time.deltaTime);
               // Debug.Log("RUNNNING");
            }

            if (lerpPos2 > 1)
            {
                isRaised = false;

                if(attachedObject == "brokenRadioDisplay")
                {
                    GameObject.Find("BrokenRadioDisplay").GetComponent<removableItemBehavior>().removedScrews++;
                }

                if(attachedObject == "brokenRadioSpeaker")
                {
                    GameObject.Find("BrokenRadioSpeaker").GetComponent<removableItemBehavior>().removedScrews++;
                }
            }
            if (lerpPos2 < 1)
            {
                this.transform.position = Vector3.Lerp(startPosition, endLocations[endLocation].transform.position, lerpPos);
                this.transform.rotation = Quaternion.Lerp(startRotation, endLocations[endLocation].transform.rotation, lerpPos);
                lerpPos2 = lerpPos2 + (1.5f * Time.deltaTime);
            }
        }
    }
}
