using UnityEngine;

public class CameraMovement : MonoBehaviour {

	float movementSpeed = 75f;
	float h, v; 

	static float zoomMagnitude = 0f;

	public static void ResetZoom() { 
		zoomMagnitude = 0f;
	}

	static bool cameraOn;

	public static bool CameraOn {
		private get { 
			return cameraOn;
		}
		set {
			cameraOn = value; 
		} 
	}

	// Update is called once per frame
	void Update () {

		if (!cameraOn) {
			return;
		}

		h = Input.GetAxis ("Horizontal"); 
		v = Input.GetAxis ("Vertical"); 
		if (Mathf.Abs (h) > 0.1f) {
			transform.parent.Translate (new Vector3 (movementSpeed * h * Time.deltaTime, 0f, 0f));
		}
		if (Mathf.Abs(v) > 0.1f) {
			transform.parent.Translate ( Vector3.forward * movementSpeed * v * Time.deltaTime);
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
			if (zoomMagnitude < 25f) {
				transform.Translate (new Vector3 (0, 0, movementSpeed * Time.deltaTime));
				zoomMagnitude += movementSpeed * Time.deltaTime;
			}

		} else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (zoomMagnitude > -5f) {
				transform.Translate (new Vector3 (0, 0, -movementSpeed * Time.deltaTime));
				zoomMagnitude -= movementSpeed * Time.deltaTime;
			}
		}

		if (Mathf.Abs(Input.GetAxis ("Pan")) > 0f) { 
			transform.Rotate (Input.GetAxis ("Pan") * 15f * Time.deltaTime, 0f, 0f); 
		} 
		transform.parent.position = new Vector3 (Mathf.Clamp (transform.parent.position.x, -50f, 110f), Mathf.Clamp (transform.parent.position.y, 30f, 60f), Mathf.Clamp (transform.parent.position.z, -45f, 45f)); 
		transform.localEulerAngles = new Vector3 (Mathf.Clamp (transform.localEulerAngles.x, 25f, 90f), 0f, 0f); 
	}
}
