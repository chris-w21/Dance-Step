using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StyleManager : AudioManager
{
    public static UnityEvent<int> onStyleChanged = new UnityEvent<int>();

    public int selectedStyle { get; private set; }

    public void selecStyle(int style)
    {
        selectedStyle = style;
        BeatManager.selectedBeatBaseLines = style;
    }

    private void OnEnable()
    {
        onStyleChanged.AddListener(UIManager.OnChangedStyle);
    }

    private void OnDisable()
    {
        onStyleChanged.RemoveListener(UIManager.OnChangedStyle);
    }
}