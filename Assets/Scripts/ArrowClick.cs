using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls user input when changing signals. Formerly known as Signal.cs
/// </summary>
public class ArrowClick : MonoBehaviour {

	private delegate void ChangeSignalLight();

	private delegate void ChangeSignalDirection(); 

	ChangeSignalLight signalLightDelegate;

	ChangeSignalDirection signalDirectionDelegate; 

	public AudioSource audiosource; 

	void Awake() { 
		audiosource = GetComponent<AudioSource> (); 
	}

	void OnMouseOver() { 
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log ("Left Click!"); 
			signalDirectionDelegate.Invoke (); 
			audiosource.Play ();
		} else if (Input.GetMouseButtonDown(1)) {
			Debug.Log ("Right Click!"); 
			signalLightDelegate.Invoke (); 
			audiosource.Play ();
		} 
		transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f); 
	}

	void OnMouseExit() { 
		transform.localScale = new Vector3 (1f, 1f, 1f); 
	}

	public void SetDelegates() { 

		signalLightDelegate = transform.parent.parent.GetComponent<Junction> ().toggleSignal;
		signalDirectionDelegate = transform.parent.parent.GetComponent<Junction> ().toggleDirection;

	}
}
