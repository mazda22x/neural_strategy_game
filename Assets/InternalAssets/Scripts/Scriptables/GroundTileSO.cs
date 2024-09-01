using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GroundTileSO", menuName = "ScriptableTiles/Ground")]
public class GroundTileSO : TileBaseSO
{
  [SerializeField] private TerrainTileData _data;
  public TerrainTileData TerrainData => _data;
}

[Serializable]
public struct TerrainTileData
{
  public TileCategoryEnum TileCategory;
}


[Serializable]
public enum TileCategoryEnum
{
  NONE,
  GRASS,
  DIRT,
}
