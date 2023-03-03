using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(UIDocument))]
public class FormChooser : MonoBehaviour
{
    [Inject] private FormElements _formElements;
    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();

        _formElements.NextFormButton.RegisterCallback<ClickEvent>(NextForm);
        _formElements.PreviousFormButton.RegisterCallback<ClickEvent>(PreviousForm);
    }

    private void NextForm(ClickEvent e)
    {
        ShowNewLayers(+1);
    }

    private void PreviousForm(ClickEvent e)
    {
        ShowNewLayers(-1);
    }

    private void ShowNewLayers(int nextFormIndex)
    {
        _formElements.CurrentColoringItem.Element.style.display = DisplayStyle.None;
        int currentFormIndex = _formElements.CurrentColoringItems.IndexOf(_formElements.CurrentColoringItem);

        currentFormIndex += nextFormIndex;

        int colorItemsCount = _formElements.CurrentColoringItems.Count;

        if (currentFormIndex < 0) currentFormIndex = colorItemsCount - 1;
        if (currentFormIndex >= colorItemsCount) currentFormIndex = 0;

        _formElements.CurrentColoringItem = _formElements.CurrentColoringItems[currentFormIndex];

        _formElements.CurrentColoringItem.ChangeLayerButtonsColor();
        _formElements.CurrentColoringItem.Element.style.display = DisplayStyle.Flex;
    }
}
