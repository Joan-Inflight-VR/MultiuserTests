using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.Surveys
{
    public class SurveyManager : MonoBehaviour
    {
        private float distance = 0.9f;

        private SurveyDataSO surveyData;
        private SurveySet surveySetPrefab;
        private SurveySet surveySet;

        private Coroutine waitingcoroutine;

        private List<QuestionSet> waitingSets;
        private LinkedList<QuestionSet> pendingToShowSets;

        private QuestionSet currentSet;

        private float pausedTimeAcum;
        private float pausedTime;
        private bool paused;
        private bool started;

        private void Awake()
        {
            waitingSets = new List<QuestionSet>();
            pendingToShowSets = new LinkedList<QuestionSet>();

            surveySetPrefab = Resources.Load<SurveySet>(Statics.Resources.surveyManagerPrefab);

            LoadSurveyData();

            paused = false;
            started = false;
            pausedTime = 0f;
            pausedTimeAcum = 0f;
        }

        private void Start()
        {
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.startSurveyManager, () => StartSurveyManager());
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.pauseSurveyManager, PauseSurveyManager);
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.resumeSurveyManager, (x) => ResumeSurveyManager((bool)x));
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.stopSurveyManager, StopSurveyManager);
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.forceNextQuestionSet, (x) => ForceSet((int)x));
            EventManager.Instance.StartListening(Statics.Events.SurveyManager.setDone, (x) =>
            {
                currentSet = null;
                EvaluateSetPrecondition((int)x);
            });
        }

        private void Update()
        {
            if (!paused && (pendingToShowSets.Count > 0 && currentSet == null))
            {
                currentSet = pendingToShowSets.First.Value;
                pendingToShowSets.RemoveFirst();
                ShowCurrentSet();
            }
        }

        private void LoadSurveyData()
        {
            try
            {
                string surveyDataJson = File.ReadAllText(Statics.surveysPath);

                if (!string.IsNullOrEmpty(surveyDataJson))
                {
                    SurveyDataSO surveyData = ScriptableObject.CreateInstance<SurveyDataSO>();
                    JsonUtility.FromJsonOverwrite(surveyDataJson, surveyData);
                    this.surveyData = surveyData;
                }

                Debug.LogWarning($"Something is wrong with your json file, loading default.");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"An error ocurred reading survey json file. Error: {e}");
            }
        }

        private void EvaluateSetPrecondition(int precondition)
        {
            foreach (QuestionSet qt in surveyData.questionSets)
            {
                if (!waitingSets.Contains(qt) && qt.precondition_set == precondition)
                {
                    qt.timed_started_at = Time.realtimeSinceStartup;
                    qt.timed_appearance_delay *= 60; // multiply by 60 so we have the seconds. (this can be removed when we move to new survey architecture and use actual seconds...)
                    waitingSets.Add(qt);
                }
            }
        }

        private void StartSurveyManager(int precondition = -1)
        {
            if (surveyData == null || started) return;

            started = true;

            EvaluateSetPrecondition(precondition);

            if (waitingcoroutine == null)
            {
                waitingcoroutine = StartCoroutine(WaitingCoroutine());
            }
        }

        private void ResumeSurveyManager(bool countPausedTime = true)
        {
            if (!paused) return;

            if (countPausedTime)
            {
                pausedTimeAcum += Time.realtimeSinceStartup - pausedTime;
            }

            paused = false;
        }

        private void StopSurveyManager()
        {
            if (!started) return;

            if (waitingcoroutine != null)
            {
                StopCoroutine(waitingcoroutine);
                waitingcoroutine = null;
            }

            waitingSets.Clear();
            pendingToShowSets.Clear();

            paused = false;
            started = false;
            pausedTime = 0f;
            pausedTimeAcum = 0f;
            currentSet = null;
        }

        private void PauseSurveyManager()
        {
            if (!started || paused) return;

            pausedTime = Time.realtimeSinceStartup;
            paused = true;
        }

        private void ForceSet(int id)
        {
            if (id < 0) return;

            QuestionSet qt = GetSet(id);

            if (pendingToShowSets.Contains(qt))
            {
                pendingToShowSets.Remove(qt);
            }
            else if (waitingSets.Contains(qt))
            {
                waitingSets.Remove(qt);
            }

            pendingToShowSets.AddFirst(qt);
        }

        private void ShowCurrentSet()
        {
            surveySet = Instantiate(surveySetPrefab);
            surveySet.transform.position = VRToolkitManager.Instance.rigContainer.transform.position + VRToolkitManager.Instance.initialForward * distance;

            surveySet.transform.position += new Vector3(0f, 0.025f, 0.05f);

            surveySet.SetUpSet(currentSet);

            Utilities.PlayOneShotSFX(VRToolkitManager.Instance.audioSettings.newWindowOpen, 1f, Vector3.zero);
        }

        private IEnumerator WaitingCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f);

                if (!paused)
                {
                    float current_time = Time.realtimeSinceStartup;

                    List<QuestionSet> setsToEvaluate = new List<QuestionSet>(waitingSets);

                    for (int i = 0; i < setsToEvaluate.Count; ++i)
                    {
                        float total_seconds = current_time - setsToEvaluate[i].timed_started_at - pausedTimeAcum;

                        if (total_seconds >= setsToEvaluate[i].timed_appearance_delay)
                        {
                            waitingSets.Remove(setsToEvaluate[i]);
                            pendingToShowSets.AddLast(setsToEvaluate[i]);
                        }
                    }
                }
            }
        }

        private QuestionSet GetSet(int id)
        {
            foreach (QuestionSet qt in surveyData.questionSets)
            {
                if (qt.id == id)
                {
                    return qt;
                }
            }

            return null;
        }
    }
}