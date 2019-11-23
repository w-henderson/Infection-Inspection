using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseCameraFollow : MonoBehaviour {

    void Update()
    {
        if (PreGame.gameRunning)
        {
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 5f, 0.1f);
            if (GameObject.Find("GameManager").GetComponent<Infection>().alive)
            {
                var newCamPos = GameObject.Find("Wallace").transform.position;
                newCamPos.z = -21f;
                transform.position = Vector3.Lerp(transform.position, newCamPos, 0.15f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(1.4f, -23.5f, -21f), 0.05f);
            }
        }
    }
}
