using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour {

	private delegate void DestructionUpdateDelegate(); 

	/// <summary>
	/// Required delegate to ensure entry points register the absence of this wagon
	/// when the wagon is unexpectedly deleted without exiting the collider
	/// </summary>
	/// <returns>The update delegate.</returns>
	private DestructionUpdateDelegate destructionUpdateDelegate; 

	/// <summary>
	/// ID unique to the train this wagon belongs to. Used to differentiate between trains. 
	/// </summary>
	private int trainID;

	public int getTrainID() { 
		return trainID; 
	}

	public void setTrainID(int _trainID) { 
		trainID = _trainID ; 
	} 

	private int trainDestID = -1;

	public bool isFirst = false; 

	private bool completed = false;

	public bool Completed { 
		get { 
			return completed; 
		}
		set {
			completed = value; 
		}
	} 

	public int getTrainDestID() { 
		return trainDestID; 
	}

	public void setTrainDestID(int _trainID) { 
		trainDestID = _trainID ; 
		if (diamond != null) {
			diamond.GetComponentInChildren<TextMesh> ().text = trainDestID.ToString("N0");
		}
	} 

	private int initialOriginID;

	public void setInitialOriginID(int _trainID) { 
		initialOriginID = _trainID ; 
	} 

	/// <summary>
	/// Has this wagon left the initial entry point yet?
	/// </summary>
	private bool leftInitialEntry; 

	private Transform diamond; 

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake() { 
		leftInitialEntry = false;
		diamond = transform.Find ("Flag"); 
	} 

	/// <summary>
	/// Automatic fixed update every physics tick. 
	/// </summary>
	public void FixedUpdate() { 
		if (diamond != null) {
			diamond.LookAt (new Vector3 (Camera.main.transform.position.x, diamond.parent.position.y, Camera.main.transform.position.z));
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerEnter(Collider other)
	{
		// Debug.Log ("Entered trigger with tag: " + other.tag); 
		if (other.tag == "Entry") {

			destructionUpdateDelegate = other.GetComponent<EntryPoint> ().OnTriggerExit; 

			Debug.Log ("Entered entry " + leftInitialEntry); 

			if (leftInitialEntry) {
				// Do not let this train derail other wagons since this has completed.
				Completed = true; 
				transform.parent.parent.parent.parent.GetComponent<Train> ().ReachedEntryPoint = true; 
			} 

			// Only the first car in the train is allowed to trigger events. 
			if (!isFirst) {
				return; 
			}

			int otherID = other.GetComponent<EntryPoint> ().info.ID; 

			Debug.Log("Entered entry point: " + otherID.ToString("N0") + " TRAIN ID " + trainID);

			// Right destination
			if (otherID == trainDestID && leftInitialEntry) {
				
				Debug.Log ("Train completed!"); 
				StateManager.current.TrainComplete (); 

			} 

			// Same destination as before
			else if (otherID == initialOriginID) {
				
				if (leftInitialEntry) {
					
					Debug.Log ("Train misguided!"); 
					StateManager.current.TrainMisGuide(); 

				}

			}

			else if (leftInitialEntry) {
				Debug.Log ("Train misguided!"); 
				StateManager.current.TrainMisGuide(); 
			}

		} else if (other.tag == "Train") {
			
			if (other.GetComponent<Wagon>().Completed) {
				Debug.Log ("Collided with inactive train!"); 
				return; 
			}

			if (completed) {
				return; 
			}
				
			int otherTrainID = other.GetComponent<Wagon>().getTrainID(); 

			Debug.Log ("Train with ID : " + otherTrainID + " collided with train with ID " + trainID); 

			Debug.Log (!transform.parent.parent.parent.parent.GetComponent<Train> ().ReachedEntryPoint); 

			if (!transform.parent.parent.parent.parent.GetComponent<Train> ().ReachedEntryPoint) {
				StateManager.current.TrainDerail (); 
			}

			transform.parent.parent.parent.parent.GetComponent<Train> ().DestroyTrain (true); 
			GameObject.Find ("Des Sound").GetComponent<AudioSource> ().Play (); 

		} else if (other.tag == "Arrow") {
			return; 
		} else if (other.tag == "Rail") {
			return; 
		} else if (other.tag == "Destroy") {
			Debug.Log ("Reached destroy endpoint"); 
			transform.parent.parent.parent.parent.GetComponent<Train> ().DestroyTrain (true); 
			return;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="other">Other.</param>
	public void OnTriggerExit(Collider other) {  
		if (other.tag == "Entry") {
			Debug.Log ("Exited " + other.GetComponent<EntryPoint> ().info.ID + " " + initialOriginID); 
			if (other.GetComponent<EntryPoint> ().info.ID == initialOriginID)
				leftInitialEntry = true; 
		} 
	}

	/// <summary>
	/// Raises the destroy event when Unity breaks this gameobject. 
	/// </summary>
	public void OnDestroy() { 
		if (destructionUpdateDelegate != null) {

			destructionUpdateDelegate.Invoke (); 

		}
	} 
}
