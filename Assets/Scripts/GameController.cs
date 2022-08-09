using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public int totalScore;
    public Text scoreText;
    public GameObject gameOver;
    public GameObject gameCamera;
    public GameObject player;
    public GameObject door;

    public Text timeText;
    public Text rotationsCounterText;

    private int rotationsDone;
    private bool stopwatchActive = false;
    private float currentTime;

    public bool doorUnlocked { get; private set; } = false;

    public bool rotating { get; private set; }

    public float currentRotation { get; private set; } = 0;

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        rotationsDone = 0;
        currentTime = 0;
        stopwatchActive = true;
        SceneManager.sceneLoaded += delegate
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
        };
    }

    private void Update()
    {
        UpdateTime();
        HandleRoation();
    }

    private void UpdateTime()
    {
        if (stopwatchActive)
        {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString(@"mm\:ss");
    }

    private void HandleRoation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            if (!rotating)
            {
                BeforeRotation();
                StartCoroutine(RotationUtils.RotateCamera(gameCamera, new Vector3(0, 0, -90), 1, AfterRotation));
                StartCoroutine(RotationUtils.RotateCamera(player, new Vector3(0, 0, -90), 1, delegate { }));
            }
        if (Input.GetKeyDown(KeyCode.E))
            if (!rotating)
            {
                BeforeRotation();
                StartCoroutine(RotationUtils.RotateCamera(gameCamera, new Vector3(0, 0, 90), 1, AfterRotation));
                StartCoroutine(RotationUtils.RotateCamera(player, new Vector3(0, 0, 90), 1, delegate { } ));
            }
    }

    public void BeforeRotation()
    {
        rotating = true;
        rotationsDone++;
        rotationsCounterText.text = rotationsDone.ToString() + " ROTATIONS";
        if (currentRotation == RotationUtils.ROTATED_BOTTOM || currentRotation == RotationUtils.ROTATED_TOP)
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }

    }

    public void AfterRotation()
    {
        rotating = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        currentRotation = gameCamera.transform.localRotation.eulerAngles.z;
        Physics2D.gravity = gameCamera.transform.localRotation.eulerAngles.z switch
        {
            RotationUtils.ROTATED_BOTTOM => new Vector2(0, -9.81f),
            RotationUtils.ROTATED_RIGHT => new Vector2(9.81f, 0),
            RotationUtils.ROTATED_TOP => new Vector2(0, 9.81f),
            RotationUtils.ROTATED_LEFT => new Vector2(-9.81f, 0),
            _ => new Vector2(0, -9.81f)
        };
    }

    public void UnlockDoor()
    {
        doorUnlocked = true;
        door.GetComponent<Animator>().SetTrigger("open");
    }

    public void UpdateScoreText() {
        scoreText.text = totalScore.ToString();
    }

    public void ShowGameOver () {
        stopwatchActive = false;
        gameOver.SetActive(true);
    }

    public void RestartGame (string lvlName) {
        SceneManager.LoadScene(lvlName);
    }
}
