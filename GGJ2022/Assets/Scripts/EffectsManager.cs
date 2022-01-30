using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectsManager : MonoBehaviour
{
    public float transitionTime = 1f;
    public PostProcessVolume surviveModePostProcessingVolume;
    public GameObject surviveModeEffects;
    public float woundFadeoutTime = 0.2f;
    public float woundFullFlashDamageAmount = 4f;
    public PostProcessVolume woundedPostProcessingVolume;

    private float targetSurviveEffectAmount = 0;
    private float surviveEffectAmount = 0;

    private float woundEffectAmount = 0;

    private void OnEnable()
    {
        PlayerInfo.OnPlayerTakesDamage += HandlePlayerTakesDamage;
    }

    private void OnDisable()
    {
        PlayerInfo.OnPlayerTakesDamage -= HandlePlayerTakesDamage;
    }

    void Update()
    {
        targetSurviveEffectAmount = PhaseManager.CurrentPhase == PhaseManager.Phase.Survive ? 1 : 0;
        surviveEffectAmount = Mathf.Lerp(surviveEffectAmount, targetSurviveEffectAmount, Time.deltaTime / transitionTime);

        surviveModePostProcessingVolume.weight = surviveEffectAmount;
        surviveModeEffects.SetActive(surviveEffectAmount > 0.5f);

        woundedPostProcessingVolume.weight = woundEffectAmount;
        woundEffectAmount = Mathf.Clamp01(Mathf.Lerp(woundEffectAmount, 0, Time.deltaTime / woundFadeoutTime));
    }

    void HandlePlayerTakesDamage(float amount)
    {
        woundEffectAmount = Mathf.Clamp01(woundEffectAmount + amount/woundFullFlashDamageAmount);
    }
}
