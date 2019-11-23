using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionMapAlert : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(ShowOnMap());
    }

    IEnumerator ShowOnMap()
    {
        var timer = 0f;
        while (timer < 10f)
        {
            while (transform.localScale.x < 2.5f)
            {
                transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                timer += Time.deltaTime;
                yield return null;
            }
            while (transform.localScale.x > 1.5f)
            {
                transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        Destroy(gameObject);
    }

}
