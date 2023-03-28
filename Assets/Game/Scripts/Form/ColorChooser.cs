using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

[RequireComponent(typeof(UIDocument))]
public class ColorChooser : MonoBehaviour
{
    private FormElements _formElements;

    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Sprite _activeSprite;

    private VisualElement _currentColorLayer;
    private VisualElement _currentPaletteButton;

    private float _choosedColorBorderWidth = 1.2f;

    private void Start()
    {
        _formElements = FormElements.Instance;

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
        SetPaletteButtonBorder(); 
    }

    private void SetLayerButton(ClickEvent e)
    {
        _currentColorLayer.Q().style.backgroundImage = new StyleBackground(_inactiveSprite);
        _currentColorLayer = e.currentTarget as VisualElement;
        _currentColorLayer.Q().style.backgroundImage = new StyleBackground(_activeSprite);
        SetPaletteButtonBorder();
    }

    private void ChangeColor(ClickEvent e)
    {
        VisualElement button = e.currentTarget as VisualElement;
        var color = button.Q().resolvedStyle.backgroundColor;

        _formElements.CurrentColoringItem.SetColor(_currentColorLayer, color);
        _currentColorLayer.Q().ElementAt(0).Q().style.unityBackgroundImageTintColor = color;
        SetPaletteButtonBorder();
    }

    public void SetPaletteButtonBorder()
    {
        foreach (var button in _formElements.ColorButtons)
        {
            var layerColor = _currentColorLayer.Q().ElementAt(0).Q().style.unityBackgroundImageTintColor.value;
            var paletteColor = button.Q().resolvedStyle.backgroundColor;
            if (layerColor == paletteColor)
            {
                if(_currentPaletteButton != null) ChangeWidth(0);
                _currentPaletteButton = button;
                ChangeWidth(_choosedColorBorderWidth);
            }
        }
    }

    private void ChangeWidth(float width)
    {
        _currentPaletteButton.Q().style.borderTopWidth = width;
        _currentPaletteButton.Q().style.borderBottomWidth = width;
        _currentPaletteButton.Q().style.borderLeftWidth = width;
        _currentPaletteButton.Q().style.borderRightWidth = width;
    }
}
