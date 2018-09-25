using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerActor : MonoBehaviour {
	private const int MAX_STEPS = 128;

	private bool isMoving;

	private enum MovementDirection {
		Left = -1,
		Right = 1
	}

	private struct RouteSnapshot {
		public Vector3 position;
		public Orientation orientation;
	}

	/// <summary>
	/// Custom Modulo that does not return negative values
	/// </summary>
	/// <param name="i">Input value</param>
	/// <param name="m">Divisior</param>
	/// <returns>i % m</returns>
	private static int Mod(int i, int m) {
		int tmp = i % m;
		return tmp < 0 ? tmp + m : tmp;
	}

	[SerializeField] private Tilemap tilemap;

	[SerializeField] private Orientation orientation = Orientation.Up;

	[SerializeField] public InputProviderPlayer InputProvider;

	private void Start() {
		InputProvider = GetComponent<InputProviderPlayer>();
	}

	private void Update() {
		if (isMoving) return;

		transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));

		float direction = InputProvider.MoveDirection.x;

		if (direction < 0) {
			StartCoroutine(Move(MovementDirection.Left));
		} else if (direction > 0) {
			StartCoroutine(Move(MovementDirection.Right));
		}
	}

	private Vector3Int GetDirectionVector(Orientation o) {
		switch (o) {
			case Orientation.Up: return Vector3Int.up;
			case Orientation.Left: return Vector3Int.left;
			case Orientation.Right: return Vector3Int.right;
			case Orientation.Down: return Vector3Int.down;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private IEnumerator Move(MovementDirection direction) {
		isMoving = true;

		List<RouteSnapshot> route = new List<RouteSnapshot>();

		Orientation o = orientation;
		Vector3Int celllPos = tilemap.WorldToCell(transform.position);
		bool isCompleteRoute = false;
		bool isValidRoute = true;

		int numSteps = 0;

		while (!isCompleteRoute && isValidRoute) {
			if (numSteps++ > MAX_STEPS) {
				isValidRoute = false;
				break;
			}

			Vector3Int directionVec = GetDirectionVector((Orientation) Mod((int) o + (int) direction, 4));
			Vector3Int downVec = GetDirectionVector((Orientation) Mod((int) o + 2, 4));

			Orientation enterFace = (Orientation) Mod((int) o - (int) direction, 4);
			Debug.Log(enterFace);

			Vector3Int adjacentCellPos = celllPos + directionVec;
			Vector3Int adjacentDownCellPos = adjacentCellPos + downVec;
			RotatedTile adjacentTile = tilemap.GetTile<RotatedTile>(adjacentCellPos);
			RotatedTile adjacentDownTile = tilemap.GetTile<RotatedTile>(adjacentDownCellPos);

			Debug.DrawLine(tilemap.GetCellCenterWorld(celllPos), tilemap.GetCellCenterWorld(adjacentCellPos), adjacentTile == null ? Color.red : Color.green);
			Debug.DrawLine(tilemap.GetCellCenterWorld(celllPos), tilemap.GetCellCenterWorld(adjacentDownCellPos), adjacentDownTile == null ? Color.red : Color.green);

			if (adjacentTile != null) {
				switch (direction) {
					case MovementDirection.Left:
						if (adjacentTile[enterFace].enter == OnEnterAction.MoveConcaveRight) {
							// TODO: Add animation
							celllPos += directionVec;
							o = (Orientation) Mod((int) o + 1, 4); // turn right
						} else {
							isValidRoute = false;
						}

						break;

					case MovementDirection.Right:
						if (adjacentTile[enterFace].enter == OnEnterAction.MoveConcaveLeft) {
							// TODO: Add animation
							celllPos += directionVec;
							o = (Orientation) Mod((int) o - 1, 4); // turn left
						} else {
							isValidRoute = false;
						}

						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
				}
			} else if (adjacentDownTile != null) {
				switch (direction) {
					case MovementDirection.Left:
						switch (adjacentDownTile[o].left) {
							case OnWalkOverAction.Invalid:
								isValidRoute = false;
								break;

							case OnWalkOverAction.MoveToCenter:
								// TODO: Add animation
								celllPos += directionVec;
								isCompleteRoute = true;
								break;

							case OnWalkOverAction.MoveConvex:
								// TODO: Add animation
								celllPos += directionVec + directionVec + downVec;
								o = (Orientation) Mod((int) o - 1, 4); // turn left
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}

						break;

					case MovementDirection.Right:
						switch (adjacentDownTile[o].right) {
							case OnWalkOverAction.Invalid:
								isValidRoute = false;
								break;

							case OnWalkOverAction.MoveToCenter:
								// TODO: Add animation
								celllPos += directionVec;
								isCompleteRoute = true;
								break;

							case OnWalkOverAction.MoveConvex:
								// TODO: Add animation
								celllPos += directionVec + directionVec + downVec;
								o = (Orientation) Mod((int) o + 1, 4); // turn right
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}

						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
				}
			} else {
				isValidRoute = false;
			}

			if (isValidRoute) {
				route.Add(new RouteSnapshot {
					position = tilemap.GetCellCenterWorld(celllPos),
					orientation = o
				});
			}
		}

		if (isValidRoute) {
			foreach (RouteSnapshot v in route) {
				transform.position = v.position;
				transform.rotation = Quaternion.Euler(0, 0, (int)v.orientation * -90.0f);
				orientation = v.orientation;
				yield return new WaitForSeconds(0.25f);
			}
		}

		isMoving = false;
	}
}
