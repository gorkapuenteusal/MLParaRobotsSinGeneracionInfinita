using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le da los componentes necesarios a la malla generada
public class MapDisplay : MonoBehaviour
{
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  public MeshCollider meshCollider;

  public void DrawMesh(MeshData meshData, Texture2D texture) {
    Mesh mesh = meshData.CreateMesh();
    
    meshFilter.sharedMesh = mesh; 
    meshCollider.sharedMesh = mesh;

    meshRenderer.sharedMaterial.mainTexture = texture;
  }
}
