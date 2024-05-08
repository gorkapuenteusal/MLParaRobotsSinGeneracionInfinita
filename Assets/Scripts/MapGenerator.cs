using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
  public const int maxSeed = int.MaxValue;

  [Range(100, 500)]
  public int mapSize = 100;
  [Range(10, 500)]
  public float noiseScale = 10;
  [Range(1, 25)]
  public int octaves = 1;
  [Range(0, 1)]
  public float persistance = 1;
  [Range(1, 3)]
  public float lacunarity = 1;

  public int seed = -1; 

  public bool autoUpdate;

  [Range (1, 20)]
  public float heightMultiplier = 1;
  public AnimationCurve heightCurve;

  public TerrainType[] regions;

  private float[,] edgeMap;

  void Awake() {
    edgeMap = EdgeGenerator.GenerateEdgeMap(mapSize);
  }

  public void DrawMapInEditor() {
    MapData mapData = MapData.Generate(mapSize, seed, noiseScale, octaves, persistance, lacunarity, edgeMap, regions);
    MapDisplay display = FindObjectOfType<MapDisplay>();
    display.DrawMesh(MeshData.Generate(mapData.heightMap, heightMultiplier, heightCurve), TextureGenerator.FromColorMap(mapData.colorMap, mapSize));
  }

  void OnValidate() {
    if (seed < 0)
      seed = -1;
    edgeMap = EdgeGenerator.GenerateEdgeMap(mapSize);
  }
}
