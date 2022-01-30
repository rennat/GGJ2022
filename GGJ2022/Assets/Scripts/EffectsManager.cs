using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectsManager : MonoBehaviour
{
    public float transitionTime = 1f;
    public PostProcessVolume surviveModePostProcessingVolume;
    public GameObject surviveModeEffects;

    private float targetSurviveEffectAmount = 0;
    private float surviveEffectAmount = 0;

    void Update()
    {
        targetSurviveEffectAmount = PhaseManager.CurrentPhase == PhaseManager.Phase.Survive ? 1 : 0;
        surviveEffectAmount = Mathf.Lerp(surviveEffectAmount, targetSurviveEffectAmount, transitionTime * Time.deltaTime);

        surviveModePostProcessingVolume.weight = surviveEffectAmount;
        surviveModeEffects.SetActive(surviveEffectAmount > 0.5f);
    }
}
