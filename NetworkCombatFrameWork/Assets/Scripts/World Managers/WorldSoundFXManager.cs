using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldSoundFXManager : Singleton<WorldSoundFXManager>
{
    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip rollSFX;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        if(array.Length == 0) return null;

        int index = Random.Range(0, physicalDamageSFX.Length);

        return array[index];
    }
}

