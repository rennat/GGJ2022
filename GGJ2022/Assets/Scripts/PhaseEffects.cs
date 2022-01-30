using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEffects : MonoBehaviour
{
    public GameObject PreparePhaseEffects;
    public GameObject SurvivePhaseEffects;

    public void OnEnable()
    {
        StartCoroutine(ActivateEnumerator());
    }

    public void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChange;
    }

    public IEnumerator ActivateEnumerator()
    {
        while (PhaseManager.Instance == null)
        {
            yield return null;
        }
        PhaseManager.OnPhaseChanged += HandlePhaseChange;
        HandlePhaseChange(PhaseManager.CurrentPhase, PhaseManager.CurrentPhase);
    }

    public void HandlePhaseChange(PhaseManager.Phase currentPhase, PhaseManager.Phase prevPhase)
    {
        if (PreparePhaseEffects != null)
            PreparePhaseEffects.SetActive(currentPhase == PhaseManager.Phase.Prepare);
        if (SurvivePhaseEffects != null)
            SurvivePhaseEffects.SetActive(currentPhase == PhaseManager.Phase.Survive);
    }
}
