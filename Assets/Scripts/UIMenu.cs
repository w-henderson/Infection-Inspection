using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public bool active = false;
    public RectTransform linkedButton;

    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = linkedButton.anchoredPosition;
        GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void Show()
    {
        active = true;
    }

    public void Hide()
    {
        active = false;
    }

    void Update()
    {
        if (active)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, Vector3.zero, 12f * Time.deltaTime);
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, Vector3.one, 12f * Time.deltaTime);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, linkedButton.anchoredPosition, 12f * Time.deltaTime);
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, Vector3.zero, 12f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Hide();
        }
    }
}