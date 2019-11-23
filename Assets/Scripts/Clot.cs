using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clot : MonoBehaviour {

    private bool triggered = false;
    private float timeLeft = 15f;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timeLeft > 0f)
        {
            if (collision.name == "Wallace")
            {
                triggered = true;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 0.5f, 0.1f));
            } else if (timeLeft == 15f)
            {
                collision.gameObject.GetComponent<Bacterium>().Die();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (timeLeft > 0f)
        {
            if (collision.name == "Wallace")
            {
                triggered = false;
            }
        }
    }

    private void Update()
    {
        timeLeft = Mathf.Clamp(timeLeft-Time.deltaTime,0f,Mathf.Infinity);
        if (timeLeft > 0f)
        {
            transform.Find("Countdown").GetComponent<TextMesh>().text = timeLeft.ToString("F0");
            if (!triggered)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 1f, 0.1f));
            }
        }
        if (timeLeft == 0f)
        {
            StartCoroutine(disappear());
        }
    }

    IEnumerator disappear()
    {
        Destroy(GetComponent<Collider>());
        for (float f = 0f; f < 1f; f += Time.deltaTime)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1-f);
            yield return null;
        }
        Destroy(gameObject);
    }

}
