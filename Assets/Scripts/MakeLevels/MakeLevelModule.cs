using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeLevelModule : MonoBehaviour
{
    string jsonPath;

    void Awake()
    {
        jsonPath = Application.dataPath + "/LevelModuleJsons/module.json";
    }

    void Start()
    {
        // SaveToJson(MakeModule());
        InstantiateModule(LoadFromJson(jsonPath));
    }

    GameObject MakeSquare(ElementJsonObject element, GameObject parent)
    {
        GameObject square = new GameObject("Square");
        square.tag = "Obstacle";

        // Sprite Renderer
        SpriteRenderer sprRenderer = square.AddComponent<SpriteRenderer>();
        string assetPath = "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png";
        sprRenderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));

        // BoxCollider2D
        BoxCollider2D boxCollider = square.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        square.transform.SetParent(parent.transform);
        square.transform.localPosition = element.position;
        square.transform.localRotation = element.rotation;
        square.transform.localScale = element.scale;

        return square;
    }

    LevelModuleJsonObject MakeModule()
    {
        LevelModuleJsonObject jsonObj = new LevelModuleJsonObject();
        jsonObj.name = "module1";
        jsonObj.scale = Vector2.one * 0.7f;
        jsonObj.width = 3f;
        jsonObj.height = 1f;

        {
            ElementJsonObject element = new ElementJsonObject();
            element.name = "square";
            element.position = new Vector2(0, 0);
            element.scale = Vector2.one * 0.6f;
            jsonObj.elements.Add(element);
        }

        {
            ElementJsonObject element = new ElementJsonObject();
            element.name = "square";
            element.position = new Vector2(-1, 0);
            element.scale = Vector2.one * 0.6f;
            jsonObj.elements.Add(element);
        }

        {
            ElementJsonObject element = new ElementJsonObject();
            element.name = "square";
            element.position = new Vector2(-2, 0);
            element.scale = Vector2.one * 0.6f;
            jsonObj.elements.Add(element);
        }

        return jsonObj;
    }

    GameObject InstantiateModule(LevelModuleJsonObject jsonObj)
    {
        GameObject module = new GameObject("Module");

        foreach (ElementJsonObject element in jsonObj.elements)
        {
            if (element.name == "square")
            {
                GameObject square = MakeSquare(element, module);
            }
        }
        module.transform.localScale = jsonObj.scale;

        return module;
    }

    void SaveToJson(LevelModuleJsonObject module)
    {
        string json = JsonUtility.ToJson(module, true);
        System.IO.File.WriteAllText(jsonPath, json);
    }

    LevelModuleJsonObject LoadFromJson(string path)
    {
        string json = System.IO.File.ReadAllText(path);
        LevelModuleJsonObject jsonObj =
            (LevelModuleJsonObject)JsonUtility.FromJson(json, typeof(LevelModuleJsonObject));
        return jsonObj;
    }
}

