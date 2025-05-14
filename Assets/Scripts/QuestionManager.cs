using GamingIsLove.Makinom;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GOTKS
{
    public class QuestionManager : MonoBehaviour
    {
        private static QuestionManager s_instance;

        [SerializeField]
        private QuestionSO[] _elementaryQuestions;

        [SerializeField]
        private QuestionSO[] _highSchoolQuestions;
        [SerializeField]
        private QuestionSO[] _collegeQuestions;

        [SerializeField]
        private QuestionUI _questionUIPrefab;
        [SerializeField]
        private Button _choicePrefab;

        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _correctAnswerSfx;
        [SerializeField]
        private AudioClip _wrongAnswerSfx;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                return;
            }

            Destroy(gameObject);
        }

        public static void AskElementaryRandomQuestion()
        {
            AskRandomQuestion(s_instance.GetElementaryRandomQuestion());
        }

        public static void AskHighSchoolRandomQuestion()
        {
            AskRandomQuestion(s_instance.GetHighSchoolRandomQuestion());
        }

        public static void AskCollegeRandomQuestion()
        {
            AskRandomQuestion(s_instance.GetCollegeRandomQuestion());
        }

        private static void AskRandomQuestion(QuestionSO question)
        {
            var canvas = FindFirstObjectByType<Canvas>();
            var questionUI = Instantiate(s_instance._questionUIPrefab, canvas.transform);
            questionUI.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.3f, false);
            questionUI.QuestionTxt.text = question.Question;
            for (int i = 0; i < question.Choices.Count; i++)
            {
                var button = Instantiate(s_instance._choicePrefab, questionUI.ChoicesHolder.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = IntToAlphabetLabel(i) + ". " + question.Choices[i];

                int answerIndex = i;
                if (i == 0)
                    button.Select();

                button.onClick.AddListener(() =>
                {
                    Maki.Game.Variables.Set("answered", 1);
                    bool correct = answerIndex == question.AnswerIndex;
                    Maki.Game.Variables.Set("correct_answer", correct ? 1 : 0);
                    s_instance._audioSource.PlayOneShot(correct ? s_instance._correctAnswerSfx : s_instance._wrongAnswerSfx);
                    Destroy(questionUI.gameObject);
                });
            }
        }

        public static string IntToAlphabetLabel(int index)
        {
            string label = "";
            index++; // Convert from 0-based to 1-based index

            while (index > 0)
            {
                index--; // Adjust because A = 0
                label = (char)('A' + (index % 26)) + label;
                index /= 26;
            }

            return label;
        }

        private QuestionSO GetElementaryRandomQuestion()
        {
            return _elementaryQuestions[Random.Range(0, _elementaryQuestions.Length)];
        }

        private QuestionSO GetHighSchoolRandomQuestion()
        {
            return _highSchoolQuestions[Random.Range(0, _highSchoolQuestions.Length)];
        }

        private QuestionSO GetCollegeRandomQuestion()
        {
            return _collegeQuestions[Random.Range(0, _collegeQuestions.Length)];
        }
    }
}