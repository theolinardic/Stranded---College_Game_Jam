using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateHud : MonoBehaviour
{
    public GameObject hudImage;
    public Text gameText;
    public static bool shouldDisplayHud;
    float lerpPosShow = 1f;
    float lerpPosHide = 1f;
    Color textFull, textHidden, imageFull, imageHidden;
    public float changeSpeed = 0.5f;
    public float displayTime = 6f;
    float timeSinceVisible;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceVisible = 0;
        shouldDisplayHud = false;

        imageFull = hudImage.GetComponent<Image>().color;
        imageFull.a = 1;

        imageHidden = imageFull;
        imageHidden.a = 0;

        textFull = gameText.GetComponent<Text>().color;
        textFull.a = 1;

        textHidden = textFull;
        textHidden.a = 0;
        //lerpPosHide = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpPosShow < 1)
        {
            hudImage.GetComponent<Image>().color = Color.Lerp(imageHidden, imageFull, lerpPosShow);
            gameText.GetComponent<Text>().color = Color.Lerp(textHidden, textFull, lerpPosShow);
            lerpPosShow = lerpPosShow + (changeSpeed * Time.deltaTime);
        }

        if (lerpPosHide < 1)
        {
            hudImage.GetComponent<Image>().color = Color.Lerp(imageFull, imageHidden, lerpPosHide);
            gameText.GetComponent<Text>().color = Color.Lerp(textFull, textHidden, lerpPosHide);
            lerpPosHide = lerpPosHide + (changeSpeed * Time.deltaTime);
        }

        if(hudImage.GetComponent<Image>().color.a > 0.9f)
        {
            
            timeSinceVisible += Time.deltaTime;
        }

        if (timeSinceVisible > displayTime && hudImage.GetComponent<Image>().color.a > 0.9f)
        {
            timeSinceVisible = 0f;
            lerpPosHide = 0f;
        }
        if(lerpPosHide > 1)
        {
            hudImage.SetActive(false);
        }


    }

    public void changeHud(string newText)
    {
        gameText.text = newText;
        lerpPosShow = 0f;
        timeSinceVisible = 0f;
        hudImage.SetActive(true);
    }

   public void displayHud()
    {
        lerpPosShow = 0f;
        timeSinceVisible = 0f;
        hudImage.SetActive(true);
    }
}
