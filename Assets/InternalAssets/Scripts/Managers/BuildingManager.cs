using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BuildingTileManager))]
public class BuildingManager : BaseManager
{
    public static BuildingManager Instance;
    [SerializeField] private List<BuildingSO> _buildingSOList = new List<BuildingSO>();
    [SerializeField] private BuildingPanel _buildingsPanel;
    [SerializeField] private BuildingTilemap _buildingTilemap;

    private bool _isActiveBuild = false;
    private Vector3Int _buildingPosition;


    public BuildingTile CastleTile => _buildingTilemap.CastleTile;

    public bool IsActiveBuild { get; }

    private void Awake()
    {

        if (Instance != null)
            Destroy(this);

        Instance = this;

        _isActiveBuild = false;
    }


    private void SetBuildingPanel(bool isShow)
    {
        if (isShow)
        {
            List<BuildingPanel.BuildingItemConfig> list = new List<BuildingPanel.BuildingItemConfig>();
            var curTile = _gameManager.GetTileInfo(_buildingPosition.x, _buildingPosition.y);
            _buildingSOList.ForEach(building =>
            {
                if (building.IsValidCell(curTile))
                {
                    bool isAllowed = building.IsValidCost(_gameManager.GetResources());
                    list.Add(new BuildingPanel.BuildingItemConfig()
                    {
                        BuildingSO = building,
                        IsAllowed = isAllowed,
                        onClick = isAllowed ? (building) => PlaceBuilding(building) : null,
                    });
                }
            });

            _buildingsPanel.ShowPanel(list);
        }
        else _buildingsPanel.HidePanel();
    }
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        _buildingTilemap.Initialize(FindObjectOfType<GridManager>(), this);
        _buildingsPanel.HidePanel();

        
    }

    public void GenerateMap()
    {
        _buildingTilemap.GenerateMap();
    }

    public void StartBuilding(Vector3Int pos)
    {
        _buildingPosition = pos;

        if (!_isActiveBuild)
        {
            SetBuildingPanel(true);
        }

        _isActiveBuild = true;
    }
    public void StopBuilding()
    {
        _buildingPosition = default;

        if (_isActiveBuild)
        {
            SetBuildingPanel(false);
        }
        _isActiveBuild = false;
    }

    public BuildingSO GetBuildingSO(BuildingTypeEnum type)
    {
        return _buildingSOList.FirstOrDefault(x => x.BuildingType == type);
    }

    public void PlaceBuilding(BuildingSO building)
    {
        if (_isActiveBuild)
        {
            ResourceManager.Instance.ChangeWallet(!building.BuildingData.Cost);
            PlaceBuilding(building.BuildingType);

            GameManager.Instance.EndTurn();
        }
    }

    public void PlaceBuilding(BuildingTypeEnum buildingType)
    {
        if (_isActiveBuild)
        {
            _buildingTilemap.PlaceBuilding(buildingType, _buildingPosition.x, _buildingPosition.y);
            StopBuilding();
        }
    }

    public void PlaceBuilding(int buildingType)
    {
        PlaceBuilding((BuildingTypeEnum)buildingType);
    }

    public List<BuildingSO> GetBuildingSOList()
    {
        return _buildingTilemap.GetBuildingTiles().Select(tile => tile.BuildingSO).ToList();
    }
}
