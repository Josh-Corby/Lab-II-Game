using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    public bool paused;
    public bool toggle;
    void Start()
    {
        toggle = false;
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (toggle == false) return;
            Pause();
        }
            
        
    }

     public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pausePanel.SetActive(paused);
    }

    public void Toggle()
    {
        toggle = !toggle;
        if (toggle == true)
            gameObject.SetActive(true);
        if (toggle == false)
        gameObject.SetActive(false);
    }
}
