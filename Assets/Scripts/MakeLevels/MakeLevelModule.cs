using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeLevelModule : MonoBehaviour
{
    [SerializeField]
    GameObject moduleObject;

    [TextArea]
    public string filename = "";

    string jsonPath;

    void Awake()
    {
        if (filename == "") filename = "new_module.json";
        jsonPath = Application.dataPath + "/LevelModuleJsons/" + filename;
    }

    void Start()
    {
        DrawGridLines();
    }

    void DrawGridLines()
    {
        const int w = 10;
        const int h = 10;

        // vertical lines
        for (int x = -w; x <= w; ++x)
        {
            GameObject line = new GameObject("line");
            line.transform.SetParent(gameObject.transform);

            LineRenderer lineRenderer = line.AddComponent<LineRenderer>() as LineRenderer;
            Vector3[] points = new Vector3[2];
            points[0] = new Vector3(x, -h, 0);
            points[1] = new Vector3(x, h, 0);

            lineRenderer.widthMultiplier = 0.05f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(points);
        }

        // horizontal lines
        for (int y = -h; y < h; ++y)
        {
            GameObject line = new GameObject("line");
            line.transform.SetParent(gameObject.transform);

            LineRenderer lineRenderer = line.AddComponent<LineRenderer>() as LineRenderer;
            Vector3[] points = new Vector3[2];
            points[0] = new Vector3(-w, y, 0);
            points[1] = new Vector3(w, y, 0);

            lineRenderer.widthMultiplier = 0.05f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(points);
        }
    }

    // GameObject MakeSquare(ElementJsonObject element, GameObject parent)
    // {
    //     GameObject square = new GameObject("Square");
    //     square.tag = "Obstacle";

    //     // Sprite Renderer
    //     SpriteRenderer sprRenderer = square.AddComponent<SpriteRenderer>();
    //     string assetPath = "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png";
    //     sprRenderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));

    //     // BoxCollider2D
    //     BoxCollider2D boxCollider = square.AddComponent<BoxCollider2D>();
    //     boxCollider.isTrigger = true;

    //     square.transform.SetParent(parent.transform);
    //     square.transform.localPosition = element.position;
    //     square.transform.localRotation = element.rotation;
    //     square.transform.localScale = element.scale;

    //     return square;
    // }

    // LevelModuleJsonObject MakeModule()
    // {
    //     LevelModuleJsonObject jsonObj = new LevelModuleJsonObject();
    //     jsonObj.name = "module1";
    //     jsonObj.scale = Vector2.one * 0.7f;
    //     jsonObj.width = 3f;
    //     jsonObj.height = 1f;

    //     {
    //         ElementJsonObject element = new ElementJsonObject();
    //         element.name = "square";
    //         element.position = new Vector2(0, 0);
    //         element.scale = Vector2.one * 0.6f;
    //         jsonObj.elements.Add(element);
    //     }

    //     {
    //         ElementJsonObject element = new ElementJsonObject();
    //         element.name = "square";
    //         element.position = new Vector2(-1, 0);
    //         element.scale = Vector2.one * 0.6f;
    //         jsonObj.elements.Add(element);
    //     }

    //     {
    //         ElementJsonObject element = new ElementJsonObject();
    //         element.name = "square";
    //         element.position = new Vector2(-2, 0);
    //         element.scale = Vector2.one * 0.6f;
    //         jsonObj.elements.Add(element);
    //     }

    //     return jsonObj;
    // }

    GameObject InstantiateModule(LevelModuleJsonObject jsonObj)
    {
        GameObject module = new GameObject("Module");

        foreach (ElementJsonObject element in jsonObj.elements)
        {
            if (element.name == "square")
            {
                // GameObject square = MakeSquare(element, module);
            }
        }
        // module.transform.localScale = jsonObj.scale;

        return module;
    }

    LevelModuleJsonObject LoadFromJson(string path)
    {
        string json = System.IO.File.ReadAllText(path);
        LevelModuleJsonObject jsonObj =
            (LevelModuleJsonObject)JsonUtility.FromJson(json, typeof(LevelModuleJsonObject));
        return jsonObj;
    }

    public void AlignObjects()
    {
        foreach (Transform child in moduleObject.transform)
        {
            child.transform.position = new Vector3(
                Mathf.Round(child.transform.position.x),
                Mathf.Round(child.transform.position.y),
                child.transform.position.z
            );
        }
    }

    public void SaveToJson()
    {
        AlignObjects();

        LevelModuleJsonObject moduleJsonObj = new LevelModuleJsonObject();

        // Compute height and width
        float left = 10f;
        float right = -10f;
        float top = -10f;
        float bottom = 10f;

        foreach (Transform child in moduleObject.transform)
        {
            left = Mathf.Min(left, child.transform.position.x);
            right = Mathf.Max(right, child.transform.position.x);
            bottom = Mathf.Min(bottom, child.transform.position.y);
            top = Mathf.Max(top, child.transform.position.y);

            ElementJsonObject elementJsonObj = new ElementJsonObject();
            if (child.gameObject.name.Contains("Obstacle"))
            {
                elementJsonObj.name = "obstacle";
            }
            else if (child.gameObject.name.Contains("Food"))
            {
                elementJsonObj.name = "food";
            }
            else if (child.gameObject.name.Contains("Breakable"))
            {
                elementJsonObj.name = "breakable";
            }
            elementJsonObj.position = (Vector2)child.transform.position;
            moduleJsonObj.elements.Add(elementJsonObj);
        }

        moduleJsonObj.name = "Module";
        moduleJsonObj.left = left;
        moduleJsonObj.right = right;
        moduleJsonObj.top = top;
        moduleJsonObj.bottom = bottom;

        string json = JsonUtility.ToJson(moduleJsonObj, true);
        System.IO.File.WriteAllText(jsonPath, json);
    }

}
