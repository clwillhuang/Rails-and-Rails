using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {

	public AudioSource whistleSource, trainLoopSource; 

	public float speed; 

	public float currentSpeed;

	Constants.Rotation direction; 

	public Path currentPath; 

	public List<Car> trainCars; 

	bool started = false; 

	bool reachedEntryPoint = false;

	public bool ReachedEntryPoint { 
		get {
			return reachedEntryPoint; 
		}
		set {
			reachedEntryPoint = value; 
		}

	}

	Constants.MovementState movement; 

	void Awake() { 
		movement = Constants.MovementState.Stop; 
		started = false;
		reachedEntryPoint = false; 
		speed = 0.6f;
		if (trainCars == null) {
			trainCars = new List<Car> (); 
		} 
	} 

	public void SetEntryPath(int originX, int originY, Constants.Rotation entryPathRotation) {

		currentPath = new Path ();

		originX = originX;
		originY = originY;

		if (entryPathRotation == Constants.Rotation.Up) {
			currentPath.destinationCoordinateX = originX - 1;
			currentPath.destinationCoordinateY = originY; 
		} else if (entryPathRotation == Constants.Rotation.Down) {
			currentPath.destinationCoordinateX = originX + 1;
			currentPath.destinationCoordinateY = originY; 
		} else if (entryPathRotation == Constants.Rotation.Right) {
			currentPath.destinationCoordinateX = originX;
			currentPath.destinationCoordinateY = originY + 1; 
		} else if (entryPathRotation == Constants.Rotation.Left) {
			currentPath.destinationCoordinateX = originX;
			currentPath.destinationCoordinateY = originY - 1; 
		}

		currentPath.entryDirection = entryPathRotation;
		currentPath.exitDirection = entryPathRotation; 

		direction = entryPathRotation;


		return;
	}

	public void BeginMovement() { 
		movement = Constants.MovementState.Go; 
		StartCoroutine (movementUpdate ()); 

		trainLoopSource = gameObject.AddComponent<AudioSource> (); 
		trainLoopSource.clip = (Resources.Load ("/Audio/train loop") as AudioClip);  
		trainLoopSource.loop = true; 

		if (!trainLoopSource.isPlaying) {
			trainLoopSource.Play (); 
		}
	} 

	public void AddCar(Car newCar) { 
		trainCars.Add (newCar); 
		return; 
	} 

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.V)) {
			if (started) 
				return; 
			direction = Constants.Rotation.Right;
			foreach (Transform point in currentPath.wayPoints) {
				if (point.eulerAngles.y < 0f) {
					point.eulerAngles = new Vector3 (0f, point.eulerAngles.y + 360f, 0f); 
				}
				if (point.eulerAngles.y > 360f) {
					point.eulerAngles = new Vector3 (0f, point.eulerAngles.y % 360f, 0f); 
				}
				foreach (Car car2 in trainCars) {
					car2.wayPointsFront.Add (point); 
					car2.wayPointsEnd.Add (point); 
				}
			}
			movement = Constants.MovementState.Go;
			started = true; 
			StartCoroutine (movementUpdate ()); 
		}
	}

	private IEnumerator movementUpdate() {

		bool stop = true; 

		currentSpeed = 0.05f; 

		while (stop) {


			if (movement == Constants.MovementState.Stop) { 
				
				if (currentPath.signal == Constants.MovementState.Go) {
					
					currentSpeed = 0.05f;
					movement = currentPath.signal; 

					trainLoopSource.mute = false; 
					trainLoopSource.Play (); 

					// Only allow the train to continue along previous path if the first car hasn't passed the first waypoint.
					if (!trainCars [0].completedFirst) {

						foreach (Transform point in currentPath.wayPoints) {
							foreach (Car car2 in trainCars) {
								// Debug.Log ("Added a waypoint!"); 
								car2.wayPointsFront.Remove (point); 
								car2.wayPointsEnd.Remove (point); 
								car2.AudioMute (false); 
							
							}
						}

						currentPath = GridBuilder.current.getGridTile (currentPath.originCoordinateX, currentPath.originCoordinateY).TrainEnter (currentPath.entryDirection); 

						if (currentPath == null) {
							if (!reachedEntryPoint) {
								StateManager.current.TrainDerail (); 
								DestroyTrain (true); 
								GameObject.Find ("Des Sound").GetComponent<AudioSource> ().Play (); 
							}
							break;
						}

						foreach (Transform point in currentPath.wayPoints) {
							foreach (Car car2 in trainCars) {
								// Debug.Log ("Added a waypoint!"); 
								car2.wayPointsFront.Add (point); 
								car2.wayPointsEnd.Add (point); 
							}
						}

					}
				} else {
					if (currentSpeed * 0.7f < 0.01) {
						currentSpeed = 0f; 
					} else {
						currentSpeed = Mathf.Max (0f, currentSpeed * 0.5f);
					}
					//yield return new WaitForSeconds (0.05f); 
					//continue; 
				}
			} else {
				
				currentSpeed = Mathf.Min (speed, currentSpeed * 1.4f); 
			}

			if (speed != 0f)
				
				foreach (Car car in trainCars) {

					if (car == null) {
						continue; 
					}

					car.Simulate (currentSpeed); 

					if (car.wayPointsFront.Count < 1 && car.isFirst) {
									
						if (currentPath != null) {
							direction = currentPath.exitDirection; 
						}
						else {
							if (currentPath == null) {
								DestroyTrain (true); 
								GameObject.Find ("Des Sound").GetComponent<AudioSource> ().Play (); 
								break;
							}
						}

						// GridBuilder.current.getGridTile (currentPath.destinationCoordinateX, currentPath.destinationCoordinateY).SetColor (Color.cyan); 
						currentPath = GridBuilder.current.getGridTile (currentPath.destinationCoordinateX, currentPath.destinationCoordinateY).TrainEnter (direction); 

						if (currentPath == null) {
							if (!reachedEntryPoint) {
								StateManager.current.TrainDerail (); 
								DestroyTrain (true); 
								GameObject.Find ("Des Sound").GetComponent<AudioSource> ().Play (); 
							}
							break;
						}

						foreach (Transform point in currentPath.wayPoints) {
							foreach (Car car2 in trainCars) {
								// Debug.Log ("Added a waypoint!"); 
								car2.wayPointsFront.Add (point); 
								car2.wayPointsEnd.Add (point); 

								if (currentPath.signal == Constants.MovementState.Stop) {
									car2.AudioMute (true); 
								}
							}
						}

						car.completedFirst = false; 
						
						movement = currentPath.signal; 

					} 
				}
			yield return new WaitForSeconds (0.05f); 
		}
	}

	void OnDestroy() { 
		if (trainCars != null) {
			foreach (Car car in trainCars) {
				if (car != null) {
					car.DestroySmoke (); 
				}
			}
		}
	} 

	/// <summary>
	/// Call for destruction of train, such as during derailment.
	/// </summary>
	/// <param name="overRide">If set to <c>true</c> over ride.</param>
	public void DestroyTrain(bool overRide) { 

		if (overRide || reachedEntryPoint) {
			StopAllCoroutines (); 

//			foreach (Car car in trainCars) {
//				car.DestroySmoke (); 
//			}
			Destroy (gameObject); 
		}
	}
}

