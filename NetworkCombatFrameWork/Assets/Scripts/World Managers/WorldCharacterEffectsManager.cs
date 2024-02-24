using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : Singleton<WorldCharacterEffectsManager>
{
    [Header("VFX")]
    public GameObject bloodSplatterVFX;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;

    [SerializeField] List<InstanceCharacterEffect> instantEffects;

    protected override void Awake()
    {
        base.Awake();
        GenerateEffectIDs();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void GenerateEffectIDs()
    {
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
}
