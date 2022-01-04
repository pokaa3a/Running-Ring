using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameCompletePage : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Text levelText;

    public void OnPointerDown(PointerEventData data)
    {
        GameAdmin.Instance.GamePrepare();
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"Level {level} passed!";
    }
}
