using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text pointsText;

    [SerializeField] private Text livesText;

    public GameObject winTextObject;
    public GameObject loseTextObject;

    void Start()
    {
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    public void UpdateScoreDisplay(int currentScore)
    {
        pointsText.text = "Score: " + currentScore.ToString();

        if (currentScore >= 2000)
        {
            Time.timeScale = 0;
            winTextObject.SetActive(true);            
        }
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        livesText.text = "Lives: " + currentLives.ToString();

        if (currentLives == 0)
        {            
            Time.timeScale = 0;
            loseTextObject.SetActive(true);            
        }
    }
}
