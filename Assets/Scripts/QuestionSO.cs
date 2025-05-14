using System.Collections.Generic;
using UnityEngine;

namespace Core.GOTKS
{
    [CreateAssetMenu(fileName = "Question", menuName = "GOTKS/Question")]
    public class QuestionSO : ScriptableObject
    {
        [SerializeField, TextArea(3, 5)]
        private string _question;
        [SerializeField]
        private string[] _choices;
        [SerializeField]
        private int _answerIndex;

        public string Question => _question;
        public IReadOnlyList<string> Choices => _choices;
        public int AnswerIndex => _answerIndex;

        private void OnValidate()
        {
            if (_choices.Length > _answerIndex && _answerIndex >= 0)
                return;

            _answerIndex = 0;
        }
    }
}