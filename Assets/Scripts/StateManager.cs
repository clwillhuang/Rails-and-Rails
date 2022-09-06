using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {

	public Constants.SelectionMode selectionMode;

	public Constants.SelectionMode SelectionMode {
		get {
			return selectionMode; 
		}
		set { 

			Debug.Log ("Selection mode set to " + value);

			if (value != Constants.SelectionMode.Menu) {
				CameraMovement.CameraOn = true;
			} else {
				CameraMovement.CameraOn = false;
			}

			if (selectionMode == Constants.SelectionMode.Demolish) {
				GridBuilder.current.SetGridColliders (false); 
				BulldozerWarning.SetActive (false); 
			}

			if (value == Constants.SelectionMode.Previous) {
				selectionMode = previousSelectionMode; 
			} else {
				previousSelectionMode = selectionMode; 
				selectionMode = value;
				if (selectionMode == Constants.SelectionMode.Demolish) {
					GridBuilder.current.SetGridColliders (true); 
					BulldozerWarning.SetActive (true);
					GameObject.Find ("Rev Sound").GetComponent<AudioSource> ().Play (); 
				}
			}
			modeText.text = "State: " + selectionMode; 
		}
	}

	Constants.SelectionMode previousSelectionMode; 

	#region UI Element Caching for quick retrieval

	public GameObject constructionBackGround, simulationBackGround, endLevelScreen, levelSelectionScreen, mainMenuScreen, instructionScreen;

	public GameObject continueButton;

	public GameObject simulateButton;

	public Text modeText; 

	public GameObject BulldozerWarning;

	#endregion

	#region Prefab Initializations

	/// <summary>
	/// Prefabs for EntryPoint
	/// </summary>
	public GameObject BoxCarGO, ContainerCarGO, HopperCarGO, OilTankGO, EngineGO, baseCar;

	/// <summary>
	/// Prefabs for Junction
	/// </summary>
	public Mesh leftArrow, straightArrow, rightArrow; 


	#endregion 

	#region Clock

	/// <summary>
	/// Is the clock currently on? 
	/// </summary>
	private bool clockOn;

	/// <summary>
	/// The current time 
	/// </summary>
	private float clockTime;

	/// <summary>
	/// Text reference to clock.
	/// </summary>
	public Text clockText;

	#endregion

	#region Train Schedule and Entry Points

	public Transform trainSchedule;

	public GameObject uniquePointPrefab;

	public GameObject trainEntryPrefab;

	public Transform trainScheduleDisplayParent;

	public GameObject emptyPrefab;

	private List < EntryPoint > entryPoints;

	// How many entry points have had all their trains spawned?
	private int emptiedEntries; 

	/// <summary>
	/// The active trains in scene.
	/// </summary>
	private List < Train > activeTrainsInScene;

	/// <summary>
	/// Adds the entry point to the list of active entry points.
	/// </summary>
	/// <param name="newEP">New E.</param>
	public void addEntryPoint(EntryPoint newEP) {
		entryPoints.Add (newEP); 
	} 

	/// <summary>
	/// Spawns the next train in the designated entry point.
	/// </summary>
	/// <param name="entryPointID">Entry point ID.</param>
	public void SpawnNextTrain(int entryPointID) { 

		GameObject.Find ("Click Sounds").GetComponent<AudioSource> ().Play ();
		
		if (entryPointID >= entryPoints.Count || entryPointID < 0) {
			Debug.LogError ("Invalid entrypoint id: " + entryPointID); 
		}

		if (selectionMode != Constants.SelectionMode.Simulate) {
			NewNotification ("You can only spawn trains in Simulation Mode"); 
			return; 
		}

		if (entryPoints [entryPointID].isOccupied()) {
			Debug.Log ("Entry Point is already occupied!"); 
			NewNotification ("A train is already occupying the entry point!"); 
			return;
		}

		ActiveTrains++;
			
		activeTrainsInScene.Add (entryPoints [entryPointID].Spawn ()); 

		Transform entryParent = trainScheduleDisplayParent.GetChild (entryPointID); 

		Destroy (entryParent.GetChild (1).gameObject);

		Debug.Log ("Child count : " + trainScheduleDisplayParent.GetChild (entryPointID).childCount);

		if (trainScheduleDisplayParent.GetChild (entryPointID).childCount <= 3) {
			trainScheduleDisplayParent.GetChild (entryPointID).gameObject.SetActive (false); 
			emptiedEntries++; 
			if (emptiedEntries == trainScheduleDisplayParent.childCount) {
				Instantiate (emptyPrefab, trainScheduleDisplayParent); 
			}
		}

		NewNotification ("A new train has spawned!"); 

		activeTrainsInScene [activeTrainsInScene.Count - 1].BeginMovement (); 

		Canvas.ForceUpdateCanvases (); 

		return;
	}


	#endregion

	#region Score Handling

	/// <summary>
	/// Text references to text elements at the bottom of the Simulation Mode
	/// </summary>
	public Text derailText, completeText, activeText; 

	/// <summary>
	/// Reference to button
	/// </summary>
	public GameObject tryAgainButton; 

	/// <summary>
	/// Text references to construction cost text at top of screen. 
	/// </summary>
	public Text constructionCostText; 

	/// <summary>
	/// The number of total trains yet to be introduced in the level
	/// </summary>
	private int totalTrains, initialTrains;

	/// <summary>
	/// Number of derailed trains so far.
	/// </summary>
	private int derailedTrains;

	private float rating; 

	/// <summary>
	/// The number of trains that have currently derailed / misguided to wrong point in the level
	/// </summary>
	public int DerailedTrains {
		get { 
			return derailedTrains; 
		}
		set {
			derailedTrains = value; 
			if (value != 0) {
				totalTrains--;
			}
			derailText.text = "DERAILED: " + derailedTrains.ToString("N0"); 
		} 
	}

	/// <summary>
	/// Number of successfully navigated trains so far.
	/// </summary>
	private int completeTrains;

	/// <summary>
	/// The number of trains that have already cleared the level
	/// </summary>
	public int CompleteTrains {
		get { 
			return completeTrains; 
		}
		set {
			completeTrains = value; 
			if (value != 0) {
				totalTrains--; 
			}
			completeText.text = "COMPLETED: " + completeTrains.ToString("N0"); 
		} 
	}

	/// <summary>
	/// Number of active trains that have spawned in the map
	/// </summary>
	private int activeTrains; 

	/// <summary>
	/// The number of trains currently active in the level
	/// </summary>
	public int ActiveTrains {
		get { 
			return activeTrains; 
		}
		set {
			activeTrains = value; 
			activeText.text = "SPAWNED: " + activeTrains.ToString("N0"); 
		} 
	}

	private int misguideTrains; 

	public void TrainMisGuide() { 
		misguideTrains++;
		NewNotification ("A train has been misguided to another destination!"); 

		// A misguided train still counts as a derailed train with respect of UI.
		DerailedTrains++; 

		if (totalTrains <= 0) {
			GoToLevelEnd (); 
		}
	}

	/// <summary>
	/// Update the level when a train derails. 
	/// </summary>
	public void TrainDerail() { 
		DerailedTrains++; 
		NewNotification ("Train has derailed!"); 
		if (totalTrains <= 0) {
			GoToLevelEnd (); 
		}
	} 

	/// <summary>
	/// Updates the level when a train reaches a destination point. 
	/// </summary>
	/// <param name="pointID">ID of the point that a train has reached.</param>
	/// <param name="destinationID">ID of the intended destination of the train.</param>
	public void TrainComplete() { 
		CompleteTrains++; 
		NewNotification ("Train has successfully reached destination!"); 
		if (totalTrains <= 0) {
			GoToLevelEnd (); 
		}
	} 

	/// <summary>
	/// Updates the level when a train is spawned into the level. 
	/// </summary>
	public void TrainActive() { 
		ActiveTrains++;
	}

	private int constructionCost; 

	public int ConstructionCost {
		get { 
			return constructionCost; 
		}
		set { 
			constructionCost = value; 
			constructionCostText.text = "Costs: " + constructionCost.ToString("C0");
		} 
	}

	#endregion

	#region Notifications

	public Transform notificationParent;

	public GameObject notificationPrefab; 

	public void NewNotification(string displayText) { 
		GameObject newnot = Instantiate (notificationPrefab, notificationParent);

		newnot.GetComponentInChildren<Text> ().text = displayText; 
	
		Destroy (newnot, 5f); 
	} 

	#endregion

	public static StateManager _current; 

	public static StateManager current  {
		get { 
			if (_current == null) {
				_current = GameObject.FindObjectOfType<StateManager> ();
			}
			return _current;
		} 
	}

	public GameObject arrowPrefab;

	void Awake() {
		previousSelectionMode = Constants.SelectionMode.Menu;
		selectionMode = Constants.SelectionMode.Menu;  

		EntryPoint.BoxCarGO = BoxCarGO;
		EntryPoint.ContainerCarGO = ContainerCarGO;
		EntryPoint.HopperCarGO = HopperCarGO;
		EntryPoint.OilTankGO = OilTankGO;
		EntryPoint.EngineGO = EngineGO;
		EntryPoint.baseCar = baseCar;

		allLevels = new List<Level> ();
		for (int i = 0; i < 15; i++) {
			allLevels.Add (new Level(i)); 
		}

		clockOn = false; 
	} 

	void Start() { 
		Debug.Log ("Start ran!");

		ClearCanvasUI ();

		mainMenuScreen.SetActive (true);

		LoadLevel (-1); 

		Camera.main.transform.parent.position = new Vector3 (60f, 50f, 0); 
		Camera.main.transform.parent.eulerAngles = new Vector3(0f, -90f, 0f);
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localEulerAngles = new Vector3 (50f, 0f, 0); 
		CameraMovement.ResetZoom (); 
	}

	void Update() { 
		if (clockOn)  {
			clockTime += Time.deltaTime; 
			clockText.text = ((int)(clockTime / 60f)).ToString ("00") + " : " + ((int)(clockTime % 60f)).ToString ("00"); 
		}
	}

	#region LevelManager

	public Transform levelParent; 

	private Level currentLevel;

	private List<Level> allLevels; 


	public void LoadLevel(int level) { 

		Debug.Log ("Load level: " + level); 

		// Destroy all trains in the scene
		if (activeTrainsInScene != null) {
			foreach (Train train in activeTrainsInScene) {
				if (train != null) {
					Destroy (train.gameObject); 
				}
			}
		} 

		// Destroy all entry points
		if (entryPoints != null) {
			foreach (EntryPoint entryPoint in entryPoints) {
				if (entryPoint != null) {
					Destroy (entryPoint.gameObject); 
				}
			}
		}

		// Destroy all smoke gameobjects
//		GameObject[] activeSmokeGOs = GameObject.FindGameObjectsWithTag ("Smoke");
//		for (int i = 0; i < activeSmokeGOs.Length; i++) {
//			Destroy (activeSmokeGOs [i].gameObject); 
//		}

		// Reset variables 
		entryPoints = new List<EntryPoint> (); 
		activeTrainsInScene = new List<Train> (); 

		totalTrains = 0;
		DerailedTrains = 0;
		ActiveTrains = 0;
		CompleteTrains = 0; 
		misguideTrains = 0;

		emptiedEntries = 0; 

		ConstructionCost = 0; 

		if (level < -1) {
			Debug.LogError ("Invalid level reference: " + level); 
		}

		if (level == -1) {
			currentLevel = new Level (-1); 
		} else {
			currentLevel = allLevels [level];
		}

		GridBuilder.current.LoadMap (currentLevel.desiredRows, currentLevel.desiredColumns, currentLevel.tileMap); 

		foreach (Transform child in trainScheduleDisplayParent) {
			Destroy (child.gameObject); 
		}

		Vector3Int trainInfo; 

		for (int i = 0; i < currentLevel.EntryPointsSize(); i++) {

			EntryPointInfo epi = currentLevel.GetEntryPoint (i); 

			// If there are no trains originating from the point
			if (epi.GetLength() == 0) {
				GridBuilder.current.MakeEntryPoint (epi); 
				continue; 
			}

			Transform newScheduleEntry = Instantiate (uniquePointPrefab, trainScheduleDisplayParent).transform; 
			newScheduleEntry.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Point " + (i+1).ToString ("N0") + " : "; 

			for (int j = 0; j < epi.GetLength(); j++) {

				Transform newTrainEntry = Instantiate (trainEntryPrefab, newScheduleEntry).transform; 

				trainInfo = epi.getNextTrain (j, false); 

				newTrainEntry.GetComponentInChildren<Text> ().text = "to Point " + trainInfo.z.ToString ("N0") + " (" + trainInfo.y.ToString ("N0") + " cars)"; 

				newTrainEntry.SetSiblingIndex (newScheduleEntry.childCount - 2); 

				totalTrains++; 
			
			}

			int index = i; 

			newScheduleEntry.GetComponentInChildren<Button>().onClick.AddListener(delegate {
				Debug.Log("Spawned next!"); 
				SpawnNextTrain(index);
			} ); 

			GridBuilder.current.MakeEntryPoint (epi); 
		}

		initialTrains = totalTrains;

		if (level != -1) {
			GoToConstruction (); 
		}

		trainScheduleDisplayParent.GetComponent<Image> ().SetAllDirty (); 
		Canvas.ForceUpdateCanvases();
		trainScheduleDisplayParent.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();

	} 

	#endregion
		
	public void DebugMessage(string message) {
		Debug.Log (message); 
	}

	public void ToggleDemolishMode() { 
		if (SelectionMode == Constants.SelectionMode.Demolish) {
			Destroy (GridBuilder.current.newConstruction); 
			SelectionMode = Constants.SelectionMode.Previous; 
		} else {
			SelectionMode = Constants.SelectionMode.Demolish;
		}
	}

	private void ClearCanvasUI() { 

		clockText.transform.parent.gameObject.SetActive (false); 
		instructionScreen.SetActive (false); 
		mainMenuScreen.SetActive (false); 
		levelSelectionScreen.SetActive (false); 
		constructionBackGround.SetActive (false); 
		simulationBackGround.SetActive (false); 
		trainSchedule.gameObject.SetActive (false); 
		simulateButton.SetActive (false); 
		endLevelScreen.SetActive (false); 

		constructionCostText.transform.parent.gameObject.SetActive (false); 
		tryAgainButton.SetActive (false); 

		if (GridBuilder.current.newConstruction != null) {
			Destroy (GridBuilder.current.newConstruction.gameObject);
			GridBuilder.current.newConstruction = null;
		}
	} 

	public void GoToSelection() {  

		clockOn = false; 

		ClearCanvasUI ();

		if (SelectionMode != Constants.SelectionMode.Menu) {
			LoadLevel (-1); 
		}

		Camera.main.transform.parent.position = new Vector3 (60f, 50f, 0); 
		Camera.main.transform.parent.eulerAngles = new Vector3(0f, -90f, 0f);
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localEulerAngles = new Vector3 (50f, 0f, 0); 
		CameraMovement.ResetZoom (); 

		levelSelectionScreen.SetActive (true); 

		SelectionMode = Constants.SelectionMode.Menu;

		// Update the star ratings and buttons based on what the player has unlocked.
		for (int lev = 0; lev < 15; lev++) {
			// Debug.Log ("Set Player Rating to " + allLevels [lev].playerHighScore); 
			RatingSystem rs = levelParent.GetChild (lev).Find ("Image/Star Rating").GetComponent<RatingSystem> (); 
			rs.startupRating = (int)(allLevels [lev].playerHighScore + 1); 
			rs.OnEnable ();
			if (lev > 0) {
				levelParent.GetChild (lev).GetComponent<Button> ().interactable = true;
			}
		} 
	} 

	/// <summary>
	/// Go to the construction mode.
	/// </summary>
	public void GoToConstruction() { 

		ClearCanvasUI (); 

		SelectionMode = Constants.SelectionMode.Free;

		clockOn = false; 
		constructionBackGround.SetActive (true);
		trainSchedule.gameObject.SetActive (true); 
		simulateButton.SetActive (true); 

		constructionCostText.transform.parent.gameObject.SetActive (true); 

		Canvas.ForceUpdateCanvases (); 
	}

	/// <summary>
	/// From simulation mode or level end, try to go back to the construction mode. 
	/// </summary>
	public void TryAgain() { 
		ClearCanvasUI (); 

		Debug.Log ("TRY AGAIN!"); 

		// Destroy all trains in the scene
		if (activeTrainsInScene != null) {
			foreach (Train train in activeTrainsInScene) {
				if (train != null) {
					Destroy (train.gameObject); 
				}
			}
		} 

		// Reset all entry points
		if (entryPoints != null) {
			foreach (EntryPoint entryPoint in entryPoints) {
				if (entryPoint != null) {
					entryPoint.Awake (); 
				}
			}
		}

		// Destroy all smoke gameobjects
//		GameObject[] activeSmokeGOs = GameObject.FindGameObjectsWithTag ("Smoke");
//		for (int i = 0; i < activeSmokeGOs.Length; i++) {
//			Destroy (activeSmokeGOs [i].gameObject); 
//		}

		// Reset variables 
		activeTrainsInScene = new List<Train> (); 

		totalTrains = 0;
		DerailedTrains = 0;
		ActiveTrains = 0;
		CompleteTrains = 0; 
		misguideTrains = 0;

		emptiedEntries = 0; 
	
		foreach (Transform child in trainScheduleDisplayParent) {
			Destroy (child.gameObject); 
		}

		Vector3Int trainInfo; 

		for (int i = 0; i < currentLevel.EntryPointsSize(); i++) {

			EntryPointInfo epi = currentLevel.GetEntryPoint (i); 

			// If there are no trains originating from the point
			if (epi.GetLength() == 0) {
				GridBuilder.current.MakeEntryPoint (epi); 
				continue; 
			}

			Transform newScheduleEntry = Instantiate (uniquePointPrefab, trainScheduleDisplayParent).transform; 

			newScheduleEntry.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Point " + (i+1).ToString ("N0") + " : "; 

			for (int j = 0; j < epi.GetLength(); j++) {

				Transform newTrainEntry = Instantiate (trainEntryPrefab, newScheduleEntry).transform; 

				trainInfo = epi.getNextTrain (j, false); 

				newTrainEntry.GetComponentInChildren<Text> ().text = "to Point " + trainInfo.z.ToString ("N0") + " (" + trainInfo.y.ToString ("N0") + " cars)"; 

				newTrainEntry.SetSiblingIndex (newScheduleEntry.childCount - 2); 

				totalTrains++; 

			}

			int index = i; 

			newScheduleEntry.GetComponentInChildren<Button>().onClick.AddListener(delegate {
				Debug.Log("Spawned next!"); 
				SpawnNextTrain(index);
			} ); 

		}

		initialTrains = totalTrains;

		GoToConstruction (); 
	} 

	/// <summary>
	/// Go into simulation mode. 
	/// </summary>
	public void GoToSimulation() { 
		rating = 0f;
		clockOn = true; 
		clockTime = 0f; 
		SelectionMode = Constants.SelectionMode.Simulate;

		DerailedTrains = 0;
		ActiveTrains = 0;
		CompleteTrains = 0; 

		completeText.text = "COMPLETED: " + CompleteTrains.ToString ("N0");
		derailText.text = "DERAILED: " + DerailedTrains.ToString ("N0");
		activeText.text = "ACTIVE: " + ActiveTrains.ToString ("N0");

		ClearCanvasUI (); 
	
		clockText.transform.parent.gameObject.SetActive (true); 
		trainSchedule.gameObject.SetActive (true); 
		simulationBackGround.SetActive (true); 
		tryAgainButton.SetActive (true);
	}

	/// <summary>
	/// Display the level complete screen.
	/// </summary>
	public void GoToLevelEnd() { 

		clockOn = false; 

		endLevelScreen.SetActive (true); 
		continueButton.SetActive (false);
		endLevelScreen.transform.Find ("Star Rating").gameObject.SetActive (false); 

		//StartCoroutine (StarRating (3f)); 

		Text left = endLevelScreen.transform.Find ("Left").GetComponent<Text> (); 

		left.text = "CONSTRUCTION\n" +
		"\t\tTOTAL COST - " + ConstructionCost.ToString ("C0") + "\n" +
		"\n" +
		"SIMULATION\n" +
		"\t\tTRAINS CLEARED - " + CompleteTrains.ToString ("N0") + "\n" +
			"\t\tTRAINS DERAILED - " + (DerailedTrains - misguideTrains).ToString ("N0") + "\n" +
			"\t\tTRAINS MISGUIDED - " + (misguideTrains).ToString("N0") +"\n" +
		"\t\tTIME TAKEN - " + ((int)(clockTime / 60f)).ToString ("00") + " : " + ((int)(clockTime % 60f)).ToString ("00") + "\n\n" +
			"TOTAL RATING (Adjusted)\n"; 

		Text right = endLevelScreen.transform.Find ("Right").GetComponent<Text> (); 

		float scoreFromCost = Mathf.Clamp (0.75f * (currentLevel.constructionBenchMark / constructionCost), -0.5f, 0.75f); 

		float scoreFromComplete = Mathf.Clamp (0.5f + 2f * (completeTrains / initialTrains), -0.5f, 1.5f); 
		float scoreFromDerailed = Mathf.Clamp (-((DerailedTrains - misguideTrains) * currentLevel.derailPenalty), -2f, 0f); 
		float scoreFromMisguide = Mathf.Clamp (-(misguideTrains * currentLevel.misdirectPenalty), -2f, 0f); 

		float scoreFromTime = Mathf.Clamp (0.75f * (currentLevel.simulationTimeBenchMark / clockTime), -1f, 1f); 

		rating = Mathf.Clamp(scoreFromCost + scoreFromTime + scoreFromComplete + scoreFromMisguide + scoreFromDerailed, 0f, 3f); 

		/* right.text = scoreFromCost.ToString ("+2") + "\n\n\n\n" + scoreFromComplete.ToString ("+2") + "\n" +
		scoreFromDerailed.ToString ("+2") + "\n" +
		scoreFromMisguide.ToString ("+2") + "\n" +
		scoreFromTime.ToString ("+2") + "\n\n" +
			(scoreFromCost + scoreFromComplete + scoreFromDerailed + scoreFromMisguide + scoreFromTime).ToString ("+2");
*/ 

		right.text = string.Format("{0:+0.00;-0.00}", scoreFromCost) + "\n\n\n\n" + string.Format("{0:+0.00;-0.00}", scoreFromComplete) + "\n" +
			string.Format("{0:+0.00;-0.00}", scoreFromDerailed) + "\n" +
			string.Format("{0:+0.00;-0.00}",scoreFromMisguide) + "\n" +
			string.Format("{0:+0.00;-0.00}",scoreFromTime)+ "\n\n" +
			string.Format("{0:+0.00;-0.00}", rating);

		if (rating > currentLevel.playerHighScore) {
			right.text += "\n(New Best)"; 
			currentLevel.playerHighScore = rating;
		}
		if (rating > 1f) {
			currentLevel.successfullyCompleted = true;
		}

		Debug.Log ("New score of " + rating); 
			
		endLevelScreen.transform.Find ("Star Rating").GetComponent<RatingSystem> ().startupRating = Mathf.RoundToInt(rating + 1);
		endLevelScreen.transform.Find ("Star Rating").gameObject.SetActive(true); 

		continueButton.SetActive (true); 
	}

	public void GoToInstructions() { 
		ClearCanvasUI (); 

		instructionScreen.SetActive (true); 
	} 

	public void GoToMenu() { 

		ClearCanvasUI ();

		mainMenuScreen.SetActive (true); 
			
	}

	public void QuitGame() { 
		Application.Quit (); 
	}

	// Obselete
//	IEnumerator StarRating(float score) { 
//
//		star1.fillAmount = 0f;
//		star2.fillAmount = 0f;
//		star3.fillAmount = 0f;
//			
//		int star = 1;
//
//		float increment = 0.05f;
//
//		while (score > 0.001f) {
//			
//			score -= increment;
//
//			if (star == 1) {
//				star1.fillAmount += increment;
//				if (star1.fillAmount  >= 1f) {
//					star++; 
//				}
//			} else if (star == 2) {
//				star2.fillAmount += increment;
//				if (star2.fillAmount  >= 1f) {
//					star++; 
//				}
//			} else {
//				star3.fillAmount += increment; 
//				if (star3.fillAmount >= 1f) {
//					break;
//				}
//			}
//
//			yield return new WaitForSeconds (0.01f); 
//		}
//
//		continueButton.SetActive (true); 
//	}
}



