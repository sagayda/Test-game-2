using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;

    public void Start()
    {
        _actionButtonsText[0].text += "asd";
    }

}
