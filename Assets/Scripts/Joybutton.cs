using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        StopAllCoroutines();
        StartCoroutine(shrinkAndGrow());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    IEnumerator shrinkAndGrow() {
    	for (float i = 1f; i > 0.8f; i-=Time.deltaTime*2) {
    		GetComponent<RectTransform>().localScale = new Vector3(i,i,i);
    		yield return null;
    	}
    	for (float i = 0.8f; i < 1f; i+=Time.deltaTime*2) {
    		GetComponent<RectTransform>().localScale = new Vector3(i,i,i);
    		yield return null;
    	}
    	GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
    }
}
