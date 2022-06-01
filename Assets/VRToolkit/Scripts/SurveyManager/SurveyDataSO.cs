using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRToolkit.Surveys
{
    [CreateAssetMenu(fileName = "SurveyData", menuName = "VRToolkit/SurveyData", order = 1)]
    public class SurveyDataSO : ScriptableObject
    {
        // names must match the json, for now I'll adapt this to the json but in the future we need to use proper naming...
        public new string name;
        public string active;
        public bool mandatory_mode;
        public List<QuestionSet> questionSets;
    }

    [Serializable]
    public class QuestionSet : IComparable<QuestionSet>
    {
        public int id;
        public string name;
        public int order;
        public int timed_appearance_delay;
        public int precondition_set;
        public float timed_started_at;
        public string title_localization_key;
        public bool skippable;
        public bool skippable_allow_skip_survey;
        public bool display_confirmation_box;

        public List<Question> questions;

        public int CompareTo(QuestionSet other)
        {
            return order.CompareTo(other.order);
        }
    }

    [Serializable]
    public class Question : IComparable<Question>
    {
        public int id;
        public int order;
        public string localization_key;
        public string answer_bundle_type_name; // enum categorical, numerical...  "LikertScale_Numerical", "LikertScale_ZeroToTen", "LikertScale_Text"

        public List<Answer> answers;

        public int CompareTo(Question other)
        {
            return order.CompareTo(other.order);
        }
    }

    [Serializable]
    public class Answer
    {
        public int id;
        public string localization_key;
        public string analytics_event_value;
        public string answer_image;
    }
}
