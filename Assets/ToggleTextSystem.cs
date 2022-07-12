using UnityEngine;
using UnityEngine.UI;

public class ToggleTextSystem : MonoBehaviour
{
    [SerializeField] private Toggle[] toggles;

    [SerializeField] private Text[] blackToggleIndexTexts;

    private void Start()
    {
        for (int i = 0; i < blackToggleIndexTexts.Length; i++)
        {
            blackToggleIndexTexts[i].enabled = false;
        }
    }

    public void OnToggled(int toggleIndex)
    {
        if (toggles[toggleIndex].isOn)
        {
            blackToggleIndexTexts[toggleIndex].enabled = true;
        }
        else
        {
            blackToggleIndexTexts[toggleIndex].enabled = false;
        }
    }
}
