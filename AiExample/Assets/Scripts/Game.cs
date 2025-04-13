using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; } // Static object of the class.
    public SoundManager SOMA;

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
        SOMA.AddSound("move", Resources.Load<AudioClip>("move"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("victory", Resources.Load<AudioClip>("victory"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("defeat", Resources.Load<AudioClip>("defeat"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("8_Bit_Adventure", Resources.Load<AudioClip>("8_Bit_Adventure"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("8_Bit_Adventure");
    }
}
