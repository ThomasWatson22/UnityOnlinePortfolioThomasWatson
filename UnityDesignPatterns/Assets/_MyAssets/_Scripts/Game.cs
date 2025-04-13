using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; } // Static object of the class.
    public SoundManager SOMA;
    [SerializeField] TMP_Text timerText;

    private float startTime;
    private float pausedTime;
    private bool isPaused;

    private void Awake() // Ensure there is only one instance.
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Will persist between scenes.
            Initialize();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances.
        }
    }

    private void Initialize()
    {
        SOMA = new SoundManager();
        SOMA.Initialize(gameObject);
        SOMA.AddSound("Jump", Resources.Load<AudioClip>("jumping"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Roll", Resources.Load<AudioClip>("rolling"), SoundManager.SoundType.SOUND_SFX);
        
        SOMA.AddSound("Death", Resources.Load<AudioClip>("death"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("Adventure", Resources.Load<AudioClip>("8_Bit_Adventure"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("Adventure");

        startTime = Time.time;
        if (timerText != null)
            StartCoroutine("UpdateTimer");
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            if(!isPaused)
            {
                float elapsedTime = Time.time - startTime;
                if(isPaused)
                {
                    pausedTime += Time.deltaTime;
                }
                if (timerText != null)
                    timerText.text = "Time: " + elapsedTime.ToString("F3") + "s";   
            }
            yield return null;
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void UnpauseTimer()
    {
        isPaused = false;
    }
}
