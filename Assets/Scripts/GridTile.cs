using UnityEngine;
using UnityEngine.EventSystems; 

public class GridTile : MonoBehaviour {

	MeshRenderer mr; 

	Vector2Int gridCoordinates; 

	private AudioSource audiosource; 

	/// <summary>
	/// The straight track.
	/// </summary>
	public bool straightTrack; 

	public float rotation;

	public bool upRight, upLeft, bottomLeft, bottomRight; 

	bool interactable; 

	public bool UpRight { get { return (upRight || tileType != Constants.Tile.Empty); } set { upRight = value; } } 

	public bool UpLeft { get { return (upLeft || tileType != Constants.Tile.Empty); } set { upLeft = value; }} 

	public bool BottomLeft  { get { return (bottomLeft || tileType != Constants.Tile.Empty); } set { bottomLeft = value; }}

	public bool BottomRight  { get { return (bottomRight || tileType != Constants.Tile.Empty); } set { bottomRight = value; }} 

	public Constants.Tile tileType;

	public Constants.Tile TileType {
		get {
			return tileType; 
		}
		set { 
			tileType = value; 
		} 
	}
		
	private Junction junction; 

	public void SetStart(){ 
		mr = GetComponent<MeshRenderer>(); 

		UpRight = false;
		UpLeft = false;
		BottomLeft = false;
		BottomRight = false; 
	}

	/// <summary>
	/// Is the entire tile completely filled?
	/// </summary>
	/// <returns><c>true</c>, if tile is completely filled, <c>false</c> otherwise.</returns>
	public bool isFilled() { 

		if (TileType != Constants.Tile.Empty) {
			Debug.Log ("Tile type not empty!"); 
			return true; 
		}
		// Debug.Log ("Empty? : " + (isEmpty ())); 
		return !(isEmpty ()); 

	}

	/// <summary>
	/// Is the entire tile empty?
	/// </summary>
	/// <returns><c>true</c>, if tile is completely empty, <c>false</c> otherwise.</returns>
	public bool isEmpty() { 

		if (TileType != Constants.Tile.Empty) {
			return false; 
		}

		return (!UpRight && !UpLeft && !BottomLeft && !BottomRight); 
	}

	public void setEmpty() { 
		UpRight = false;
		UpLeft = false;
		BottomLeft = false;
		BottomRight = false; 
		return;
	}

	public void setEmptyCorner(int id) { 
		if (id == 0) {
			UpRight = false; 
		} else if (id == 1) {
			BottomRight = false; 
		} else if (id == 2) {
			BottomLeft = false;
		} else if (id == 3) {
			UpLeft = false; 
		}
		return;
	}

	/// <summary>
	/// Sets booleans to make the entire tile considered not empty.
	/// </summary>
	public void setFilled() { 
		UpRight = true;
		UpLeft = true;
		BottomLeft = true;
		BottomRight = true; 
		// mr.material.color = Color.black;
	}

	public void SetGrid(int _row, int _column, float _yAxis, bool _interactable) { 
		transform.localPosition = new Vector3 (_row * 5f, 0f, _column * 5f);
		transform.position = new Vector3 (transform.position.x, _yAxis, transform.position.z); 
		gridCoordinates = new Vector2Int ((int)transform.localPosition.x / 5, (int)transform.localPosition.z / 5); 
		SetStart (); 
//		if (gridCoordinates.x == 3 && gridCoordinates.y == 3) {
//			SetColor (Color.red); 
//		}
		interactable = _interactable; 
	}


	public void OnMouseDown() { 

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Menu) {
			return; 
		}

		if (EventSystem.current.IsPointerOverGameObject ()) { 

			Debug.Log ("Over UI"); 
			return;

		}

		if (!interactable) {
			StateManager.current.NewNotification ("Edge tiles are not interactable");
			return;
		}
		if (TileType == Constants.Tile.Water) {
			StateManager.current.NewNotification ("No interactions with water tiles"); 
			return;
		}

		SetColor (GridBuilder.current.highlightedColor); 

		Debug.Log ("Tile " + gridCoordinates + " was clicked"); 

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Build) {
			GridBuilder.current.startDragCoordinates = gridCoordinates;
			GridBuilder.current.endDragCoordinates = gridCoordinates; 
			if (GridBuilder.current.ConstructionMode != Constants.TrackPiece.StraightHorizontal && 
				GridBuilder.current.ConstructionMode != Constants.TrackPiece.StraightVertical) {
				GridBuilder.current.SetTrack (); 
			} 
			SetColor (GridBuilder.current.highlightedColor);
		}

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Demolish) {

			if (tileType == Constants.Tile.Forest || tileType == Constants.Tile.Rock) {

				PlayDes (); 

				tileType = Constants.Tile.Empty; 
				foreach (Transform child in transform) {
					if (child.tag == "Terrain") {

						Destroy (child.gameObject); 
						StateManager.current.ConstructionCost += 50;

					}
				}
			}
		}
	}

	public void PlayDes() { 
		if (audiosource == null) {
			audiosource = gameObject.AddComponent<AudioSource> ();
			audiosource.volume -= 0.85f;
			audiosource.clip = Resources.Load ("Audio/bulldozed") as AudioClip; 
		}

		audiosource.Play (); 
	}

	public void OnMouseEnter() {
		if (!interactable || TileType == Constants.Tile.Water) {
			return;
		}

		SetColor(GridBuilder.current.highlightedColor); 

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Build) {
			GridBuilder.current.endDragCoordinates = gridCoordinates;

			if (GridBuilder.current.newConstruction != null) {
				GridBuilder.current.newConstruction.transform.position = new Vector3(transform.position.x, 1f, transform.position.z); 
			}
		}
	} 

	public void OnMouseExit() { 

		if (!interactable) {
			return;
		}
	
		if (TileType == Constants.Tile.Empty || TileType == Constants.Tile.Forest || TileType == Constants.Tile.Rock) {

			SetColor (GridBuilder.current.regularGrassColor); 

		} else if (TileType == Constants.Tile.Water) {
			
			SetColor (GridBuilder.current.waterColor); 

		} 
	}

	public void SetColor(Color _color) {
		mr.material.color = _color;
	}

	public void OnMouseUp() { 
		if (!interactable) {
			return;
		}

		GridBuilder.current.EndDrag (); 
	}

	public Path TrainEnter(Constants.Rotation trainDirection) {
		
		Debug.Log ("Tile " + gridCoordinates); 

		if (junction == null) {
			Debug.Log ("Returning null!"); 
			return null; 
		}
		Path returnPath = junction.returnPath(trainDirection, straightTrack); 
		if (returnPath == null) {
			Debug.Log ("Train has derailed!"); 
			return null; 
		}
		return returnPath;
	}

	public void addPathToJunctionStraight(Path newPath) { 
		if (junction == null) {
			junction = gameObject.AddComponent <Junction> (); 
			junction.AddPathStraight (newPath); 
		} else {
			junction.AddPathStraight (newPath); 
		}
		return; 
	} 

	public void addPathToJunction(Path newPath) { 

		if (TileType != Constants.Tile.Empty) {
			Debug.Log ("Error: Adding junction to non-empty tile " + gridCoordinates); 
		}

		if (junction == null) {
			junction = gameObject.AddComponent <Junction> (); 
			junction.AddPath (newPath); 
		} else {
			junction.AddPath (newPath); 
		}
		return; 
	} 

	/// <summary>
	/// Remove the junction attached to the same GameObject, due to demolition or level clearing.
	/// </summary>
	public void RemoveJunction() { 
		straightTrack = false;
		if (junction == null && GetComponent<Junction>() == null) {
			return; 
		}
		junction = null;
		if (GetComponent<Junction> () != null) {
			Destroy (GetComponent<Junction> ()); 
		} else {
			Debug.Log ("Junction reference was not null, but a junction component was found on tile: " + gridCoordinates); 
		}
		foreach (Transform child in transform) {
			if (child.tag == "Arrow") {
				Destroy (child.gameObject); 
			}
		}
	}
}
