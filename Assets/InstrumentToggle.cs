using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InstrumentToggle : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private Image fillimage, disabledSpeakerImage;

    [SerializeField] private CanvasGroup group;

    [SerializeField] private TMP_Text label;

    [SerializeField] private bool isOn = true;

    private float oldEnabledVolume = 1f;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (isOn)
            {
                volumeSlider.interactable = false;
                fillimage.enabled = false;
                disabledSpeakerImage.enabled = true;
                oldEnabledVolume = volumeSlider.value;
                volumeSlider.value = 0;
                isOn = false;
                label.color = new Color(label.color.r, label.color.g, label.color.b, 0.5f);
            }
            else
            {
                volumeSlider.interactable = true;
                fillimage.enabled = true;
                disabledSpeakerImage.enabled = false;
                volumeSlider.value = oldEnabledVolume;
                isOn = true;
                (this as IPointerEnterHandler).OnPointerEnter(eventData);
                label.color = new Color(label.color.r, label.color.g, label.color.b, 1);
            }
        }
        Debug.Log(volumeSlider.interactable);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (isOn)
        {
            volumeSlider.interactable = true;
            fillimage.enabled = true;
            disabledSpeakerImage.enabled = false;
            isOn = true;
            group.alpha = 0.75f;
            label.color = new Color(label.color.r, label.color.g, label.color.b, 1);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (isOn)
        {
            volumeSlider.interactable = true;
            fillimage.enabled = true;
            disabledSpeakerImage.enabled = false;
            isOn = true;
            group.alpha = 1f;
            label.color = new Color(label.color.r, label.color.g, label.color.b, 1);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (isOn)
        {
            volumeSlider.interactable = true;
            fillimage.enabled = true;
            disabledSpeakerImage.enabled = false;
            isOn = true;
            group.alpha = 1f;
            label.color = new Color(label.color.r, label.color.g, label.color.b, 1);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        volumeSlider.interactable = false;
        group.alpha = 0.5f;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        group.alpha = 1f;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        volumeSlider.interactable = false;
        group.alpha = 0.5f;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        group.alpha = 1f;
    }
}
