using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class workbenchIttem : MonoBehaviour
{

    public bool shouldFollowMouse;
    bool shouldRotateTo, shouldRotateAway, backToStart, clickedThisFrame, unscrew;

    float lerpPos, lerpPosPosition, screwLerpPos;
    Quaternion startRotation, endRotation;

    Vector3 suitcasePosition;
    Quaternion suitcaseRotation;

    GameObject currentScrew;
    Collider thisCollider;
    Collider suitcaseCollider;
    float objectHeight = 4f;
    int unscrewCounter;
    Quaternion screwStartRotation, screwEndRotation, screwdriverStartRotation, screwdriverEndRotation;
    Vector3 screwStartPosition, screwEndPosition, screwdriverStartPosition, screwdriverEndPosition;
    int totalSavedScrews = 0;

    public int removedItems = 0;

    public bool isJournalOpen = true;

    bool shouldCoverLerp;
    float coverLerp = 0, coverLerp2;
    Vector3 coverStart, coverMid, coverEnd;
    Quaternion coverStartRot, coverMidRot, coverEndRot;
    bool hasRun = false, BLhasRun = false, BRhasRun = false;
    bool shouldBLLerp = false, shouldBRLerp = false;
    float blLerp = 0, brLerp;

    Vector3 BLStart, BLMid, BLEnd, BRStart, BRMid, BREnd;
    Quaternion BLStartRot, BLMidRot, BLEndRot, BRStartRot, BRMidRot, BREndRot;

    bool shouldAntennaLerp = false;
    float antennaLerp = 0;
    Vector3 antennaStart, antennaEnd;
    Quaternion antennaStartRot, antennaEndRot;

    bool shouldFadeBlack = false;
    float endLerp = 0;
    Color full, empty;
    bool shouldFadeBack = false;
    // Start is called before the first frame update
    void Start()
    {
        suitcaseRotation = this.transform.rotation;
        suitcasePosition = this.transform.position;
        shouldFollowMouse = false;
        startRotation = Quaternion.Euler(90f, 0f, 0f);
        endRotation = Quaternion.Euler(180f, 0f, 0f);

        thisCollider = GetComponent<Collider>();
        suitcaseCollider = GameObject.Find("Suitcase").GetComponent<Collider>();
        unscrewCounter = 0;
        totalSavedScrews = 0;
        isJournalOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If gameplay is in workbench Mode:
        if(cameraMovement.workbenchMode == true)
        {
            
            if (shouldCoverLerp)
            {
                if(coverLerp == 0)
                {
                    coverStart = GameObject.Find("BC").transform.position;
                    coverMid = GameObject.Find("batteryCoverMid").transform.position;
                    coverEnd = GameObject.Find("batteryCoverEnd").transform.position;

                    coverStartRot= GameObject.Find("BC").transform.rotation;
                    coverMidRot = GameObject.Find("batteryCoverMid").transform.rotation;
                    coverEndRot = GameObject.Find("batteryCoverEnd").transform.rotation;
                    coverLerp = 0.01f;
                }

                if (coverLerp < 1)
                {
                    GameObject.Find("BC").transform.position = Vector3.Lerp(coverStart, coverMid, coverLerp);
                    GameObject.Find("BC").transform.rotation = Quaternion.Lerp(coverStartRot, coverMidRot, coverLerp);
                    coverLerp = coverLerp + (1.2f * Time.deltaTime);
                }
                if(coverLerp > 1)
                {
                    if(hasRun == false)
                    {
                        coverLerp = 0.01f;
                        coverStart = coverMid;
                        coverMid = coverEnd;
                        coverStartRot = coverMidRot;
                        coverMidRot = coverEndRot;
                        hasRun = true;
                    }
                    else
                    {
                        shouldCoverLerp = false;
                    }
                }
            }

            if (shouldAntennaLerp)
            {
                if(antennaLerp == 0f)
                {
                    antennaLerp = 0.01f;
                    antennaStart = GameObject.Find("Antenna").transform.position;
                    antennaEnd = GameObject.Find("AntennaEndPos").transform.position;
                    antennaStartRot = GameObject.Find("Antenna").transform.rotation;
                    antennaEndRot = GameObject.Find("AntennaEndPos").transform.rotation;
                }

                if(antennaLerp < 1)
                {
                    GameObject.Find("Antenna").transform.position = Vector3.Lerp(antennaStart, antennaEnd, antennaLerp);
                    GameObject.Find("Antenna").transform.rotation = Quaternion.Lerp(antennaStartRot, antennaEndRot, antennaLerp);
                    antennaLerp = antennaLerp + (0.7f * Time.deltaTime);
                }
                if(antennaLerp > 1)
                {
                    shouldAntennaLerp = false;
                    shouldFadeBlack = true;
                }
            }

            if (shouldFadeBlack)
            {
                if (endLerp == 0)
                {
                    full = GameObject.Find("blackBar").GetComponent<Image>().color;
                    full.a = 1;
                    empty = full;
                    empty.a = 0;
                }

                if (endLerp < 1)
                {
                    GameObject.Find("blackBar").GetComponent<Image>().color = Color.Lerp(empty, full, endLerp);
                    endLerp = endLerp + (0.4f * Time.deltaTime);
                }
                if (endLerp > 1)
                {

                    Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                    shouldFadeBlack = false;
                    DontDestroyOnLoad(this.gameObject);
                    SceneManager.LoadScene("Day2After");
                    Cursor.lockState = CursorLockMode.Locked;
                    GameObject.Find("Player").GetComponent<playerMovement>().disableReticle();
                    Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                    endLerp = 0;
                    shouldFadeBack = true;
                }
            }

            if (shouldFadeBack)
            {
                if (endLerp == 0)
                {
                    full = GameObject.Find("blackBar").GetComponent<Image>().color;
                    full.a = 1;
                    empty = full;
                    empty.a = 0;
                }

                if (endLerp < 1)
                {
                    GameObject.Find("blackBar").GetComponent<Image>().color = Color.Lerp(full, empty, endLerp);
                    endLerp = endLerp + (0.4f * Time.deltaTime);
                }
                if (endLerp > 1)
                {
                    shouldFadeBack = false;
                }
            }


            if (shouldBLLerp)
            {
                if (blLerp == 0)
                {
                    BLStart = GameObject.Find("BatteryLeft").transform.position;
                    BLMid = GameObject.Find("batteryMid").transform.position;
                    BLEnd = GameObject.Find("batteryFinal1").transform.position;

                    BLStartRot = GameObject.Find("BatteryLeft").transform.rotation;
                    BLMidRot = GameObject.Find("batteryMid").transform.rotation;
                    BLEndRot = GameObject.Find("batteryFinal1").transform.rotation;
                    blLerp = 0.01f;
                }

                if (blLerp < 1)
                {
                    GameObject.Find("BatteryLeft").transform.position = Vector3.Lerp(BLStart, BLMid, blLerp);
                    GameObject.Find("BatteryLeft").transform.rotation = Quaternion.Lerp(BLStartRot, BLMidRot, blLerp);
                    blLerp = blLerp + (1.2f * Time.deltaTime);
                }
                if (blLerp > 1)
                {
                    if (BLhasRun == false)
                    {
                        blLerp = 0.01f;
                        BLStart = BLMid;
                        BLMid = BLEnd;
                        BLStartRot = BLMidRot;
                        BLMidRot = BLEndRot;
                        BLhasRun = true;

                    }
                    else
                    {
                        shouldBLLerp = false;
                        GameObject.Find("BatteryLeft").transform.position = GameObject.Find("batteryFinal1").transform.position;
                    }
                }
            }
            if (shouldBRLerp)
            {
                if (brLerp == 0)
                {
                    BRStart = GameObject.Find("BatteryRight").transform.position;
                    BRMid = GameObject.Find("batteryMid").transform.position;
                    BREnd = GameObject.Find("batteryFinal2").transform.position;

                    BRStartRot = GameObject.Find("BatteryRight").transform.rotation;
                    BRMidRot = GameObject.Find("batteryMid").transform.rotation;
                    BREndRot = GameObject.Find("batteryFinal2").transform.rotation;
                    brLerp = 0.01f;
                }

                if (brLerp < 1)
                {
                    GameObject.Find("BatteryRight").transform.position = Vector3.Lerp(BRStart, BRMid, brLerp);
                    GameObject.Find("BatteryRight").transform.rotation = Quaternion.Lerp(BRStartRot, BRMidRot, brLerp);
                    brLerp = brLerp + (1.2f * Time.deltaTime);
                }
                if (brLerp > 1)
                {
                    if (BRhasRun == false)
                    {
                        brLerp = 0.01f;
                        BRStart = BRMid;
                        BRMid = BREnd;
                        BRStartRot = BRMidRot;
                        BRMidRot = BREndRot;
                        BRhasRun = true;
                    }
                    else
                    {
                        shouldBRLerp = false;
                        GameObject.Find("BatteryRight").transform.position = GameObject.Find("batteryFinal2").transform.position;
                    }
                }
            }
            // moves screwdriver back to original starting position
            if (backToStart)
            {
                if (lerpPosPosition < 1)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, suitcasePosition, lerpPosPosition);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, suitcaseRotation, lerpPosPosition);
                    lerpPosPosition = lerpPosPosition + (1.5f * Time.deltaTime);
                }
                else
                {
                    lerpPosPosition = 1;
                    backToStart = false;
                }
            }

            if (Input.GetButtonUp("Fire1") && this.transform.position == suitcasePosition)
            {
                RaycastHit hit;
              //  Debug.Log("Clicked!");
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.name == this.transform.name)
                    {
                        Cursor.visible = false;
                        shouldFollowMouse = true;
                        suitcaseCollider.enabled = !suitcaseCollider.enabled;
                        thisCollider.enabled = !thisCollider.enabled;

                    }
                   // Debug.Log(hit.transform.name);
                    if(hit.transform.tag == "Screw" && hit.transform.gameObject.GetComponent<screwBehavior>().isRaised == true)
                    {
                   //     Debug.Log(hit.transform.gameObject.name);
                        hit.transform.gameObject.GetComponent<screwBehavior>().shouldRun = true;
                        hit.transform.gameObject.GetComponent<screwBehavior>().endLocation = totalSavedScrews;
                        totalSavedScrews++;

                    }

                    if(hit.transform.name == "RadioFrame" && GameObject.Find("BrokenRadioDisplay").GetComponent<removableItemBehavior>().removedScrews == 4)
                    {
                        GameObject.Find("BrokenRadioDisplay").GetComponent<removableItemBehavior>().shouldRun = true;
                        GameObject.Find("sparks").GetComponent<ParticleSystem>().Emit(100);
                    }

                    if (hit.transform.name == "workbenchRadio" && GameObject.Find("BrokenRadioSpeaker").GetComponent<removableItemBehavior>().removedScrews == 4)
                    {
                        GameObject.Find("BrokenRadioSpeaker").GetComponent<removableItemBehavior>().shouldRun = true;
                    }

                    if (hit.transform.name == "OpenJournal" && isJournalOpen == false)
                    {
                        Debug.Log(isJournalOpen);
                        hit.transform.gameObject.GetComponent<Animator>().SetTrigger("openBook");
                    }

                    if (hit.transform.name == "OpenJournal" && isJournalOpen == true)
                    {
                        Debug.Log("close");
                        hit.transform.gameObject.GetComponent<Animator>().SetTrigger("putAwayBook");
                    }

                    if (hit.transform.name == "BC")
                    {
                        Debug.Log("battery");
                        shouldCoverLerp = true;
                    }

                    if(hit.transform.name == "BatteryLeft")
                    {
                        Debug.Log("BL");
                        shouldBLLerp = true;
                    }

                    if(hit.transform.name == "BatteryRight")
                    {
                        Debug.Log("BR");
                        shouldBRLerp = true;
                    }

                    if(hit.transform.name == "Antenna" && GameObject.Find("workbenchRadio").transform.rotation == GameObject.Find("radioTopRotation").transform.rotation)
                    {
                        Debug.Log("Ant");
                        shouldAntennaLerp = true;
                    }


                }


            }

            if (shouldFollowMouse && unscrew == false)
            {
                Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectHeight);
                newPos = new Vector3(Camera.main.ScreenToWorldPoint(newPos).x, Camera.main.ScreenToWorldPoint(newPos).y, Camera.main.ScreenToWorldPoint(newPos).z);
                transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);

                
                if(this.gameObject.name == "Screwdriver")
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        RaycastHit hit3;
                        var ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                       // Debug.Log(ray3);
                        if (Physics.Raycast(ray3, out hit3))
                        {

                            if (hit3.transform.name == "Suitcase")
                            {
                                Cursor.visible = true;
                                shouldFollowMouse = false;
                                backToStart = true;
                                lerpPosPosition = 0;
                                thisCollider.enabled = !thisCollider.enabled;
                                suitcaseCollider.enabled = !suitcaseCollider.enabled;
                            }
                        }
                        RaycastHit screwdriverHit;
                        if (Physics.Raycast(GameObject.Find("screwdriverTip").transform.position, Vector3.down, out screwdriverHit))
                        {

                            if (screwdriverHit.transform.tag == "Screw")
                            {
                                //Cursor.lockState = CursorLockMode.Locked;
                                unscrew = true;
                                unscrewCounter = 0;
                                screwLerpPos = 0;
                                currentScrew = screwdriverHit.transform.gameObject;
                                screwStartRotation = currentScrew.transform.rotation;
                                screwEndRotation = currentScrew.transform.rotation;
                                screwEndRotation *= Quaternion.Euler(0, -180, 0);
                                screwStartPosition = currentScrew.transform.position;
                                screwEndPosition = new Vector3(screwStartPosition.x, screwStartPosition.y+0.1f, screwStartPosition.z);
                                screwdriverStartPosition = GameObject.Find("Screwdriver").transform.position;
                                screwdriverEndPosition = new Vector3(screwdriverStartPosition.x, screwdriverStartPosition.y + 0.1f, screwdriverStartPosition.z);
                                screwdriverStartRotation = GameObject.Find("Screwdriver").transform.rotation;
                                screwdriverEndRotation = screwdriverStartRotation;
                                screwdriverEndRotation *= Quaternion.Euler(0, -180, 0);
                                screwdriverHit.transform.gameObject.GetComponent<screwBehavior>().isRaised = true;

                            }
     
                        }


                    }


                    RaycastHit newHit;
                    var newRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(newRay, out newHit)){
                        if(newHit.transform.name == "workbenchRadio" || newHit.transform.name == "RadioFrame")
                        {
                         //   Debug.Log("over");
                            objectHeight = 2.26f;
                            if (this.transform.rotation == startRotation)
                            {
                                lerpPos = 0.01f;
                                shouldRotateTo = true;
                            }
                        }
                        else
                        {
                         //   Debug.Log(this.transform.rotation);
                            objectHeight = 4f;
                            if (this.transform.rotation == endRotation)
                            {
                                lerpPos = 0.01f;
                                shouldRotateAway = true;
                            }
                        }
                    }
                }


                if (shouldRotateTo)
                {
                    if(lerpPos > 1)
                    {
                        shouldRotateTo = false;
                    }
                    if(transform.eulerAngles.x != 180f)
                    {
                        this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpPos);
                        lerpPos = lerpPos + (3.5f * Time.deltaTime);
                    }
                }

                if (shouldRotateAway)
                {
                    if (lerpPos > 1)
                    {
                        shouldRotateAway = false;
                    }
                    if (this.transform.eulerAngles.x != 90f)
                    {
                        this.transform.rotation = Quaternion.Lerp(endRotation, startRotation, lerpPos);
                        lerpPos = lerpPos + (3.5f * Time.deltaTime);
                    }
                }
                

            }
        }

        if (unscrew)
        {

            if (screwLerpPos > 1)
            {
                unscrewCounter++;
                screwLerpPos = 0;
                screwStartPosition = screwEndPosition;
                screwEndPosition = new Vector3(screwStartPosition.x, screwStartPosition.y + 0.1f, screwStartPosition.z);
                screwdriverStartPosition = screwdriverEndPosition;
                screwdriverEndPosition = new Vector3(screwdriverStartPosition.x, screwdriverStartPosition.y + 0.1f, screwdriverStartPosition.z);

                screwdriverStartRotation = GameObject.Find("Screwdriver").transform.rotation;
                screwdriverEndRotation = screwdriverStartRotation;
                screwdriverEndRotation *= Quaternion.Euler(0, -180, 0);
            }
            if (screwLerpPos < 1)
            {
                //Debug.Log(currentScrew.gameObject.name);
                currentScrew.transform.rotation = Quaternion.Lerp(screwStartRotation, screwEndRotation, screwLerpPos);
                currentScrew.transform.position = Vector3.Lerp(screwStartPosition, screwEndPosition, screwLerpPos);

                GameObject.Find("Screwdriver").transform.rotation = Quaternion.Lerp(screwdriverStartRotation, screwdriverEndRotation, screwLerpPos);
                GameObject.Find("Screwdriver").transform.position = Vector3.Lerp(screwdriverStartPosition, screwdriverEndPosition, screwLerpPos);
                //this.transform.rotation = Quaternion.Lerp(endRotation, startRotation, lerpPos);
                screwLerpPos = screwLerpPos + (2.3f * Time.deltaTime);
            }
            if (unscrewCounter == 5)
            {
                this.transform.rotation = endRotation;
                unscrew = false;
            }
        }
    }
}
