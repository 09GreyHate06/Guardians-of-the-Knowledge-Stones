using TMPro;
using UnityEngine;

namespace Core.GOTKS
{
    public class QuestionUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _questionTxt;
        [SerializeField]
        private GameObject _choicesHolder;

        public TextMeshProUGUI QuestionTxt => _questionTxt;
        public GameObject ChoicesHolder => _choicesHolder;
    }
}