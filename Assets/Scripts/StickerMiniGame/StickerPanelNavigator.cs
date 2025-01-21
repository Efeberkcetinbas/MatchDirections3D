using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerPanelNavigator : MonoBehaviour
{
    public List<GameObject> Panels;
    private int currentPanelIndex = 0;

    public void ShowPanel(int index)
    {
        for (int i = 0; i < Panels.Count; i++)
        {
            Panels[i].SetActive(i == index);
        }
    }

    public void OnLeftArrowClicked()
    {
        currentPanelIndex = (currentPanelIndex - 1 + Panels.Count) % Panels.Count;
        ShowPanel(currentPanelIndex);
    }

    public void OnRightArrowClicked()
    {
        currentPanelIndex = (currentPanelIndex + 1) % Panels.Count;
        ShowPanel(currentPanelIndex);
    }
}
