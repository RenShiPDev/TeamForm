using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class NextFormButton : MonoBehaviour
{
    private FormElements _formElements;

    [SerializeField] private float _transitionDuration;

    private int _pageId;

    private void Start()
    {
        _formElements = FormElements.Instance;
        var uiDocument = GetComponent<UIDocument>();

        _formElements.NextPageButton.RegisterCallback<ClickEvent>(NextPage);
    }

    private void NextPage(ClickEvent e)
    {
        switch (_pageId)
        {
            case 0:
                ShowLogoPage();
                break;
            case 1:
                ShowTeamPage();
                break;
        }

        _pageId++;
    }

    private void ShowTeamPage()
    {
        var durationSettings = new List<TimeValue>() { new TimeValue(_transitionDuration, TimeUnit.Second) };

        foreach (var element in _formElements.HidingElements)
        {
            element.style.transitionDuration = durationSettings;
            StartCoroutine(Hide(element));
        }

        _formElements.Panels[1].style.display = DisplayStyle.Flex;
        _formElements.LastLogo = _formElements.CurrentColoringItem;

        for(int i = 0; i <= _formElements.LastForm.FormButtonsLayers.Count; i++)
        {
            SetFormsColors(_formElements.FinalLogo, _formElements.LastLogo, i);
            SetFormsColors(_formElements.FinalForm, _formElements.LastForm, i);
        }

        foreach (var element in _formElements.Panels[1].Children())
        {
            element.style.transitionDuration = durationSettings;
            element.style.display = DisplayStyle.Flex;
            StartCoroutine(Show(element));
        }
    }

    private void SetFormsColors(VisualElement final, ColoringItem last, int index)
    {
        var logoLayer = final.Children().ToList().ElementAt(index);
        var savedLayer = last.Element.Children().ToList().ElementAt(index);
        logoLayer.style.unityBackgroundImageTintColor = savedLayer.style.unityBackgroundImageTintColor;
        logoLayer.style.backgroundImage = savedLayer.resolvedStyle.backgroundImage;
    }

    private void ShowLogoPage()
    {
        var previousColoringItem = _formElements.CurrentColoringItem;
        _formElements.LastForm = previousColoringItem;
        _formElements.CurrentColoringItems = _formElements.Logos;
        _formElements.CurrentColoringItem = _formElements.CurrentColoringItems[0];

        _formElements.CurrentColoringItem.LoadColors();
        _formElements.CurrentColoringItem.Element.style.display = DisplayStyle.Flex;
        _formElements.Labels[1].style.display = DisplayStyle.Flex;
        _formElements.ScaledForm.style.display = DisplayStyle.Flex;

        for (int i = 0; i <= _formElements.LastForm.FormButtonsLayers.Count; i++)
        {
            SetFormsColors(_formElements.ScaledForm.ElementAt(0), _formElements.LastForm, i);
        }

        StartTransitions(previousColoringItem);
    }

    private void StartTransitions(ColoringItem previousColoringItem)
    {
        var durationSettings = new List<TimeValue>() { new TimeValue(_transitionDuration, TimeUnit.Second) };
        _formElements.Labels[0].style.transitionDuration = durationSettings;
        _formElements.Labels[1].style.transitionDuration = durationSettings;
        _formElements.CurrentColoringItem.Element.style.transitionDuration = durationSettings;
        previousColoringItem.Element.style.transitionDuration = durationSettings;
        _formElements.ScaledForm.style.transitionDuration = durationSettings;

        StartCoroutine(Show(_formElements.CurrentColoringItem.Element));
        StartCoroutine(Show(_formElements.Labels[1]));
        StartCoroutine(Show(_formElements.ScaledForm));

        StartCoroutine(Hide(_formElements.Labels[0]));

        StartCoroutine(Hide(previousColoringItem.Element));
    }

    private IEnumerator Show(VisualElement element)
    {
        element.style.marginLeft = 1000;
        yield return null;
        element.style.marginLeft = 0;

        yield return new WaitForSeconds(_transitionDuration);

        var durationSettings = new List<TimeValue>() { new TimeValue(0, TimeUnit.Second) };
        element.style.transitionDuration = durationSettings;
        yield break;
    }
    private IEnumerator Hide(VisualElement element)
    {
        element.style.marginLeft = 0;
        yield return null;
        element.style.marginLeft = -1000;
        yield break;
    }
    private IEnumerator Scale(VisualElement element)
    {
        element.style.scale = new Scale(Vector3.one);
        yield return null;
        element.style.scale = new Scale(Vector3.one*2);
        yield break;
    }
}
