using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColoringItem
{
    public Dictionary<VisualElement, VisualElement> FormButtonsLayers;
    public VisualElement Element;
    internal int _formId;

    public ColoringItem(Dictionary<VisualElement, VisualElement> formButtonsLayers, VisualElement element, int formId)
    {
        FormButtonsLayers = formButtonsLayers;
        Element = element;
        _formId = formId;
    }

    public void LoadColors()
    {
        foreach (var layer in FormButtonsLayers.Values)
        {
            float r = PlayerPrefs.GetFloat(layer.name + _formId + "r");
            float g = PlayerPrefs.GetFloat(layer.name + _formId + "g");
            float b = PlayerPrefs.GetFloat(layer.name + _formId + "b");
            var color = new Color(r, g, b);
            layer.Q().style.unityBackgroundImageTintColor = color;
        }
    }

    public void SetColor(VisualElement button, Color color)
    {
        FormButtonsLayers.TryGetValue(button, out VisualElement formLayer);
        formLayer.Q().style.unityBackgroundImageTintColor = color;
        SaveColor(formLayer.name, color);
    }

    public void SaveColor(string layerName, Color color)
    {
        PlayerPrefs.SetFloat(layerName + _formId + "r", color.r);
        PlayerPrefs.SetFloat(layerName + _formId + "g", color.g);
        PlayerPrefs.SetFloat(layerName + _formId + "b", color.b);
    }

    public void ChangeLayerButtonsColor()
    {
        LoadColors();

        foreach (var button in FormButtonsLayers.Keys)
        {
            FormButtonsLayers.TryGetValue(button, out VisualElement layer);
            var color = layer.resolvedStyle.unityBackgroundImageTintColor;
            button.Q().ElementAt(0).Q().style.unityBackgroundImageTintColor = color;
        }
        LoadColors();
    }
}


public class FormElements : MonoBehaviour
{
    private static FormElements _instance;
    public static FormElements Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<VisualElement> Panels = new List<VisualElement>();
    public List<VisualElement> HidingElements = new List<VisualElement>();

    public List<VisualElement> ColorButtons = new List<VisualElement>();

    public List<VisualElement> FormColorButtons = new List<VisualElement>();
    public List<VisualElement> LayerButtons = new List<VisualElement>();

    public List<VisualElement> Labels = new List<VisualElement>();

    public List<ColoringItem> CurrentColoringItems = new List<ColoringItem>();
    public List<ColoringItem> Forms = new List<ColoringItem>();
    public List<ColoringItem> Logos = new List<ColoringItem>();

    public VisualElement NextFormButton;
    public VisualElement PreviousFormButton;

    public VisualElement NextPageButton;

    public ColoringItem LastForm;
    public ColoringItem LastLogo;

    public VisualElement ScaledForm;

    public VisualElement FinalForm;
    public VisualElement FinalLogo;

    public TextField TeamTextField;

    public ColoringItem CurrentColoringItem;

    public Color RedColor;

    public GameObject UIGameObject;
}

