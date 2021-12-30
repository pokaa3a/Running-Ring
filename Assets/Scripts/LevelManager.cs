using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<LevelModule> modules;

    [SerializeField]
    private GameObject movingRingObject;
    private MovingRing movingRing;

    void Awake()
    {
        movingRing = movingRingObject.GetComponent<MovingRing>();
    }

    // *** Temporary Solution ***
    public GameObject MakeLevel()
    {
        int numModules = 5;
        float gap = 2f;     // gap between each two modules
        float startingY = 6f;

        GameObject root = new GameObject("Level");
        root.transform.SetParent(gameObject.transform);

        for (int i = 0; i < numModules; ++i)
        {
            GameObject module = (GameObject)Instantiate(modules[0].prefab);

            module.transform.SetParent(root.transform);
            module.transform.localPosition = new Vector2(0, startingY + gap * i);
        }

        float finishLineHeight = 15f;
        DrawFinishLine(finishLineHeight, root);
        movingRing.finishHeight = finishLineHeight + 3f;

        return root;
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
}
