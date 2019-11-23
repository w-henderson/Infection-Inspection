using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public string type;
    private float speedMultiplier = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Wallace")
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(remove());
            if (type == "life")
            {
                StartCoroutine(heal());
            }
            if (type == "speed")
            {
                StartCoroutine(speed());
            }
            if (type == "pill")
            {
                StartCoroutine(pill());
            }
        }
    }

    IEnumerator pill()
    {
        foreach (Bacterium bacterium in GameObject.FindObjectsOfType<Bacterium>())
        {
            bacterium.Die();
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

    IEnumerator speed()
    {
        GameObject.Find("Wallace").GetComponent<Wallace>().maxSpeed *= speedMultiplier;
        GameObject.Find("Wallace").GetComponent<Wallace>().speed *= speedMultiplier;
        yield return new WaitForSeconds(10f);
        GameObject.Find("Wallace").GetComponent<Wallace>().maxSpeed /= speedMultiplier;
        GameObject.Find("Wallace").GetComponent<Wallace>().speed /= speedMultiplier;
        Destroy(gameObject);
    }

    IEnumerator heal()
    {
        for (float i = 0f; i < 2f; i += Time.deltaTime)
        {
            if (GameObject.Find("GameManager").GetComponent<Infection>().health + (20f * Time.deltaTime) <= 100f)
            {
                GameObject.Find("GameManager").GetComponent<Infection>().health += 20f * Time.deltaTime;
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator remove()
    {
        while (transform.localScale.x > 0f)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return null;
        }
    }
}
