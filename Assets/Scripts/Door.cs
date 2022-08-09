using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string lvlName;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (GameController.instance.doorUnlocked)
            {
                SceneManager.LoadScene(lvlName);
            }
        }
    }
}
