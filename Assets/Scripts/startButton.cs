using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void StartButton()
    {
        GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(1);
    }

    public static void Continue()
    {
        GameObject.Find("startButton").SetActive(false);
        GameObject.Find("continue").SetActive(false);
        GameObject.Find("day1").SetActive(true);
        GameObject.Find("day2").SetActive(true);
        GameObject.Find("day3").SetActive(true);
    }

    public static void StartDay2()
    {
        GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(2);
    }

    public static void StartDay3()
    {
        GameObject.Find("GameLevelController").GetComponent<levelManager>().switchToDay(3);
    }
}
