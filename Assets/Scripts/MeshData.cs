using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
  public Vector3[] vertices;
  public int[] triangles;
  public Vector2[] uvs;

  int triangleIdx;

  private MeshData(int size) {
    vertices = new Vector3[size * size];
    uvs = new Vector2[size * size];
    triangles = new int[(size * size - 2 * size + 1) * 6];
  }

  public static MeshData Generate(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve) {
    int size = heightMap.GetLength(0);
    float topLeftX = (size - 1) / -2f;
    float topLeftZ = (size - 1) / 2f;

    MeshData meshData = new MeshData(size);
    int vertexIdx = 0;

    for (int y = 0; y < size; y++)
    {
      for (int x = 0; x < size; x++)
      {
        meshData.vertices[vertexIdx] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
        meshData.uvs[vertexIdx] = new Vector2(x / (float)size, y / (float)size);
        if (x < size - 1 && y < size - 1) {
          meshData.AddTriangle(vertexIdx, vertexIdx + size + 1, vertexIdx + size);
          meshData.AddTriangle(vertexIdx + size + 1, vertexIdx, vertexIdx + 1);
        }

        vertexIdx++;
      }
    }
    
    return meshData;
  }

  public void AddTriangle(int a, int b, int c) {
    triangles[triangleIdx] = a;
    triangles[triangleIdx + 1] = b;
    triangles[triangleIdx + 2] = c;
    triangleIdx += 3;
  }

  public Mesh CreateMesh() {
    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    mesh.RecalculateNormals();
    return mesh;
  }
}
