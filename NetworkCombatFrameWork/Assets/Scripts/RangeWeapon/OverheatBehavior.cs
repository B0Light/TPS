﻿using UnityEngine;
using System.Collections.Generic;

public class OverheatBehavior : MonoBehaviour
{
    [System.Serializable]
    public struct RendererIndexData
    {
        public Renderer Renderer;
        public int MaterialIndex;

        public RendererIndexData(Renderer renderer, int index)
        {
            this.Renderer = renderer;
            this.MaterialIndex = index;
        }
    }

    [Header("Visual")]
    public ParticleSystem SteamVfx;

    public float SteamVfxEmissionRateMax = 8f;

    //Set gradient field to HDR
    [GradientUsage(true)]
    public Gradient OverheatGradient;

    public Material OverheatingMaterial;

    [Header("Sound")] 
    public AudioClip CoolingCellsSound;
    public AnimationCurve AmmoToVolumeRatioCurve;


    WeaponController m_Weapon;
    AudioSource m_AudioSource;
    List<RendererIndexData> m_OverheatingRenderersData;
    MaterialPropertyBlock m_OverheatMaterialPropertyBlock;
    float m_LastAmmoRatio;
    ParticleSystem.EmissionModule m_SteamVfxEmissionModule;

    void Awake()
    {
        var emissionModule = SteamVfx.emission;
        emissionModule.rateOverTimeMultiplier = 0f;

        m_OverheatingRenderersData = new List<RendererIndexData>();
        foreach (var renderer in GetComponentsInChildren<Renderer>(true))
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                if (renderer.sharedMaterials[i] == OverheatingMaterial)
                    m_OverheatingRenderersData.Add(new RendererIndexData(renderer, i));
            }
        }

        m_OverheatMaterialPropertyBlock = new MaterialPropertyBlock();
        m_SteamVfxEmissionModule = SteamVfx.emission;

        m_Weapon = GetComponent<WeaponController>();

        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.clip = CoolingCellsSound;
    }

    void Update()
    {
        // visual smoke shooting out of the gun
        float currentAmmoRatio = m_Weapon.CurrentAmmoRatio;
        if (currentAmmoRatio != m_LastAmmoRatio)
        {
            m_OverheatMaterialPropertyBlock.SetColor("_EmissionColor",
                OverheatGradient.Evaluate(1f - currentAmmoRatio));

            foreach (var data in m_OverheatingRenderersData)
            {
                data.Renderer.SetPropertyBlock(m_OverheatMaterialPropertyBlock, data.MaterialIndex);
            }

            m_SteamVfxEmissionModule.rateOverTimeMultiplier = SteamVfxEmissionRateMax * (1f - currentAmmoRatio);
        }

        // cooling sound
        if (CoolingCellsSound)
        {
            if (!m_AudioSource.isPlaying
                && currentAmmoRatio != 1
                && m_Weapon.IsWeaponActive
                && m_Weapon.IsCooling)
            {
                m_AudioSource.Play();
            }
            else if (m_AudioSource.isPlaying
                     && (currentAmmoRatio == 1 || !m_Weapon.IsWeaponActive || !m_Weapon.IsCooling))
            {
                m_AudioSource.Stop();
                return;
            }

            m_AudioSource.volume = AmmoToVolumeRatioCurve.Evaluate(1 - currentAmmoRatio);
        }

        m_LastAmmoRatio = currentAmmoRatio;
    }
}
