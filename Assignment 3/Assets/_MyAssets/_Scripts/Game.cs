using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; set; } // Static object of the class.
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
        SOMA.AddSound("Alert", Resources.Load<AudioClip>("alert"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Patrol", Resources.Load<AudioClip>("patrol"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Idle", Resources.Load<AudioClip>("idle"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Upbeat", Resources.Load<AudioClip>("upbeat"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("Upbeat");
    }
}
