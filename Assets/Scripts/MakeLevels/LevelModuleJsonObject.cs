using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModuleJsonObject
{
    public string name = "";
    public Vector2 scale = Vector2.one;
    public float height = 0;
    public float width = 0;
    public List<ElementJsonObject> elements = new List<ElementJsonObject>();
}

[System.Serializable]
public class ElementJsonObject
{
    public string name = "";
    public Vector2 position = Vector2.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector2 scale = Vector2.one;
}
