using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite onSprite, offSprite;

    public void OnToggled(Toggle toggle, Image image)
    {
        image.sprite = toggle.isOn ? onSprite : offSprite;
    }
}