using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(UIDocument))]
public class TeamNameInputHandler : MonoBehaviour
{
    [Inject] private FormElements _formElements;

    private Color _normalNextButtonColor;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();

        _formElements.TeamTextField.RegisterCallback<InputEvent>(OnInput);
        _normalNextButtonColor = _formElements.NextPageButton.resolvedStyle.unityBackgroundImageTintColor;
        _formElements.TeamTextField.value = PlayerPrefs.GetString("TeamName");
    }

    private void OnInput(InputEvent e)
    {
        if (e.newData.Length > 10)
        {
            (e.target as VisualElement).Q().style.color = _formElements.RedColor;
            var inactiveColor = _normalNextButtonColor;
            inactiveColor.a = 0.25f;
            _formElements.NextPageButton.style.unityBackgroundImageTintColor = inactiveColor;
            _formElements.NextPageButton.ElementAt(0).style.color = inactiveColor;
        }
        else
        {
            (e.currentTarget as TextField).style.color = Color.white;
            _formElements.NextPageButton.style.unityBackgroundImageTintColor = _normalNextButtonColor;
            _formElements.NextPageButton.ElementAt(0).style.color = _normalNextButtonColor;

            PlayerPrefs.SetString("TeamName", e.newData);
        }
    }
}
