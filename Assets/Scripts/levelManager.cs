using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    public int currentDay;

    public AudioSource transitionSound, mainMusic;
    public GameObject player;
    Animator playerAnimator;
    public bool shouldPlayAwakeAnim, hasPlayedAwakeAnimation;
    bool shouldDisplayDayLabel;
    float timeSinceStartDayLabel;

    public GameObject day1Button, day2Button, day3Button;
    float day17Timer = 0;

    float menuAnimTimer = 0;
    bool menuAnim = false;
    bool animStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceStartDayLabel = 100;
        hasPlayedAwakeAnimation = false;
        currentDay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");

        if (menuAnim == true)
        {
            if (GameObject.Find("planeTest").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("planeStart") && animStarted == false)
            {
                animStarted = true;
            }
            if (GameObject.Find("planeTest").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("planeEnd") && animStarted == true)
            {
                
                menuAnim = false;
                DontDestroyOnLoad(this.gameObject);
                SceneManager.LoadScene("Day1");
                timeSinceStartDayLabel = 0f;
                shouldDisplayDayLabel = true;
                transitionSound.PlayDelayed(0.2f);
                currentDay = 1;
            }

        }
        //prologue
        if (currentDay == 0)
        {

        }

        //day1
        if (currentDay == 1)
        {
            if(shouldDisplayDayLabel == true)
            {
                // prevent glitches
                if(SceneManager.GetActiveScene().name == "Day1")
                {
                    
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = true;
                    timeSinceStartDayLabel = timeSinceStartDayLabel + Time.deltaTime;
                }
                if(timeSinceStartDayLabel > 4)
                {
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = false;
                    shouldDisplayDayLabel = false;
                    shouldPlayAwakeAnim = true;
                }
            }
            if(shouldPlayAwakeAnim == true && hasPlayedAwakeAnimation == false)
            {
                playerAnimator = player.GetComponent<Animator>();
                playerAnimator.SetBool("playAwake", true);
                hasPlayedAwakeAnimation = false;
                shouldPlayAwakeAnim = false;
            }
        }

        //day 2
        if (currentDay == 2)
        {
            if(shouldDisplayDayLabel == true)
            {
                if (SceneManager.GetActiveScene().name == "Day2")
                {
                    
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = true;
                    timeSinceStartDayLabel = timeSinceStartDayLabel + Time.deltaTime;
                }
                if (timeSinceStartDayLabel > 4)
                {
                    Camera.main.transform.position = GameObject.Find("workBenchCameraPos").transform.position;
                    Camera.main.transform.eulerAngles = GameObject.Find("workBenchCameraPos").transform.eulerAngles;
                    cameraMovement.workbenchMode = true;
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = false;

                    shouldDisplayDayLabel = false;
                    Cursor.visible = true;
                    //  shouldDisplayDayLabel = false;
                    //shouldPlayAwakeAnim = true;

                }
            }
        }

        //day 3
        if (currentDay == 3)
        {
            if (shouldDisplayDayLabel == true)
            {
                if (SceneManager.GetActiveScene().name == "Day3")
                {
                    
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = true;
                    timeSinceStartDayLabel = timeSinceStartDayLabel + Time.deltaTime;
                }
                if (timeSinceStartDayLabel > 4)
                {
                   // Camera.main.transform.position = GameObject.Find("workBenchCameraPos").transform.position;
                //    Camera.main.transform.eulerAngles = GameObject.Find("workBenchCameraPos").transform.eulerAngles;
                 //   cameraMovement.workbenchMode = true;
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = false;

                    shouldDisplayDayLabel = false;
               //     Cursor.visible = true;
                    //  shouldDisplayDayLabel = false;
                    //shouldPlayAwakeAnim = true;

                }
            }

        }

        //day 17
        if (currentDay == 17)
        {
            if (shouldDisplayDayLabel == true)
            {
                if (SceneManager.GetActiveScene().name == "Day17")
                {
                    
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = true;
                    timeSinceStartDayLabel = timeSinceStartDayLabel + Time.deltaTime;
                }
                if (timeSinceStartDayLabel > 4)
                {
                    GameObject.Find("dayImage").GetComponent<Image>().enabled = false;

                    shouldDisplayDayLabel = false;

                }
            }

            if (day17Timer == 0)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    GameObject.Find("Camera").GetComponent<Animator>().SetTrigger("playEnd");
                }
            }
        }
    }

    public void switchToDay(int day)
    {
        if(day == 1)
        {

            mainMusic.Stop();
            GameObject.Find("planeTest").GetComponent<Animator>().SetTrigger("playCrash");
            menuAnim = true;

            GameObject.Find("startButton").GetComponent<Button>().enabled = false;
            GameObject.Find("startButton").GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);

            GameObject.Find("continue").GetComponent<Button>().enabled = false;
            GameObject.Find("continue").GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);


        }

        if(day == 2)
        {
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                mainMusic.Stop();
            }
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("Day2");
            timeSinceStartDayLabel = 0f;
            shouldDisplayDayLabel = true;
            transitionSound.PlayDelayed(0.2f);
            currentDay = 2;

        }

        if (day == 3)
        {
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                mainMusic.Stop();
            }
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("Day3");
            timeSinceStartDayLabel = 0f;
            shouldDisplayDayLabel = true;
            transitionSound.PlayDelayed(0.2f);
            currentDay = 3;

        }

        if (day == 17)
        {
            
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("Day17");
            timeSinceStartDayLabel = 0f;
            shouldDisplayDayLabel = true;
            transitionSound.PlayDelayed(0.2f);
            currentDay = 17;

        }
    }

    public void Continue()
    {
        GameObject.Find("startButton").SetActive(false);
        GameObject.Find("continue").SetActive(false);
        GameObject.Find("exit").SetActive(false);
        day1Button.SetActive(true);
        day2Button.SetActive(true);
        day3Button.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }


}
