using System.Collections.Generic;
using UnityEngine;

namespace YeLazzers
{
	public static class Utils
	{
		static Vector3Int
			LEFT = new Vector3Int(-1, 0, 0),
			RIGHT = new Vector3Int(1, 0, 0),
			DOWN = new Vector3Int(0, -1, 0),
			DOWN_LEFT = new Vector3Int(-1, -1, 0),
			DOWN_RIGHT = new Vector3Int(1, -1, 0),
			UP = new Vector3Int(0, 1, 0),
			UP_LEFT = new Vector3Int(-1, 1, 0),
			UP_RIGHT = new Vector3Int(1, 1, 0);

		static Vector3Int[] directions_when_y_is_even =
					{DOWN_LEFT, DOWN, LEFT, RIGHT, UP_LEFT, UP };
		// { LEFT, RIGHT, DOWN, DOWN_LEFT, UP, UP_LEFT };
		static Vector3Int[] directions_when_y_is_odd =
					{ DOWN, DOWN_RIGHT, LEFT, RIGHT, UP, UP_RIGHT };
		// { LEFT, RIGHT, DOWN, DOWN_RIGHT, UP, UP_RIGHT };

		public static List<Vector3Int> Neighbors(Vector3Int node)
		{
			List<Vector3Int> neighbors = new List<Vector3Int>();

			Vector3Int[] directions = (node.y % 2) == 0 ?
					 directions_when_y_is_even :
					 directions_when_y_is_odd;
			foreach (var direction in directions)
			{
				Vector3Int neighborPos = node + direction;
				neighbors.Add(neighborPos);
			}
			return neighbors;
		}

		public static Vector3Int GetOffsetCell(Vector3Int node, int vector)
		{
			int index = 0;
			switch (vector)
			{
				case 1: index = 0; break;
				case 4: index = 2; break;
				case 7: index = 4; break;
				case 3: index = 1; break;
				case 6: index = 3; break;
				case 9: index = 5; break;
			}

			return (node.y % 2) == 0 ?
								directions_when_y_is_even[index] :
								directions_when_y_is_odd[index];
		}

		public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 20, Color color = default(Color))
		{
			if (color == null) color = Color.white;

			GameObject gameObject = new GameObject($"CellText: {text}", typeof(TextMesh));
			Transform transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;

			TextMesh textMesh = gameObject.GetComponent<TextMesh>();
			textMesh.text = text;
			textMesh.fontSize = fontSize;
			textMesh.color = color;
			textMesh.anchor = TextAnchor.MiddleCenter;
			textMesh.alignment = TextAlignment.Center;

			// Читабельность текста.
			textMesh.characterSize = fontSize / 200f;

			return textMesh;
		}

		public static Vector3 GetMouseWorldPosition()
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPosition.z = 0f;
			return worldPosition;
		}
	}
}