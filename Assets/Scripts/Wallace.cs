using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallace : MonoBehaviour
{

    public Vector3 movementThisFrame = Vector3.zero;
    public float speed = 0.01f;
    public float maxSpeed = 0.15f;
    private float slowingSpeed = 0.15f;

    public float boostCost = 25f;
    public bool boosting = false;
    public float boostMultiplier = 1f;

    public Joystick joystick;
    public Joybutton boost;
    public Joybutton clotButton;

    public bool[,] AIPoints = new bool[162, 162];

    void FixedUpdate()
    {
        if (GameObject.Find("GameManager").GetComponent<Infection>().alive && PreGame.gameRunning)
        {
            if (joystick.Vertical != 0 || joystick.Horizontal != 0)
            {
                if (boost.pressed || Input.GetKeyDown(KeyCode.B))
                {
                    if (GameObject.Find("GameManager").GetComponent<Infection>().energy >= boostCost)
                    {
                        StartCoroutine(swoosh());
                    }
                    else
                    {
                        Notification.Send("Not enough energy!");
                    }
                    boost.pressed = false;
                }
                //movementThisFrame += new Vector3(joystick.Horizontal * speed, joystick.Vertical * speed);
                //movementThisFrame = Vector3.ClampMagnitude(movementThisFrame, maxSpeed);
                movementThisFrame = new Vector3(joystick.Horizontal, joystick.Vertical) * speed * 15f;
                slowingSpeed = movementThisFrame.magnitude;
            }
            else
            {
                slowingSpeed -= 0.005f;
                slowingSpeed = Mathf.Clamp(slowingSpeed, 0f, 0.15f);
                movementThisFrame = Vector3.ClampMagnitude(movementThisFrame, slowingSpeed);
            }
            var unadjustedMovementThisFrame = movementThisFrame;
            if (clotButton.pressed)
            {
                GameObject.Find("GameManager").GetComponent<Infection>().SpawnClot(transform.position);
                clotButton.pressed = false;
            }
            transform.Find("MovementDirection").GetComponent<LineRenderer>().SetPosition(0, transform.position);
            transform.Find("MovementDirection").GetComponent<LineRenderer>().SetPosition(1, transform.position + (unadjustedMovementThisFrame * 15));
            var arrowHeadNewPos = transform.position + (unadjustedMovementThisFrame * 15);
            arrowHeadNewPos.z = -0.5f;
            GameObject.Find("ArrowHead").transform.position = arrowHeadNewPos;
            GameObject.Find("ArrowHead").transform.right = unadjustedMovementThisFrame;
            GameObject.Find("ArrowHead").transform.localScale = new Vector3(Mathf.Clamp(unadjustedMovementThisFrame.magnitude / 0.375f, 0.125f, 0.4f), Mathf.Clamp(unadjustedMovementThisFrame.magnitude / 0.375f, 0.125f, 0.4f), 1f);

            GetComponent<Rigidbody2D>().velocity = (movementThisFrame * boostMultiplier) * 3600f * Time.fixedDeltaTime;
        }
    }

    public void Die()
    {
        StartCoroutine(eaten());
    }

    IEnumerator swoosh()
    {
        boosting = true;
        GameObject.Find("GameManager").GetComponent<Infection>().energy -= boostCost;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
        for (float i = 0f; i < 0.5f; i += Time.deltaTime)
        {
            boostMultiplier = 3;
            yield return null;
        }
        for (float i = 0f; i < 1f; i += Time.deltaTime)
        {
            boostMultiplier = 3 - (i * 2); // who knows what this math does
            yield return null;
        }
        boosting = false;
    }

    IEnumerator eaten()
    {
        Destroy(GameObject.Find("ArrowHead"));
        Destroy(GameObject.Find("MovementDirection"));
        while (transform.localScale.x > 0f)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
