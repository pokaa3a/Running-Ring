using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlignButton : MonoBehaviour, IPointerDownHandler
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
        Debug.Log("Align!");
        levelMaker.AlignObjects();
    }
}
