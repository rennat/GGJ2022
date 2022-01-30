using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PhaseManager : MonoBehaviour
{
    public enum Phase
    {
        Default,
        Prepare,
        Survive
    }

    public delegate void PhaseDelegate(Phase currentPhase, Phase previousPhase);

    private static PhaseManager _instance;
    public static PhaseManager Instance => _instance;
    public static Phase CurrentPhase => (Phase)_instance.CurPhase;
    public static PhaseDelegate OnPhaseChanged;

    public GameObject[] wanderSpawnPoints;
    public GameObject[] rushSpawnPoints;
    public int curDay = 0;
    public float tickTime = 1f;
    public float wanderSpawnRadius = 0.3f;

    public TMP_Text dayLabel;
    public TMP_Text phaseLabel;
    public TMP_Text modeLabel;
    public TMP_Text modeTimer;

    private Phase curPhase = Phase.Default;
    public Phase CurPhase
    {
        get => curPhase;
        set
        {
            if (curPhase != value)
            {
                var prevPhase = curPhase;
                curPhase = value;
                OnPhaseChanged?.Invoke(curPhase, prevPhase);
            }
        }
    }

    [Serializable]
    public struct DayDefinition {
        public float phase1Duration;
        public int phase1NPCCount;
        public GameObject phase1NPCType;

        public float phase2Duration;
        public int phase2NPCCount;
        public GameObject phase2NPCType;
    }

    public DayDefinition[] dayConfig;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Why u do dis? There should be only one Phase Manager");
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(dayLoop());
    }

    IEnumerator dayLoop () {
        for (int i = 0; i < dayConfig.Length; i++) {
            curDay = i+1;
            CurPhase = Phase.Prepare;
            updateUI(curDay, CurPhase);

            DayDefinition thisDay = dayConfig[i];
            float curPhaseTime = 0f;

            StartCoroutine(SpawnWanderNPCs(thisDay.phase1NPCCount, thisDay.phase1NPCType));
            while (curPhaseTime < dayConfig[i].phase1Duration) {
                if (winConditionMet()) {
                    yield break;
                }
                updateTime(dayConfig[i].phase1Duration - curPhaseTime);
                curPhaseTime += tickTime + Time.deltaTime;
                yield return new WaitForSeconds(tickTime);
            }

            curPhaseTime = 0f;
            CurPhase = Phase.Survive;
            updateUI(curDay, CurPhase);

            convertNPCs();

            StartCoroutine(SpawnRushNPCs(thisDay.phase2NPCCount, thisDay.phase2Duration, thisDay.phase2NPCType));
            while (curPhaseTime < dayConfig[i].phase2Duration) {
                if (winConditionMet()) {
                    yield break;
                }
                updateTime(dayConfig[i].phase2Duration - curPhaseTime);
                curPhaseTime += tickTime + Time.deltaTime;
                yield return new WaitForSeconds(tickTime);
            }
        }
        yield return null;
    }

    IEnumerator SpawnWanderNPCs(int numNPCs, GameObject npcType) {
        for (int i = 0; i < numNPCs; i++) {
            // Pick a random spawn point
            GameObject curSpawn = wanderSpawnPoints[UnityEngine.Random.Range(0, wanderSpawnPoints.Length)];
            if (curSpawn != null) {
                GameObject newNPC = Instantiate(npcType, this.transform);
                newNPC.transform.position = curSpawn.transform.position + new Vector3(UnityEngine.Random.Range(-wanderSpawnRadius, wanderSpawnRadius), UnityEngine.Random.Range(-wanderSpawnRadius, wanderSpawnRadius));
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator SpawnRushNPCs(int numNPCs, float duration, GameObject npcType) {
        float waitTime = duration / numNPCs;
        for (int i = 0; i < numNPCs; i++) {
            // Pick a random spawn point
            GameObject curSpawn = rushSpawnPoints[UnityEngine.Random.Range(0, rushSpawnPoints.Length)];
            if (curSpawn != null) {
                GameObject newNPC = Instantiate(npcType, this.transform);
                newNPC.transform.position = curSpawn.transform.position;
            }
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    bool winConditionMet() {
        return false;
    }

    void updateUI(int day, Phase phase) {
        dayLabel.text = day.ToString();
        phaseLabel.text = phase.ToString();

        if (phase == Phase.Prepare) {
            modeLabel.text = "Prepare";
        } else {
            modeLabel.text = "Survive";
        }
    }

    void updateTime(float time) {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        modeTimer.text = niceTime;
    }

    void convertNPCs() {
        foreach (NPCManager liveNPC in GetComponentsInChildren<NPCManager> ()) {
            liveNPC.Convert();
        }
    }
}
