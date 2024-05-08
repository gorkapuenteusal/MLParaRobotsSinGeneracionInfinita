using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
  public static float[,] GenerateNoiseMap(int size, int seed, float scale, int octaves, float persistance, float lacunarity) {
    float[,] noiseMap = new float[size, size];
    
    System.Random prng = new System.Random(seed);
    Vector2[] octaveOffsets = new Vector2[octaves];

    float offsetX, offsetY;
    for (int i = 0; i < octaves; i++)
    {
        offsetX = prng.Next(-100000, 100000);
        offsetY = prng.Next(-100000, 100000);
        octaveOffsets[i] = new Vector2(offsetX, offsetY);
    }

    float maxHeight = float.MinValue;
    float minHeight = float.MaxValue;

    float amplitude, frequency, height;
    for (int y = 0; y < size; y++)
    {
      for (int x = 0; x < size; x++)
      {
          amplitude = 1;
          frequency = 1;
          height = 0;

          for (int i = 0; i < octaves; i++)
          {
              float sampleX = (x - size / 2f + octaveOffsets[i].x) / scale * frequency;
              float sampleY = (y - size / 2f + octaveOffsets[i].y) / scale * frequency;

              float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
              height += perlinValue * amplitude;

              amplitude *= persistance;
              frequency *= lacunarity;
          }

          maxHeight = (height > maxHeight) ? height : maxHeight;
          minHeight = (height < minHeight) ? height : minHeight;

          noiseMap[x, y] = height;
      }
    }

    for (int y = 0; y < size; y++)
    {
      for (int x = 0; x < size; x++)
      {
        noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
      }
    }

    return noiseMap;
  }
}
