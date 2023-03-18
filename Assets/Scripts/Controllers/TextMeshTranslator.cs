using Manager;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class TextMeshTranslator : MonoBehaviour
    {
        private TMP_Text _textToTranslate;

        [Inject]
        private void Construct(TranslateTextManager translateTextManager)
        {
            _textToTranslate = GetComponent<TMP_Text>();
           _textToTranslate.text = translateTextManager.GetAndTranslateText(_textToTranslate.text);
        }
    }
}
