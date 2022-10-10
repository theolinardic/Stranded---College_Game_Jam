using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removableItemBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public int removedScrews;
    public bool shouldRun;
    public GameObject middleLocation, endLocation;

    Vector3 startPosition;
    Quaternion startRotation;

    float lerpPos = 0f;
    float lerpPos2;
    bool hasRun = false;
    bool hasEverRun = false;
    void Start()
    {
        lerpPos = 0f;
        shouldRun = false;
        removedScrews = 0;

        startPosition = this.transform.position;
        startRotation = this.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if(removedScrews == 4 && shouldRun == true && hasEverRun == false)
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
                this.transform.position = Vector3.Lerp(startPosition, middleLocation.transform.position, lerpPos);
                this.transform.rotation = Quaternion.Lerp(startRotation, middleLocation.transform.rotation, lerpPos);
                lerpPos = lerpPos + (2.5f * Time.deltaTime);
            }

            if (lerpPos2 > 1)
            {
                shouldRun = false;
                hasEverRun = true;
                GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().removedItems++;
            }
            if (lerpPos2 < 1 && hasRun == true)
            {
                this.transform.position = Vector3.Lerp(startPosition, endLocation.transform.position, lerpPos2);
                this.transform.rotation = Quaternion.Lerp(startRotation, endLocation.transform.rotation, lerpPos2);
                lerpPos2 = lerpPos2 + (1.5f * Time.deltaTime);
            }
        }
    }
}
