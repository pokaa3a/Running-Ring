using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public class Config
    {
        public const float radius = 1.5f;
        public const float lineWidthMultiplier = 0.02f;
        public const int numPositions = 100;
        public const float cx = 0;
        public const float cy = -2.5f;
    }

    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawRingLine();
    }

    void DrawRingLine()
    {
        Vector3[] positions = new Vector3[Config.numPositions];

        for (int i = 0; i < Config.numPositions; ++i)
        {
            float radian = Mathf.Deg2Rad * (i * 360f / Config.numPositions);
            Vector3 pos = new Vector3(
                Config.cx + Mathf.Sin(radian) * Config.radius,
                Config.cy + Mathf.Cos(radian) * Config.radius,
                0);
            positions[i] = pos;
        }
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = Config.lineWidthMultiplier;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
