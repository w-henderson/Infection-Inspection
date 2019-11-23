using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infection : MonoBehaviour {

    public GameObject mapInfectionAlert;
    public List<Vector3> possibleSpawnLocations;
    public GameObject bacterium;
    public GameObject bossBacterium;
    public GameObject clot;

    public AudioClip introMusic;
    public AudioClip mainMusic;

    private string previousSpawnSide;
    public int wave = 1;
    private bool deployed = true;
    public float health = 100f;
    public float energy = 0f;

    public float gameTime = 0f;
    public bool alive = true;

    public void StartGame()
    {
        StartCoroutine(manageSpawning());
        StartCoroutine(heal());
    }

    public void SpawnClot(Vector3 position)
    {
        if (energy >= 100f)
        {
            Instantiate(clot, position, Quaternion.Euler(Vector3.zero));
            energy -= 100f;
        } else
        {
            Notification.Send("Not enough energy!");
        }
    }

    void Update () {
        if (PreGame.gameRunning)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().clip = mainMusic;
                GetComponent<AudioSource>().Play();
            }
            gameTime += Time.deltaTime;
            GameObject.Find("Slider").GetComponent<Slider>().value = health / 100f;
            GameObject.Find("Health").GetComponent<Text>().text = "Health: " + health.ToString("F0") + "%";
            GameObject.Find("Energy").GetComponent<Slider>().value = energy / 100f;
            GameObject.Find("EnergyCount").GetComponent<Text>().text = "Energy: " + energy.ToString("F0") + "%";
            if (alive)
            {
                GameObject.Find("Sidebar").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Sidebar").GetComponent<RectTransform>().anchoredPosition, new Vector3(-950f, 0f, 0f), 0.05f);
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Infect();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    health = 0f;
                }
                if (Input.GetKeyDown(KeyCode.H) && energy >= 100f && health != 100f)
                {
                    energy -= 100f;
                    health = Mathf.Clamp(health + 25f, 0f, 100f);
                }
                if (GameObject.Find("Slider").GetComponent<Slider>().value <= 0f)
                {
                    GameObject.Find("Wallace").GetComponent<Wallace>().Die();
                    alive = false;
                }
            }
        }
	}

    public void Infect()
    {
        var randomChoice = Random.Range(0, possibleSpawnLocations.Count);
        var spawnLocation = possibleSpawnLocations[randomChoice];
        var spawnSide = "left";
        if (randomChoice < 2)
        {
            spawnSide = "right";
        } else if (randomChoice >= 4)
        {
            spawnSide = "middle";
        }
        while (spawnSide == previousSpawnSide)
        {
            randomChoice = Random.Range(0, possibleSpawnLocations.Count);
            spawnLocation = possibleSpawnLocations[randomChoice];
            spawnSide = "left";
            if (randomChoice < 2)
            {
                spawnSide = "right";
            }
            else if (randomChoice >= 4)
            {
                spawnSide = "middle";
            }
        }
        Instantiate(mapInfectionAlert, spawnLocation, Quaternion.Euler(Vector3.zero));
        StartCoroutine(slowSpawn(spawnLocation, wave, randomChoice));
        previousSpawnSide = spawnSide;
    }

    IEnumerator slowSpawn(Vector3 spawnLocation, int count, int choice)
    {
        deployed = false;
        for (int i = 0; i < count; i++)
        {
            if (i == count-1 && count % 5 == 0)
            {
                Notification.Send("A boss has spawned! You can only kill it by boosting at it.");
                var bacteria = Instantiate(bossBacterium, spawnLocation+new Vector3(0f,0f,-1f), Quaternion.Euler(Vector3.zero));
                bacteria.GetComponent<Bacterium>().pathChoice = choice;
                yield return new WaitForSeconds(0.4f);
            }
            else
            {
                var bacteria = Instantiate(bacterium, spawnLocation, Quaternion.Euler(Vector3.zero));
                bacteria.GetComponent<Bacterium>().pathChoice = choice;
                yield return new WaitForSeconds(0.4f);
            }
        }
        deployed = true;
    }

    IEnumerator manageSpawning()
    {
        while (alive)
        {
            var timeUntilNextWave = 15f;
            GameObject.Find("WaveName").GetComponent<Text>().text = "Wave " + wave.ToString() + " begins in";
            while (timeUntilNextWave > 0)
            {
                if (!alive)
                {
                    break;
                }
                GameObject.Find("WaveTimer").GetComponent<Text>().text = ((int)timeUntilNextWave).ToString() + " seconds";
                timeUntilNextWave -= Time.deltaTime;
                yield return null;
            }
            if (alive)
            {
                Infect();
                GameObject.Find("WaveName").GetComponent<Text>().text = "Currently deploying";
                GameObject.Find("WaveTimer").GetComponent<Text>().text = "Wave " + wave.ToString();
                yield return new WaitForSeconds(1f);
                wave += 1;
            }
        }
        var prevHighScore = PlayerPrefs.GetInt("iiHigh");
        StartCoroutine(waitThenLoad());
        while (!alive)
        {
            if (wave > prevHighScore)
            {
                GameObject.Find("RoundNumber").GetComponent<Text>().text = "You survived until Wave " + wave.ToString() + ", beating your previous high of Wave "+prevHighScore.ToString()+"!";
            } else if (wave == prevHighScore)
            {
                GameObject.Find("RoundNumber").GetComponent<Text>().text = "You survived until Wave " + wave.ToString() + ", the same as your current high score!";
            } else
            {
                var distanceAway = prevHighScore - wave;
                GameObject.Find("RoundNumber").GetComponent<Text>().text = "You survived until Wave " + wave.ToString() + ", just " + distanceAway.ToString() + " away from your high score of Wave "+prevHighScore.ToString() + "!";
            }
            GameObject.Find("Sidebar").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Sidebar").GetComponent<RectTransform>().anchoredPosition, new Vector3(-450f,0f,0f),0.05f);
            GameObject.Find("Death").GetComponent<RectTransform>().localScale = Vector3.Lerp(GameObject.Find("Death").GetComponent<RectTransform>().localScale, new Vector3(1f, 1f, 1f), 0.025f);
            GameObject.Find("Death").GetComponent<RectTransform>().rotation = Quaternion.Lerp(GameObject.Find("Death").GetComponent<RectTransform>().rotation, Quaternion.Euler(0f, 0f, 0f), 0.025f);
            GameObject.Find("ClotButton").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("ClotButton").GetComponent<RectTransform>().anchoredPosition, new Vector3(-400f, -500f, 0f), 0.05f);
            GameObject.Find("Boost").GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GameObject.Find("Boost").GetComponent<RectTransform>().anchoredPosition, new Vector3(-150f, -500f, 0f), 0.05f);
            yield return null;
        }
    }

    IEnumerator waitThenLoad()
    {
        if (wave > PlayerPrefs.GetInt("iiHigh"))
        {
            PlayerPrefs.SetInt("iiHigh", wave);
            PlayerPrefs.Save();
        }
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    IEnumerator heal()
    {
        while (alive)
        {
            if (health < 100f)
            {
                var noneAttacking = true;
                foreach (Bacterium bacterium in GameObject.FindObjectsOfType<Bacterium>())
                {
                    if (bacterium.inHeart)
                    {
                        noneAttacking = false;
                    }
                }
                if (noneAttacking)
                {
                    health = Mathf.Clamp(health + 1f, 0f, 100f);
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }
}
