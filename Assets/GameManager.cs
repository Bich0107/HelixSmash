using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] float gameOverDelay;
    [SerializeField] Canvas playCanvas;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] ScoreDisplayer displayer;
    Ball player;
    BrickSetter brickSetter;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        player = FindObjectOfType<Ball>();
        brickSetter = FindObjectOfType<BrickSetter>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ToNextStage()
    {
        player.gameObject.SetActive(true);
        player.Reset();

        brickSetter.ToNextStage();
    }

    public void Replay() {
        // replay
        player.Reset();
        playCanvas.enabled = true;
        gameOverCanvas.enabled = false;
        brickSetter.Reset();
        brickSetter.SpawnBricks();
    }

    public void GameOver()
    {
        StartCoroutine(CR_GameOver());
    }

    IEnumerator CR_GameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        playCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        displayer.Display();
    }
}
