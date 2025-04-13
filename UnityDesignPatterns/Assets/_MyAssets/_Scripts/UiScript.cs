using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] TMP_Text bestTime;
    [SerializeField] Image heart1;
    [SerializeField] Image heart2;
    [SerializeField] Image heart3;
    [SerializeField] bool isGameScene;
    private PlayerScript player;
    int playerHitPoints;
    bool settingsOpen = false;
    Game gameScript;

    // Start is called before the first frame update
    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        gameScript = Game.Instance;

        player = FindObjectOfType<PlayerScript>();
        //bestTime.text = "Best Time: ";

        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        if (settingsOpen == false)
        {
            Time.timeScale = 0f;
            settingsOpen = true;
            settingsPanel.SetActive(true);
            if (isGameScene) 
                gameScript.PauseTimer();
        }
        else
        {
            Time.timeScale = 1f;
            settingsOpen = false;
            settingsPanel.SetActive(false);
            if (isGameScene)
                gameScript.UnpauseTimer();
        }
    }

    public void ChangeSFXVolume(float value)
    {
        Game.Instance.SOMA.SetVolume(value, SoundManager.SoundType.SOUND_SFX);
    }

    public void ChangeMusicVolume(float value)
    {
        Game.Instance.SOMA.SetVolume(value, SoundManager.SoundType.SOUND_MUSIC);
    }

    public void CheckPlayerHitpoints()
    {
        if (playerHitPoints == 3)
        {
            heart1.gameObject.SetActive(true);
        }
        else if (playerHitPoints == 2)
        {
            heart1.gameObject.SetActive(false);
            heart2.gameObject.SetActive(true);
        }
        else if (playerHitPoints == 1)
        {
            heart2.gameObject.SetActive(false);
            heart3.gameObject.SetActive(true);
        }
        else if (playerHitPoints == 0)
        {
            heart3.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (player != null)
        {
            playerHitPoints = player.GetPlayerHitPoints();
            CheckPlayerHitpoints();
        }
    }
}
