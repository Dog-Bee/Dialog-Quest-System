using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {

       [SerializeField] private TextMeshProUGUI dialogueText;
       [SerializeField] private Button nextButton;
       private PlayerConversant _playerConversant;
        
        void Start()
        {
            ButtonsInitialize();
           _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
           UpdateUI();
        }


        private void ButtonsInitialize()
        {
            nextButton.onClick.AddListener(Next);
        }

        private void Next()
        {
           _playerConversant.GetNext();
           UpdateUI();
        }

        private void UpdateUI()
        {
            dialogueText.text = _playerConversant.GetText();
        }

    }
}

