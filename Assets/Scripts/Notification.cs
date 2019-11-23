using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{

    public List<string> messages = new List<string>();
    public List<float> messageDurations = new List<float>();
    
    public static void Send(string message)
    {
        GameObject.FindObjectOfType<Notification>().messages.Add(message);
        GameObject.FindObjectOfType<Notification>().messageDurations.Add(3f);
    }

    void Update()
    {
        if (messages.Count > 0)
        {
            GetComponent<Text>().text = messages[0];
            messageDurations[0] -= Time.deltaTime;
            if (messageDurations[0] < 0f)
            {
                messages.RemoveAt(0);
                messageDurations.RemoveAt(0);
            }
        } else
        {
            GetComponent<Text>().text = "";
        }
    }

}
