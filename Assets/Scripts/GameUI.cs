using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;
    bool win;

    // Start is called before the first frame update
    void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        PlayerController.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedFinishPoint += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (win == true)
                {
                    SceneManager.LoadScene(3);
                }
                else
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
        win = true;
    }
    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
        win = false;
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        PlayerController.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedFinishPoint -= ShowGameWinUI;
    }
}
