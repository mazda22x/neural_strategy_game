using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using YeLazzers;

[RequireComponent(typeof(ResourceTileManager))]
public class ResourceTilemap : BaseTilemap<ResourceTile>
{

    [SerializeField] private List<ResourceGeneratorConfig> ResourceConfigs = new List<ResourceGeneratorConfig>();

    private ResourceTileManager _resManager;


    public override void Initialize(GridManager gridManager)
    {
        base.Initialize(gridManager);


        _resManager = GetComponent<ResourceTileManager>();
        _grid = new BaseGrid<ResourceTile>(_gridManager.GetComponent<Grid>(), _gridManager.GridArea);
    }


    public override void GenerateMap()
    {
        ResourceConfigs.ForEach(config =>
        {
            for (int i = 0; i < config.Count; i++) FillResource(config.Type, config.Decay);
        });
        RenderGrid();
    }

    public void FillResource(TileTypeEnum type, float decayFactor)
    {
        float chance = 100f;
        int filled = 1, visited = -1;

        var gridSize = _gridManager.GridArea.size;

        Vector2Int randomCell = new Vector2Int(UnityEngine.Random.Range(0, _gridManager.GridArea.size.x),
                                                UnityEngine.Random.Range(0, _gridManager.GridArea.size.y));

        GroundTile groundTile = _gridManager.GetTileInfo(randomCell.x, randomCell.y).GroundTile;

        var resourceSO = _resManager.GetResourceTile(groundTile.TileData.TileCategory, type);
        if (resourceSO == null)
            Debug.Log($"Failed to Load Resource: [{groundTile.TileData.TileCategory} | {type}]");

        ResourceTile rt = new ResourceTile(this, randomCell.x, randomCell.y, resourceSO, TileSizeEnum.XL);

        int[,] infoGrid = new int[gridSize.x, gridSize.y];

        List<ResourceTile> deque = new List<ResourceTile> { rt };

        while (deque.Count > 0)
        {
            ResourceTile _rt = deque.First();
            Vector3Int _pos = _rt.Position;
            deque.RemoveAt(0);

            infoGrid[_pos.x, _pos.y] = filled;
            _grid.AddGridObject(_pos.x, _pos.y, _rt);

            if (chance >= UnityEngine.Random.Range(0, 101))
            {
                Utils.Neighbors(_pos).ForEach(nPos =>
                {
                    // Ливаем, если сосед отработан или не совпадают тайлы
                    if (!_grid.IsWithinBounds(nPos.x, nPos.y)
                        || infoGrid[nPos.x, nPos.y] != 0
                        || _gridManager.GetTileInfo(nPos.x, nPos.y).GroundTile.TileData.TileCategory != _rt.TTileData.TileCategory
                    ) return;

                    var nrt = new ResourceTile(this, nPos.x, nPos.y, _resManager.GetResourceTile(_rt.TTileData.TileCategory, type), scaleSize(chance / 100f, _rt.RTileData.TileMaxSize));
                    deque.Add(nrt);
                    infoGrid[nPos.x, nPos.y] = visited;
                    _grid.AddGridObject(nPos.x, nPos.y, nrt);
                });
            }

            chance *= decayFactor;
        }
    }
    private TileSizeEnum scaleSize(float percent, TileSizeEnum maxSize)
    {

        switch ((int)Math.Round(Convert.ToInt32(maxSize) * percent))
        {
            case 1: default: return TileSizeEnum.S;
            case 2: return TileSizeEnum.M;
            case 3: return TileSizeEnum.L;
            case 4: return TileSizeEnum.XL;
        }

    }


    [Serializable]
    private struct ResourceGeneratorConfig
    {
        public TileTypeEnum Type;
        public int Count;
        public float Decay;
    }

}
public class ResourceTile : IBaseGridObject
{
    private int _x, _y;

    public Vector3Int Position => new Vector3Int(_x, _y);

    private ResourceTilemap _tilemap;

    private ResourceTileSO _sObject;

    public TerrainTileData TTileData => _sObject.TerrainData;
    public ResourceTileData RTileData => _sObject.ResourceData;


    private TileSizeEnum _tileSize = TileSizeEnum.S;
    public TileSizeEnum TileSize
    {
        get => _tileSize;
        private set => Mathf.Clamp(Convert.ToInt32(value), 1, Convert.ToInt32(_sObject.ResourceData.TileMaxSize));
    }

    public Tile Tile => RTileData.Tiles[Convert.ToInt32(_tileSize)];

    public ResourceTile(ResourceTilemap tilemap, ResourceTileSO sObject, TileSizeEnum tileSize = TileSizeEnum.S)
    {
        _tilemap = tilemap;
        _sObject = sObject;

        TileSize = tileSize;
    }

    public ResourceTile(ResourceTilemap tilemap, int x, int y, ResourceTileSO sObject, TileSizeEnum tileSize = TileSizeEnum.S) : this(tilemap, sObject, tileSize)
    {
        _x = x;
        _y = y;
    }

    public ResourceTile SetPosition(int x, int y)
    {
        _x = x;
        _y = y;

        _tilemap.TriggerGroundTileChanged(this, _x, _y);

        return this;
    }

    public ResourceTile SetSObject(ResourceTileSO sObject)
    {
        _sObject = sObject;

        _tilemap.TriggerGroundTileChanged(this, _x, _y);
        return this;
    }

    public override string ToString()
    {
        return Tile.ToString();
    }
}

