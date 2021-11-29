using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public static VictoryScreen instance;
    public float waitForAnyKey = 2f;
    public GameObject anyKeyText;
    public string mainMenuScene;

    public int highscore;
    public int score;
    public Text scoreText;
    public Text highscoreText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
        score = CharacterTracker.instance.currentScore;
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " Points";
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if(waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
