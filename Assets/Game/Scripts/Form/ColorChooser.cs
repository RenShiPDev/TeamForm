using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(UIDocument))]
public class ColorChooser : MonoBehaviour
{
    [Inject] private FormElements _formElements;

    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Sprite _activeSprite;

    private VisualElement _currentColorLayer;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();

        var formColorButtons = _formElements.LayerButtons;
        foreach(var button in formColorButtons)
        {
            button.RegisterCallback<ClickEvent>(SetLayerButton);
        }
        foreach (var button in _formElements.ColorButtons)
        {
            button.RegisterCallback<ClickEvent>(ChangeColor);
        }

        _currentColorLayer = formColorButtons[formColorButtons.Count-1];
        _currentColorLayer.Q().style.backgroundImage = new StyleBackground(_activeSprite);
    }

    private void SetLayerButton(ClickEvent e)
    {
        _currentColorLayer.Q().style.backgroundImage = new StyleBackground(_inactiveSprite);
        _currentColorLayer = e.currentTarget as VisualElement;
        _currentColorLayer.Q().style.backgroundImage = new StyleBackground(_activeSprite);
    }

    private void ChangeColor(ClickEvent e)
    {
        VisualElement button = e.currentTarget as VisualElement;
        var color = button.Q().resolvedStyle.backgroundColor;

        _formElements.CurrentColoringItem.SetColor(_currentColorLayer, color);
        _currentColorLayer.Q().ElementAt(0).Q().style.unityBackgroundImageTintColor = color;
    }
}
