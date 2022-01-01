using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private TextMesh textMesh;

    public int _number = 1;
    public int number
    {
        get => _number;
        set
        {
            _number = value;
            textMesh.text = $"{_number}";
        }
    }
}
