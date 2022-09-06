using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to every tile where track splits. Stores the state of the track signal. Stores all possible paths from this tile.
/// </summary>
public class Junction : MonoBehaviour { 

	public List < Path > paths;

	public int[] arrowDirections; 

	GameObject signalArrow; 

	MeshRenderer mr;

	MeshFilter mf; 

	/// <summary>
	/// The state of the signal, i.e. which path # does it indicate currently? 
	/// </summary>
	public int signalDirection; 

	/// <summary>
	/// Is the stop signal on? 
	/// </summary>
	Constants.MovementState stopSignal; 

	public void Awake() { 
		paths = new List<Path>();
		signalDirection = 0; 
		stopSignal = Constants.MovementState.Go; 
		arrowDirections = new int[3]; 
	}

	/// <summary>
	/// Returns the path currently indicated by the signal. 
	/// </summary>
	/// <returns>The currently selected path. NULL if rotation does not match, or no path exists.</returns>
	/// <param name="trainRotation">Train rotation.</param>
	/// <param name="straightTrack">If set to <c>true</c>, consider the tile as a straight track.</param>
	public Path returnPath(Constants.Rotation trainRotation, bool straightTrack) {
		// Debug.Log ("Sigdi: " + signalDirection + "   pc " + paths.Count); 
		if (signalDirection >= paths.Count) {
			Debug.Log ("No waypoint path found!");  
			signalDirection = 0; 
			return null; 
		} 

		// Different case if the track is straight
		if (straightTrack) {
			if (paths[0].entryDirection == trainRotation ) {
				return paths [0]; 
			} else if (paths[1].entryDirection == trainRotation) {
				return paths [1]; 
			} else {
				return null; 
			}
		}

		if (trainRotation != paths[signalDirection].entryDirection) {
			Debug.Log ("Improper train rotation: " + trainRotation + "   expected entry: " + paths[signalDirection].entryDirection); 
			return null; 
		}

		return paths [signalDirection]; 
	} 

	/// <summary>
	/// Increment the signal on player interaction.
	/// </summary>
	public void toggleDirection() { 
		if (paths.Count == 0) {
			Debug.LogError ("No waypoint paths in list! Tile Coordinates: "); 
		}
		signalDirection = (signalDirection + 1) % paths.Count;
		Debug.Log ("Paths count : " + paths.Count); 
		if (arrowDirections[signalDirection] == 0) {
			mf.mesh = StateManager.current.leftArrow;
		} else if (arrowDirections[signalDirection] == 1) {
			mf.mesh = StateManager.current.straightArrow; 
		} else if (arrowDirections[signalDirection] == 2) {
			mf.mesh = StateManager.current.rightArrow; 
		}
	}

	public void toggleSignal() { 
		stopSignal = (Constants.MovementState)(((int)stopSignal + 1) % 2); 
		foreach (var path in paths) {
			path.signal = stopSignal; 
		} 
		if (stopSignal == Constants.MovementState.Go) {
			mr.material.color = Color.green;
		} else {
			mr.material.color = Color.red;
		}
	}

	public void AddPath(Path newPath) { 
		int direction = newPath.signalDirection;
		if (direction >= 0 && direction <= 2) {
			arrowDirections [paths.Count] = direction;
		} else {
			Debug.LogError ("Invalid direction arrow given!"); 
			return; 
		}
		if (paths.Count == 0) {
			signalArrow = Instantiate (StateManager.current.arrowPrefab, transform); 

			mr = signalArrow.transform.GetComponentInChildren<MeshRenderer> (); 
			mf = signalArrow.transform.GetComponentInChildren<MeshFilter> (); 

			signalArrow.transform.localPosition = new Vector3 (0f, 8f, 0f); 
			signalArrow.transform.localScale = new Vector3 (15f, 15f, 20f); 
			signalArrow.transform.eulerAngles = new Vector3 (-90f, -90f + 90f * ((int)newPath.entryDirection), 0f);  
			GetComponent<GridTile> ().rotation = (int)newPath.entryDirection + 1; 

			Debug.Log ("Changing with direction " + direction); 
			if (direction == 0) {
				mf.mesh = StateManager.current.leftArrow;
			} else if (direction == 1) {
				mf.mesh = StateManager.current.straightArrow; 
			} else if (direction == 2) {
				mf.mesh = StateManager.current.rightArrow; 
			}

			mr.material.color = Color.green; 

			signalArrow.GetComponentInChildren<ArrowClick> ().SetDelegates (); 
		}
		paths.Add (newPath); 

	} 

	public void AddPathStraight(Path newPath) { 
		paths.Add (newPath); 
	} 

	private Constants.TrackPiece trackPiece; 
} 
