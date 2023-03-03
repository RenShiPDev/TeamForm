using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(UIDocument))]
public class ChangeColorButton : MonoBehaviour
{
    [Inject] private FormElements _formElements;

    private VisualElement _changeColorsButton;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        
        _changeColorsButton = uiDocument.rootVisualElement.Q<VisualElement>("ChangeColor");
        _changeColorsButton.RegisterCallback<ClickEvent>(ChangeColors);

        if (PlayerPrefs.GetInt("firstColorsSaved") == 0)
        {
            foreach (var form in _formElements.Logos)
                SetRandomColors(form);
            foreach (var form in _formElements.Forms)
                SetRandomColors(form);
        }
        else
            foreach (var form in _formElements.CurrentColoringItems)
                LoadFormColors(form);

        _formElements.CurrentColoringItem.ChangeLayerButtonsColor();
        PlayerPrefs.SetInt("firstColorsSaved", 1);
    }

    private void ChangeColors(ClickEvent e)
    {
        SetRandomColors(_formElements.CurrentColoringItem);
        _formElements.CurrentColoringItem.ChangeLayerButtonsColor();
    }

    private void SetRandomColors(ColoringItem form)
    {
        List<int> randomList = GenerateRandomNumberList(0, _formElements.ColorButtons.Count);
        var formLayers = form.FormButtonsLayers;

        for (int i = 0; i < formLayers.Count(); i++)
        {
            int randElement = randomList[i];
            var color = _formElements.ColorButtons[randElement].Q().resolvedStyle.backgroundColor;
            form.SetColor(_formElements.LayerButtons[i], color);
        }
    }

    private void LoadFormColors(ColoringItem form)
    {
        form.LoadColors();
    }

    private List<int> GenerateRandomNumberList(int min, int max)
    {
        List<int> randomList = new List<int>();
        for(int i = min; i < max; i++)
        {
            randomList.Add(i);
        }

        System.Random rand = new System.Random();
        List<int> randomizedList = randomList.OrderBy(item => rand.Next()).ToList();

        return randomizedList;
    }
}
