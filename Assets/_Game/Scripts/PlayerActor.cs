using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerActor : MonoBehaviour {
	private const int MAX_STEPS = 128;

	private const float CONCAVE_RADIUS = 0.5f;
	private const float CONVEX_RADIUS = 1.5f;

	private const float MOVE_ANIMATION_TIME_PER_TILE = 0.25f;
	private const float JUMP_ANIMATION_TIME_PER_TILE = 0.125f;

	private const float CONCAVE_ANIMATION_TIME = 1.0f / Mathf.PI;
	private const float CONVEX_ANIMATION_TIME = 3.0f / 16.0f * Mathf.PI;

	private bool isMoving;

	private enum MovementDirection {
		Left = -1,
		Right = 1
	}

	[SerializeField] private GameObject trackingPoint;

	[SerializeField] private Tilemap tilemap;

	[SerializeField] private Orientation orientation = Orientation.Up;

	[SerializeField] private MovementDirection facing = MovementDirection.Right;

	[SerializeField] private InputProvider inputProvider;

	[SerializeField] private Animator animator;

	[SerializeField] private float peekingDistance;

	private void Start() {
		Cinemachine.CinemachineVirtualCamera cam = Camera.main.GetComponent<Cinemachine.CinemachineVirtualCamera>();

		if (cam != null) {
			cam.Follow = trackingPoint.transform;
		}
	}

	private void Update() {
		animator.SetBool("isRunning", isMoving);
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

		animator.SetBool("isPeeking", inputProvider.PeekingPressed);
		if (inputProvider.PeekingPressed) {
			trackingPoint.transform.localPosition = Vector3.up * peekingDistance;
		} else {
			trackingPoint.transform.localPosition = Vector3.zero;
		}
	}

	private void UpdateOrientation() {
		transform.rotation = GetRotation(orientation, facing);
	}

	private static Quaternion GetRotation(Orientation orientation, MovementDirection facing) {
		Vector3 euler = new Vector3(0, 0, (int) orientation * -90.0f);
		switch (orientation) {
			case Orientation.Left:
			case Orientation.Right:
				euler.x = facing == MovementDirection.Right ? 0 : 180.0f;
				break;

			case Orientation.Up:
			case Orientation.Down:
				euler.y = facing == MovementDirection.Right ? 0 : 180.0f;
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return Quaternion.Euler(euler);
	}

	private static Vector3Int GetDirectionVector(Orientation o) {
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

		List<IEnumerator> route = new List<IEnumerator>();

		Orientation o = orientation;
		Vector3Int cellPos = tilemap.WorldToCell(transform.position);
		bool isCompleteRoute = false;
		bool isValidRoute = true;

		int numSteps = 0;

		route.Add(Move(o.RotateClockwise((int) direction), 0.5f, MOVE_ANIMATION_TIME_PER_TILE * 0.5f));

		while (!isCompleteRoute && isValidRoute) {
			if (numSteps++ > MAX_STEPS) {
				isValidRoute = false;
				break;
			}

			Orientation directionOrientation = o.RotateClockwise((int) direction);
			Vector3Int directionVec = GetDirectionVector(directionOrientation);
			Vector3Int downVec = GetDirectionVector(o.GetDown());

			Orientation enterFace = directionOrientation.GetDown();

			Vector3Int adjacentCellPos = cellPos + directionVec;
			Vector3Int adjacentDownCellPos = adjacentCellPos + downVec;
			RotatedTile adjacentTile = tilemap.GetTile<RotatedTile>(adjacentCellPos);
			RotatedTile adjacentDownTile = tilemap.GetTile<RotatedTile>(adjacentDownCellPos);

			Debug.DrawLine(tilemap.GetCellCenterWorld(cellPos), tilemap.GetCellCenterWorld(adjacentCellPos), adjacentTile == null ? Color.red : Color.green);
			Debug.DrawLine(tilemap.GetCellCenterWorld(cellPos), tilemap.GetCellCenterWorld(adjacentDownCellPos), adjacentDownTile == null ? Color.red : Color.green);

			if (adjacentTile != null) {
				cellPos += directionVec;
				switch (direction) {
					case MovementDirection.Left:
						if (adjacentTile[enterFace].enter == OnEnterAction.MoveConcaveRight) {
							o = o.GetRight();
							route.Add(Rotate(directionOrientation, directionOrientation.GetRight(), o, CONCAVE_RADIUS, CONCAVE_ANIMATION_TIME));
						} else {
							isValidRoute = false;
						}

						break;

					case MovementDirection.Right:
						if (adjacentTile[enterFace].enter == OnEnterAction.MoveConcaveLeft) {
							o = o.GetLeft();
							route.Add(Rotate(directionOrientation, directionOrientation.GetLeft(), o, CONCAVE_RADIUS, CONCAVE_ANIMATION_TIME));
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
								cellPos += directionVec;
								isCompleteRoute = true;
								route.Add(Move(directionOrientation, 0.5f, MOVE_ANIMATION_TIME_PER_TILE * 0.5f));
								break;

							case OnWalkOverAction.MoveConvex:
								// TODO: Add animation
								cellPos += directionVec * 2 + downVec;
								o = o.GetLeft();
								route.Add(Rotate(directionOrientation, directionOrientation.GetLeft(), o, CONVEX_RADIUS, CONVEX_ANIMATION_TIME));
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
								cellPos += directionVec;
								isCompleteRoute = true;
								route.Add(Move(directionOrientation, 0.5f, MOVE_ANIMATION_TIME_PER_TILE * 0.5f));
								break;

							case OnWalkOverAction.MoveConvex:
								cellPos += directionVec * 2 + downVec;
								o = o.GetRight();
								route.Add(Rotate(directionOrientation, directionOrientation.GetRight(), o, CONVEX_RADIUS, CONVEX_ANIMATION_TIME));
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
		}

		facing = direction;
		UpdateOrientation();

		if (isValidRoute) {
			foreach (IEnumerator a in route) {
				yield return a;
			}
		}

		isMoving = false;
	}

	private IEnumerator Jump() {
		isMoving = true;
		Vector3Int celllPos = tilemap.WorldToCell(transform.position);
		Vector3Int direction = GetDirectionVector(orientation);
		RotatedTile tile = null;
		int i = 0;
		for (; i < MAX_STEPS; ++i) {
			celllPos += direction;
			RotatedTile hit = tilemap.GetTile<RotatedTile>(celllPos);
			if (hit != null) {
				tile = hit;
				break;
			}
		}

		if (tile != null) {
			animator.SetBool("isFalling", true);
			orientation = orientation.GetDown();
			UpdateOrientation();

			if (i != 0) {
				yield return Move(orientation.GetDown(), i, i * JUMP_ANIMATION_TIME_PER_TILE);
			}

			animator.SetBool("isFalling", false);

			Debug.DrawLine(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position)), tilemap.GetCellCenterWorld(celllPos), Color.red);
			OnLandAction action = tile[orientation].land;

			switch (action) {
				case OnLandAction.None:
					transform.position = tilemap.GetCellCenterWorld(celllPos - direction);
					break;

				case OnLandAction.MoveConcaveLeft:
					transform.position = tilemap.GetCellCenterWorld(celllPos);
					animator.SetTrigger("shouldSlide");
					yield return Move(MovementDirection.Left);
					break;

				case OnLandAction.MoveConcaveRight:
					transform.position = tilemap.GetCellCenterWorld(celllPos);
					animator.SetTrigger("shouldSlide");
					yield return Move(MovementDirection.Right);
					break;

				case OnLandAction.MoveConvexLeft:
					transform.position = tilemap.GetCellCenterWorld(celllPos - direction);
					facing = MovementDirection.Left;
					UpdateOrientation();
					animator.SetTrigger("shouldSlide");
					yield return Rotate(orientation.GetLeft(), orientation.GetDown(), orientation.GetLeft(), 1.0f, 0.2f);
					yield return Move(MovementDirection.Left);
					break;

				case OnLandAction.MoveConvexRight:
					transform.position = tilemap.GetCellCenterWorld(celllPos - direction);
					facing = MovementDirection.Right;
					UpdateOrientation();
					animator.SetTrigger("shouldSlide");
					yield return Rotate(orientation.GetRight(), orientation.GetDown(), orientation.GetRight(), 1.0f, 0.1f);
					yield return Move(MovementDirection.Right);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		isMoving = false;
	}

	private IEnumerator Move(Orientation direction, float distance, float time) {
		Vector3 vec = GetDirectionVector(direction);
		vec *= distance;

		Vector3 start = transform.position;

		for (float t = 0; t <= time; t += Time.deltaTime) {
			transform.position = Vector3.Lerp(start, start + vec, t / time);
			yield return null;
		}

		transform.position = start + vec;
	}

	private IEnumerator Rotate(Orientation startDirection, Orientation endDirection, Orientation target, float curveRadius, float time) {
		Vector3 startDir = GetDirectionVector(startDirection);
		Vector3 endDir = GetDirectionVector(endDirection);

		Vector3 startPos = transform.position;
		Vector3 endPos = transform.position + (startDir + endDir) * curveRadius;

		Vector3 rotationCenter = startPos + endDir * curveRadius;

		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = GetRotation(target, facing);

		Quaternion targetRortation = Quaternion.Euler(0, 0, startDirection.GetLeft() == endDirection ? 90.0f : -90.0f);

		for (float t = 0; t <= time; t += Time.deltaTime) {
			Quaternion rot = Quaternion.Lerp(Quaternion.identity, targetRortation, t / time);
			Debug.DrawLine(transform.position, rotationCenter, Color.red);

			transform.position = rotationCenter - rot * endDir * curveRadius;
			transform.rotation = Quaternion.Slerp(startRotation, endRotation, t / time);

			yield return null;
		}

		transform.position = endPos;
		orientation = target;
		UpdateOrientation();
	}
}
