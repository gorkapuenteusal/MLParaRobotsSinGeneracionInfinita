using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapData 
{
  public readonly float[,] heightMap;
  public readonly Color[] colorMap;

  private MapData (float[,] heightMap, Color[] colorMap) {
    this.heightMap = heightMap;
    this.colorMap = colorMap;
  }

  public static MapData Generate(int size, int seed, float noiseScale, int octaves, float persistance, float lacunarity, float[,] edgeMap, TerrainType[] regions) {
    if (seed == -1)
      seed = new System.Random().Next(0, MapGenerator.maxSeed);

    float[,] noiseMap = Noise.GenerateNoiseMap(size, seed, noiseScale, octaves, persistance, lacunarity);
    Color[] colorMap = new Color[size * size];

    float currentHeight;
    for (int y = 0; y < size; y++)
    {
      for (int x = 0; x < size; x++)
      {
        noiseMap[x, y] = Mathf.Clamp01(edgeMap[x, y] - noiseMap[x, y]);
        currentHeight = noiseMap[x, y];
        for (int i = 0; i < regions.Length; i++)
        {
          if (currentHeight >= regions[i].height)
            colorMap[y * size + x] = regions[i].color;
          else break;
        }
      }
    }
    
    return new MapData(noiseMap, colorMap);
  }
}
