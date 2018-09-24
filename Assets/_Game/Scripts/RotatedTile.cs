using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;

#endif

public enum OnWalkOverAction {
	Invalid,
	MoveToCenter,
	MoveConvex
}

public enum OnEnterAction {
	Invalid,
	MoveConcaveLeft,
	MoveConcaveRight,
}

public enum OnLandAction {
	None,
	MoveConcaveLeft,
	MoveConcaveRight,
	MoveConvexLeft,
	MoveConvexRight,
}

[System.Serializable]
public struct MovementAction {
	[SerializeField] public OnWalkOverAction left;
	[SerializeField] public OnWalkOverAction right;
	[SerializeField] public OnEnterAction enter;
	[SerializeField] public OnLandAction land;
}

public class RotatedTile : TileBase {
	private enum Rotation {
		_0,
		_90,
		_180,
		_270
	}

	[SerializeField] private Sprite sprite;
	[SerializeField] private Rotation rotation;
	[SerializeField] private MovementAction top;
	[SerializeField] private MovementAction left;
	[SerializeField] private MovementAction right;
	[SerializeField] private MovementAction bottom;

	public MovementAction Top => top;

	public MovementAction Left => left;

	public MovementAction Right => right;

	public MovementAction Bottom => bottom;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		tileData.sprite = sprite;
		tileData.color = Color.white;
		tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, GetDegrees()), Vector3.one);
		tileData.flags = TileFlags.LockTransform;
		tileData.colliderType = Tile.ColliderType.Grid;
	}

	private float GetDegrees() {
		switch (rotation) {
			case Rotation._90: return 90.0f;

			case Rotation._180: return 180.0f;

			case Rotation._270: return 270.0f;

			default: return 0.0f;
		}
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/RotatedTile")]
	public static void CreateRoadTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save Rotated Tile", "New Rotated Tile", "Asset", "Save Rotated Tile", "Assets");
		if (path == "") return;
		AssetDatabase.CreateAsset(CreateInstance<RotatedTile>(), path);
	}
#endif
}
