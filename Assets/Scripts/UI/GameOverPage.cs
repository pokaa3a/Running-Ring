using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Text progressText;

    public void OnPointerDown(PointerEventData data)
    {
        GameAdmin.Instance.GamePrepare();
    }

    public void SetProgressText(float progress)
    {
        int progressInt = (int)Mathf.Round(progress * 100f);
        progressText.text = $"Complete {progressInt}%";
    }
}
