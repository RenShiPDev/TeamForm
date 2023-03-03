using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class FormElementsInstaller : MonoInstaller
{
    [SerializeField] private UIDocument _UIPrefab;
    [SerializeField] private List<string> _notHidingElements = new List<string>();

    private UIDocument _uiDocument;
    private int _formsCount;

    public override void InstallBindings()
    {
        var formElements = Container.InstantiateComponentOnNewGameObject<FormElements>();

        Container.Bind<FormElements>().FromInstance(formElements);
        _uiDocument = Container.InstantiatePrefabForComponent<UIDocument>(_UIPrefab);


        formElements.ColorButtons = GetColorButtons();
        formElements.FormColorButtons = GetFormElements("FormColors", true);
        formElements.LayerButtons = GetFormElements("FormColors", false);

        _formsCount = 0;
        formElements.Forms = GetForms(formElements, "Form");
        formElements.Logos = GetForms(formElements, "Logo");

        formElements.CurrentColoringItems = formElements.Forms;
        formElements.CurrentColoringItem = formElements.CurrentColoringItems[0];
        formElements.CurrentColoringItem.Element.style.display = DisplayStyle.Flex;

        formElements.Labels = GetLabels();
        formElements.HidingElements = GetHidingElements(_notHidingElements);
        formElements.Panels = GetPanels();

        formElements.FinalForm = _uiDocument.rootVisualElement.Q<VisualElement>("FinalForm").ElementAt(0);
        formElements.FinalLogo = _uiDocument.rootVisualElement.Q<VisualElement>("FinalLogo").ElementAt(0);

        formElements.TeamTextField = _uiDocument.rootVisualElement.Q<TextField>("TeamTextField");

        formElements.RedColor = _uiDocument.rootVisualElement.Q<VisualElement>("RedColorItem").Q().resolvedStyle.backgroundColor;

        SetButtons(formElements);
    }

    private List<VisualElement> GetColorButtons()
    {
        List<VisualElement> colorButtons = new List<VisualElement>();

        var colorBoxes = _uiDocument.rootVisualElement.Q<VisualElement>("ColorBoxes").Children();
        foreach (var colorBox in colorBoxes)
            foreach (var color in colorBox.Children())
                colorButtons.Add(color);


        return colorButtons;
    }

    private List<VisualElement> GetFormElements(string parentName, bool getChildren)
    {
        List<VisualElement> elements = new List<VisualElement>();

        var formElemets = _uiDocument.rootVisualElement.Q<VisualElement>(parentName).Children();
        foreach (var element in formElemets)
        {
            if(getChildren)
                elements.Add(element.ElementAt(0));
            else
                elements.Add(element);
        }

        return elements;
    }

    private List<ColoringItem> GetForms(FormElements formElements, string name)
    {
        var forms = _uiDocument.rootVisualElement.Q<VisualElement>(name).Children();
        var allForms = new List<ColoringItem>();

        foreach (var uiForm in forms)
        {
            Dictionary<VisualElement, VisualElement> formButtonsLayers = new Dictionary<VisualElement, VisualElement>();
            for (int i = 0; i < formElements.FormColorButtons.Count; i++)
            {
                formButtonsLayers.Add(formElements.LayerButtons[i], uiForm.Q().ElementAt(i));
            }
            uiForm.style.display = DisplayStyle.None;
            allForms.Add(new ColoringItem(formButtonsLayers, uiForm, _formsCount));
            _formsCount++;
        }
        return allForms;
    }


    private List<VisualElement> GetLabels()
    {
        List<VisualElement> labels = new List<VisualElement>();

        labels.Add(_uiDocument.rootVisualElement.Q<VisualElement>("LabelChooseForm"));
        labels.Add(_uiDocument.rootVisualElement.Q<VisualElement>("LabelChooseLogo"));

        foreach (var label in labels)
            label.style.display = DisplayStyle.None;

        labels[0].style.display = DisplayStyle.Flex;
        return labels;
    }

    private List<VisualElement> GetHidingElements(List<string> ignored)
    {
        var elements = _uiDocument.rootVisualElement.Q<VisualElement>("Panel").Children().ToList();
        var ignoredElems = ignored.Select(FindElement).ToList();
        return elements.ToList().Except(ignoredElems).ToList();
    }

    private List<VisualElement> GetPanels()
    {
        var panels = new List<VisualElement>();
        panels.Add(_uiDocument.rootVisualElement.Q<VisualElement>("Panel")); 
        panels.Add(_uiDocument.rootVisualElement.Q<VisualElement>("TeamNamePanel"));
        return panels;
    }

    private void SetButtons(FormElements formElements)
    {
        formElements.NextFormButton = _uiDocument.rootVisualElement.Q<VisualElement>("RightButton");
        formElements.PreviousFormButton = _uiDocument.rootVisualElement.Q<VisualElement>("LeftButton");
        formElements.NextPageButton = _uiDocument.rootVisualElement.Q<VisualElement>("NextButton");
    }


    private VisualElement FindElement(string name)
    {
        return _uiDocument.rootVisualElement.Q<VisualElement>(name);
    }
}