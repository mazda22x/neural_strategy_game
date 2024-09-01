using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building")]
public class BuildingSO : TileBaseSO
{
	[SerializeField] private Sprite _buildingSprite;
	public Sprite BuildingSprite => _buildingSprite;

	[SerializeField] private BuildingTypeEnum _buildingType;
	public BuildingTypeEnum BuildingType => _buildingType;

	[SerializeField] private BuildingData _buildingData;
	public BuildingData BuildingData => _buildingData;


	public bool IsValidCell(GridManager.TileInfo tileInfo)
	{
		// Если требуется ресурс
		if (_buildingData.Validation.TileTypes.Length > 0)
		{
			// Если ресурса на клетке нет или он не подходит
			if (tileInfo.ResourceTile == null ||
					_buildingData.Validation.TileTypes.Where(type => type == tileInfo.ResourceTile.RTileData.TileType).Count() == 0)
			{
				return false;
			}
		}
		else
		{
			// Если клетка занята ресурсом
			if (tileInfo.ResourceTile != null)
				return false;

			// Если нужна конкретная земля, а требования не совпадают
			if (_buildingData.Validation.GroundTile != TileCategoryEnum.NONE &&
						_buildingData.Validation.GroundTile != tileInfo.GroundTile.TileData.TileCategory)
				return false;
		}

		return true;
	}

	public bool IsValidCost(BuildingData.ResourceStruct wallet)
	{
		if (wallet.Wood >= _buildingData.Cost.Wood
			&& wallet.Stone >= _buildingData.Cost.Stone
			&& wallet.Copper >= _buildingData.Cost.Copper
			&& wallet.Food >= _buildingData.Cost.Food)
		{
			return true;
		}
		return false;
	}
}

[Serializable]
public struct BuildingData
{
	public int Health;
	public ResourceStruct Income;
	public ResourceStruct Cost;
	public BuildingValidation Validation;


	[Serializable]
	public struct ResourceStruct
	{
		public int
			Wood,
			Stone,
			Copper,
			Food;

		public static ResourceStruct operator !(ResourceStruct values) => new ResourceStruct()
		{
			Wood = -values.Wood,
			Stone = -values.Stone,
			Copper = -values.Copper,
			Food = -values.Food,
		};
	}

	[Serializable]
	public struct BuildingValidation
	{
		public TileCategoryEnum GroundTile;
		public TileTypeEnum[] TileTypes;
	}
}


[Serializable]
public enum BuildingTypeEnum
{
	CASTLE = 1,
	LUMBER,
	BLACKSMITH,
	CHURCH,
	FARM,
	WINDMILL,
	MINE,
}

