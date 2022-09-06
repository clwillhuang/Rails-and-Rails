using System.Collections.Generic;

public class Level { 

	public int[,] tileMap;

	int trainNumbers; 

	List < EntryPointInfo > entryPoints;

	public EntryPointInfo GetEntryPoint(int index) {
		if (index < entryPoints.Count && index >= 0) {
			entryPoints [index].ResetTrainIndex (); 
			return entryPoints [index]; 
		} else {
			return null;
		}
	} 

	public int EntryPointsSize() { 
		return entryPoints.Count ; 
	}

	/// <summary>
	/// Number of desired tile rows in this level. 
	/// </summary>
	public int desiredRows;

	/// <summary>
	/// Number of desired tile columns in this level. 
	/// </summary>
	public int desiredColumns; 

	/// <summary>
	/// Average score expected for the player in the construction mode. (in dollars) 
	/// </summary>
	public float constructionBenchMark;

	/// <summary>
	/// Average time expected for the player to simulate the level successfully  (in seconds)
	/// </summary>
	public float simulationTimeBenchMark;

	/// <summary>
	/// Player score penalty for derailing a train 
	/// </summary>
	public float derailPenalty;

	/// <summary>
	/// Player score penalty for misdirecting a train to the wrong point. 
	/// </summary>
	public float misdirectPenalty;

	/// <summary>
	/// The highest score achieved by the player. (0-3f)
	/// </summary>
	public float playerHighScore;

	/// <summary>
	/// Has the player successfully completed the level.
	/// </summary>
	public bool successfullyCompleted;

	/// <summary>
	/// Required score to successfully complete this level.  (in star rating 0-3)
	/// </summary>
	public float scoreRequirement; 

	/// <summary>
	/// Returns the level info for a given level. 
	/// </summary>
	/// <returns>The level.</returns>
	/// <param name="levelNumber">Level number.</param>
	public Level (int levelNumber) { 

		entryPoints = new List<EntryPointInfo> (); 

		desiredRows = 19;
		desiredColumns = 19;
		successfullyCompleted = false;
		misdirectPenalty = 0.25f;
		derailPenalty = 0.4f;
		playerHighScore = 0f;
		scoreRequirement = 1f;

		if (levelNumber == -1) {
			tileMap = new int [19, 19] {
				{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 3, 2, 2, 2, 2, 2 },
				{ 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 2, 2, 2, 2 },
				{ 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 3, 0, 2, 2, 2, 3 },
				{ 2, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 0, 2, 2, 3 },
				{ 2, 0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 0, 1, 3 },
				{ 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 1, 0 },
				{ 2, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 2, 0, 3, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0 },
				{ 2, 0, 3, 1, 3, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 2, 0, 0, 0 },
				{ 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 3, 0, 0, 1, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 3, 3, 3, 0, 1, 3, 3 },
				{ 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 3, 3, 0, 1, 3, 3 },
				{ 0, 0, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 1, 0, 0, 0, 3 },
				{ 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 3, 3, 1, 0, 0, 0, 0, 0, 0 },
				{ 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 3, 3, 1, 0, 0, 0, 0, 0, 0 },
				{ 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 3, 3, 1, 0, 0, 0, 0, 0, 0 }
			}; 
		}

		// Level 1
		if (levelNumber == 0) {

			constructionBenchMark = 2500f;
			simulationTimeBenchMark = 40f;

			entryPoints.Add (new EntryPointInfo (6, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain(1, 4, 2); 

			entryPoints.Add (new EntryPointInfo (2, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain(2, 4, 1); 

			tileMap = new int [19, 19] {
				{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 3, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0 },
				{ 0, 0, 3, 1, 3, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 2, 0, 0, 0 },
				{ 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 3, 0, 0, 1, 0, 0 },
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 3, 3, 3, 0, 1, 0, 0 },
				{ 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 3, 3, 0, 1, 0, 0 },
				{ 0, 0, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 3, 3, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 3, 3, 1, 0, 0, 0, 0 }
			}; 

		}

		// Level 2
		if (levelNumber == 1) {

			constructionBenchMark = 3000f;
			simulationTimeBenchMark = 60f;

			entryPoints.Add (new EntryPointInfo (2, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain(1, 5, 2); 

			entryPoints.Add (new EntryPointInfo (2, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain(2, 4, 1);  

			entryPoints.Add (new EntryPointInfo (8, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (3, 3, 2); 

			tileMap = new int [19, 19] {
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 3, 3, 1, 1, 1, 2, 3, 0, 0, 1, 1, 0, 0, 0 },
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 3, 3, 1, 1, 1, 0, 3, 0, 0, 1, 1, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 1, 3, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0 },
				{ 0, 0, 0, 1, 0, 1, 3, 1, 1, 1, 0, 3, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 1, 3, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 2, 0, 1, 3, 1, 2, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 0, 1, 1, 1, 2, 2, 0, 0, 2, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 2, 0, 2, 1, 2, 2, 2, 0, 0, 0, 2, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 0, 0, 1, 2, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 1, 1, 1, 3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 2, 1, 3, 3, 0, 0, 1, 1, 0, 2, 1, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 2, 2, 0, 3, 0, 0, 1, 1, 0, 2, 2, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 2, 1, 3, 3, 0, 0, 1, 1, 0, 2, 1, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 2, 2, 0, 3, 0, 0, 1, 1, 0, 2, 2, 1, 3, 0, 0 }
			}; 
		}

		// Level 3
		if (levelNumber == 2) {

			constructionBenchMark = 4000f;
			simulationTimeBenchMark = 100f;

			entryPoints.Add (new EntryPointInfo (2, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain(1, 5, 2); 

			entryPoints.Add (new EntryPointInfo (2, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain(2, 4, 1);  
			entryPoints [1].addTrain(3, 4, 1);  

			entryPoints.Add (new EntryPointInfo (8, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (4, 3, 1); 

			tileMap = new int [19, 19] {
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 3, 3, 1, 1, 1, 0, 3, 0, 0, 1, 1, 0, 0, 0 },
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 3, 3, 1, 1, 1, 0, 3, 0, 0, 1, 1, 0, 2, 1 },
				{ 0, 0, 0, 1, 1, 1, 3, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1 },
				{ 0, 0, 0, 1, 0, 1, 3, 1, 1, 1, 0, 3, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 0, 1, 3, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 2, 0, 1, 3, 1, 3, 3, 1, 0, 0, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 0, 1, 1, 1, 3, 3, 1, 0, 1, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 2, 0, 2, 1, 3, 3, 3, 2, 0, 0, 2, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1, 0, 0, 1, 2, 1, 2, 2, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 2, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 0, 1, 1, 1, 3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 3, 1, 3, 3, 1, 0, 1, 1, 0, 1, 1, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 3, 1, 0, 3, 1, 0, 1, 1, 0, 1, 2, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 3, 1, 3, 3, 1, 0, 1, 1, 0, 1, 1, 1, 3, 0, 0 },
				{ 0, 0, 0, 1, 3, 1, 0, 3, 1, 0, 1, 1, 0, 1, 1, 1, 3, 0, 0 }
			}; 
		}

		// Level 4
		if (levelNumber == 3) {

			constructionBenchMark = 5000f;
			simulationTimeBenchMark = 120f;

			entryPoints.Add (new EntryPointInfo (3, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain(1, 5, 3); 

			entryPoints.Add (new EntryPointInfo (4, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain(2, 4, 1);  
			entryPoints [1].addTrain(3, 4, 1);  

			entryPoints.Add (new EntryPointInfo (9, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (4, 3, 2); 
			entryPoints [2].addTrain (5, 3, 1); 

			tileMap = new int [19, 19] {
				{ 0, 0, 0, 2, 2, 3, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0}, 
				{ 0, 0, 0, 2, 2, 3, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 0, 2, 2, 3, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 3, 3, 0, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 3, 0, 0},
				{ 0, 0, 2, 0, 3, 3, 2, 2, 2, 2, 0, 2, 2, 2, 2, 3, 0, 0, 0},
				{ 0, 0, 2, 2, 3, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 0, 0, 0},
				{ 0, 0, 2, 2, 3, 1, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0},
				{ 0, 0, 2, 2, 3, 1, 2, 2, 1, 1, 2, 2, 2, 3, 0, 0, 2, 0, 0},
				{ 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 2, 1, 0, 1, 0, 2, 0, 0},
				{ 0, 0, 0, 1, 2, 1, 0, 0, 2, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 3, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 0, 2, 2, 2, 2, 0, 1, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 2, 2, 3, 0, 0, 1, 1, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 1, 3, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 1, 3, 0, 1, 1, 2, 1, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 1, 3, 0, 1, 1, 2, 1, 2, 2, 2, 2, 2, 0, 0},
				{ 0, 0, 2, 2, 2, 1, 3, 0, 1, 1, 2, 1, 2, 2, 2, 2, 2, 0, 0},
			}; 
		}

		// Level 5
		if (levelNumber == 4) {

			constructionBenchMark = 6000f;
			simulationTimeBenchMark = 250f;

			entryPoints.Add (new EntryPointInfo (10, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 5, 3); 
			entryPoints [0].addTrain (2, 3, 2); 
			entryPoints [0].addTrain (8, 4, 4); 

			entryPoints.Add (new EntryPointInfo (4, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain (3, 9, 4);  
			entryPoints [1].addTrain (4, 3, 1);  

			entryPoints.Add (new EntryPointInfo (9, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (5, 3, 1); 
			entryPoints [2].addTrain (6, 3, 4); 

			entryPoints.Add (new EntryPointInfo (17, 5, Constants.Rotation.Up, 4)); 
			entryPoints [3].addTrain (7, 5, 2); 

			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 0 
				{ 1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 1 
				{ 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 2
				{ 2, 1, 0, 1, 1, 3, 3, 1, 1, 1, 2, 3, 0, 0, 1, 1, 0, 0, 0 }, // 3
				{ 2, 1, 0, 1, 1, 1, 3, 1, 0, 1, 2, 0, 0, 0, 1, 1, 0, 0, 0 }, // 4
				{ 2, 0, 1, 1, 0, 1, 3, 1, 1, 1, 0, 3, 0, 0, 0, 1, 1, 0, 0 }, // 5
				{ 2, 2, 1, 0, 0, 0, 1, 3, 1, 1, 1, 0, 0, 0, 0, 0, 3, 3, 1 }, // 6
				{ 1, 2, 1, 1, 0, 0, 1, 3, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1 }, // 7
				{ 1, 3, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 2, 1, 0, 0, 0, 0 }, // 8 
				{ 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0 }, // 9
				{ 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 12
				{ 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 13
				{ 0, 0, 1, 0, 1, 1, 1, 3, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 1, 2, 1, 3, 3, 1, 1, 1, 1, 0, 2, 1, 1, 3, 0, 0 }, // 15
				{ 0, 1, 2, 2, 0, 3, 1, 0, 1, 1, 0, 2, 2, 1, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 17
				{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 18
			}; 
		}

		// Level 6
		if (levelNumber == 5) {

			constructionBenchMark = 8000f;
			simulationTimeBenchMark = 300f;

			entryPoints.Add (new EntryPointInfo (2, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 5, 2); 
			entryPoints [0].addTrain (2, 4, 3); 

			entryPoints.Add (new EntryPointInfo (7, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain (3, 4, 1);  
			entryPoints [1].addTrain (4, 3, 4);  

			entryPoints.Add (new EntryPointInfo (11, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (5, 5, 1); 
			entryPoints [2].addTrain (6, 4, 2); 

			entryPoints.Add (new EntryPointInfo (14, 1, Constants.Rotation.Right, 4)); 
			entryPoints [3].addTrain (7, 4, 2); 
			entryPoints [3].addTrain (8, 4, 3);

			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 0 
				{ 1, 0, 0, 3, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 1 
				{ 0, 0, 0, 3, 0, 0, 1, 1, 1, 3, 3, 0, 0, 1, 0, 0, 0, 0, 0 }, // 2
				{ 2, 0, 0, 3, 1, 3, 3, 1, 1, 1, 3, 3, 0, 0, 1, 1, 0, 0, 0 }, // 3
				{ 2, 1, 0, 1, 1, 1, 3, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, // 4
				{ 2, 0, 1, 1, 0, 1, 3, 1, 1, 1, 0, 3, 1, 2, 0, 1, 1, 0, 0 }, // 5
				{ 2, 2, 1, 0, 0, 0, 1, 3, 1, 1, 1, 1, 1, 0, 0, 0, 3, 3, 1 }, // 6
				{ 1, 2, 1, 1, 2, 0, 1, 3, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1 }, // 7
				{ 1, 3, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 2, 1, 0, 0, 0, 0 }, // 8 
				{ 0, 0, 0, 0, 2, 0, 2, 1, 2, 1, 0, 1, 1, 0, 2, 0, 0, 0, 0 }, // 9
				{ 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 2, 1, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 0, 2, 0, 0, 2, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 12
				{ 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 13
				{ 0, 0, 1, 0, 1, 1, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 1, 2, 1, 3, 3, 1, 1, 1, 1, 0, 2, 1, 1, 3, 0, 0 }, // 15
				{ 0, 1, 2, 2, 0, 3, 1, 0, 1, 1, 0, 2, 2, 1, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 17
				{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0 }, // 18
			}; 
		}

		// Level 7
		if (levelNumber == 6) {

			constructionBenchMark = 8000f;
			simulationTimeBenchMark = 320f;

			entryPoints.Add (new EntryPointInfo (4, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 5, 2); 
			entryPoints [0].addTrain (2, 6, 3); 

			entryPoints.Add (new EntryPointInfo (2, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain (3, 4, 1);  
			entryPoints [1].addTrain (4, 3, 4);  
			entryPoints [1].addTrain (5, 3, 3); 
			entryPoints [1].addTrain (6, 3, 1); 

			entryPoints.Add (new EntryPointInfo (11, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (7, 5, 1); 
			entryPoints [2].addTrain (8, 4, 2); 

			entryPoints.Add (new EntryPointInfo (15, 1, Constants.Rotation.Right, 4)); 
			entryPoints [3].addTrain (9, 3, 2); 
			entryPoints [3].addTrain (10, 3, 3);

			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, // 0 
				{ 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, // 1 
				{ 0, 2, 0, 3, 0, 1, 0, 1, 1, 1, 0, 2, 0, 0, 1, 1, 0, 0, 0 }, // 2
				{ 2, 0, 0, 0, 0, 3, 3, 1, 2, 1, 0, 3, 0, 1, 0, 0, 1, 0, 0 }, // 3
				{ 0, 0, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0 }, // 4
				{ 2, 0, 0, 0, 2, 3, 3, 0, 0, 1, 0, 3, 0, 2, 0, 0, 0, 0, 0 }, // 5
				{ 2, 2, 0, 0, 2, 3, 0, 3, 0, 2, 0, 0, 0, 0, 0, 0, 3, 3, 1 }, // 6
				{ 0, 1, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 7
				{ 0, 3, 3, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, // 8 
				{ 0, 0, 3, 3, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 9
				{ 0, 0, 0, 3, 3, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 1, 1, 1, 0, 1, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, // 12
				{ 0, 0, 0, 0, 0, 1, 1, 0, 3, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0 }, // 13
				{ 0, 0, 0, 0, 0, 1, 0, 3, 0, 0, 1, 3, 0, 3, 0, 3, 0, 0, 0 }, // 14
				{ 0, 0, 0, 0, 2, 0, 3, 3, 0, 0, 0, 0, 0, 3, 0, 3, 3, 0, 0 }, // 15
				{ 0, 0, 2, 2, 0, 3, 0, 0, 0, 0, 0, 2, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 17
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 18
			}; 
		}

		// Level 8
		if (levelNumber == 7) {

			constructionBenchMark = 4000f;
			simulationTimeBenchMark = 400f;

			entryPoints.Add (new EntryPointInfo (3, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 5, 2); 
			entryPoints [0].addTrain (2, 4, 3);
			entryPoints [0].addTrain (3, 4, 2);

			entryPoints.Add (new EntryPointInfo (7, 1, Constants.Rotation.Right, 2)); 
			entryPoints [1].addTrain (4, 4, 4);  
			entryPoints [1].addTrain (9, 4, 3);  

			entryPoints.Add (new EntryPointInfo (10, 1, Constants.Rotation.Right, 3)); 
			entryPoints [2].addTrain (5, 4, 4);  

			entryPoints.Add (new EntryPointInfo (15, 1, Constants.Rotation.Right, 4)); 
			entryPoints [3].addTrain (6, 3, 2); 
			entryPoints [3].addTrain (7, 4, 1);  
			entryPoints [3].addTrain (8, 4, 1);  


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, 1, 2, 2 }, // 0 
				{ 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 3, 1, 2, 2 }, // 1 
				{ 2, 2, 0, 3, 0, 0, 0, 1, 1, 1, 0, 2, 0, 0, 3, 3, 2, 2, 2 }, // 2
				{ 0, 0, 0, 0, 0, 3, 3, 1, 2, 1, 0, 3, 0, 0, 0, 3, 3, 0, 0 }, // 3
				{ 0, 0, 0, 0, 0, 2, 3, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0 }, // 4
				{ 2, 0, 0, 0, 3, 2, 3, 0, 0, 1, 0, 3, 0, 2, 0, 0, 0, 0, 0 }, // 5
				{ 2, 2, 1, 0, 3, 2, 0, 3, 0, 2, 0, 0, 0, 0, 0, 1, 3, 3, 1 }, // 6
				{ 0, 0, 1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 }, // 7
				{ 0, 3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 2, 0, 0, 0, 0, 0 }, // 8 
				{ 2, 2, 2, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0 }, // 9
				{ 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 0, 1, 0, 0, 1, 1, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 3, 0, 0, 0, 1, 0, 0, 0 }, // 12
				{ 0, 3, 3, 1, 0, 0, 1, 0, 3, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, // 13
				{ 0, 3, 1, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0 }, // 15
				{ 0, 0, 3, 3, 0, 3, 0, 0, 0, 0, 0, 2, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 17
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 18
			}; 
		}

		// Level 9
		if (levelNumber == 8) {

			constructionBenchMark = 12000f;
			simulationTimeBenchMark = 440f;

			entryPoints.Add (new EntryPointInfo (3, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 3, 2); 
			entryPoints [0].addTrain (2, 4, 3);
			entryPoints [0].addTrain (3, 3, 2);

			entryPoints.Add (new EntryPointInfo (7, 17, Constants.Rotation.Left, 2)); 
			entryPoints [1].addTrain (4, 3, 4);  
			entryPoints [1].addTrain (5, 4, 3);  

			entryPoints.Add (new EntryPointInfo (1, 6, Constants.Rotation.Down, 3)); 
			entryPoints [2].addTrain (6, 6, 4);  

			entryPoints.Add (new EntryPointInfo (15, 1, Constants.Rotation.Right, 4)); 
			entryPoints [3].addTrain (7, 3, 2);  

			entryPoints.Add (new EntryPointInfo (17, 14, Constants.Rotation.Up, 5)); 
			entryPoints [4].addTrain (8, 4, 2); 
			entryPoints [4].addTrain (9, 4, 1);  


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 2, 2, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 3, 3, 1, 2, 2 }, // 0 
				{ 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 3, 1, 2, 2 }, // 1 
				{ 2, 2, 0, 3, 0, 0, 0, 1, 1, 1, 0, 2, 0, 0, 3, 3, 2, 2, 2 }, // 2
				{ 0, 0, 0, 0, 0, 3, 3, 1, 2, 1, 0, 3, 0, 0, 0, 3, 3, 0, 0 }, // 3
				{ 0, 0, 2, 2, 0, 2, 3, 0, 3, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0 }, // 4
				{ 2, 0, 0, 0, 2, 2, 3, 0, 0, 1, 0, 3, 0, 2, 0, 0, 0, 0, 0 }, // 5
				{ 2, 2, 1, 0, 2, 2, 0, 3, 0, 2, 0, 3, 3, 3, 0, 1, 3, 3, 1 }, // 6
				{ 0, 0, 1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 3, 0, 1, 0, 0, 2 }, // 7
				{ 0, 3, 0, 0, 3, 0, 0, 0, 1, 1, 0, 1, 0, 2, 0, 0, 0, 0, 2 }, // 8 
				{ 2, 2, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2 }, // 9
				{ 0, 0, 0, 0, 1, 3, 3, 2, 2, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 0, 1, 0, 3, 1, 1, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 3, 0, 0, 3, 3, 0, 0 }, // 12
				{ 0, 2, 0, 1, 0, 0, 1, 0, 3, 1, 0, 3, 1, 1, 0, 3, 3, 0, 0 }, // 13
				{ 0, 2, 1, 1, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0 }, // 15
				{ 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 2, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 17
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 18
			}; 
		}

		// Level 10
		if (levelNumber == 9) {

			constructionBenchMark = 15000f;
			simulationTimeBenchMark = 480f;

			entryPoints.Add (new EntryPointInfo (17, 10, Constants.Rotation.Up, 1)); 
			entryPoints [0].addTrain (1, 3, 3); 
			entryPoints [0].addTrain (2, 4, 4);
			entryPoints [0].addTrain (3, 3, 2);

			entryPoints.Add (new EntryPointInfo (17, 16, Constants.Rotation.Up, 2)); 
			entryPoints [1].addTrain (4, 3, 1);  
			entryPoints [1].addTrain (5, 4, 1);  

			entryPoints.Add (new EntryPointInfo (1, 6, Constants.Rotation.Down, 3)); 
			entryPoints [2].addTrain (6, 6, 4);  

			entryPoints.Add (new EntryPointInfo (1, 12, Constants.Rotation.Down, 4)); 
			entryPoints [3].addTrain (7, 3, 5);  

			entryPoints.Add (new EntryPointInfo (17, 4, Constants.Rotation.Up, 5)); 
			entryPoints [4].addTrain (8, 4, 1); 
			entryPoints [4].addTrain (9, 4, 1);  


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 0, 2 }, // 0 
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 0, 2 }, // 1 
				{ 0, 0, 0, 3, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 3, 3, 0, 0, 2 }, // 2
				{ 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 3, 3, 0, 0 }, // 3
				{ 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 3, 0, 0, 0, 0, 0 }, // 4
				{ 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0 }, // 5
				{ 0, 0, 1, 0, 0, 0, 2, 1, 0, 0, 2, 0, 0, 0, 0, 1, 3, 3, 1 }, // 6
				{ 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 1, 0, 3, 2 }, // 7
				{ 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 0, 0, 3, 2 }, // 8 
				{ 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 2 }, // 9
				{ 0, 3, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 2, 2 }, // 10
				{ 1, 3, 1, 3, 1, 0, 0, 1, 1, 0, 3, 0, 0, 0, 0, 0, 0, 1, 2 }, // 11
				{ 1, 1, 0, 3, 1, 0, 1, 1, 0, 1, 0, 1, 3, 0, 0, 0, 0, 2, 2 }, // 12
				{ 1, 0, 0, 1, 0, 0, 1, 0, 3, 1, 0, 3, 1, 0, 0, 0, 0, 2, 2 }, // 13
				{ 0, 0, 1, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0 }, // 15
				{ 0, 0, 0, 0, 0, 3, 2, 2, 1, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 0, 0, 1, 2, 2, 1, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 }, // 17
				{ 0, 0, 0, 0, 0, 1, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 }, // 18
			}; 
		}

		// Level 11
		if (levelNumber == 10) {

			constructionBenchMark = 16000f;
			simulationTimeBenchMark = 490f;

			entryPoints.Add (new EntryPointInfo (2, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 3, 3); 

			entryPoints.Add (new EntryPointInfo (9, 1, Constants.Rotation.Right, 2)); 
			entryPoints [1].addTrain (2, 5, 1);  
			entryPoints [1].addTrain (3, 5, 3);  
			entryPoints [1].addTrain (4, 5, 2);  

			entryPoints.Add (new EntryPointInfo (4, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (5, 4, 4);  
			entryPoints [2].addTrain (6, 5, 2);

			entryPoints.Add (new EntryPointInfo (1, 12, Constants.Rotation.Down, 4)); 
			entryPoints [3].addTrain (7, 6, 1);  
			entryPoints [3].addTrain (8, 6, 3);  

			entryPoints.Add (new EntryPointInfo (17, 4, Constants.Rotation.Up, 5)); 
			entryPoints [4].addTrain (9, 6, 1); 
			entryPoints [4].addTrain (10, 6, 2);  


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 0, 2 }, // 0 
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 0, 2 }, // 1 
				{ 0, 0, 0, 3, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 3, 3, 0, 0, 2 }, // 2
				{ 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 3, 3, 0, 0 }, // 3
				{ 1, 0, 0, 0, 0, 0, 1, 0, 3, 1, 0, 1, 1, 3, 0, 0, 0, 0, 0 }, // 4
				{ 1, 2, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 3, 3, 0, 0, 0, 0, 0 }, // 5
				{ 2, 2, 1, 0, 0, 0, 0, 1, 0, 2, 3, 2, 0, 0, 0, 1, 3, 3, 1 }, // 6
				{ 3, 3, 1, 0, 0, 0, 0, 1, 0, 2, 3, 2, 0, 0, 0, 1, 0, 3, 2 }, // 7
				{ 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 2, 1, 3, 3, 0, 0, 0, 3, 2 }, // 8 
				{ 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 3, 2 }, // 9
				{ 0, 3, 1, 0, 1, 0, 0, 1, 3, 0, 1, 0, 1, 0, 0, 0, 0, 2, 2 }, // 10
				{ 1, 3, 1, 0, 1, 0, 0, 1, 1, 0, 3, 0, 0, 0, 0, 0, 0, 1, 2 }, // 11
				{ 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 3, 0, 0, 0, 0, 2, 2 }, // 12
				{ 1, 0, 0, 1, 0, 0, 1, 0, 3, 1, 0, 3, 1, 0, 0, 0, 0, 2, 2 }, // 13
				{ 0, 0, 1, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0 }, // 15
				{ 0, 0, 0, 1, 0, 3, 2, 2, 1, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 1, 0, 1, 2, 2, 1, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 }, // 17
				{ 0, 0, 0, 0, 0, 1, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 }, // 18
			}; 
		}

		// Level 12
		if (levelNumber == 11) {

			constructionBenchMark = 17000f;
			simulationTimeBenchMark = 500f;

			entryPoints.Add (new EntryPointInfo (4, 1, Constants.Rotation.Right, 1)); 
			entryPoints [0].addTrain (1, 3, 6); 

			entryPoints.Add (new EntryPointInfo (14, 1, Constants.Rotation.Right, 2)); 
			entryPoints [1].addTrain (2, 2, 6);  
			entryPoints [1].addTrain (3, 3, 6);  
			entryPoints [1].addTrain (4, 6, 6);  

			entryPoints.Add (new EntryPointInfo (16, 17, Constants.Rotation.Left, 3)); 
			entryPoints [2].addTrain (5, 6, 6);  

			entryPoints.Add (new EntryPointInfo (1, 13, Constants.Rotation.Down, 4)); 
			entryPoints [3].addTrain (7, 4, 6);  
			entryPoints [3].addTrain (8, 4, 6);  

			entryPoints.Add (new EntryPointInfo (4, 17, Constants.Rotation.Left, 5)); 
			entryPoints [4].addTrain (9, 4, 6); 

			entryPoints.Add (new EntryPointInfo (17, 14, Constants.Rotation.Up, 6)); 
			entryPoints [5].addTrain (10, 3, 1); 
			entryPoints [5].addTrain (11, 2, 1);  


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 1, 1, 0, 2, 2, 2, 2, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2 }, // 0 
				{ 0, 1, 0, 0, 2, 2, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2 }, // 1 
				{ 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 2 }, // 2
				{ 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, // 3
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 3, 0, 0 }, // 4
				{ 3, 3, 0, 0, 0, 0, 2, 2, 2, 2, 2, 3, 0, 1, 1, 1, 3, 0, 0 }, // 5
				{ 0, 3, 0, 0, 0, 0, 2, 2, 0, 2, 2, 3, 0, 1, 2, 0, 0, 0, 1 }, // 6
				{ 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 2, 1, 1, 0, 0, 0, 2 }, // 7
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 1, 1, 1, 0, 0, 2 }, // 8 
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 1, 1, 0, 3, 2 }, // 9
				{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, 1, 0, 3, 2 }, // 10
				{ 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 3, 2 }, // 11
				{ 0, 0, 1, 1, 0, 0, 0, 3, 1, 1, 2, 2, 0, 0, 1, 1, 0, 0, 2 }, // 12
				{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 1, 3, 3, 0, 2 }, // 13
				{ 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 3, 3, 3, 3, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, // 15
				{ 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 1, 0, 0, 0, 0, 0 }, // 16
				{ 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 1, 0, 1, 1, 0, 0, 1, 1, 3 }, // 17
				{ 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 1, 1, 3 }, // 18
			}; 
		}

		// Level 13
		if (levelNumber == 12) {

			constructionBenchMark = 18000f;
			simulationTimeBenchMark = 550f;

			entryPoints.Add (new EntryPointInfo (1, 2, Constants.Rotation.Down, 1)); 
			entryPoints [0].addTrain (1, 5, 3);
			entryPoints [0].addTrain (2, 4, 3);

			entryPoints.Add (new EntryPointInfo (1, 13, Constants.Rotation.Down, 2)); 
			entryPoints [1].addTrain (3, 2, 6);  
			entryPoints [1].addTrain (4, 3, 1);  
			entryPoints [1].addTrain (5, 3, 1);  

			entryPoints.Add (new EntryPointInfo (7, 1, Constants.Rotation.Right, 3)); 
			entryPoints [2].addTrain (6, 6, 4);  
			entryPoints [2].addTrain (7, 5, 1);

			entryPoints.Add (new EntryPointInfo (14, 1, Constants.Rotation.Right, 4)); 
			entryPoints [3].addTrain (8, 3, 1);  
			entryPoints [3].addTrain (9, 3, 3);  

			entryPoints.Add (new EntryPointInfo (12, 17, Constants.Rotation.Left, 5)); 
			entryPoints [4].addTrain (10, 4, 1); 
			entryPoints [4].addTrain (11, 4, 1); 
			entryPoints [4].addTrain (12, 4, 1); 

			entryPoints.Add (new EntryPointInfo (3, 17, Constants.Rotation.Left, 6)); 
			entryPoints [5].addTrain (13, 3, 2); 


			tileMap = new int [19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 0, 0, 0, 1, 0, 0, 2, 0, 0, 3, 1, 2, 1, 0, 3, 3, 1, 0, 2 }, // 0 
				{ 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 1, 1, 1, 0, 3, 3, 1, 0, 2 }, // 1 
				{ 0, 0, 0, 3, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 3, 3, 0, 0, 2 }, // 2
				{ 0, 0, 3, 3, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 3, 3, 0, 0 }, // 3
				{ 0, 0, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 3, 1, 3, 3, 0, 0 }, // 4
				{ 0, 0, 0, 1, 0, 2, 3, 0, 0, 0, 0, 1, 3, 3, 3, 0, 3, 0, 0 }, // 5
				{ 0, 0, 1, 1, 0, 2, 2, 1, 3, 3, 0, 0, 1, 2, 0, 1, 3, 3, 1 }, // 6
				{ 0, 0, 1, 1, 1, 0, 2, 1, 0, 0, 1, 0, 0, 3, 0, 1, 0, 3, 2 }, // 7
				{ 2, 3, 0, 3, 0, 1, 2, 2, 0, 3, 1, 1, 0, 0, 0, 0, 0, 3, 2 }, // 8 
				{ 2, 0, 3, 1, 0, 0, 0, 0, 0, 3, 0, 2, 2, 0, 1, 0, 0, 3, 0 }, // 9
				{ 2, 3, 1, 0, 3, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 }, // 10
				{ 1, 3, 1, 0, 3, 0, 0, 3, 1, 0, 3, 1, 1, 1, 3, 1, 0, 1, 0 }, // 11
				{ 1, 1, 0, 0, 1, 1, 2, 3, 1, 1, 0, 1, 3, 1, 3, 1, 0, 0, 0 }, // 12
				{ 1, 0, 0, 1, 0, 0, 1, 0, 3, 1, 0, 3, 1, 1, 1, 0, 0, 2, 2 }, // 13
				{ 0, 0, 1, 1, 1, 0, 0, 3, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0 }, // 14
				{ 0, 0, 0, 0, 0, 0, 3, 3, 2, 0, 1, 0, 0, 0, 0, 0, 3, 1, 1 }, // 15
				{ 0, 0, 2, 0, 0, 3, 2, 2, 1, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0 }, // 16
				{ 0, 0, 2, 3, 0, 1, 0, 0, 0, 0, 0, 2, 2, 0, 2, 1, 0, 0, 0 }, // 17
				{ 0, 0, 2, 3, 0, 1, 0, 0, 0, 0, 0, 2, 2, 0, 2, 2, 0, 0, 0 }, // 18
			}; 
		}
			
		// Level 14
		if (levelNumber == 13)
		{

			constructionBenchMark = 19000f;
			simulationTimeBenchMark = 600f;

			entryPoints.Add(new EntryPointInfo(3, 1, Constants.Rotation.Right, 1));
			entryPoints[0].addTrain(1, 5, 5);
			entryPoints[0].addTrain(2, 5, 2);
			entryPoints[0].addTrain(3, 4, 3);

			entryPoints.Add(new EntryPointInfo(9, 1, Constants.Rotation.Right, 2));
			entryPoints[1].addTrain(4, 4, 1);
			entryPoints[1].addTrain(5, 3, 4);
			entryPoints[1].addTrain(6, 5, 3);

			entryPoints.Add(new EntryPointInfo(15, 1, Constants.Rotation.Right, 3));
			entryPoints[2].addTrain(7, 4, 4);

			entryPoints.Add(new EntryPointInfo(3, 17, Constants.Rotation.Left, 4));
			entryPoints[3].addTrain(8, 5, 5);
			entryPoints[3].addTrain(9, 4, 3);

			entryPoints.Add(new EntryPointInfo(10, 17, Constants.Rotation.Left, 5));
			entryPoints[4].addTrain(10, 6, 2);
			entryPoints[4].addTrain(11, 5, 3);
			entryPoints[4].addTrain(12, 5, 4);

			entryPoints.Add(new EntryPointInfo(15, 17, Constants.Rotation.Left, 6));
			entryPoints[5].addTrain(13, 5, 2);


			tileMap = new int[19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 0, 2, 1, 2, 2, 2, 2 }, // 0
				{ 2, 1, 0, 1, 1, 3, 3, 1, 1, 1, 1, 3, 2, 1, 2, 1, 2, 2, 1 }, // 1
				{ 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 3, 1 }, // 2
				{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, // 3
				{ 3, 0, 0, 0, 1, 1, 1, 1, 0, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, // 4
				{ 2, 1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 1, 1, 0, 0, 0, 0 }, // 5
				{ 2, 1, 1, 0, 3, 0, 0, 0, 1, 2, 0, 0, 3, 0, 0, 0, 0, 0, 1 }, // 6
				{ 2, 2, 1, 0, 1, 3, 3, 2, 3, 0, 2, 1, 3, 2, 0, 0, 0, 3, 1 }, // 7
				{ 2, 2, 2, 0, 1, 3, 3, 0, 3, 0, 0, 1, 3, 0, 0, 0, 0, 3, 3 }, // 8
				{ 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 2, 1, 3 }, // 9
				{ 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 10
				{ 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 2, 0, 2, 0, 1, 0, 0, 2, 2 }, // 11
				{ 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 3 }, // 12
				{ 0, 0, 1, 0, 0, 1, 1, 0, 3, 0, 3, 1, 1, 1, 0, 0, 0, 3, 2 }, // 13
				{ 0, 0, 1, 0, 0, 0, 1, 0, 0, 3, 1, 0, 1, 1, 0, 0, 0, 0, 1 }, // 14
				{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 1, 1, 0, 0, 1, 1, 3, 0, 0 }, // 15
				{ 2, 2, 2, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 3, 1, 0 }, // 16
				{ 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, // 17
				{ 2, 2, 2, 2, 1, 0, 0, 0, 0, 1, 1, 3, 0, 0, 1, 1, 0, 0, 1 } // 18
			};
		}


		// Level 15
		if (levelNumber == 14)
		{

			constructionBenchMark = 20000f;
			simulationTimeBenchMark = 660f;

			entryPoints.Add(new EntryPointInfo(9, 1, Constants.Rotation.Right, 1));
			entryPoints[0].addTrain(1, 5, 2);
			entryPoints[0].addTrain(2, 3, 5);
			entryPoints[0].addTrain(3, 4, 6);

			entryPoints.Add(new EntryPointInfo(1, 3, Constants.Rotation.Down, 2));
			entryPoints[1].addTrain(4, 9, 6);
			entryPoints[1].addTrain(5, 4, 6);

			entryPoints.Add(new EntryPointInfo(1, 13, Constants.Rotation.Down, 3));
			entryPoints[2].addTrain(6, 3, 7);
			entryPoints[2].addTrain(7, 3, 7);
			entryPoints[2].addTrain(8, 3, 6);
			entryPoints[2].addTrain(9, 3, 6);

			entryPoints.Add(new EntryPointInfo(4, 17, Constants.Rotation.Left, 4));
			entryPoints[3].addTrain(10, 3, 2);
			entryPoints[3].addTrain(11, 4, 3);

			entryPoints.Add(new EntryPointInfo(11, 17, Constants.Rotation.Left, 5));
			entryPoints[4].addTrain(12, 3, 2);
			entryPoints[4].addTrain(13, 4, 3);

			entryPoints.Add(new EntryPointInfo(15, 17, Constants.Rotation.Left, 6));
			entryPoints[5].addTrain(14, 3, 2);
			entryPoints[5].addTrain(15, 3, 4);

			entryPoints.Add(new EntryPointInfo(17, 8, Constants.Rotation.Up, 7));
			entryPoints[6].addTrain(16, 3, 5);
			entryPoints[6].addTrain(17, 3, 4);
			entryPoints[6].addTrain(18, 4, 5);

			tileMap = new int[19, 19] {
				//0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
				{ 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 2, 2, 2, 2 }, // 0
				{ 2, 1, 0, 0, 1, 3, 3, 1, 1, 1, 0, 3, 0, 0, 1, 1, 2, 2, 1 }, // 1
				{ 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 3, 1 }, // 2
				{ 1, 0, 0, 1, 1, 3, 3, 1, 1, 1, 0, 3, 0, 0, 1, 1, 0, 0, 3 }, // 3
				{ 3, 3, 3, 1, 1, 1, 3, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, // 4
				{ 2, 1, 0, 1, 0, 1, 3, 1, 1, 1, 0, 3, 0, 1, 1, 0, 0, 0, 0 }, // 5
				{ 2, 1, 1, 0, 0, 0, 1, 3, 1, 1, 1, 0, 3, 0, 1, 0, 0, 0, 1 }, // 6
				{ 2, 2, 1, 1, 0, 0, 0, 3, 1, 0, 1, 1, 3, 1, 1, 0, 0, 3, 1 }, // 7
				{ 2, 2, 2, 1, 1, 0, 0, 0, 1, 0, 0, 1, 3, 0, 1, 0, 0, 3, 3 }, // 8
				{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 2, 0, 1, 0, 0, 2, 0, 3 }, // 9
				{ 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 2, 2, 1, 1, 0, 0, 2, 1 }, // 10
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, // 11
				{ 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 3 }, // 12
				{ 0, 0, 1, 2, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2 }, // 13
				{ 0, 0, 1, 0, 1, 1, 1, 3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 }, // 14
				{ 3, 0, 0, 1, 2, 1, 3, 3, 1, 0, 1, 1, 0, 0, 1, 1, 3, 0, 0 }, // 15
				{ 2, 2, 2, 1, 0, 1, 3, 3, 1, 0, 1, 1, 0, 0, 0, 1, 3, 1, 0 }, // 16
				{ 2, 2, 2, 2, 2, 0, 3, 3, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, // 17
				{ 2, 2, 2, 2, 1, 0, 0, 0, 0, 1, 1, 3, 0, 0, 1, 1, 0, 0, 1 } // 18
			};
		}

	} 

} 
