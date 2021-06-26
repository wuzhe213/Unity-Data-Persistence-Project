using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Text bestScoreText;
    public InputField nameField;
    public Button startButton;
    public Button quitButton;
    public Text errorText;

    public BestScore bestScore;

    public static GameManager Instance;

    public string playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        nameField.onEndEdit.AddListener(ChangeName);
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeName(string name)
    {
        playerName = name;
    }

    private void StartGame()
    {
        if (nameField.text == "" || nameField.text == null)
        {
            errorText.gameObject.SetActive(true);
        }
        else
        {
            errorText.gameObject.SetActive(false);

            SceneManager.LoadScene(1);
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();    
#endif
    }

    public void SetBestScore(int score)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.bestScore.score = score;
        }
    }

    public void SetBestName(string name)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.bestScore.name = name;
        }
    }

    public bool HasBestScore()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.bestScore.name != null &&
            GameManager.Instance.bestScore.name != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
public class BestScore
{
    public string name;
    public int score;
}
