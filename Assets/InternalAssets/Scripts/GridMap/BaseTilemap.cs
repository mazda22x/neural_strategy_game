using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IBaseGridObject
{
	public Tile Tile { get; }
}

public abstract class BaseTilemap<TGridObject> : MonoBehaviour where TGridObject : IBaseGridObject
{
	protected GridManager _gridManager;
	protected Tilemap _tilemap;

	protected BaseGrid<TGridObject> _grid;

	public abstract void GenerateMap();

	public virtual void Initialize(GridManager gridManager)
	{
		_gridManager = gridManager;

		_tilemap = GetComponent<Tilemap>();
	}

	public virtual void Clear()
	{
		_grid.Clear();
		_tilemap.ClearAllTiles();
	}

	public virtual TGridObject GetGridObject(int x, int y)
	{
		return _grid.GetGridObject(x, y);
	}

	protected virtual void RenderGrid()
	{
		List<Tile> tiles = new List<Tile>();
		foreach (var gt in _grid.GetSimplifiedGrid())
		{
			tiles.Add(gt?.Tile);
		}

		if (tiles.Count > 0)
			_tilemap.SetTilesBlock(_gridManager.GridArea, tiles.ToArray());
	}


	public virtual void TriggerGroundTileChanged(TGridObject gridObject, int x, int y)
	{
		_tilemap.SetTile(new Vector3Int(x, y), gridObject.Tile);
		_grid.TriggerGridObjectChanged(x, y);
	}
}