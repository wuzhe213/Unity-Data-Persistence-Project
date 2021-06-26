using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;

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

        LoadBestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        nameField.onEndEdit.AddListener(ChangeName);
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);

        Debug.Log("Start, bestscore, playerName: " + bestScore.playerName);
        if (bestScore != null && bestScore.playerName != null && bestScore.playerName != "")
        {
            nameField.text = bestScore.playerName;
        }

        if (GameManager.Instance.HasBestScore())
        {
            bestScoreText.text = "Best Score: " +
                             GameManager.Instance.bestScore.name + ": " +
                             GameManager.Instance.bestScore.score;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeName(string name)
    {
        Debug.Log("ChangeName, name: " + name);
        if (name == null || name == "")
        {
            return;
        }
        playerName = name;
        bestScore.playerName = playerName;
        WriteBestScore();
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

    private void SetBestScore(int score)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.bestScore.score = score;
        }
    }

    private void SetBestName(string name)
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

    private void WriteBestScore()
    {
        string json = JsonUtility.ToJson(Instance.bestScore);
        Debug.Log("json: " + json);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void UpdateBestScore(int score)
    {
        if (Instance.HasBestScore())
        {
            if (Instance.bestScore.score < score)
            {
                Instance.SetBestName(playerName);
                Instance.SetBestScore(score);
                WriteBestScore();
            }
        }
        else
        {
            Instance.SetBestName(playerName);
            Instance.SetBestScore(score);
            WriteBestScore();
        }
    }

    private void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("load, json: " + json);
            BestScore bestScore = JsonUtility.FromJson<BestScore>(json);
            Instance.SetBestName(bestScore.name);
            Instance.SetBestScore(bestScore.score);
            Instance.bestScore = bestScore;
            Instance.playerName = bestScore.playerName;
        }

        Debug.Log("Best score, name: " + bestScore.name);
        Debug.Log("Best score, score: " + bestScore.score);
        Debug.Log("Best score, plaplayerNameyName: " + bestScore.playerName);
    }

    [Serializable]
    public class BestScore
    {
        public string name;
        public int score;
        public string playerName;
    }

}
