using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour { 

	public Transform diamond; 

	public static GameObject BoxCarGO, ContainerCarGO, HopperCarGO, OilTankGO, EngineGO, baseCar;

	public Color[] possibleColors;

	public Transform entryPointWayPoint; 

	public Transform destinationPoint; 

	Constants.EntryType entryType; 

	public EntryPointInfo info; 

	public GameObject factory; 

	int activeObjectsInCollider;

	public void Awake() {
		activeObjectsInCollider = 0; 
	}

	public void TransferInfo(EntryPointInfo _info) { 
		info = _info;
		transform.Find("GameObject/EntryDiamond").GetComponentInChildren<TextMesh>().text = info.ID.ToString("N0"); 

		Path exitPath = new Path (); 

		exitPath.wayPoints = new List<Transform> ();
		exitPath.wayPoints.Add (destinationPoint); 
		exitPath.originCoordinateX = info.originX;
		exitPath.originCoordinateY = info.originY;

		exitPath.exitDirection = info.exitRotation;
		exitPath.entryDirection = (Constants.Rotation) (((int)info.exitRotation + 2) % 4); 
		exitPath.signal = Constants.MovementState.Go;
		exitPath.signalDirection = 0; 

		GridBuilder.current.getGridTile (info.originX, info.originY).addPathToJunctionStraight(exitPath); 

		entryType = (Constants.EntryType)(Random.Range(0,4)); 

	}


	void Update() { 
		diamond.transform.parent.LookAt (new Vector3 (Camera.main.transform.position.x, diamond.transform.parent.position.y, Camera.main.transform.position.z));
	}

	public Train Spawn() { 

		Vector3Int newTrainInfo = info.getNextTrain (0, true); 

		int trainID = newTrainInfo.x;
		int trainCarLength = newTrainInfo.y;
		int traindestID = newTrainInfo.z; 

		Debug.Log ("Remaining number of trains " + info.GetLength ()); 

		Transform newTrain = new GameObject ("Train").transform; 

		Train newTrainScript = newTrain.gameObject.AddComponent<Train> (); 

		newTrain.position = entryPointWayPoint.position; 

		for (int i = 0; i < trainCarLength; i++) {
			
			GameObject newCarGO = Instantiate (baseCar, entryPointWayPoint.position, entryPointWayPoint.rotation, newTrain);

			newCarGO.transform.Translate (Vector3.forward * 13f * i); 

			GameObject newWagonGO; 
		
			if (i == 0) {		
				newWagonGO = GameObject.Instantiate (EngineGO, newCarGO.transform.Find("Front/Wagon")); 
			} else if (entryType == Constants.EntryType.Goods) {
				newWagonGO = GameObject.Instantiate (BoxCarGO, newCarGO.transform.Find("Front/Wagon")); 
			} else if (entryType == Constants.EntryType.Freight) {
				newWagonGO = GameObject.Instantiate (ContainerCarGO, newCarGO.transform.Find("Front/Wagon")); 
			} else if (entryType == Constants.EntryType.Mining) {
				newWagonGO = GameObject.Instantiate (HopperCarGO, newCarGO.transform.Find("Front/Wagon")); 
			} else if (entryType == Constants.EntryType.Oil) {
				newWagonGO = GameObject.Instantiate (OilTankGO, newCarGO.transform.Find("Front/Wagon")); 
			} else {
				newWagonGO = new GameObject ("No wagon!");
			}

			newWagonGO.transform.localPosition = new Vector3 (0f, 0f, 2.5f); 

			newWagonGO.GetComponent<Wagon> ().setTrainID (trainID); 
			newWagonGO.GetComponent<Wagon> ().setTrainDestID (traindestID); 
			newWagonGO.GetComponent<Wagon> ().setInitialOriginID (info.ID); 
			if (i == 0) {
				newWagonGO.GetComponent<Wagon> ().isFirst = true;
			}


			// Randomize a color to paint the new train car.
			Transform DynamicParent = newWagonGO.transform.Find ("Dynamic"); 
			Color randomColor = possibleColors [Random.Range (0, possibleColors.GetLength (0) - 1)]; 
			foreach (Transform childObject in DynamicParent) {
				childObject.GetComponent<MeshRenderer> ().material.color = randomColor; 
			}

			Car newCarScript = new Car (newCarGO.transform.Find ("Front"), 
				newCarGO.transform.Find ("End"), 
				newCarGO.transform.Find ("Front/Wagon"), 
				newCarGO.transform.Find ("Front/Wagon/Guide"), 
				entryPointWayPoint, 
				i == 0);

			newTrainScript.trainCars.Add(newCarScript); 


		}
		newTrainScript.SetEntryPath (info.originX, info.originY, info.exitRotation); 

		return newTrain.GetComponent<Train>();
	} 

	/// <summary>
	/// Called when a train enters the collision box.
	/// </summary>
	public void OnTriggerEnter(Collider other) { 
		activeObjectsInCollider++; 
		return; 
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	public void OnTriggerExit() { 
		activeObjectsInCollider--;
		activeObjectsInCollider = Mathf.Max (0, activeObjectsInCollider); 
		return; 
	} 

	/// <summary>
	/// Are there any objects with colliders, (i.e. trains), in the entry point? 
	/// </summary>
	/// <returns><c>true</c>, if occupied was ised, <c>false</c> otherwise.</returns>
	public bool isOccupied() { 
		return activeObjectsInCollider > 0; 
	}
		
}
	

/// <summary>
/// Helper script for entry point for better formatting. Allows to store train data.
/// </summary>
[System.Serializable]
public class EntryPointInfo { 

	private List < int > trainIDNumbers;

	private List < int > trainLength; 

	private List < int > trainDestinationIDs;

	public int originX, originY;

	public int ID;

	public Constants.Rotation exitRotation;

	private int nextTrainIndex = 0; 

	public void ResetTrainIndex() { 
		nextTrainIndex = 0; 
	}

	public EntryPointInfo(int _originX, int _originY, Constants.Rotation rotation, int _ID) { 
		trainIDNumbers = new List<int> (); 
		trainLength = new List<int> (); 
		trainDestinationIDs = new List<int> (); 
		originX = _originX;
		originY = _originY;
		exitRotation = rotation;
		ID = _ID;
	}

	public void addTrain(int newTrainID, int newTrainLength, int newTrainDestID) {
		trainIDNumbers.Add (newTrainID);
		trainLength.Add (newTrainLength);
		trainDestinationIDs.Add (newTrainDestID); 
		return; 
	} 

	/// <summary>
	/// Gets the next train. x = trainID, y = trainLength, y = trainDestID;
	/// </summary>
	/// <returns>The next train.</returns>
	/// <param name="trainIndex">Train index.</param>
	/// <param name="removeAfter">If set to <c>true</c> return the next train to be spawned, not the trainIndex.</param>
	public Vector3Int getNextTrain(int trainIndex, bool removeAfter) { 
		if (trainIndex >= trainIDNumbers.Count || trainIndex < 0) {
			Debug.Log ("Invalid index: " + trainIndex) ; 
			return Vector3Int.zero; 
		} 
		if (nextTrainIndex >= trainIDNumbers.Count || nextTrainIndex < 0) {
			Debug.Log ("Invalid next train index: " + nextTrainIndex) ; 
			return Vector3Int.zero; 
		} 
		Vector3Int nextTrain; 
		if (removeAfter) {
			nextTrain = new Vector3Int (trainIDNumbers [nextTrainIndex], trainLength [nextTrainIndex], trainDestinationIDs [nextTrainIndex]); 
			nextTrainIndex++;
		} else {
			nextTrain = new Vector3Int (trainIDNumbers [trainIndex], trainLength [trainIndex], trainDestinationIDs [trainIndex]); 
		}
		return nextTrain; 
	}

	public int GetLength() { 
		return trainLength.Count; 
	}


} 
