using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public class Config
    {
        /*
            ---- Ring finish line ----   ┐
                                         │
                                 {afterGoalHeight}
                                         │
            ======== Goal line =======   ┘   ┐
                                             │
                                {lastModuleToGoalHeight}
                                             │
                ┌──────────────┐             ┘   ┐
                │   Module j   │                 │
                └──────────────┘     ┐           │
                                     │           │
                                {moduleGap}      │
                                     │     {modulesHeight}
                ┌──────────────┐     ┘           │
                │   Module i   │                 │
                └──────────────┘       ┐         ┘
                                       │
                             {levelStartHeight}
                                       │
            --------- y == 0 --------- ┘

            └─────────────────────────┘
        {ScreenSizeUtils.horizontalSize == 9}
        */
        public static float modulesHeight = 90f;
        public static float moduleGap = Mathf.Ceil(ScreenSizeUtils.horizontalSize / 2f);

        public static float levelStartHeight = Camera.main.orthographicSize + 1f;
        public static float lastModuleToGoalHeight =
            Mathf.Ceil(ScreenSizeUtils.horizontalSize / 2f);
        public static float afterGoalHeight = 0f;

        public static string jsonDir = Application.dataPath + "/LevelModuleJsons/";
        // public const float levelStartY = 6f;
    }

    public class ModuleDimension
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    [SerializeField]
    private LevelElements levelElements;

    [SerializeField]
    private GameObject movingRingObject;
    private MovingRing movingRing;

    private List<string> moduleJsons = new List<string>();
    private GameObject levelObject = null;

    void Awake()
    {
        movingRing = movingRingObject.GetComponent<MovingRing>();

        string[] allJsons = Directory.GetFiles(Config.jsonDir);
        foreach (string jsonFile in allJsons)
        {
            if (Path.GetExtension(jsonFile).Equals(".json"))
            {
                moduleJsons.Add(jsonFile);
            }
        }

        Config.modulesHeight *= ScreenSizeUtils.ScaleBasedOnWidth();
        Config.moduleGap *= ScreenSizeUtils.ScaleBasedOnWidth();
        Config.lastModuleToGoalHeight *= ScreenSizeUtils.ScaleBasedOnWidth();
        Config.afterGoalHeight *= ScreenSizeUtils.ScaleBasedOnWidth();
    }

    void DrawFinishLine(float height, GameObject parent)
    {
        GameObject finishLine = new GameObject("FinishLine");
        SpriteRenderer sprRenderer = finishLine.AddComponent<SpriteRenderer>();

        string assetPath = "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/Square.png";
        sprRenderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));

        finishLine.transform.SetParent(parent.transform);
        finishLine.transform.localPosition = new Vector2(0, height);
        finishLine.transform.localScale = new Vector2(10f, 0.1f);
    }

    GameObject InstantiateModuleFromFile(string jsonPath, ModuleDimension dimensionInOut)
    {
        string jsonStr = System.IO.File.ReadAllText(jsonPath);
        LevelModuleJsonObject jsonObj =
            (LevelModuleJsonObject)JsonUtility.FromJson(jsonStr, typeof(LevelModuleJsonObject));

        GameObject module = new GameObject("Module");
        foreach (ElementJsonObject elementJsonObj in jsonObj.elements)
        {
            GameObject elementPrefab = null;
            GameObject elementObject = null;
            // TODO: add default prefab to elementPrefab 
            if (elementJsonObj.name == "square")
            {
                elementPrefab = levelElements.square;
                elementObject = (GameObject)Instantiate(elementPrefab);
            }
            else if (elementJsonObj.name == "food")
            {
                elementPrefab = levelElements.food;
                elementObject = (GameObject)Instantiate(elementPrefab);

                Food food = elementObject.GetComponent<Food>();
                food.number = Random.Range(1, 10);
            }
            else if (elementJsonObj.name == "breakable")
            {
                elementPrefab = levelElements.breakable;
                elementObject = (GameObject)Instantiate(elementPrefab);
            }
            elementObject.transform.SetParent(module.transform);
            elementObject.transform.localPosition = elementJsonObj.position;
            elementObject.transform.localRotation = elementJsonObj.rotation;
        }

        dimensionInOut.left = jsonObj.left;
        dimensionInOut.right = jsonObj.right;
        dimensionInOut.top = jsonObj.top;
        dimensionInOut.bottom = jsonObj.bottom;

        return module;
    }

    public GameObject MakeLevel()
    {
        levelObject = new GameObject("Level");
        levelObject.transform.SetParent(gameObject.transform);
        levelObject.transform.localPosition = new Vector2(0, Config.levelStartHeight);

        float newModuleY = 0;
        while (newModuleY < Config.modulesHeight)
        {
            int idx = Random.Range(0, moduleJsons.Count);
            ModuleDimension dimension = new ModuleDimension();
            GameObject newModule = InstantiateModuleFromFile(moduleJsons[idx], dimension);
            newModule.transform.SetParent(levelObject.transform);
            newModule.transform.localScale = new Vector3(
                ScreenSizeUtils.ScaleBasedOnWidth(),
                ScreenSizeUtils.ScaleBasedOnWidth(),
                1f);

            // TODO: also change X position?
            newModule.transform.localPosition = new Vector2(0, newModuleY);
            newModuleY +=
                (dimension.top - dimension.bottom) * ScreenSizeUtils.ScaleBasedOnWidth() +
                    Config.moduleGap;
        }

        DrawFinishLine(Config.modulesHeight + Config.lastModuleToGoalHeight, levelObject);
        movingRing.finishHeight = Config.levelStartHeight + Config.modulesHeight +
            Config.lastModuleToGoalHeight + Config.afterGoalHeight;

        return levelObject;
    }

    public void DestroyLevel()
    {
        if (levelObject != null) Destroy(levelObject);
    }
}
