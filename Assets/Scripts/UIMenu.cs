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
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, Vector3.zero, 0.2f);
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, Vector3.one, 0.2f);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, linkedButton.anchoredPosition, 0.2f);
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, Vector3.zero, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Hide();
        }
    }
}