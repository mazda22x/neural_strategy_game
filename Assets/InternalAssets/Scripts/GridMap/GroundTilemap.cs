using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTilemap : BaseTilemap<GroundTile>
{
    [SerializeField] private List<GroundTileSO> groundTiles = new List<GroundTileSO>();
    [SerializeField] private int NoiseDensity = 50;
    [SerializeField] private int IterationsCount = 3;

    private Func<int, int, GroundTile> _groundTileCreator;


    public override void Initialize(GridManager gridManager)
    {
        base.Initialize(gridManager);

        _groundTileCreator = (x, y) =>
            new GroundTile(this, x, y, groundTiles[UnityEngine.Random.Range(1, 101) > NoiseDensity ? 1 : 0]);
        _grid = new BaseGrid<GroundTile>(_gridManager.GetComponent<Grid>(), _gridManager.GridArea, _groundTileCreator);
    }

    public override void GenerateMap()
    {
        for (int i = 0; i < IterationsCount; i++)
            IterateCellularAutomation();

        RenderGrid();
    }

    public void IterateCellularAutomation()
    {
        GroundTile[,] tempGrid = _grid.Clone();

        for (int x = 0; x < _gridManager.GridArea.size.x; x++)
        {
            for (int y = 0; y < _gridManager.GridArea.size.y; y++)
            {
                int neighborSandCount = 0;
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (_grid.IsWithinBounds(i, j))
                        {
                            if ((i != x || j != y) && _grid.GetGridObject(i, j).TileData.TileCategory == groundTiles[1].TerrainData.TileCategory)
                            {
                                neighborSandCount++;
                            }
                        }
                        else neighborSandCount++;
                    }
                }
                tempGrid[x, y] = new GroundTile(this, x, y, neighborSandCount > 4 ? groundTiles[1] : groundTiles[0]);

                // grid[x, y] = TerrainTiles[neighborSandCount > 4 ? DIRT : GRASS].Clone(new Vector2Int(x, y));
            }
        }
        _grid.SetGrid(tempGrid);
        // RenderGrid();
    }
}

public class GroundTile : IBaseGridObject
{
    private int _x, _y;

    private GroundTilemap _tilemap;

    private GroundTileSO _sObject;

    public TerrainTileData TileData => _sObject.TerrainData;

    public Tile Tile => _sObject.Tile;

    public GroundTile(GroundTilemap tilemap, int x, int y, GroundTileSO sObject)
    {
        _tilemap = tilemap;
        _x = x;
        _y = y;

        _sObject = sObject;
    }

    public GroundTile SetSObject(GroundTileSO sObject)
    {
        _sObject = sObject;

        _tilemap.TriggerGroundTileChanged(this, _x, _y);
        return this;
    }

    public override string ToString()
    {
        return TileData.TileCategory.ToString();
    }
}