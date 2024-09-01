using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ResourceTile", menuName = "ScriptableTiles/Resource")]
public class ResourceTileSO : GroundTileSO
{
  [SerializeField] private ResourceTileData _resData;
  public ResourceTileData ResourceData => _resData;


  public Tile GetTile(TileSizeEnum size)
  {
    return _resData.Tiles[Mathf.Clamp(Convert.ToInt32(size), 0, Convert.ToInt32(_resData.TileMaxSize) - 1)];
  }
}

[Serializable]
public struct ResourceTileData
{
  public TileTypeEnum TileType;
  public TileSizeEnum TileMaxSize => (TileSizeEnum)(Tiles.Count - 1);

  public List<Tile> Tiles;
}


[Serializable]
public enum TileTypeEnum
{
  NONE,
  WOOD,
  STONE,
  COPPER,
}

[Serializable]
public enum TileSizeEnum
{
  S = 0,
  M = 1,
  L = 2,
  XL = 3,
}

