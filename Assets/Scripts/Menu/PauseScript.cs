using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject ui;
    public GameObject pauseMenu;
    public GameObject mainButtons;
    public GameObject settingsButtons;
    public GameObject player;
    public GameObject tppCamera;
    public GameObject fppCamera;
    bool isPaused;
    bool wasTppCameraDisabled = false;
    public AudioSource source;
    public PauseAudio pauseAudio;


    public void Pause()
    {
        //if tppCamera was disabled during game, we make wasTppCameraDisabled true, else false
        if(!tppCamera.activeSelf)
        {
            wasTppCameraDisabled = true;
        }
        else
        {
            wasTppCameraDisabled = false;
        }
        isPaused = true;
        tppCamera.SetActive(true); //we enable tppCamera always so that we can see pause menu
        Time.timeScale = 0; // we stop time
        pauseMenu.SetActive(true); // we enable pause menu
        ui.SetActive(false); // we disable UI
        player.SetActive(false); // we disable player
        settingsButtons.SetActive(false); // we disable settings buttons
        mainButtons.SetActive(true); // we enable main buttons
        Cursor.visible = true; // we make cursor visible
        Cursor.lockState = CursorLockMode.None; // we unlock cursor
        pauseAudio.PauseAllSounds();
        source.Play();
        Debug.Log("Pause");
    }
    

    public void Resume()
    {
        //if tppCamera was disabled during game, we disable it again when we resume the game
        if(wasTppCameraDisabled)
        {
            tppCamera.SetActive(false);
        }
        isPaused = false;
        Time.timeScale = 1; // we resume time
        pauseMenu.SetActive(false); // we disable pause menu
        ui.SetActive(true); // we enable UI
        player.SetActive(true); // we enable player
        Cursor.visible = false; // we make cursor invisible
        Cursor.lockState = CursorLockMode.Locked; // we lock cursor
        pauseAudio.ResumeAllSounds();
        source.Pause();
        Debug.Log("Resume");
    }

    public void Settings()
    {
        settingsButtons.SetActive(true); // we enable settings buttons
        mainButtons.SetActive(false); // we disable main buttons
    }

    public void BackToMain()
    {
        settingsButtons.SetActive(false); // we disable settings buttons
        mainButtons.SetActive(true); // we enable main buttons
    }
    
    void Start()
    {
        Time.timeScale = 1; // we resume time
        pauseMenu.SetActive(false); // we disable pause menu
        isPaused = false;
        player.SetActive(true); // we enable player
        Cursor.visible = false; // we make cursor invisible
    }
    void Update()
    {
        // if we press escape, we pause or resume the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
