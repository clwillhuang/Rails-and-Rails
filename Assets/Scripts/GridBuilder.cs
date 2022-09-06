using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles tile creation and rail construction.
/// </summary>
public class GridBuilder : MonoBehaviour {

	// Prefab Objects : Objects that can be cloned during program run time. 
	// Must be public for Unity Editor usage. 

	#region Prefab Objects

	/// <summary>
	/// The prefab for a single default tile. 
	/// </summary>
	public GameObject gridPrefab;

	public GameObject entryPointPrefab;

	/// <summary>
	/// Prefabs for each track piece. 
	/// </summary>
	public GameObject straightTrack, doubleStraight, curve, sBendLeft, sBendRight, sBendLeftJunction, sBendRightJunction, threeWayJunction, fourWayJunction; 

	public List < GameObject > trees;

	public GameObject rock;

	#endregion

	/// <summary>
	/// Coordinates of the tile when the user first started dragging. 
	/// </summary>
	public Vector2Int startDragCoordinates; 

	/// <summary>
	/// Latest tile that the user is over while dragged.
	/// </summary>
	public Vector2Int endDragCoordinates; 

	/// <summary>
	/// The y-coordinate of all tiles in world space.
	/// </summary>
	public float yAxis = 0.5f;

	/// <summary>
	/// The number of desired grid rows. 
	/// </summary>
	int gridRows = 12; 

	/// <summary>
	/// The number of desired grid columns.
	/// </summary>
	int gridColumns = 12; 

	bool lastPressed = false; 

	public Color regularGrassColor, highlightedColor, waterColor;

	public Transform tileParent;

	private GridTile[,] gridTiles;

	public GridTile getGridTile(int x, int y) { 
		if (x < 0 || y < 0 || x >= gridTiles.GetLength (0) || y >= gridTiles.GetLength (1)) {
			Debug.LogError ("Invalid array index for gridTiles: " + x + " " + y); 
			return null; 
		} else { 
			return gridTiles [x, y]; 
		}
	}

	/// <summary>
	/// The selected type of track that the player is trying to build. Appears grayed-out.
	/// </summary>
	public GameObject newConstruction; 

	/// <summary>
	/// Current type of track piece that the user has selected to be constructed.
	/// </summary>
	private Constants.TrackPiece constructionMode = Constants.TrackPiece.Free;

	public Constants.TrackPiece ConstructionMode { 
		get {
			return constructionMode; 
		} set {
			StateManager.current.SelectionMode = Constants.SelectionMode.Build;
			constructionMode = value; 
			GridBuilder.current.MakeNewConstruction (); 
		}
	}

	/// <summary>
	/// Current rotation of construction track. 'Up' is the starting orientation. 
	/// NOTE: MAY NOT ACTUALLY REFLECT THE DIRECTION OF TRACK
	/// </summary>
	private Constants.Rotation constructionModeRotation = Constants.Rotation.Up; 

	public Constants.Rotation ConstructionModeRotation { 
		get {
			return constructionModeRotation; 
		} set {
			constructionModeRotation = value; 
		}
	}

	public static GridBuilder _current; 

	public static GridBuilder current  {
		get { 
			if (_current == null) {
				_current = GameObject.FindObjectOfType<GridBuilder> ();
			}
			return _current;
		} 
	}

	public void Start() { 	
		startDragCoordinates = new Vector2Int (-1, -1);
	
	} 

	public void Update() { 
		if (Input.GetKeyDown (KeyCode.R)) {
			RotateTrack (); 
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			
			if (StateManager.current.SelectionMode == Constants.SelectionMode.Build || StateManager.current.SelectionMode == Constants.SelectionMode.Free) {
				StateManager.current.SelectionMode = Constants.SelectionMode.Demolish;
				if (newConstruction != null) {
					Destroy (newConstruction); 
				}
			} 

			else if (StateManager.current.SelectionMode == Constants.SelectionMode.Demolish) {
				StateManager.current.SelectionMode = Constants.SelectionMode.Free; 
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) && StateManager.current.SelectionMode == Constants.SelectionMode.Build) {
			StateManager.current.SelectionMode = Constants.SelectionMode.Free;
			Destroy (newConstruction); 
		}
	} 

	public void LoadMap(int levelRows, int levelColumns, int[,] tileMap) { 

		foreach (Transform child in tileParent) {
			Destroy (child.gameObject);
		}

		int newtype;

		gridTiles = new GridTile[levelRows, levelColumns]; 
		for (int i = 0; i < levelRows; i++) {
			for (int j = 0; j < levelColumns; j++) {

				bool inter = !(i < 2 || j < 2 || i >= 17 || j >= 17);

				GameObject currentTile = Instantiate (gridPrefab, tileParent) as GameObject;
				gridTiles [i,j] = currentTile.GetComponent<GridTile>(); 
				gridTiles [i,j].SetGrid (i, j, yAxis, inter); 

				newtype = tileMap [i, j]; 

				// Forest Tile
				if (newtype == 1) {

					gridTiles [i, j].TileType = Constants.Tile.Forest; 
					gridTiles [i, j].SetColor (regularGrassColor); 

					GameObject newTree = Instantiate (trees [Random.Range (0, trees.Count)], currentTile.transform); 
					newTree.transform.localPosition = new Vector3 (Random.Range (-0.3f, 0.3f), Random.Range (0f, 1.5f), Random.Range (-0.3f, 0.3f));
					newTree.transform.localScale = new Vector3 (1f / 5f, 1f, 1 / 5f);
					newTree.transform.localEulerAngles = new Vector3 (0f, Random.Range (0, 4) * 90f, 0f); 
					newTree.tag = "Terrain";

				} 

				// Water Tile
				else if (newtype == 2) {
					gridTiles [i, j].TileType = Constants.Tile.Water; 
					gridTiles [i, j].SetColor (waterColor); 

				} 

				// Rock Tile
				else if (newtype== 3) { 
					gridTiles [i, j].TileType = Constants.Tile.Rock; 
					gridTiles [i, j].SetColor (regularGrassColor); 

					GameObject newRock = Instantiate (rock, currentTile.transform); 
					newRock.transform.localPosition = new Vector3 (0f, Random.Range (0.2f, 0.4f), 0f);
					newRock.transform.localScale = new Vector3 (1f / 5f, 1f, 1 / 5f);
					newRock.transform.localEulerAngles = new Vector3 (0f, Random.Range (0, 4) * 90f, 0f);
					newRock.tag = "Terrain";
				} 

				// Empty Tile
				else {
					gridTiles [i, j].TileType = Constants.Tile.Empty;
					gridTiles [i, j].SetColor (regularGrassColor); 
				}

			}
		}
	}
		
	/// <summary>
	/// Given data about a new entry point, spawn it on the map. 
	/// </summary>
	/// <param name="epi">Entry point information.</param>
	public void MakeEntryPoint(EntryPointInfo epi) { 
		
		Transform newEntryPoint = Instantiate (entryPointPrefab, transform).transform; 

		newEntryPoint.localPosition = new Vector3 ((epi.originX) * 5f, 0f, (epi.originY) * 5f);
		newEntryPoint.position = new Vector3(newEntryPoint.position.x, 1f, newEntryPoint.position.z); 
		newEntryPoint.SetParent (gridTiles [epi.originX, epi.originY].transform); 

		epi.ResetTrainIndex (); 

		newEntryPoint.GetComponent<EntryPoint> ().TransferInfo (epi);

		switch (epi.exitRotation) {

		case Constants.Rotation.Right:
			for (int i = 0; i < 2; i++) { 
				gridTiles [epi.originX, epi.originY - i].setFilled (); 
			}
			break; 
		case Constants.Rotation.Left:
			for (int i = 0; i < 2; i++) { 
				gridTiles [epi.originX, epi.originY + i].setFilled (); 
			}
			break; 
		case Constants.Rotation.Up:
			for (int i = 0; i < 2; i++) { 
				gridTiles [epi.originX + i, epi.originY].setFilled (); 
			}
			break; 
		case Constants.Rotation.Down:
			for (int i = 0; i < 2; i++) { 
				gridTiles [epi.originX - i, epi.originY].setFilled (); 
			}
			break; 
		default:
			break;
		}


		newEntryPoint.eulerAngles = new Vector3 (0f, 90f * ((int)epi.exitRotation + 1), 0f);

		StateManager.current.addEntryPoint (newEntryPoint.GetComponent<EntryPoint> ());
	} 

	/// <summary>
	/// Called when a mouse press ends, signalling end of drags.
	/// </summary>
	public void EndDrag() {

		if (StateManager.current.SelectionMode != Constants.SelectionMode.Build) {
			return; 
		}
		if (startDragCoordinates.x == -1f || startDragCoordinates.y == -1f) {
			return; 
		}

		GameObject newTile; 

		if (ConstructionMode == Constants.TrackPiece.StraightHorizontal) {
			for (int yCoord = Mathf.Min (startDragCoordinates.y, endDragCoordinates.y); yCoord <= Mathf.Max (startDragCoordinates.y, endDragCoordinates.y); yCoord++) {

				// Stop construction if tile is obstructed
				if (gridTiles [endDragCoordinates.x, yCoord].isFilled ()) { 
					StateManager.current.NewNotification ("Construction obstructed."); 
					break; 
				}
				
				newTile = Instantiate (straightTrack, transform); 
				newTile.transform.localPosition = new Vector3 (endDragCoordinates.x * 5f, yAxis, yCoord * 5f);
				newTile.transform.SetParent (gridTiles [endDragCoordinates.x, yCoord].transform); 

				gridTiles [endDragCoordinates.x, yCoord].setFilled (); 
				gridTiles [endDragCoordinates.x, yCoord].straightTrack = true; 
				gridTiles [endDragCoordinates.x, yCoord].SetColor (regularGrassColor); 
		
				newTile.GetComponent<WayFinding> ().setTrackDown (Constants.Rotation.Up, Constants.TrackPiece.StraightHorizontal, endDragCoordinates.x, yCoord); 

				StateManager.current.ConstructionCost += 100; 
			
			}
	
		} else if (ConstructionMode == Constants.TrackPiece.StraightVertical) {
			for (int xCoord = Mathf.Min (startDragCoordinates.x, endDragCoordinates.x); xCoord <= Mathf.Max (startDragCoordinates.x, endDragCoordinates.x); xCoord++) {

				// Stop construction if tile is obstructed
				if (gridTiles [xCoord, endDragCoordinates.y].isFilled ()) { 
					StateManager.current.NewNotification ("Construction obstructed."); 
					break; 
				}
				
				newTile = Instantiate (straightTrack, transform); 
				newTile.transform.Rotate (0f, 90f, 0f); 
				newTile.transform.localPosition = new Vector3 (xCoord * 5f, yAxis, endDragCoordinates.y * 5f); 
				newTile.transform.SetParent (gridTiles [xCoord, endDragCoordinates.y].transform); 

				gridTiles [xCoord, endDragCoordinates.y].setFilled (); 
				gridTiles [xCoord, endDragCoordinates.y].straightTrack = true; 

				newTile.GetComponent<WayFinding> ().setTrackDown (Constants.Rotation.Right, Constants.TrackPiece.StraightVertical, xCoord, endDragCoordinates.y); 

				StateManager.current.ConstructionCost += 100; 

			}
		} else {
			return; 
		}

		startDragCoordinates = new Vector2Int (-1, -1);

		// Debug.Log ("Drag ended on " + endDragCoordinates); 

	}

	/// <summary>
	/// Automatically called whenever the player selects a new track piece type to construct. Updates the construction tile helper.
	/// </summary>
	public void MakeNewConstruction() { 

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Free) {
			StateManager.current.SelectionMode = Constants.SelectionMode.Build;
		}

		if (newConstruction != null) {
			Destroy (newConstruction); 
		} 

		ConstructionModeRotation = Constants.Rotation.Up;

		if (ConstructionMode == Constants.TrackPiece.StraightHorizontal) {
			newConstruction = Instantiate (straightTrack, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.StraightVertical) {
			newConstruction = Instantiate (straightTrack, transform); 
			newConstruction.transform.Rotate(0f, 90f, 0f); 
			ConstructionModeRotation = Constants.Rotation.Right; 
		} else if (ConstructionMode == Constants.TrackPiece.DoubleStraight) {
			newConstruction = Instantiate (doubleStraight, transform); 
		}else if (ConstructionMode == Constants.TrackPiece.Curve) {
			newConstruction = Instantiate (curve, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.SBendLeft) {
			newConstruction = Instantiate (sBendLeft, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.SBendRight) {
			newConstruction = Instantiate (sBendRight, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.SBendLeftJunction) {
			newConstruction = Instantiate (sBendLeftJunction, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.SBendRightJunction) {
			newConstruction = Instantiate (sBendRightJunction, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.ThreeWayJunction) {
			newConstruction = Instantiate (threeWayJunction, transform); 
		} else if (ConstructionMode == Constants.TrackPiece.FourWayJunction) {
			newConstruction = Instantiate (fourWayJunction, transform); 
		} else
			return; 
		
		newConstruction.transform.position = gridTiles [(int)endDragCoordinates.x, (int)endDragCoordinates.y].transform.position;
	} 

	public void RotateTrack() { 

		if (StateManager.current.SelectionMode != Constants.SelectionMode.Build) {
			return; 
		}

		if (newConstruction == null) {
			Debug.Log ("New construction null!"); 
			return; 
		}

		if (ConstructionMode == Constants.TrackPiece.StraightHorizontal) {
			constructionMode = Constants.TrackPiece.StraightVertical; 
			newConstruction.transform.eulerAngles = new Vector3 (0f, 90f, 0f); 
			constructionModeRotation = Constants.Rotation.Right; 
		} else if (ConstructionMode == Constants.TrackPiece.StraightVertical) {
			constructionMode = Constants.TrackPiece.StraightHorizontal; 
			newConstruction.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
			constructionModeRotation = Constants.Rotation.Up; 
		} 
		else if (ConstructionMode != Constants.TrackPiece.FourWayJunction) {
			newConstruction.transform.Rotate (0f, 90f, 0f); 
			ConstructionModeRotation = (Constants.Rotation)(((int)constructionModeRotation + 1) % 4);
			Debug.Log ("Rotated track");  
		}
	} 

	/// <summary>
	/// Lays the current track down where newConstruction once was. Dereference newConstruction reference. 
	/// </summary>
	public void SetTrack() { 

		if (StateManager.current.SelectionMode != Constants.SelectionMode.Build) {
			return; 
		}

		if (newConstruction == null) {
			Debug.Log ("New construction null!"); 
			return; 
		}

		int x = (int)endDragCoordinates.x;
		int y = (int)endDragCoordinates.y; 

		if (gridTiles[(int)endDragCoordinates.x, (int)endDragCoordinates.y].isFilled()) {
			Debug.Log ("Construction at " + endDragCoordinates + " was obstructed");
			StateManager.current.NewNotification ("Construction obstructed."); 
			return; 
		}

		// Check if nothing is obstructing construction
		if (ConstructionMode == Constants.TrackPiece.StraightVertical || ConstructionMode == Constants.TrackPiece.StraightHorizontal) {
			// No other tiles to check. 
		} else if (ConstructionMode == Constants.TrackPiece.Curve) {
			
			if (ConstructionModeRotation == Constants.Rotation.Up) {

				if (gridTiles[x-1, y-1].isFilled() || 
					gridTiles[x,y-1].UpRight || 
					gridTiles[x-1,y].BottomLeft) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x-1, y-1].setFilled(); 
					gridTiles [x, y - 1].UpRight = true;
					gridTiles [x - 1, y].BottomLeft = true; 

				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x-1, y+1].isFilled() ||
					gridTiles[x-1,y].BottomRight ||
					gridTiles[x,y+1].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles[x-1, y+1].setFilled(); 
					gridTiles [x - 1, y].BottomRight = true; 
					gridTiles [x, y + 1].UpLeft = true;

				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x+1, y+1].isFilled() || 
					gridTiles[x,y+1].BottomLeft ||
					gridTiles[x+1,y].UpRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles[x+1, y+1].setFilled();
					gridTiles [x, y + 1].BottomLeft = true; 
					gridTiles [x + 1, y].UpRight = true; 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x+1, y-1].isFilled() ||
					gridTiles[x,y-1].BottomRight || 
					gridTiles[x+1,y].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles[x+1, y-1].setFilled();
					gridTiles [x, y - 1].BottomRight = true;
					gridTiles[x+1,y].UpLeft = true; 
				}
			}  
			StateManager.current.ConstructionCost += 200; 
		} else if (ConstructionMode == Constants.TrackPiece.SBendLeft) {
			
			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles [x, y - 1].BottomRight ||
					gridTiles [x + 1, y].UpLeft ||
				    gridTiles [x + 1, y - 1].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x, y - 1].BottomRight = true;
					gridTiles [x + 1, y].UpLeft = true;
					gridTiles [x + 1, y - 1].setFilled();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x, y-1].UpRight ||
					gridTiles[x-1, y].BottomLeft ||
					gridTiles[x-1, y-1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].UpRight = true; 
					gridTiles[x-1, y].BottomLeft = true; 
					gridTiles [x - 1, y - 1].setFilled();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x, y+1].UpLeft ||
					gridTiles[x-1, y].BottomRight ||
					gridTiles[x-1, y+1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].UpLeft = true; 
					gridTiles [x - 1, y].BottomRight = true; 
					gridTiles [x - 1, y + 1].setFilled(); 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x, y+1].BottomLeft ||
					gridTiles[x+1, y+1].isFilled() ||
					gridTiles[x+1, y].UpRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].BottomLeft = true; 
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x + 1, y].UpRight = true; 
				}
			}  
			StateManager.current.ConstructionCost += 250;
		} else if (ConstructionMode == Constants.TrackPiece.SBendRight) {

			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles[x, y-1].UpRight ||
					gridTiles[x-1, y].BottomLeft ||
					gridTiles[x-1, y-1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].UpRight = true; 
					gridTiles [x - 1, y].BottomLeft = true; 
					gridTiles [x - 1, y - 1].setFilled(); 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x, y+1].UpLeft ||
					gridTiles[x-1, y+1].isFilled() ||
					gridTiles[x-1, y].BottomRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].UpLeft = true;
					gridTiles [x - 1, y + 1].setFilled();
					gridTiles [x - 1, y].BottomRight = true; 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x, y+1].BottomLeft ||
					gridTiles[x+1, y+1].isFilled() ||
					gridTiles[x+1, y].UpRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].BottomLeft = true;
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x + 1, y].UpRight = true;
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x, y-1].BottomRight ||
					gridTiles[x+1, y-1].isFilled() ||
					gridTiles[x+1, y].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].BottomRight = true;
					gridTiles [x + 1, y - 1].setFilled();
					gridTiles [x + 1, y].UpLeft = true;
				}
			}   
			StateManager.current.ConstructionCost += 250;
		} else if (ConstructionMode == Constants.TrackPiece.SBendLeftJunction) {

			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles [x, y - 1].BottomRight ||
					gridTiles [x + 1, y].isFilled() ||
					gridTiles [x + 1, y - 1].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x, y - 1].BottomRight = true;
					gridTiles [x + 1, y].setFilled();
					gridTiles [x + 1, y - 1].setFilled();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x, y-1].isFilled() ||
					gridTiles[x-1, y].BottomLeft ||
					gridTiles[x-1, y-1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].setFilled(); 
					gridTiles[x-1, y].BottomLeft = true; 
					gridTiles [x - 1, y - 1].setFilled();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x, y+1].UpLeft ||
					gridTiles[x-1, y].isFilled() ||
					gridTiles[x-1, y+1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].UpLeft = true; 
					gridTiles [x - 1, y].setFilled(); 
					gridTiles [x - 1, y + 1].setFilled(); 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x, y+1].isFilled() ||
					gridTiles[x+1, y+1].isFilled() ||
					gridTiles[x+1, y].UpRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].setFilled(); 
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x + 1, y].UpRight = true; 
				}
			}  
			StateManager.current.ConstructionCost += 350;
		} else if (ConstructionMode == Constants.TrackPiece.SBendRightJunction) {

			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles[x, y-1].UpRight ||
					gridTiles[x-1, y].isFilled() ||
					gridTiles[x-1, y-1].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].UpRight = true; 
					gridTiles [x - 1, y].setFilled(); 
					gridTiles [x - 1, y - 1].setFilled(); 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x, y+1].isFilled() ||
					gridTiles[x-1, y+1].isFilled() ||
					gridTiles[x-1, y].BottomRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].setFilled();
					gridTiles [x - 1, y + 1].setFilled();
					gridTiles [x - 1, y].BottomRight = true; 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x, y+1].BottomLeft ||
					gridTiles[x+1, y+1].isFilled() ||
					gridTiles[x+1, y].isFilled()) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y + 1].BottomLeft = true;
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x + 1, y].setFilled();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x, y-1].isFilled() ||
					gridTiles[x+1, y-1].isFilled() ||
					gridTiles[x+1, y].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].setFilled();
					gridTiles [x + 1, y - 1].setFilled();
					gridTiles [x + 1, y].UpLeft = true;
				}
			}         
			StateManager.current.ConstructionCost += 350;
		} else if (ConstructionMode == Constants.TrackPiece.ThreeWayJunction) {

			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles[x, y-1].isFilled() ||
					gridTiles[x-1, y-1].isFilled() ||
					gridTiles[x+1, y-1].isFilled() ||
					gridTiles[x-1,y].BottomLeft || 
					gridTiles[x+1,y].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x, y - 1].setFilled();
					gridTiles [x - 1, y - 1].setFilled();
					gridTiles [x + 1, y - 1].setFilled();
					gridTiles [x - 1, y].BottomLeft = true; 
					gridTiles [x + 1, y].UpLeft = true;
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles[x-1, y-1].isFilled() ||
					gridTiles[x-1, y].isFilled() ||
					gridTiles[x-1, y+1].isFilled() || 
					gridTiles[x,y-1].UpRight ||
					gridTiles[x,y+1].UpLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x - 1, y - 1].setFilled();
					gridTiles [x - 1, y].setFilled();
					gridTiles [x - 1, y + 1].setFilled();
					gridTiles [x, y - 1].UpRight = true; 
					gridTiles[x,y+1].UpLeft = true; 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles[x-1, y+1].isFilled() ||
					gridTiles[x, y+1].isFilled() ||
					gridTiles[x+1, y+1].isFilled() || 
					gridTiles[x-1,y].BottomRight ||
					gridTiles[x+1,y].UpRight) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x - 1, y + 1].setFilled();
					gridTiles [x, y + 1].setFilled();
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x - 1, y].BottomRight = true; 
					gridTiles[x+1,y].UpRight = true; 
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles[x+1, y-1].isFilled() ||
					gridTiles[x+1, y].isFilled() ||
					gridTiles[x+1, y+1].isFilled() || 
					gridTiles[x,y-1].BottomRight || 
					gridTiles[x,y+1].BottomLeft) 
				{
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else {
					gridTiles [x + 1, y - 1].setFilled();
					gridTiles [x + 1, y].setFilled();
					gridTiles [x + 1, y + 1].setFilled();
					gridTiles [x, y - 1].BottomRight = true; 
					gridTiles[x,y+1].BottomLeft = true; 
				}
			}   

			StateManager.current.ConstructionCost += 450;
		} else if (ConstructionMode == Constants.TrackPiece.FourWayJunction) {

			if (gridTiles[x, y-1].isFilled() ||
				gridTiles[x-1, y-1].isFilled() ||
				gridTiles[x+1, y-1].isFilled() ||
				gridTiles[x, y-2].isFilled() || 
				gridTiles[x-1,y-2].BottomRight ||
				gridTiles[x-1,y].BottomLeft ||
				gridTiles[x+1,y-2].UpRight || 
				gridTiles[x+1,y].UpLeft) 
			{
				StateManager.current.NewNotification ("Construction obstructed."); return;
			} else {
				gridTiles [x, y - 1].setFilled();
				gridTiles [x - 1, y - 1].setFilled();
				gridTiles [x + 1, y - 1].setFilled();
				gridTiles [x, y - 2].setFilled();
				gridTiles [x - 1, y - 2].BottomRight = true; 
				gridTiles [x - 1, y].BottomLeft = true; 
				gridTiles [x + 1, y - 2].UpRight = true; 
				gridTiles [x + 1, y].UpLeft = true;  
			} 

			StateManager.current.ConstructionCost += 600;
		} else if (ConstructionMode == Constants.TrackPiece.DoubleStraight) {


			if (ConstructionModeRotation == Constants.Rotation.Up) {
				if (gridTiles [x, y + 1].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x, y + 1].setFilled ();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Right) {
				if (gridTiles [x + 1, y].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x + 1, y].setFilled ();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Down) {
				if (gridTiles [x, y - 1].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x, y - 1].setFilled ();
				}
			} else if (ConstructionModeRotation == Constants.Rotation.Left) {
				if (gridTiles [x - 1, y].isFilled()) {
					StateManager.current.NewNotification ("Construction obstructed."); return;
				} else { 
					gridTiles [x - 1, y].setFilled ();
				}
			}
			StateManager.current.ConstructionCost += 250;
		}



		// Finalize the construction of track

		Debug.Log ("Built at " + x + y); 

		newConstruction.transform.SetParent (gridTiles [(int)endDragCoordinates.x, (int)endDragCoordinates.y].transform); 
		newConstruction.transform.SetAsFirstSibling (); 
		newConstruction.GetComponent<WayFinding> ().setTrackDown (ConstructionModeRotation, ConstructionMode, x, y); 
		newConstruction = null; 
	
		GridBuilder.current.MakeNewConstruction ();

		gridTiles [(int)endDragCoordinates.x, (int)endDragCoordinates.y].setFilled (); 
	}

	/// <summary>
	/// Toggles the setting of the grid colliders which detect key presses on tracks.
	/// </summary>
	/// <param name="colliderState">If set to <c>true</c> turn on colliders, turn off otherwise.</param>
	public void SetGridColliders(bool colliderState) { 
		for (int i = 0; i < gridTiles.GetLength(0); i++) {
			for (int j = 0; j < gridTiles.GetLength(1); j++) {
				foreach (Transform child in gridTiles[i,j].transform) {
					if (child.tag == "Rail") {
						if (child.GetComponent<BoxCollider> () != null) {
							child.GetComponent<BoxCollider> ().enabled = colliderState;
						} else {
							Debug.Log ("Child " + child.name); 
							child.GetComponentInChildren<BoxCollider> ().enabled = colliderState;
						}
						break; 
					}
				}
			}
		}
	} 

	#region Unity Events

	/// <summary>
	/// Selects the straight track (horizontal).
	/// </summary>
	public void SelectStraightHorizontal() { 
		ConstructionMode = Constants.TrackPiece.StraightHorizontal;
		startDragCoordinates = new Vector2Int (-1, -1); 
	}

	/// <summary>
	/// Selects the straight track (vertical).
	/// </summary>
	public void SelectStraightVertical() { 
		ConstructionMode = Constants.TrackPiece.StraightVertical;
		startDragCoordinates = new Vector2Int (-1, -1); 
	}

	/// <summary>
	/// Selects the curved track.
	/// </summary>
	public void SelectCurve() { 
		ConstructionMode = Constants.TrackPiece.Curve;
	}

	/// <summary>
	/// Selects the left s-bend track.
	/// </summary>
	public void SelectSBendLeft() { 
		ConstructionMode = Constants.TrackPiece.SBendLeft;
	}

	/// <summary>
	/// Selects the right s-bend track.
	/// </summary>
	public void SelectSBendRight() { 
		ConstructionMode = Constants.TrackPiece.SBendRight;
	}

	/// <summary>
	/// Selects the left s-bend junction track.
	/// </summary>
	public void SelectSBendLeftJunction() { 
		ConstructionMode = Constants.TrackPiece.SBendLeftJunction;
	}

	/// <summary>
	/// Selects the right s-bend junction track.
	/// </summary>
	public void SelectSBendRightJunction() { 
		ConstructionMode = Constants.TrackPiece.SBendRightJunction;
	}

	/// <summary>
	/// Selects the three way junction track.
	/// </summary>
	public void SelectThreeWayJunction() { 
		ConstructionMode = Constants.TrackPiece.ThreeWayJunction;
	}

	/// <summary>
	/// Selects the four way junction track.
	/// </summary>
	public void SelectFourWayJunction() { 
		ConstructionMode = Constants.TrackPiece.FourWayJunction;
	}

	/// <summary>
	/// Selects the straight track with signal
	/// </summary>
	public void SelectDoubleStraight() { 
		ConstructionMode = Constants.TrackPiece.DoubleStraight; 
	} 

	#endregion

}
