using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private PlayerManager _playerManager;

    public event UnityAction OnTurnEnd;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;


        _gridManager.Initialize(this);
        _resourceManager.Initialize(this);
        _buildingManager.Initialize(this);


        _gridManager.GenerateMap();
        _buildingManager.GenerateMap();


        _playerManager.Initialize(this, _buildingManager.CastleTile.Position);
    }

    public GridManager.TileInfo GetTileInfo(int x, int y)
    {
        return _gridManager.GetTileInfo(x, y);
    }

    public BuildingData.ResourceStruct GetResources()
    {
        return _resourceManager.GetResources();
    }

    public void EndTurn()
    {
        _buildingManager.GetBuildingSOList().ForEach(building =>
        {
            if (ResourceManager.Instance.CheckCost(building.BuildingData.Income))
            {
                ResourceManager.Instance.ChangeWallet(building.BuildingData.Income);
            }
        });

        

        OnTurnEnd?.Invoke();
    }
}
