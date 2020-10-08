using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacterium : MonoBehaviour
{

    private List<GameObject> paths = new List<GameObject>();

    private Transform path;
    private Transform option;

    public bool bossMode = false;

    public Sprite attacking;

    public bool inHeart = false;

    public int pathChoice;
    public float bacteriumSpeed = 0.1f;
    private float bacteriumDamage = 2f;

    private Vector2 defendLocation = new Vector2(1.5f, -25.5f);
    private float defendLocationRadius = 4f;

    private bool paused = false;
    private List<Vector3> lastThirtyPositions = new List<Vector3>();

    private void Awake()
    {
        if (!bossMode)
        {
            bacteriumSpeed = Random.Range(0.08f, 0.14f);
        }
        else
        {
            bacteriumSpeed = 0.06f;
            bacteriumDamage = 4f;
        }
        paths.Add(GameObject.Find("Paths/Path1"));
        paths.Add(GameObject.Find("Paths/Path2"));
        paths.Add(GameObject.Find("Paths/Path3"));
        paths.Add(GameObject.Find("Paths/Path4"));
        paths.Add(GameObject.Find("Paths/Path5"));
        StartCoroutine(approachHeart());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Wallace")
        {
            if (!bossMode)
            {
                StopAllCoroutines();
                StartCoroutine(eaten());
            }
            else if (GameObject.FindObjectOfType<Wallace>().boosting)
            {
                StopAllCoroutines();
                StartCoroutine(eaten());
            }
        }
        else if (collision.gameObject.tag == "BacterialObstacle")
        {
            paused = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BacterialObstacle")
        {
            paused = false;
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        StartCoroutine(eaten());
    }

    IEnumerator eaten()
    {
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        Destroy(GetComponent<Collider>());
        if (!bossMode)
        {
            GameObject.Find("GameManager").GetComponent<Infection>().energy = Mathf.Clamp(GameObject.Find("GameManager").GetComponent<Infection>().energy + 2, 0f, 100f);
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<Infection>().energy = Mathf.Clamp(GameObject.Find("GameManager").GetComponent<Infection>().energy + 10, 0f, 100f);
        }
        GetComponent<AudioSource>().Play();
        while (GetComponent<AudioSource>().isPlaying)
        {
            if (transform.localScale.x > 0f)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            }
            yield return null;
        }
        while (transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator approachHeart()
    {
        yield return new WaitForFixedUpdate();
        path = paths[pathChoice].transform;
        if (Random.value > 0.5f)
        {
            option = path.transform.Find("Option1");
        }
        else
        {
            option = path.transform.Find("Option2");
        }
        var nodes = new List<Vector3>();
        for (int i = 0; i < option.childCount; i++)
        {
            nodes.Add(option.GetChild(i).position);
        }
        Vector3 chosenPointForAttack = defendLocation + (Random.insideUnitCircle * defendLocationRadius);
        chosenPointForAttack.z = -1f;
        nodes.Add(chosenPointForAttack);
        foreach (Vector3 node in nodes)
        {
            while (transform.position != node)
            {
                transform.right = Vector3.Lerp(transform.right, transform.position - node, 0.05f);
                if (!paused)
                {
                    transform.position = Vector3.MoveTowards(transform.position, node, bacteriumSpeed);
                    lastThirtyPositions.Add(transform.position);
                    if (lastThirtyPositions.Count > 30)
                    {
                        lastThirtyPositions.RemoveAt(0);
                    }
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    var bounce = lastThirtyPositions;
                    bounce.Reverse();
                    foreach (Vector3 bounceLocation in bounce)
                    {
                        transform.position = bounceLocation;
                        yield return new WaitForFixedUpdate();
                    }
                    paused = false;
                }
            }
        }
        foreach (SpriteRenderer sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sprite = attacking;
        }
        inHeart = true;
        while (enabled)
        {
            transform.Rotate(0f, 0f, 5f);
            if (GameObject.Find("GameManager").GetComponent<Infection>().health - (bacteriumDamage * Time.deltaTime) <= 0f)
            {
                GameObject.Find("GameManager").GetComponent<Infection>().health = 0f;
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<Infection>().health -= bacteriumDamage * Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
