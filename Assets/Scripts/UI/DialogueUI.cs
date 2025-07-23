using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.Serialization;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {

       [SerializeField] private TextMeshProUGUI dialogueText;
       private PlayerConversant _playerConversant;
        
        void Start()
        {
           _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
           dialogueText.text = _playerConversant.GetText();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

