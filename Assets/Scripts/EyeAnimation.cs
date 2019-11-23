using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimation : MonoBehaviour {

    public Vector3 LEyePivot;
    public Vector3 REyePivot;
    public float power;

    private bool menuScreen = true;

    private void Awake()
    {
        StartCoroutine(animateWallace());
    }

    void Update () {
        if (PreGame.gameRunning)
        {
            if (menuScreen)
            {
                menuScreen = false;
                StopAllCoroutines();
            }
            var movement = transform.root.GetComponent<Wallace>().movementThisFrame;
            transform.Find("LEye").localPosition = LEyePivot + (movement * power);
            transform.Find("REye").localPosition = REyePivot + (movement * power);
        }
	}

    IEnumerator animateWallace()
    {
        StartCoroutine(animateWallacesEyes());
        var upPosition = transform.root.position + new Vector3(0f, 0.05f, 0f);
        var downPosition = transform.root.position + new Vector3(0f, -0.05f, 0f);
        while (true)
        {
            while (transform.root.position != upPosition)
            {
                transform.root.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.root.position, upPosition, 0.002f));
                yield return null;
            }
            while (transform.root.position != downPosition)
            {
                transform.root.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.root.position, downPosition, 0.002f));
                yield return null;
            }
        }
    }

    IEnumerator animateWallacesEyes()
    {
        while (true)
        {
            var location = Random.insideUnitCircle.normalized * 0.3f;
            var location3 = new Vector3(location.x, location.y, 0f);
            while (transform.Find("LEye").localPosition != LEyePivot + location3)
            {
                transform.Find("LEye").localPosition = Vector3.Lerp(transform.Find("LEye").localPosition, LEyePivot + location3, 0.1f);
                transform.Find("REye").localPosition = Vector3.Lerp(transform.Find("REye").localPosition, REyePivot + location3, 0.1f);
                yield return null;
            }
        }
    }
}
