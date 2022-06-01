using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRToolkit.InputManager.UI;
using VRToolkit.Localization;
using VRToolkit.Managers;
using VRToolkit.Utils;
using UnityEngine.Events;
using VRToolkit.AnalyticsWrapper;

namespace VRToolkit.Surveys
{
    public class SurveySet : MonoBehaviour
    {
        public AnswerItem answerPrefab;

        public CanvasGroup canvasGroup;
        public TextMeshProUGUI title;
        public TextMeshProUGUI question;

        public CanvasGroup headerCanvasGroup;

        public Transform answerContainer;
        public ToggleGroup answersGroup;
        public CanvasGroup answersCanvasGroup;

        public FillerInteractionHandler confirmationButton;

        public GameObject thankyouContainer;

        private QuestionSet set;

        private Answer selectedAnswer;

        private int questionIndex;

        private float StartQuestionTime;

        public void SetUpSet(QuestionSet set)
        {
            FaderFollower.ShowFadeFollower();

            thankyouContainer.SetActive(false);
            title.gameObject.SetActive(true);
            question.gameObject.SetActive(true);
            answerContainer.gameObject.SetActive(true);

            EventManager.Instance.TriggerEvent(Statics.Events.toggleInteraction, false);

            this.set = set;

            questionIndex = 0;

            canvasGroup.alpha = 0;
            headerCanvasGroup.alpha = 1;

            title.text = LocalizationManager.Get(set.title_localization_key);

            int[] questionIds = new int[set.questions.Count];

            for (int i = 0; i < set.questions.Count; ++i)
            {
                questionIds[i] = set.questions[i].id;
            }

            AnalyticsObject details = new AnalyticsObject();
            details.AddData("set_name", set.name);
            details.AddData("questions", questionIds);

            AnalyticsManager.RecordEvent(Statics.AnalyticsEvents.SurveyManager.surveySetStart, details);

            NextQuestion();

            canvasGroup.alpha = 1;

            confirmationButton.gameObject.SetActive(false);
        }

        public void NextQuestion()
        {
            answersCanvasGroup.alpha = 0;
            selectedAnswer = null;
            confirmationButton.OnExit();
            confirmationButton.gameObject.SetActive(false);

            if (questionIndex < set.questions.Count)
            {
                Question q = set.questions[questionIndex];

                question.text = LocalizationManager.Get(q.localization_key);

                FillAnswers(q.answers);

                questionIndex++;

                StartQuestionTime = Time.realtimeSinceStartup;
            }
            else
            {
                thankyouContainer.SetActive(true);
                title.gameObject.SetActive(false);
                question.gameObject.SetActive(false);
                answerContainer.gameObject.SetActive(false);
                Invoke(nameof(CloseSet), 2f);
            }
        }

        public void CloseSet()
        {
            canvasGroup.alpha = 0;

            EventManager.Instance.TriggerEvent(Statics.Events.toggleInteraction, true);
            EventManager.Instance.TriggerEvent(Statics.Events.SurveyManager.setDone, set.id);

            AnalyticsObject details = new AnalyticsObject();
            details.AddData("set_name", set.name);

            AnalyticsManager.RecordEvent(Statics.AnalyticsEvents.SurveyManager.surveySetfinish, details);

            FaderFollower.HideFadeFollower();

            Destroy(gameObject);
        }

        private void FillAnswers(List<Answer> answers)
        {
            foreach (Transform trans in answerContainer)
            {
                Destroy(trans.gameObject);
            }

            answersGroup.CleanToggles();

            foreach (Answer ans in answers)
            {
                AnswerItem ansItem = Instantiate(answerPrefab, answerContainer, false);
                answersGroup.RegisterToggle(ansItem.GetComponent<Toggle>());
                ansItem.SetUp(ans.answer_image, ans.localization_key, () => AnswerSelected(ans));
            }

            answersCanvasGroup.alpha = 1;
        }

        private void AnswerSelected(Answer selectedAnswer)
        {
            confirmationButton.filler.OnFillCompleted.RemoveAllListeners();

            float total_time_screen = Time.realtimeSinceStartup - StartQuestionTime;

            this.selectedAnswer = selectedAnswer;

            UnityAction action = () =>
            {
                AnalyticsObject details = new AnalyticsObject();
                details.AddData("question_id", set.questions[questionIndex - 1].localization_key);
                details.AddData("question_type", set.questions[questionIndex - 1].answer_bundle_type_name);
                details.AddData("answer_value", this.selectedAnswer.analytics_event_value);
                details.AddData("time_on_screen", total_time_screen);

                AnalyticsManager.RecordEvent(Statics.AnalyticsEvents.SurveyManager.surveyQuestionAnswer, details);

                NextQuestion();
            };

            confirmationButton.filler.OnFillCompleted.AddListener(action);
            confirmationButton.gameObject.SetActive(true);
        }
    }
}