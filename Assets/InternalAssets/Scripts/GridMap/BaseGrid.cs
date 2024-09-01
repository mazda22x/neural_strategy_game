using System;
using UnityEngine;

public class BaseGrid<TGridObject>
{
	public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
	public class OnGridObjectChangedEventArgs : EventArgs
	{
		public int x, y;
	}

	private const int FONT_SIZE = 15;

	private Vector3Int _gridPosition;
	private Vector3Int _gridSize;
	private TGridObject[,] _gridArray;
	private TextMesh[,] _posTextArray;
	private TextMesh[,] _valTextArray;

	private Grid _grid;


	public BaseGrid(Grid grid, BoundsInt gridArea, Func<int, int, TGridObject> createGridObject = null)
	{
		_gridPosition = gridArea.position;
		_gridSize = gridArea.size;

		_grid = grid;


		_gridArray = new TGridObject[_gridSize.x, _gridSize.y];
		_posTextArray = new TextMesh[_gridSize.x, _gridSize.y];
		_valTextArray = new TextMesh[_gridSize.x, _gridSize.y];

		GameObject cellsObject = new GameObject($"Cells");
		cellsObject.transform.SetParent(grid.transform, false);


		for (int x = 0; x < _gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < _gridArray.GetLength(1); y++)
			{
				if (createGridObject != null)
					_gridArray[x, y] = createGridObject(x, y);

				GameObject cellObject = new GameObject($"Cell ({x}, {y})");
				cellObject.transform.SetParent(cellsObject.transform);

				Vector3 cellPosition = _grid.GetCellCenterWorld(new Vector3Int(x, y));
				// _valTextArray[x, y] = Utils.CreateWorldText(_gridArray[x, y]?.ToString(), cellObject.transform, cellPosition, FONT_SIZE, Color.white);
				// _posTextArray[x, y] = Utils.CreateWorldText($"{x}, {y}", cellObject.transform, new Vector3(cellPosition.x, cellPosition.y - FONT_SIZE * 1.5f / 100f), FONT_SIZE, Color.white);
			}
		}

		OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs args) =>
		{
			string valText = _gridArray[args.x, args.y].ToString();
			// _valTextArray[args.x, args.y].text = valText;
			// _valTextArray[args.x, args.y].name = $"CellText: {valText}";
		};
	}

	public void FillGrid(Action<BaseGrid<TGridObject>> fillAction)
	{
		if (fillAction != null)
			fillAction(this);
	}

	public void TriggerGridObjectChanged(int x, int y)
	{
		if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
	}

	public void SetGridObject(int x, int y, TGridObject obj)
	{
		if (IsWithinBounds(x, y))
		{
			_gridArray[x, y] = obj;
			if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
		}
	}


	public void SetGridObject(Vector3 worldPosition, TGridObject newObj)
	{
		Vector3Int cellPosition = _grid.WorldToCell(worldPosition);
		SetGridObject(cellPosition.x, cellPosition.y, newObj);
	}

	public void AddGridObject(int x, int y, TGridObject newObj)
	{
		SetGridObject(x, y, newObj);
	}

	public void AddGridObject(Vector3 worldPosition, Func<int, int, TGridObject> objCreator)
	{
		Vector3Int cellPosition = _grid.WorldToCell(worldPosition);
		AddGridObject(cellPosition.x, cellPosition.y, objCreator(cellPosition.x, cellPosition.y));
	}

	public void SetGrid(TGridObject[,] grid)
	{
		for (int i = 0; i < _gridArray.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				SetGridObject(i, j, grid[i, j]);
			}
		}
	}

	public TGridObject GetGridObject(int x, int y)
	{
		return IsWithinBounds(x, y) ? _gridArray[x, y] : default;
	}

	public TGridObject GetGridObject(Vector3 worldPosition)
	{
		Vector3Int cellPosition = _grid.WorldToCell(worldPosition);
		return GetGridObject(cellPosition.x, cellPosition.y);
	}

	public bool IsWithinBounds(int x, int y)
	{
		return x >= 0 && y >= 0 && x < _gridSize.x && y < _gridSize.y;
	}

	public TGridObject[] GetSimplifiedGrid()
	{
		TGridObject[] tiles = new TGridObject[_gridSize.x * _gridSize.y];
		for (int i = 0; i < _gridSize.x; i++)
		{
			for (int j = 0; j < _gridSize.y; j++)
			{
				// Flat-top is reverted. So [j, i]
				tiles[i * _gridSize.y + j] = _gridArray[j, i];
			}
		}
		return tiles;
	}

	public TGridObject[,] Clone()
	{
		return _gridArray.Clone() as TGridObject[,];
	}

	public void Clear()
	{
		for (int i = 0; i < _gridSize.x; i++)
		{
			for (int j = 0; j < _gridSize.y; j++)
			{
				_gridArray[i, j] = default(TGridObject);
				// _valTextArray[i, j] = null;
				// _posTextArray[i, j] = null;
			}
		}
	}
}