using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModuleJsonObject
{
    public string name = "";
    public float left = 0;
    public float right = 0;
    public float top = 0;
    public float bottom = 0;
    public List<ElementJsonObject> elements = new List<ElementJsonObject>();
}

[System.Serializable]
public class ElementJsonObject
{
    public string name = "";
    public Vector2 position = Vector2.zero;
    public Quaternion rotation = Quaternion.identity;
}
