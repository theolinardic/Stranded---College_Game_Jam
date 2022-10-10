using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class line : MonoBehaviour
{

    public Vector3 initalPosition;
    public int pointCount = 50;
    public LineRenderer radLine;

    private Vector3 secondPosition;
    private Vector3[] points;
    private float segmentWidth;
    public float testVar;
    public float heightTest;
    float heighClamped;
    public float startHeight;

    public float goalHeight, goalLength;
    public float percentage;
    int percent;
    public GameObject signalStrength;
    int level;
    public bool stopMovement, started;
    float timer, timer2, timer3;
    Color original;
    float endLerp = 0, endLerp2 = 0;
    bool shouldFadeBlack = false;
    Color full, empty;
    bool day3BookClosed = false;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.Find("Camera").GetComponent<AudioSource>();
   //     audio.Play(0);
        stopMovement = true;
        level = 0;
        startHeight = this.transform.localScale.y;

        points = new Vector3[pointCount];
     //   segmentWidth = 10;
        initalPosition = this.transform.position;
        secondPosition = GameObject.Find("Line (1)").transform.position;

        goalHeight = 0.6f;
        goalLength = 7f;

        if(SceneManager.GetActiveScene().name == "Day3")
        {
            testVar = 1;
            heightTest = 0.5f;
        }
        else
        {
            testVar = 0;
            heightTest = 0.5f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Day3" && day3BookClosed == false)
        {
            if (Input.GetButtonDown("Interact"))
            {
                GameObject.Find("Camera").GetComponent<Animator>().SetTrigger("close");
            }

            if (GameObject.Find("Camera").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("done"))
            {
                day3BookClosed = true;
                stopMovement = false;
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
                shouldFadeBlack = false;
                GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(17);
            }
        }

        if (Input.GetButton("LeftDialUp") && heightTest <1 && stopMovement == false && SceneManager.GetActiveScene().name == "Day3")
        {
            var leftKnobStartRotation = GameObject.Find("leftKnob").transform.rotation;
            var leftKnobEndRotation = leftKnobStartRotation;
            leftKnobEndRotation *= Quaternion.Euler(-50, 0, 0);

            GameObject.Find("leftKnob").transform.localRotation = Quaternion.Slerp(leftKnobStartRotation, leftKnobEndRotation, Time.deltaTime * 1f);
            heightTest += 0.001f;
        }

        if (Input.GetButton("LeftDialDown")&& heightTest > 0 && stopMovement == false &&SceneManager.GetActiveScene().name == "Day3")
        {
            var leftKnobStartRotation = GameObject.Find("leftKnob").transform.rotation;
            var leftKnobEndRotation = leftKnobStartRotation;
            leftKnobEndRotation *= Quaternion.Euler(50, 0, 0);

            GameObject.Find("leftKnob").transform.localRotation = Quaternion.Slerp(leftKnobStartRotation, leftKnobEndRotation, Time.deltaTime * 1f);
            heightTest -= 0.001f;
        }

        if (Input.GetButton("RightDialUp") && testVar < 8 && stopMovement == false &&SceneManager.GetActiveScene().name == "Day3")
        {
            var rightKnobStartRotation = GameObject.Find("rightKnob").transform.rotation;
            var rightKnobEndRotation = rightKnobStartRotation;
            rightKnobEndRotation *= Quaternion.Euler(-50, 0, 0);

            GameObject.Find("rightKnob").transform.localRotation = Quaternion.Slerp(rightKnobStartRotation, rightKnobEndRotation, Time.deltaTime * 1f);
            testVar += 0.01f;
        }

        if (Input.GetButton("RightDialDown") && testVar > 0 && stopMovement == false &&SceneManager.GetActiveScene().name == "Day3")
        {
            var rightKnobStartRotation = GameObject.Find("rightKnob").transform.rotation;
            var rightKnobEndRotation = rightKnobStartRotation;
            rightKnobEndRotation *= Quaternion.Euler(50, 0, 0);

            GameObject.Find("rightKnob").transform.localRotation = Quaternion.Slerp(rightKnobStartRotation, rightKnobEndRotation, Time.deltaTime * 1f);
            testVar -= 0.01f;
        }
        testVar = Mathf.Clamp(testVar, 0, 8);
        heighClamped = Mathf.Clamp(heightTest, 0, 1);
        heightTest = heighClamped;
        this.transform.localScale = new Vector3(this.transform.localScale.x, startHeight * heightTest , this.transform.localScale.z);
        var dir = secondPosition - initalPosition;
        segmentWidth = Vector3.Distance(initalPosition, secondPosition) / pointCount;
    //    var angleDifference = Vector3.SignedAngle(transform.right, dir, Vector3.forward);
    if (SceneManager.GetActiveScene().name == "Day3" || SceneManager.GetActiveScene().name == "Day17")
        {
            for (var i = 0; i < pointCount; i++)
            {
                float x = (segmentWidth) * i;
                float y = Mathf.Sin(x * (testVar));
                y = y + Random.Range(0.0f, 0.001f);
                points[i] = new Vector3(x, y, 0);
            }
            radLine.SetPositions(points);
        }


        percentage = (((1-((Mathf.Abs(heightTest - goalHeight))/goalHeight))/2) + ((1 - ((Mathf.Abs(testVar - goalLength)) / goalLength)) / 2)) * 100;
        if (percentage < 0)
        {
            percentage = 0;
        }
        //  percentage = (((heightTest / goalHeight) * 100) / 2) + (((testVar / goalLength) * 10) / 2);

        if(percentage > 99.2f)
        {
            if(started == false)
            {
                started = true;
                stopMovement = true;
                signalStrength.GetComponent<TextMesh>().text = "TESTING CONNECTION";
                timer = 0;
                timer2 = 0;
                timer3 = 0;
            }
            else
            {
                timer = timer + (0.5f * Time.deltaTime);
            }

            if(timer > 1)
            {

                if(timer2 == 0)
                {
                    timer2 = 0.01f;
                    original = signalStrength.GetComponent<TextMesh>().color;
                    signalStrength.GetComponent<TextMesh>().color = Color.red;
                    signalStrength.GetComponent<TextMesh>().text = "CONNECTION FAILED";
                }
                else
                {
                    timer2 = timer2 + (0.5f * Time.deltaTime);
                }
                
                if (timer2 >1)
                {
                    if(timer3 == 0)
                    {
                        timer3 = 0.01f;
                        signalStrength.GetComponent<TextMesh>().color = original;
                        signalStrength.GetComponent<TextMesh>().text = "CHECKING NEXT CHANNEL";
                    }
                    else
                    {
                        timer3 = timer3 + (0.5f * Time.deltaTime);
                    }

                    if(timer3 > 1)
                    {
                        stopMovement = false;

                        if (level == 0)
                        {
                            goalHeight = 0.2f;
                            goalLength = 3.9f;
                            started = false;
                        }

                        else if (level == 1)
                        {
                            goalHeight = 0.9f;
                            goalLength = 5.1f;
                            started = false;
                        }

                        else if (level == 2)
                        {
                            goalHeight = 0.4f;
                            goalLength = 0.6f;
                            started = false;
                        }

                        else if (level == 3)
                        {
                            goalHeight = 0.9f;
                            goalLength = 7f;
                            started = false;
                        }
                        else if(level == 4)
                        {
                            shouldFadeBlack = true;
                        }

                        if(level != 4) {
                            level++;
                            testVar = 1.7f;
                            heightTest = 0.65f;
                        }
                    }
                    
                }

                    
            }

        }
        else
        {
            percent = (int)percentage;
            if (SceneManager.GetActiveScene().name == "Day3")
            {
                signalStrength.GetComponent<TextMesh>().text = percent.ToString() + "%";
            }

        }



    }
}
