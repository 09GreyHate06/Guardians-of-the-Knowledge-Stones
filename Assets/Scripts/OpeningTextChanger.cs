using TMPro;
using UnityEngine;

namespace Core.GOTKS
{
    public class OpeningTextChanger : MonoBehaviour
    {
        [SerializeField]
        private string[] _texts;
        [SerializeField]
        private float _delay;

        private Animator _animator;
        private TextMeshProUGUI _tmp;

        private int _index;

        private float _time;
        private bool flag;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (flag)
                return;

            if (_time > _delay)
            {
                flag = true;
                _animator.Play("OpeningAnim");
                return;
            }

            _time += Time.deltaTime;
        }

        public void Next()
        {
            if(_index >= _texts.Length)
            {
                _tmp.enabled = false;
                _animator.enabled = false;
                return;
            }

            _tmp.text = _texts[_index++];
        }
    }
}
