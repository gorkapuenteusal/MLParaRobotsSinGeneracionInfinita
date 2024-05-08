using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator 
{
  public static Texture2D FromColorMap(Color[] colorMap, int size) {
    Texture2D texture = new Texture2D(size, size);
    texture.filterMode = FilterMode.Point;
    texture.wrapMode = TextureWrapMode.Clamp;
    texture.SetPixels(colorMap);
    texture.Apply();

    return texture;
  }
}
