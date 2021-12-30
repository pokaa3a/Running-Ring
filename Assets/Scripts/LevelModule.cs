using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Module", menuName = "ScriptableObjects/LevelModule")]
public class LevelModule : ScriptableObject
{
    public int id;
    public float width;
    public float height;
    public GameObject prefab;

    [TextArea]
    public string description;
}
