using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeUtils
{
    const float horizontalSize = 9f;
    // iPhone 12 screen size
    public const float nominalScreenWidth = 1170f;
    public const float nominalScreenHeight = 2532f;

    public static float HorizontalScale()
    {
        float camVerticalSize = Camera.main.orthographicSize * 2f;
        float camHorizontalSize = camVerticalSize * Camera.main.aspect;
        return camHorizontalSize / horizontalSize;
    }
}
