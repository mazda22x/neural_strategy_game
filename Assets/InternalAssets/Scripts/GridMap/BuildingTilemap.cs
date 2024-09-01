using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingTilemap : BaseTilemap<BuildingTile>
{
    private BuildingManager _buildingManager;

    private BuildingTile _castleTile;
    public BuildingTile CastleTile => _castleTile;

    public void Initialize(GridManager gridManager, BuildingManager buildingManager)
    {
        base.Initialize(gridManager);

        _buildingManager = buildingManager;
        _grid = new BaseGrid<BuildingTile>(_gridManager.GetComponent<Grid>(), _gridManager.GridArea);
    }

    public override void GenerateMap()
    {
        var cellPosition = _gridManager.GetRandomCell();
        _castleTile = PlaceBuilding(BuildingTypeEnum.CASTLE, cellPosition.x, cellPosition.y);
    }

    public BuildingTile PlaceBuilding(BuildingTypeEnum buildingType, int x, int y)
    {
        GridManager.TileInfo tileInfo = _gridManager.GetTileInfo(x, y);
        if (tileInfo.BuildingTile != null)
            return tileInfo.BuildingTile;

        BuildingTile building = new BuildingTile(this, x, y, _buildingManager.GetBuildingSO(buildingType));
        _grid.AddGridObject(x, y, building);
        _tilemap.SetTile(new Vector3Int(x, y), building.Tile);

        return building;
    }

    public List<BuildingTile> GetBuildingTiles()
    {
        List<BuildingTile> tiles = new();

        foreach (var item in _grid.GetSimplifiedGrid())
        {
            if (item != null)
            {
                tiles.Add(item);
            }
        }
        return tiles;
    }
}

public class BuildingTile : IBaseGridObject
{
    private int _x, _y;
    public Vector3Int Position => new Vector3Int(_x, _y);

    private BuildingTilemap _tilemap;

    private BuildingSO _buildingSO;

    public BuildingSO BuildingSO => _buildingSO;

    public Tile Tile => _buildingSO.Tile;



    public BuildingTile(BuildingTilemap tilemap, int x, int y, BuildingSO buildingBindConfig)
    {
        _tilemap = tilemap;
        _buildingSO = buildingBindConfig;
        _x = x;
        _y = y;

    }
}

