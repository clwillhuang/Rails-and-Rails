using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Car { 

	#region Unity GameObject References

	public Transform frontBogie, endBogie;
	public Transform wagon; 
	public List<Transform> wayPointsFront, wayPointsEnd; 
	public Transform Guide; 

	#endregion

	// Helper variables required for rotation updating. 
	Vector3 targetPos;
	Vector3 _direction;
	Quaternion _lookRotation; 
	Vector3 heading; 
	Vector3 positioning; 
	float final; 

	// Audio
	AudioSource audiosource;

	// Smoke cube prefab. Smoke given off if this car is the front. 
	GameObject smokeCube;

	// Origin position of all smoke cubes
	Transform smokeOrigin; 

	// Keep track of active smoke in scene, and time it is has been there for
	List < GameObject > activeSmoke; 
	List < float > smokeTimer; 

	/// <summary>
	/// Is this train car the front car? 
	/// </summary>
	public bool isFirst;

	/// <summary>
	/// Has this train car reached the first waypoint of a new track tile? 
	/// </summary>
	public bool completedFirst;

	public Car(Transform _frontBogie, Transform _endBogie, Transform _wagon, Transform _guide, Transform firstEntryPoint, bool _isFirst) { 
		frontBogie = _frontBogie;
		endBogie = _endBogie;
		wagon = _wagon;
		Guide = _guide; 
		audiosource = frontBogie.GetComponent<AudioSource> (); 

		wayPointsFront = new List<Transform> (); 
		wayPointsEnd = new List<Transform> (); 

		Debug.Log ("Added new points!"); 

		wayPointsFront.Add (firstEntryPoint);
		wayPointsEnd.Add (firstEntryPoint); 

		completedFirst = false; 

		isFirst = _isFirst;

		if (isFirst) {
			smokeCube = Resources.Load ("Smoke", typeof(GameObject)) as GameObject;

			Debug.Log ("WAGON NAME: " + wagon.name); 

			smokeOrigin = wagon.Find ("Engine (Clone)/Smoke Origin"); 

			activeSmoke = new List<GameObject> (); 
			smokeTimer = new List<float> (); 
		}
	}

	public void AudioMute(bool mute) { 
		audiosource.mute = mute; 
	} 

	public void Simulate(float speed) { 

		// Simulate the first bogie 

		if (wayPointsFront.Count > 0) {

			float distToEnd = Vector3.Distance (frontBogie.position, wayPointsFront [0].position); 

			positioning = Vector3.Lerp (frontBogie.position, wayPointsFront [0].position, Mathf.Min(1f, speed / distToEnd)); 

			frontBogie.position = positioning; 

			frontBogie.eulerAngles = new Vector3(0f, Quaternion.Slerp(frontBogie.rotation, wayPointsFront[0].rotation, Mathf.Min(1f, speed / distToEnd)).eulerAngles.y, 0f); 

			//frontBogie.eulerAngles = new Vector3(0f, Vector3.Lerp (frontBogie.rotation.eulerAngles, wayPointsFront [0].rotation.eulerAngles, Mathf.Min(1f, speed / distToEnd)).y, 0f); 

			// Account for overflow, i.e. if the train goes past the waypoint in the frame, continue to the next waypoint
			if (speed / distToEnd > 1f) {

				wayPointsFront.RemoveAt (0); 

				if (isFirst) {
					completedFirst = true; 
				}
			}
		}

		heading = Guide.position - endBogie.position;

		final = Vector3.Dot (heading, Guide.forward); 

		if (final < 0f) {
			speed += 0.1f;
		} else if (final > 0f) {
			speed -= 0.1f; 
		}  

		// Simulate the second bogie

		if (wayPointsEnd.Count > 0) {

			float distToEnd = Vector3.Distance (endBogie.position, wayPointsEnd [0].position); 

			positioning = Vector3.Lerp (endBogie.position, wayPointsEnd [0].position, Mathf.Min(1f, speed / distToEnd));

			endBogie.position = positioning;

			endBogie.eulerAngles = new Vector3(0f, Quaternion.Slerp(endBogie.rotation, wayPointsEnd[0].rotation, Mathf.Min(1f, speed / distToEnd)).eulerAngles.y, 0f); 


			//endBogie.eulerAngles = new Vector3(0f, Vector3.Lerp (endBogie.rotation.eulerAngles, wayPointsEnd [0].rotation.eulerAngles, Mathf.Min(1f, speed / distToEnd)).y, 0f); 


			// Account for overflow, i.e. if the train goes past the waypoint in the frame, continue to the next waypoint
			if (speed / distToEnd > 1f) {

				float remaining = speed * (speed / distToEnd - 1f);

				wayPointsEnd.RemoveAt (0); 

			}
		}

		targetPos = new Vector3 (endBogie.position.x, GridBuilder.current.yAxis, endBogie.position.z); 

		_direction = (targetPos - wagon.position).normalized;

		_lookRotation = Quaternion.LookRotation (_direction);

		wagon.eulerAngles = new Vector3 (wagon.eulerAngles.x, _lookRotation.eulerAngles.y, wagon.eulerAngles.z);

		// If front car, generate smoke. 
		if (isFirst) {

			// Only make smoke every few updates
			if (Random.Range(0, 9) == 1) { 
				GameObject newSmoke = GameObject.Instantiate (smokeCube);
				newSmoke.transform.position = smokeOrigin.position; 
				newSmoke.transform.eulerAngles = Vector3.zero; 
				newSmoke.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f); 
				activeSmoke.Add (newSmoke); 
				smokeTimer.Add (4f); 
			}

			for (int i = activeSmoke.Count - 1; i >= 0; i--) {
				if (smokeTimer[i] < 0f) {
					GameObject.Destroy (activeSmoke [i].gameObject); 
					activeSmoke.RemoveAt (i); 
					smokeTimer.RemoveAt (i); 
				}
				smokeTimer[i] -= 0.05f;
				activeSmoke[i].transform.Translate (Vector3.up * Time.deltaTime * 7f); 
				activeSmoke[i].transform.localScale = new Vector3 (activeSmoke[i].transform.localScale.x + Random.Range(0.01f, 0.15f), activeSmoke[i].transform.localScale.y + Random.Range(0.01f, 0.1f), activeSmoke[i].transform.localScale.z + Random.Range(0.01f, 0.15f)); 
			}
				

		}

		return;


	}

	public void DestroySmoke() { 
		if (activeSmoke == null) {
			return;
		}
		if (activeSmoke.Count == 0) {
			return; 
		}
		for (int i = activeSmoke.Count - 1; i >= 0; i--) {
			GameObject.Destroy (activeSmoke [i].gameObject); 
			activeSmoke.RemoveAt (i); 
			smokeTimer.RemoveAt (i); 
		} 
	} 

}

