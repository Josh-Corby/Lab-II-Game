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
    public Canvas TitleScreen;
    public Canvas GameCanvas;
    public GameObject MenuScreen;
    public GameObject GameOverPanel;
    public TMP_Text victoryText;
    public GameObject HelpPanel1;
    public GameObject HelpPanel2;

    [Header("Audio")]
    public AudioSource SFX;
    public AudioClip gameOverSound;
    public AudioClip victorySound;

    //Enabling and disabling relevant canvases and panels
    private void Start()
    {
        TitleScreen.enabled = true;
        Time.timeScale = 0f;
        MenuScreen.SetActive(true);
        GameCanvas.enabled = false;
        
        GameOverPanel.SetActive(false);
        HelpPanel1.SetActive(false);
        HelpPanel2.SetActive(false);
    }

    //Enabling and disabling relevant canvases and panels
    public void StartGame()
    {
        TitleScreen.enabled = false;
        MenuScreen.SetActive(false);
        GameCanvas.enabled = true;
        //Set timescale to 1;
        Time.timeScale = 1f;
        //Tell the game manager to start the game
        _GM.StartGame();

        //Update health UI for player and enemy
        UpdatePlayerHP(_GM.playerHealth);
        UpdateEnemyHP(_GM.enemyHealth);
        
    }

    //Send values recieved from game manager to functions that update health UI
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

    //Update the UI of cards left in deck
    public void UpdateDeckCount(int count)
    {
        PlayerDeckCount.text = "Cards left: " + count.ToString();
        EnemyDeckCount.text = "Cards left: " + count.ToString();
    }

    //Update player health
    public void UpdatePlayerHP(int targetHP)
    {
        PlayerHP.text =targetHP.ToString();
    }

    //Update enemy health
    public void UpdateEnemyHP(int targetHP)
    {
        EnemyHP.text = targetHP.ToString();
    }

    public void ToggleHelpPanel1()
    {
        HelpPanel1.SetActive(true);
        MenuScreen.SetActive(false);
    }

    public void ToggleHelpPanel2()
    {
        HelpPanel1.SetActive(false);
        HelpPanel2.SetActive(true);
    }

    public void CloseHelpPanel2()
    {
        HelpPanel2.SetActive(false);
        MenuScreen.SetActive(true);
    }
    /*
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
    */
    

    //Turn on game over canvas saying who won depending on values recieved from game manager
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
