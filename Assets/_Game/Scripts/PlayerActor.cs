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

	[SerializeField] private GameObject trackingPoint;

	[SerializeField] private Tilemap tilemap;

	[SerializeField] private Orientation orientation = Orientation.Up;

	[SerializeField] private MovementDirection facing = MovementDirection.Right;

	[SerializeField] public InputProvider inputProvider;

	private void Start() {
		Cinemachine.CinemachineVirtualCamera cam = Camera.main.GetComponent<Cinemachine.CinemachineVirtualCamera>();

		if (cam != null) {
			cam.Follow = trackingPoint.transform;
		}
	}

	private void Update() {
		if (isMoving) return;

		transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));

		float direction = inputProvider.MoveDirection.x;

		if (direction < 0) {
			StartCoroutine(Move(MovementDirection.Left));
		} else if (direction > 0) {
			StartCoroutine(Move(MovementDirection.Right));
		} else if (inputProvider.JumpPressed) {
			StartCoroutine(Jump());
		}

		if (inputProvider.PeekingPressed) {
			trackingPoint.transform.localPosition = Vector3.up * 9;
		} else {
			trackingPoint.transform.localPosition = Vector3.zero;
		}
	}

	private void UpdateOrientation() {
		Vector3 euler = new Vector3(0, 0, (int) orientation * -90.0f);
		if (orientation == Orientation.Left || orientation == Orientation.Right) {
			euler.x = facing == MovementDirection.Right ? 0 : 180.0f;
		} else if (orientation == Orientation.Up || orientation == Orientation.Down) {
			euler.y = facing == MovementDirection.Right ? 0 : 180.0f;
		}

		transform.rotation = Quaternion.Euler(euler);
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

			Orientation directionOrientation = o.RotateClockwise((int) direction);
			Vector3Int directionVec = GetDirectionVector(directionOrientation);
			Vector3Int downVec = GetDirectionVector(o.GetDown());

			Orientation enterFace = directionOrientation.GetDown();

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
							o = o.GetRight();
						} else {
							isValidRoute = false;
						}

						break;

					case MovementDirection.Right:
						if (adjacentTile[enterFace].enter == OnEnterAction.MoveConcaveLeft) {
							// TODO: Add animation
							celllPos += directionVec;
							o = o.GetLeft();
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
								o = o.GetLeft();
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
								o = o.GetRight();
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
			facing = direction;
			foreach (RouteSnapshot v in route) {
				transform.position = v.position;
				orientation = v.orientation;
				UpdateOrientation();
				yield return new WaitForSeconds(0.25f);
			}
		}

		isMoving = false;
	}

	private IEnumerator Jump() {
		isMoving = true;
		Vector3Int celllPos = tilemap.WorldToCell(transform.position);
		Vector3Int direction = GetDirectionVector(orientation);
		RotatedTile tile = null;
		for (int i = 0; i < MAX_STEPS; ++i) {
			celllPos += direction;
			RotatedTile hit = tilemap.GetTile<RotatedTile>(celllPos);
			if (hit != null) {
				tile = hit;
				break;
			}
		}

		if (tile != null) {
			Debug.DrawLine(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position)), tilemap.GetCellCenterWorld(celllPos), Color.red);
			OnLandAction action = tile[orientation.GetDown()].land;

			yield return new WaitForSeconds(0.25f);
			switch (action) {
				case OnLandAction.None:
					transform.position = celllPos - direction;
					orientation = orientation.GetDown();
					UpdateOrientation();
					break;

				case OnLandAction.MoveConcaveLeft:
					transform.position = celllPos;
					orientation = orientation.GetDown();
					UpdateOrientation();
					yield return new WaitForSeconds(0.25f);
					yield return Move(MovementDirection.Left);
					break;

				case OnLandAction.MoveConcaveRight:
					transform.position = celllPos;
					orientation = orientation.GetDown();
					UpdateOrientation();
					yield return new WaitForSeconds(0.25f);
					yield return Move(MovementDirection.Right);
					break;

				case OnLandAction.MoveConvexLeft:
					transform.position = celllPos + GetDirectionVector(orientation.GetRight());
					orientation = orientation.GetRight();
					UpdateOrientation();
					yield return new WaitForSeconds(0.25f);
					yield return Move(MovementDirection.Left);
					break;

				case OnLandAction.MoveConvexRight:
					transform.position = celllPos + GetDirectionVector(orientation.GetLeft());
					orientation = orientation.GetLeft();
					UpdateOrientation();
					yield return new WaitForSeconds(0.25f);
					yield return Move(MovementDirection.Right);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		isMoving = false;
	}
}
