using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadButton : MonoBehaviour, IPointerDownHandler
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
        Debug.Log("Load module from file!");
        levelMaker.LoadFromJson();
    }
}
