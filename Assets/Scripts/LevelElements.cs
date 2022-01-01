using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Element", menuName = "ScriptableObjects/LevelElements")]
public class LevelElements : ScriptableObject
{
    public GameObject obstacle;
    public GameObject breakable;
    public GameObject food;
}
