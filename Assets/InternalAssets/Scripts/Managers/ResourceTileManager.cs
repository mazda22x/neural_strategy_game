using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ResourceTileManager : MonoBehaviour
{

    [SerializeField] private List<ResourceTileSO> srTiles;

    public ResourceTileSO GetResourceTile(TileCategoryEnum category)
    {
        TileTypeEnum type;

        switch (Random.Range(0, 3))
        {
            case 0: default: type = TileTypeEnum.WOOD; break;
            case 1: type = TileTypeEnum.STONE; break;
            case 2: type = TileTypeEnum.COPPER; break;
        }
        return srTiles.Where((tile) => tile.TerrainData.TileCategory == category && tile.ResourceData.TileType == type).FirstOrDefault();
    }

    public ResourceTileSO GetResourceTile(TileCategoryEnum category, TileTypeEnum type)
    {
        return srTiles.Where((tile) => tile.TerrainData.TileCategory == category && tile.ResourceData.TileType == type).FirstOrDefault();
    }

    public TileBase GetResourceTile(TileCategoryEnum category, TileTypeEnum type, TileSizeEnum size)
    {
        return srTiles.Where((tile) => tile.TerrainData.TileCategory == category && tile.ResourceData.TileType == type).FirstOrDefault().GetTile(size);
    }
}
