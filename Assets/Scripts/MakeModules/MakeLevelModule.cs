using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeLevelModule : MonoBehaviour
{
    [SerializeField]
    GameObject moduleObject;

    [SerializeField]
    private LevelElements levelElements;

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
        const int h = 50;

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

    // This method is almost a duplicate of LevelManager.InstantiateModuleFromFile()
    public void LoadFromJson()
    {
        string jsonStr = System.IO.File.ReadAllText(jsonPath);
        LevelModuleJsonObject jsonObj =
            (LevelModuleJsonObject)JsonUtility.FromJson(jsonStr, typeof(LevelModuleJsonObject));

        foreach (ElementJsonObject elementJsonObj in jsonObj.elements)
        {
            GameObject elementPrefab = null;
            GameObject elementObject = null;

            if (elementJsonObj.name == "square")
            {
                elementPrefab = levelElements.square;
            }
            else if (elementJsonObj.name == "food")
            {
                elementPrefab = levelElements.food;
            }
            else if (elementJsonObj.name == "breakable")
            {
                elementPrefab = levelElements.breakable;
            }
            elementObject = (GameObject)Instantiate(elementPrefab);
            elementObject.transform.SetParent(moduleObject.transform);
            elementObject.transform.localPosition = elementJsonObj.position;
            elementObject.transform.localRotation = elementJsonObj.rotation;
        }
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
            if (child.gameObject.name.Contains("Square"))
            {
                elementJsonObj.name = "square";
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
