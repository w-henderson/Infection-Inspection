using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour
{

    public static bool gameRunning;
    public Color wallaceColor;
    public GameObject joystick;

    public Transform cam;

    private float hue;
    private float saturation;
    private float value;

    public bool instructionsShown = false;
    public bool moreInfoShown = false;

    private void Awake()
    {
        cam = Camera.main.transform;
        joystick.SetActive(false);
        if (!PlayerPrefs.HasKey("iiHigh") || !PlayerPrefs.HasKey("iiLastVersion"))
        {
            PlayerPrefs.SetString("iiLastVersion", Application.version);
            PlayerPrefs.SetInt("iiHigh", 0);
            PlayerPrefs.Save();
            GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: Wave 0";
        }
        else
        {
            GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: Wave " + PlayerPrefs.GetInt("iiHigh").ToString();
        }
        gameRunning = false;
        wallaceColor = GameObject.Find("Wallace").GetComponent<SpriteRenderer>().color;
        Color.RGBToHSV(wallaceColor, out hue, out saturation, out value);
    }

    public void StartGame()
    {
        gameRunning = true;
        joystick.SetActive(true);
        GetComponent<Infection>().StartGame();
    }

    public void ShowInstructions()
    {
        instructionsShown = !instructionsShown;
    }

    public void MoreInfo()
    {
        moreInfoShown = !moreInfoShown;
    }

    void Update()
    {
        if (gameRunning)
        {
            GetComponent<AudioSource>().loop = false;
            GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition, new Vector3(-2500f, 0f, 0f), 0.05f);
            GameObject.Find("ClotButton").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("ClotButton").GetComponent<RectTransform>().anchoredPosition, new Vector3(-400f, 150f, 0f), 0.05f);
            GameObject.Find("Boost").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Boost").GetComponent<RectTransform>().anchoredPosition, new Vector3(-150f, 150f, 0f), 0.05f);
        }
        else
        {
            if (instructionsShown)
            {
                GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition, new Vector3(1500f, 0f, 0f), 0.05f);
                GameObject.Find("Tutorial").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Tutorial").GetComponent<RectTransform>().anchoredPosition, new Vector3(950f, 0f, 0f), 0.05f);
                cam.position = Vector3.Lerp(cam.position, new Vector3(0.8f, -23.5f, -21f), 0.05f);
            }
            else
            {
                GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("StartScreen").GetComponent<RectTransform>().anchoredPosition, Vector3.zero, 0.05f);
                GameObject.Find("Tutorial").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Tutorial").GetComponent<RectTransform>().anchoredPosition, new Vector3(-950f, 0f, 0f), 0.05f);
                cam.position = Vector3.Lerp(cam.position, new Vector3(2f, -23.5f, -21f), 0.05f);
            }
            if (moreInfoShown)
            {
                GameObject.Find("TutorialPage1").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("TutorialPage1").GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 1080f, 0f), 0.05f);
                GameObject.Find("TutorialPage2").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("TutorialPage2").GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 0f, 0f), 0.05f);
            }
            else
            {
                GameObject.Find("TutorialPage1").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("TutorialPage1").GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 0f, 0f), 0.05f);
                GameObject.Find("TutorialPage2").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("TutorialPage2").GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, -1080f, 0f), 0.05f);
            }
        }
    }
}
