using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : GameBehaviour<UIManager>
{
    public TMP_Text PlayerHP;
    public TMP_Text EnemyHP;

    public Canvas GameCanvas;
    public Canvas TitleScreen;
    public GameObject GameOverPanel;
    public TMP_Text victoryText;

    public AudioSource SFX;
    public AudioClip gameOverSound;
    public AudioClip victorySound;
    private void Start()
    {
        Time.timeScale = 0f;
        TitleScreen.enabled = true;
        GameCanvas.enabled = false;
        
        GameOverPanel.SetActive(false);

    }

    public void StartGame()
    {
        TitleScreen.enabled = false;
        GameCanvas.enabled = true;
        Time.timeScale = 1f;
        _GM.StartGame();
        UpdatePlayerHP(_GM.playerHealth);
        UpdateEnemyHP(_GM.enemyHealth);
    }

    public void UpdateHP(string target, int targetHP)
    {
        if (target == "Player")
        {
            UpdatePlayerHP(targetHP);
        }
        else if (target == "Enemy")
        {
            UpdateEnemyHP(targetHP);
        }
    }

    public void UpdatePlayerHP(int targetHP)
    {
        PlayerHP.text =targetHP.ToString();
    }

    public void UpdateEnemyHP(int targetHP)
    {
        EnemyHP.text = targetHP.ToString();
    }

    public void GameOver(string winner)
    {
        GameOverPanel.SetActive(true);
        victoryText.text = (winner + " wins!");
        if(winner == "Player")
        {
            SFX.clip = victorySound;
            SFX.Play();
        }
        if(winner == "Enemy")
        {
            SFX.clip = gameOverSound;
            SFX.Play();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
