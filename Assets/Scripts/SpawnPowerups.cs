using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerups : MonoBehaviour {

    public List<GameObject> powerups;

    private void Awake()
    {
        //StartCoroutine(randomlySpawnPowerup());
    }

    Vector3 powerupLocation()
    {
        Vector3 chosenLocation = new Vector3(-40f, -40f, 0f);
        while (Physics2D.Raycast(chosenLocation,Vector2.zero).collider.name != "Background")
        {
            chosenLocation = new Vector3(Random.Range(-41f, 41f), Random.Range(-41f, 41f), 0f);
        }
        chosenLocation.z = -0.5f;
        return chosenLocation;
    }

    IEnumerator randomlySpawnPowerup()
    {
        while (GetComponent<Infection>().alive)
        {
            yield return new WaitForSeconds(Random.Range(25f, 45f));
            GetComponent<AudioSource>().Play();
            Instantiate(powerups[Random.Range(0, powerups.Count)], powerupLocation(), Quaternion.Euler(Vector3.zero));
        }
    }

}
