using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingTileManager : MonoBehaviour
{
    [SerializeField] private List<BuildingSO> _buildingTiles;

    public BuildingSO GetBuildingTile(BuildingTypeEnum buildingType)
    {
        return _buildingTiles.Where((building) => building.BuildingType == buildingType).FirstOrDefault();
    }
    public BuildingSO GetBuildingTile(int buildingType)
    {
        return GetBuildingTile((BuildingTypeEnum)buildingType);
    }
}
