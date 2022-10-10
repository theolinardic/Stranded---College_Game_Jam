using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;

    public AudioSource pickupSound;
    public Animator pickedUpItemAnimator;
    public float speed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Vector3 velocity;
    public float x;
    public float z;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public bool isGrounded;

    public Camera rayCamera;

    public float pickupRange = 100f;

    public bool itemPickedUp;
    public Text pickedUpLabel;

    public GameObject postFX;
    Volume volume;
    DepthOfField DOF;
    public Animator playerAnimator;

    bool shouldBriefcaseLerp;
    float briefcaseLerp = 0f;

    Vector3 startBriefPosition, endBriefPosition;
    Quaternion startBriefRotation, endBriefRotation, startCameraRotation, endCameraRotation;

    GameObject pickedUpItem;

    bool isBriefInFront, isBriefOpen, shouldBriefScrewdriverLerp, isItemZoomedIn;

    float screwdrierLerp;
    int pickedUpItemsBriefcase;

    Vector3 screwDriverStartPosition;
    Vector3 screwDriverEndPosition;

    Quaternion screwDriverStartRotation;
    Quaternion screwDriverEndRotation;

    Vector3 radioStartPosition;
    Vector3 radioEndPosition;

    Quaternion radioStartRotation;
    Quaternion radioEndRotation;

    Vector3 briefcaseStartPosition;
    Vector3 briefcaseEndPosition;

    Quaternion briefcaseStartRotation;
    Quaternion briefcaseEndRotation;

    Color full, empty;

    bool shouldBriefScrewdriverLerpAway, shouldBriefRadioLerp, shouldBriefRadioLerpAway;
    float radioLerp, radioLerpAway, briefLerpAway;
    float screwdrierAwayLerp = 0;

    int removedTotalBriefcase = 0;

    bool shouldRadioTurnBack;
    float radioTurnBack = 0f;
    Quaternion radioStart, radioEnd;

    bool shouldSuitcaseLerp, isSuitInFront;
    float suitcaseLerp = 0f;

    bool shouldSuitJournalLerp = false;
    float journalLerp = 0;

    bool shouldSuitJournalLerpAway = false;
    float journalLerpAway = 0;

    Vector3 journalStartPos, journalEndPos;
    Quaternion journalStartRot, journalEndRot;

    bool shouldDacLerp, shouldDacLerpAway;
    float dacLerp, dacLerpAway;
    Vector3 dacStartPos, dacEndPos;
    Quaternion dacStartRot, dacEndRot;

    bool shouldMatchLerp, shouldMatchLerpAway;
    float matchLerp, matchLerpAway;

    Vector3 matchStartPos, matchEndPos;
    Quaternion matchStartRot, matchEndRot;

    bool wasJournalPickedUp, wasMatchPickedUp, wasDacPickedUp;

    bool shouldSuitcaseLerpAway;
    float suitcaseLerpAway = 0;

    public GameObject campfire;

    bool day1EndedAnim;

    bool isBriefDone;

    bool animPlaying, EOD;

    float endLerp = 0, endLerp2 = 0;

    bool shouldRadRotateUp = false;
    float radUpLerp = 0;

    void Start()
    {
        shouldSuitcaseLerp = false;
        radioTurnBack = 0f;
        briefcaseLerp = 0;
        briefLerpAway = 0;
        pickedUpItemsBriefcase = 0;
        isItemZoomedIn = false;
        volume = postFX.GetComponent<Volume>();

        volume.sharedProfile.TryGet<DepthOfField>(out DOF);
        isBriefOpen = false;

    }
    void Update()
    {

        
        if(Input.GetButtonDown("Interact") && SceneManager.GetActiveScene().name == "Day2After")
        {
            GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(3);
        }
        if(wasJournalPickedUp && wasDacPickedUp && wasMatchPickedUp && isBriefDone)
        {
            day1EndedAnim = true;
           // Debug.Log("FADE");
        }
        if(SceneManager.GetActiveScene().name == "Day1" && day1EndedAnim == true)
        {
            if(endLerp == 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                full = GameObject.Find("blackBar").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }

            if (endLerp < 1)
            {
                GameObject.Find("blackBar").GetComponent<Image>().color = Color.Lerp(empty, full, endLerp);
                endLerp = endLerp + (0.8f * Time.deltaTime);
            }
            if(endLerp > 1)
            {
                disableReticle();
                if(endLerp2 == 0)
                {
                    campfire.SetActive(true);
                    //tp camera spawn fire
                    Camera.main.transform.position = GameObject.Find("endDayCameraPos").transform.position;
                    Camera.main.transform.rotation = GameObject.Find("endDayCameraPos").transform.rotation;
                }
                if(endLerp2 < 1)
                {
                    GameObject.Find("blackBar").GetComponent<Image>().color = Color.Lerp(full, empty, endLerp2);
                    endLerp2 = endLerp2 + (0.8f * Time.deltaTime);
                }
                if(endLerp2 > 1)
                {
                    day1EndedAnim = false;
                    EOD = true;
                }
            }

            

        }
        if (EOD)
        {
            if (Input.GetButtonDown("Interact"))
            {
                GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(2);

            }
        }
        if (Input.GetButtonDown("Tab"))
        {
            GameObject.Find("Canvas").GetComponent<updateHud>().displayHud();
        }
        //isGrounded uses physics.checksphere to check if the defined groundcheck object is touching the ground (groundmask [which is a layer set in inspector]) within a certain radius (groundDistance)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            //-2 instead of 0 is to make transition to ground smoother and more consistent
            velocity.y = -2f;
        }

        if (itemPickedUp == false && cameraMovement.workbenchMode == false)
        {
            //this method is good for tracking input because it will also work automatically with controller inputs instead of having to manually rebind those
            //vertical - > forward (w) = 1 & backward (s) = -1 & controller logic is the same (up on stick & down on stick)
            //horizontal - > left (a) = -1 & right (d) = 1 & controller logic is the same (left on stick & right on stick)
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
            if (x != 0 || z != 0)
            {

            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //equation for needed jump height is the square root of (the jump height * -2 * gravity)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (Input.GetButtonDown("Interact"))
            {
                RaycastHit hit;

                // Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward, Color.green, 10f, true);
                if (Physics.Raycast(rayCamera.transform.position, rayCamera.transform.forward, out hit, pickupRange))
                {
                    ItemPickup hitTarget = hit.transform.GetComponent<ItemPickup>();
                    if (hitTarget != null)
                    {
                        itemPickedUp = true;
                        hitTarget.pickUp();
                    }
                    if (hit.collider.gameObject.name == "Workbench")
                    {
                        bool newFalse = false;
                        //hit.collider.gameObject.GetComponent<description>().turnFalse();

                        GameObject.Find("Canvas").GetComponent<updateHud>().changeHud("Dissasemble and salvage parts from the Broken Radio");
                        rayCamera.transform.position = GameObject.Find("workBenchCameraPos").transform.position;
                        rayCamera.transform.eulerAngles = GameObject.Find("workBenchCameraPos").transform.eulerAngles;
                        cameraMovement.workbenchMode = true;
                    }

                    if(hit.collider.gameObject.name == "briefcase" && SceneManager.GetActiveScene().name == "Day1")
                    {
                        hit.collider.gameObject.GetComponent<description>().turnFalse();
                        Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                     //   hit.transform.GetComponent<description>().time +=1000;
                        itemPickedUp = true;
                        enableDepthOfField();
                        pickedUpItem = hit.collider.gameObject;

                        //pickedUpItem.GetComponent<Animator>().enabled = false;

                        shouldBriefcaseLerp = true;
                    }

                    if(hit.collider.gameObject.name == "Suitcase" && SceneManager.GetActiveScene().name == "Day1")
                    {
                        Debug.Log("SUITCASE");
                        hit.collider.gameObject.GetComponent<description>().turnFalse();
                        Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                      //  hit.transform.GetComponent<description>().time += 1000;
                        itemPickedUp = true;
                        enableDepthOfField();
                        pickedUpItem = hit.collider.gameObject;
                        shouldSuitcaseLerp = true;
                    }
                }


            }
            if (Input.GetButtonDown("Shift"))
                speed = 35f;
            else
                speed = 15f;

            //transform.right gets current player position and moves to the right (horizontal movement) and is multiplied by x (the players horizontal input)
            //transform.forward gets current player position and moves foward (vertical movement) and is multiplied by z (players vertical input)
            Vector3 move = transform.right * x + transform.forward * z;

            //time.deltatime is so movement speed does not change with framerate changes
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            //we multiply by time.deltatime again because velocity equation is time^2
            controller.Move(velocity * Time.deltaTime);
        }
        else if (cameraMovement.workbenchMode == true)
        {
            if (Input.GetButtonDown("Interact"))
            {
                //Debug.Log(GameObject.Find("workbenchRadio").transform.rotation);
                //  GameObject.Find("workbenchRadio").transform.rotation = Quaternion.Lerp(GameObject.Find("workbenchRadio").transform.rotation, new Quaternion, 0.1f);

            }
        }
        else
        {
            if (Input.GetButtonDown("Interact"))
            {
                //itemPickedUp = false;
                pickedUpLabel.text = "";

            }
        }

        if(itemPickedUp == true)
        {
            if (pickedUpItem.name == "briefcase")
            {
                

                if (Input.GetButtonUp("Fire1"))
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == "briefRadio" && isItemZoomedIn == false)
                        {
                            pickupSound.Play(0);
                            isItemZoomedIn = true;
                            shouldBriefRadioLerp = true;
                            radioLerp = 0;
                            GameObject.Find("radioToolTip").GetComponent<Image>().enabled = true;
                        }

                        if (hit.transform.name == "briefRadio" && isItemZoomedIn == true && radioLerp > 0.9f)
                        {
                            isItemZoomedIn = false;
                            shouldBriefRadioLerpAway = true;
                            radioLerpAway = 0;
                        }

                        if (hit.transform.name == "briefScrewdriver" && isItemZoomedIn == false)
                        {
                            pickupSound.Play(0);
                            isItemZoomedIn = true;
                            shouldBriefScrewdriverLerp = true;
                            screwdrierLerp = 0;
                            GameObject.Find("screwdriverToolTip").GetComponent<Image>().enabled = true;
                        }

                        if (hit.transform.name == "briefScrewdriver" && isItemZoomedIn == true && screwdrierLerp > 0.9f)
                        {
                            isItemZoomedIn = false;
                            shouldBriefScrewdriverLerpAway = true;
                            screwdrierAwayLerp = 0;

                        }

                    }
                }

            }

            if(pickedUpItem.name == "Suitcase")
            {
                if(wasJournalPickedUp && wasDacPickedUp && wasMatchPickedUp)
                {
                    GameObject.Find("box").GetComponent<Animator>().SetTrigger("close");

                }
                if(wasJournalPickedUp && wasDacPickedUp && wasMatchPickedUp && GameObject.Find("box").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && suitcaseLerpAway == 0)
                {
                    shouldSuitcaseLerpAway = true;
                }
                pickedUpItem.GetComponent<Collider>().enabled = false;
                if (Input.GetButtonUp("Fire1"))
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == "journal" && isItemZoomedIn == false)
                        {
                            pickupSound.Play(0);
                            shouldSuitJournalLerp = true;
                            journalLerp = 0;
                            GameObject.Find("journalToolTip").GetComponent<Image>().enabled = true;
                        }

                        if (hit.transform.name == "journal" && isItemZoomedIn == true && journalLerpAway > 0.9f)
                        {
                            shouldSuitJournalLerpAway = true;
                            journalLerpAway = 0;
                        }

                        if (hit.transform.name == "DAC" && isItemZoomedIn == false)
                        {
                            pickupSound.Play(0);
                            shouldDacLerp = true;
                            dacLerp = 0;
                            GameObject.Find("dacToolTip").GetComponent<Image>().enabled = true;
                        }

                        if (hit.transform.name == "DAC" && isItemZoomedIn == true && dacLerp > 0.9f)
                        {
                            shouldDacLerpAway = true;
                            dacLerpAway = 0;
                        }

                        if (hit.transform.name == "matchbox" && isItemZoomedIn == false)
                        {
                            pickupSound.Play(0);
                            shouldMatchLerp = true;

                            matchLerp = 0;
                            GameObject.Find("matchboxToolTip").GetComponent<Image>().enabled = true;
                        }

                        if (hit.transform.name == "matchbox" && isItemZoomedIn == true && matchLerp > 0.9f)
                        {
                            shouldMatchLerpAway = true;
                            matchLerpAway = 0;
                        }

                    }
                }
            }
        }

        if(shouldBriefcaseLerp == true)
        {
            //Debug.Log(briefcaseLerp);
            if(briefcaseLerp == 0f)
            {
                briefcaseLerp = 0.01f;
                Vector3 cameraRotationTemp = Camera.main.transform.eulerAngles;
                startCameraRotation = Camera.main.transform.rotation;
                endCameraRotation = Quaternion.Euler(0, cameraRotationTemp.y, cameraRotationTemp.z);

                startBriefPosition = GameObject.Find("briefcase").transform.position;
                endBriefPosition = GameObject.Find("briefcaseCameraPos").transform.position;

                startBriefRotation = GameObject.Find("briefcase").transform.rotation;
                endBriefRotation = GameObject.Find("briefcaseCameraPos").transform.rotation;
 

            }
            if (briefcaseLerp < 1)
            {
                GameObject.Find("briefcase").transform.position = Vector3.Lerp(startBriefPosition, endBriefPosition, briefcaseLerp);
                GameObject.Find("briefcase").transform.rotation = Quaternion.Lerp(startBriefRotation, endBriefRotation, briefcaseLerp);

                Camera.main.transform.rotation = Quaternion.Lerp(startCameraRotation, endCameraRotation, briefcaseLerp);

                briefcaseLerp = briefcaseLerp + (0.5f * Time.deltaTime);
            }
            if(briefcaseLerp > 1)
            {
               // pickedUpItem.GetComponent<Animator>().enabled = false;

                Camera.main.transform.rotation = endCameraRotation;

                shouldBriefcaseLerp = false;

               // pickedUpItem = GameObject.Find("briefcase");

                isBriefInFront = true;
                pickedUpItemAnimator = pickedUpItem.GetComponent<Animator>();
                pickedUpItemAnimator.SetTrigger("openBriefcase");
                pickedUpItem.GetComponent<Collider>().enabled = false;
                isBriefOpen = true;
                disableReticle();
             

            }
        }


        if(shouldBriefScrewdriverLerpAway == true)
        {
            if (screwdrierAwayLerp == 0f)
            {
                screwdrierAwayLerp = 0.01f;

                full = GameObject.Find("screwdriverToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (screwdrierAwayLerp < 1)
            {
                GameObject.Find("briefScrewdriver").transform.position = Vector3.Lerp(screwDriverEndPosition, screwDriverStartPosition, screwdrierAwayLerp);
                GameObject.Find("briefScrewdriver").transform.rotation = Quaternion.Lerp(screwDriverEndRotation, screwDriverStartRotation, screwdrierAwayLerp);
                GameObject.Find("screwdriverToolTip").GetComponent<Image>().color = Color.Lerp(full, empty, screwdrierAwayLerp);

                screwdrierAwayLerp = screwdrierAwayLerp + (1.5f * Time.deltaTime);
            }
            if (screwdrierAwayLerp > 1)
            {
                GameObject.Find("screwdriverToolTip").GetComponent<Image>().enabled = false;
                shouldBriefScrewdriverLerpAway = false;
                pickedUpItemsBriefcase++;
            }
        }

        if (shouldBriefScrewdriverLerp == true)
        {
            if (screwdrierLerp == 0f)
            {
                screwdrierLerp = 0.01f;

                screwDriverStartPosition = GameObject.Find("briefScrewdriver").transform.position;
                screwDriverEndPosition = GameObject.Find("briefScrewdriverPosition").transform.position;

                screwDriverStartRotation = GameObject.Find("briefScrewdriver").transform.rotation;
                screwDriverEndRotation = GameObject.Find("briefScrewdriverPosition").transform.rotation;

                full = GameObject.Find("screwdriverToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (screwdrierLerp < 1)
            {
                GameObject.Find("briefScrewdriver").transform.position = Vector3.Lerp(screwDriverStartPosition, screwDriverEndPosition, screwdrierLerp);
                GameObject.Find("briefScrewdriver").transform.rotation = Quaternion.Lerp(screwDriverStartRotation, screwDriverEndRotation, screwdrierLerp);
                GameObject.Find("screwdriverToolTip").GetComponent<Image>().color = Color.Lerp(empty, full, screwdrierLerp);

                screwdrierLerp = screwdrierLerp + (1.5f * Time.deltaTime);
            }
            if (screwdrierLerp > 1)
            {
                shouldBriefScrewdriverLerp = false;
            }
        }

        if (shouldBriefRadioLerpAway == true)
        {

            if (radioLerpAway == 0f)
            {
                radioLerpAway = 0.01f;

                full = GameObject.Find("radioToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (radioLerpAway < 1)
            {
                GameObject.Find("briefRadio").transform.position = Vector3.Lerp(screwDriverEndPosition, screwDriverStartPosition, radioLerpAway);
                GameObject.Find("briefRadio").transform.rotation = Quaternion.Lerp(screwDriverEndRotation, screwDriverStartRotation, radioLerpAway);
                GameObject.Find("radioToolTip").GetComponent<Image>().color = Color.Lerp(full, empty, radioLerpAway);

                radioLerpAway = radioLerpAway + (1.5f * Time.deltaTime);
            }
            if (radioLerpAway > 1)
            {
                GameObject.Find("radioToolTip").GetComponent<Image>().enabled = false;
                shouldBriefRadioLerpAway = false;
                pickedUpItemsBriefcase++;
            }
        }

        if (shouldBriefRadioLerp == true)
        {
        //    Debug.Log(radioLerp);
            if (radioLerp == 0f)
            {
                radioLerp = 0.01f;

                screwDriverStartPosition = GameObject.Find("briefRadio").transform.position;
                screwDriverEndPosition = GameObject.Find("briefRadioPosition").transform.position;

                screwDriverStartRotation = GameObject.Find("briefRadio").transform.rotation;
                screwDriverEndRotation = GameObject.Find("briefRadioPosition").transform.rotation;

                full = GameObject.Find("radioToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (radioLerp < 1)
            {
                GameObject.Find("briefRadio").transform.position = Vector3.Lerp(screwDriverStartPosition, screwDriverEndPosition, radioLerp);
                GameObject.Find("briefRadio").transform.rotation = Quaternion.Lerp(screwDriverStartRotation, screwDriverEndRotation, radioLerp);
                GameObject.Find("radioToolTip").GetComponent<Image>().color = Color.Lerp(empty, full, radioLerp);

                radioLerp = radioLerp + (1.5f * Time.deltaTime);
            }
            if (radioLerp > 1)
            {
                shouldBriefRadioLerp = false;
            }
        }


        if (radioLerpAway > 0.99f && screwdrierAwayLerp > 0.99f)
        {
          //  Debug.Log("testestset");
            if (briefLerpAway == 0f)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Camera.main.GetComponent<cameraMovement>().canMoveCamera = false;
                pickedUpItemAnimator.SetTrigger("closeBriefcase");
                briefLerpAway = 0.01f;

                briefcaseStartPosition = GameObject.Find("briefcase").transform.position;
                briefcaseEndPosition = GameObject.Find("briefPutAwayPosition").transform.position;

                briefcaseStartRotation = GameObject.Find("briefcase").transform.rotation;
                briefcaseEndRotation = GameObject.Find("briefPutAwayPosition").transform.rotation;

            }
            if (briefLerpAway < 1 && pickedUpItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                pickedUpItem.GetComponent<Animator>().enabled = false;
                Camera.main.transform.rotation = Quaternion.Lerp(endCameraRotation, startCameraRotation, briefLerpAway);
                GameObject.Find("briefcase").transform.position = Vector3.Lerp(briefcaseStartPosition, briefcaseEndPosition, briefLerpAway);
                GameObject.Find("briefcase").transform.rotation = Quaternion.Lerp(briefcaseStartRotation, briefcaseEndRotation, briefLerpAway);

                briefLerpAway = briefLerpAway + (0.75f * Time.deltaTime);
            }
            if (briefLerpAway > 1)
            {
                Cursor.lockState = CursorLockMode.None;
                isBriefOpen = false;
                itemPickedUp = false;
                radioLerpAway = 0;
                enableReticle();
                disableDepthOfField();
                Camera.main.GetComponent<cameraMovement>().canMoveCamera = true;
                GameObject.Find("briefcase").SetActive(false);
              //  GameObject.Find("Canvas").GetComponent<updateHud>().changeHud("Look for more luggage");

                isBriefDone = true;
            }
        }

        if(GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().removedItems == 2)
        {
           // Debug.Log(radioTurnBack);
          if(radioTurnBack == 0)
            {
                radioTurnBack = 0.01f;
                radioStart = GameObject.Find("workbenchRadio").transform.rotation;
                radioEnd = GameObject.Find("radioBackRotation").transform.rotation;
                //Debug.Log("radTest");
            }

          if(radioTurnBack < 1)
            {
                GameObject.Find("workbenchRadio").transform.rotation = Quaternion.Lerp(radioStart, radioEnd, radioTurnBack);
                radioTurnBack = radioTurnBack + (0.8f * Time.deltaTime);
            }
          if(radioTurnBack > 1)
            {
                GameObject.Find("Screwdriver").GetComponent<workbenchIttem>().removedItems++;
            }
        }
        if(SceneManager.GetActiveScene().name == "Day2")
        {
            if (GameObject.Find("BatteryLeft").transform.position == GameObject.Find("batteryFinal1").transform.position && GameObject.Find("BatteryRight").transform.position == GameObject.Find("batteryFinal2").transform.position)
            {
                //   Debug.Log("Done");
                shouldRadioTurnBack = true;
                GameObject.Find("BatteryLeft").transform.parent = GameObject.Find("radioBackRotation").transform;
                GameObject.Find("BatteryRight").transform.parent = GameObject.Find("radioBackRotation").transform;
                GameObject.Find("BC").transform.parent = GameObject.Find("radioBackRotation").transform;
            }
        }


        if(shouldRadioTurnBack == true)
        {
            if (radUpLerp == 0)
            {
                radUpLerp = 0.01f;
                radioStartRotation = GameObject.Find("workbenchRadio").transform.rotation;
                radioEndRotation = GameObject.Find("radioTopRotation").transform.rotation;
            }

            if(radUpLerp < 1)
            {
                GameObject.Find("workbenchRadio").transform.rotation = Quaternion.Lerp(radioStartRotation, radioEndRotation, radUpLerp);
                radUpLerp = radUpLerp + (1.2f * Time.deltaTime);
            }
            if(radUpLerp > 1)
            {
                GameObject.Find("workbenchRadio").transform.rotation = GameObject.Find("radioTopRotation").transform.rotation;
                shouldRadioTurnBack = false;
            }
        }

        if (shouldSuitcaseLerp == true)
        {
            Debug.Log("here");
            //Debug.Log(briefcaseLerp);
            if (suitcaseLerp == 0f)
            {

                GameObject.Find("box").GetComponent<Animator>().enabled = false;
                GameObject.Find("Suitcase").GetComponent<Collider>().enabled = false;
                suitcaseLerp = 0.01f;
                Vector3 cameraRotationTemp = Camera.main.transform.eulerAngles;
                startCameraRotation = Camera.main.transform.rotation;
                endCameraRotation = Quaternion.Euler(0, cameraRotationTemp.y, cameraRotationTemp.z);

                startBriefPosition = GameObject.Find("Suitcase").transform.position;

                endBriefPosition = GameObject.Find("suitcaseCameraPos").transform.position;


                startBriefRotation = GameObject.Find("Suitcase").transform.rotation;
                endBriefRotation = GameObject.Find("suitcaseCameraPos").transform.rotation;

            }
            if (suitcaseLerp < 1)
            {
                Debug.Log(suitcaseLerp);
                //   Debug.Log(GameObject.Find("Suitcase").transform.position);
                //    Debug.Log(GameObject.Find("box").transform.position);
                GameObject.Find("Suitcase").transform.position = Vector3.Lerp(startBriefPosition, endBriefPosition, suitcaseLerp);
                GameObject.Find("Suitcase").transform.rotation = Quaternion.Lerp(startBriefRotation, endBriefRotation, suitcaseLerp);

                Camera.main.transform.rotation = Quaternion.Lerp(startCameraRotation, endCameraRotation, suitcaseLerp);

                suitcaseLerp = suitcaseLerp + (0.5f * Time.deltaTime);
            }
            if (suitcaseLerp > 1)
            {

                GameObject.Find("box").GetComponent<Animator>().enabled = true;
                GameObject.Find("box")  .GetComponent<Animator>().SetTrigger("open");
                Camera.main.transform.rotation = endCameraRotation;

                shouldSuitcaseLerp = false;

                isSuitInFront = true;
                disableReticle();
             //   GameObject.Find("Suitcase").transform.DetachChildren();
            }
        }

        if (shouldSuitcaseLerpAway == true)
        {
            //Debug.Log(briefcaseLerp);
            if (suitcaseLerpAway == 0f)
            {

                GameObject.Find("box").GetComponent<Animator>().enabled = false;
                suitcaseLerpAway = 0.01f;
                Vector3 cameraRotationTemp = Camera.main.transform.eulerAngles;

                startBriefPosition = GameObject.Find("Suitcase").transform.position;
                endBriefPosition = GameObject.Find("suitPutAwayPosition").transform.position;


                startBriefRotation = GameObject.Find("Suitcase").transform.rotation;
                endBriefRotation = GameObject.Find("suitPutAwayPosition").transform.rotation;

            }
            if (suitcaseLerpAway < 1)
            {
                GameObject.Find("Suitcase").transform.position = Vector3.Lerp(startBriefPosition, endBriefPosition, suitcaseLerpAway);
                GameObject.Find("Suitcase").transform.rotation = Quaternion.Lerp(startBriefRotation, endBriefRotation, suitcaseLerpAway);

                Camera.main.transform.rotation = Quaternion.Lerp(endCameraRotation, startCameraRotation, suitcaseLerpAway);

                suitcaseLerpAway = suitcaseLerpAway + (0.5f * Time.deltaTime);
             //  Debug.Log(suitcaseLerpAway);
            }
            if (suitcaseLerpAway > 1)
            {
                Camera.main.transform.rotation = endCameraRotation;

                shouldSuitcaseLerpAway = false;

                isSuitInFront = false;

                Cursor.lockState = CursorLockMode.None;
                itemPickedUp = false;
                enableReticle();
                disableDepthOfField();
                Camera.main.GetComponent<cameraMovement>().canMoveCamera = true;
                GameObject.Find("Suitcase").SetActive(false);

             //   Debug.Log("suittest");
                //   GameObject.Find("Suitcase").transform.DetachChildren();
            }
        }


        if (shouldSuitJournalLerpAway == true)
        {

            if (journalLerp == 0f)
            {
                journalLerp = 0.01f;

                full = GameObject.Find("journalToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (journalLerp < 1)
            {
                GameObject.Find("journal").transform.position = Vector3.Lerp(journalEndPos, journalStartPos, journalLerp);
                GameObject.Find("journal").transform.rotation = Quaternion.Lerp(journalEndRot, journalStartRot, journalLerp);
                GameObject.Find("journalToolTip").GetComponent<Image>().color = Color.Lerp(full, empty, journalLerp);

                journalLerp = journalLerp + (1.5f * Time.deltaTime);
            }
            if (journalLerp > 1)
            {
                GameObject.Find("journalToolTip").GetComponent<Image>().enabled = false;
                shouldSuitJournalLerpAway = false;
                isItemZoomedIn = false;
                wasJournalPickedUp = true;
                // pickedUpItemsBriefcase++;
            }
        }

        if (shouldSuitJournalLerp == true)
        {
            //    Debug.Log(radioLerp);
            if (journalLerpAway == 0f)
            {
                journalLerpAway = 0.01f;

                journalStartPos = GameObject.Find("journal").transform.position;
                journalEndPos = GameObject.Find("journalCameraPos").transform.position;

                journalStartRot = GameObject.Find("journal").transform.rotation;
                journalEndRot = GameObject.Find("journalCameraPos").transform.rotation;
                    //GameObject.Find("journalCameraPos").transform.rotation;

                full = GameObject.Find("journalToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;
            }
            if (journalLerpAway < 1)
            {
                GameObject.Find("journal").transform.position = Vector3.Lerp(journalStartPos, journalEndPos, journalLerpAway);
                GameObject.Find("journal").transform.rotation = Quaternion.Lerp(journalStartRot, journalEndRot, journalLerpAway);
                GameObject.Find("journalToolTip").GetComponent<Image>().color = Color.Lerp(empty, full, journalLerpAway);

                journalLerpAway = journalLerpAway + (1.5f * Time.deltaTime);
            }
            if (journalLerpAway > 1)
            {
                shouldSuitJournalLerp = false;
                isItemZoomedIn = true;
            }
        }

        if (shouldDacLerp == true)
        {

            if (dacLerp == 0f)
            {
                GameObject.Find("dacToolTip").GetComponent<Image>().enabled = true;
                dacLerp = 0.01f;

                full = GameObject.Find("dacToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;

                dacStartPos = GameObject.Find("DAC").transform.position;
                dacEndPos = GameObject.Find("dacCameraPos").transform.position;

                dacStartRot = GameObject.Find("DAC").transform.rotation;
                dacEndRot = GameObject.Find("dacCameraPos").transform.rotation;

            }
            if (dacLerp < 1)
            {
                GameObject.Find("DAC").transform.position = Vector3.Lerp(dacStartPos, dacEndPos, dacLerp);
                GameObject.Find("DAC").transform.rotation = Quaternion.Lerp(dacStartRot, dacEndRot, dacLerp);
                GameObject.Find("dacToolTip").GetComponent<Image>().color = Color.Lerp(empty, full, dacLerp);

                dacLerp = dacLerp + (1.5f * Time.deltaTime);
            }
            if (dacLerp > 1)
            {

                shouldDacLerp = false;
                isItemZoomedIn = true;
                // pickedUpItemsBriefcase++;
            }
        }

        if (shouldDacLerpAway == true)
        {

            if (dacLerpAway == 0f)
            {

                dacLerpAway = 0.01f;

                full = GameObject.Find("dacToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;

            }
            if (dacLerpAway < 1)
            {
                GameObject.Find("DAC").transform.position = Vector3.Lerp(dacEndPos, dacStartPos, dacLerpAway);
                GameObject.Find("DAC").transform.rotation = Quaternion.Lerp(dacEndRot, dacStartRot, dacLerpAway);
                GameObject.Find("dacToolTip").GetComponent<Image>().color = Color.Lerp(full, empty, dacLerpAway);

                dacLerpAway = dacLerpAway + (1.5f * Time.deltaTime);
            }
            if (dacLerpAway > 1)
            {
                wasDacPickedUp = true;
                shouldDacLerpAway = false;
                isItemZoomedIn = false;
                GameObject.Find("dacToolTip").GetComponent<Image>().enabled = false;
                // pickedUpItemsBriefcase++;
            }
        }

        if (shouldMatchLerp == true)
        {

            if (matchLerp == 0f)
            {
                GameObject.Find("matchboxToolTip").GetComponent<Image>().enabled = true;
                matchLerp = 0.01f;

                full = GameObject.Find("matchboxToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;

                matchStartPos = GameObject.Find("matchbox").transform.position;
                matchEndPos = GameObject.Find("matchCameraPos").transform.position;

                matchStartRot = GameObject.Find("matchbox").transform.rotation;
                matchEndRot = GameObject.Find("matchCameraPos").transform.rotation;

            }
            if (matchLerp < 1)
            {
                GameObject.Find("matchbox").transform.position = Vector3.Lerp(matchStartPos, matchEndPos, matchLerp);
                GameObject.Find("matchbox").transform.rotation = Quaternion.Lerp(matchStartRot, matchEndRot, matchLerp);
                GameObject.Find("matchboxToolTip").GetComponent<Image>().color = Color.Lerp(empty, full, matchLerp);

                matchLerp = matchLerp + (1.5f * Time.deltaTime);
            }
            if (matchLerp > 1)
            {

                shouldMatchLerp = false;
                isItemZoomedIn = true;
                // pickedUpItemsBriefcase++;
            }
        }

        if (shouldMatchLerpAway == true)
        {

            if (matchLerpAway == 0f)
            {

                matchLerpAway = 0.01f;

                full = GameObject.Find("matchboxToolTip").GetComponent<Image>().color;
                full.a = 1;
                empty = full;
                empty.a = 0;

            }
            if (matchLerpAway < 1)
            {
                GameObject.Find("matchbox").transform.position = Vector3.Lerp(matchEndPos, matchStartPos, matchLerpAway);
                GameObject.Find("matchbox").transform.rotation = Quaternion.Lerp(matchEndRot, matchStartRot, matchLerpAway);
                GameObject.Find("matchboxToolTip").GetComponent<Image>().color = Color.Lerp(full, empty, matchLerpAway);

                matchLerpAway = matchLerpAway + (1.5f * Time.deltaTime);
            }
            if (matchLerpAway > 1)
            {
                wasMatchPickedUp = true;
                shouldMatchLerpAway = false;
                isItemZoomedIn = false;
                GameObject.Find("matchboxToolTip").GetComponent<Image>().enabled = false;
                // pickedUpItemsBriefcase++;
            }
        }



    }

    public void enableDepthOfField()
    {
        DepthOfFieldModeParameter newtest = new DepthOfFieldModeParameter(DepthOfFieldMode.Bokeh, true);
        DOF.mode.SetValue(newtest);
    }

    public void disableDepthOfField()
    {
        DepthOfFieldModeParameter newtest = new DepthOfFieldModeParameter(DepthOfFieldMode.Off, true);
        DOF.mode.SetValue(newtest);
    }

    public void enableReticle()
    {
        GameObject.Find("reticle").GetComponent<Image>().enabled = true;
    }
    public void disableReticle()
    {
        GameObject.Find("reticle").GetComponent<Image>().enabled = false;
    }

    public void disableAnimator()
    {
       playerAnimator.enabled = false;
    }

    public void displayHud(string newText){

        GameObject.Find("Canvas").GetComponent<updateHud>().changeHud(newText);
    }

    public void enableCamera()
    {
        GameObject.Find("playerCamera").GetComponent<cameraMovement>().canMoveCamera = true;
    }

    public void disableCamera()
    {
        GameObject.Find("playerCamera").GetComponent<cameraMovement>().canMoveCamera = false;
    }

    public void spawnFire()
    {
        campfire.SetActive(true);
    }



}
