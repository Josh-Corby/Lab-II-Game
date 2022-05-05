using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : GameBehaviour<UIManager>
{
    [Header("Text")]
    public TMP_Text PlayerHP;
    public TMP_Text EnemyHP;
    public TMP_Text PlayerDeckCount;
    public TMP_Text EnemyDeckCount;

    public TMP_Text PlayerHealthChange;
    public TMP_Text EnemyHealthChange;

    [Header("Canvas Objects")]
    public Canvas GameCanvas;
    public Canvas TitleScreen;
    public GameObject GameOverPanel;
    public TMP_Text victoryText;

    [Header("Audio")]
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

    public void UpdateDeckCount(int count)
    {
        PlayerDeckCount.text = "Cards left: " + count.ToString();
        EnemyDeckCount.text = "Cards left: " + count.ToString();
    }

    public void UpdatePlayerHP(int targetHP)
    {
        PlayerHP.text =targetHP.ToString();
    }

    public void UpdateEnemyHP(int targetHP)
    {
        EnemyHP.text = targetHP.ToString();
    }

    //public void HealthChangeText(string target, string type, int value)
    //{
    //    if(type == "Attack")
    //    {
    //        if(target == "Player")
    //        {
    //            PlayerHealthChange.color = Color.red;
    //            PlayerHealthChange.text = value.ToString();
    //        }

    //        if(target == "Enemy")
    //        {
    //            EnemyHealthChange.color = Color.red;
    //            EnemyHealthChange.text = value.ToString();
    //        }
    //    }
        
    //    if(type == "Heal")
    //    {

    //    }

    //}

    //public void GameOver(string winner)
    //{
    //    Time.timeScale = 0f;
    //    GameOverPanel.SetActive(true);
    //    victoryText.text = (winner + " wins!");
    //    if(winner == "Player")
    //    {
    //        SFX.clip = victorySound;
    //        SFX.Play();
    //    }
    //    if(winner == "Enemy" || winner == "Draw")
    //    {
    //        SFX.clip = gameOverSound;
    //        SFX.Play();
    //    }
    //}

    public IEnumerator GameOver(string winner)
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
        victoryText.text = (winner + " wins!");
        if (winner == "Player")
        {
            SFX.clip = victorySound;
            SFX.Play();
        }
        if (winner == "Enemy" || winner == "Draw")
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
