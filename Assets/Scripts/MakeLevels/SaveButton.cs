using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    GameObject levelMakerObject;
    MakeLevelModule levelMaker;

    void Awake()
    {
        levelMaker = levelMakerObject.GetComponent<MakeLevelModule>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Save to file!");
        levelMaker.SaveToJson();
    }
}
