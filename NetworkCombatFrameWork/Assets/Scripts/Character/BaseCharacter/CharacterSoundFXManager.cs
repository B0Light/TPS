using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damaged Grunts")] 
    [SerializeField] protected AudioClip[] damageGrunts;
    
    [Header("Attack Grunts")] 
    [SerializeField] protected AudioClip[] attackGrunts;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch= true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        audioSource.pitch = 1;

        if(randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
    }

    public void PlayDamageGrunt()
    {
        if(damageGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(damageGrunts));
    }

    public virtual void PlayAttackGrunt()
    {
        if(attackGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(attackGrunts));
    }
}

