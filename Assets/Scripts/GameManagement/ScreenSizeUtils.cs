using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeUtils
{
    public const float horizontalSize = 9f;
    // iPhone 12 screen size
    public const float nominalScreenWidth = 1170f;
    public const float nominalScreenHeight = 2532f;

    // Compute scaling factor where horizontal length has 9 units
    public static float ScaleBasedOnWidth()
    {
        float camVerticalSize = Camera.main.orthographicSize * 2f;
        float camHorizontalSize = camVerticalSize * Camera.main.aspect;
        return camHorizontalSize / horizontalSize;
    }
}
