using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledTexture : MonoBehaviour
{
  public Material tiledMaterial;
  public Vector2 tiling;

  void Start()
  {
    Material mat = new Material(tiledMaterial);
    GetComponent<MeshRenderer>().material = mat;
    Vector3 scale = transform.localScale;

    scale.x *= tiling.x;
    scale.y *= tiling.y;

    mat.mainTextureScale = scale;
  }
}
