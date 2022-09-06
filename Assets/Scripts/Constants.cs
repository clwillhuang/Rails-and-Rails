using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

	[System.Serializable]
	public enum SelectionMode { Free, Build, Demolish, Simulate, Menu, Previous }; 

	[System.Serializable]
	public enum TrackPiece { Free, 
		StraightHorizontal, 
		StraightVertical, 
		DoubleStraight,
		Curve, 
		SBendLeft, 
		SBendRight, 
		SBendLeftJunction, 
		SBendRightJunction, 
		ThreeWayJunction, 
		FourWayJunction}; 

	[System.Serializable]
	public enum Tile { Empty, Forest, Rock, Water, Track }; 

	[System.Serializable]
	public enum Rotation { Up, Right, Down, Left } 

	public enum MovementState { Go, Stop } ;

	public enum EntryType { Oil, Goods, Mining, Freight }; 
 }
	
[System.Serializable]
public class Path 
{
	/// <summary>
	/// Transform waypoints that should be already set in the engine
	/// </summary>
	public List<Transform> wayPoints;

	public int originCoordinateX, originCoordinateY; 


	/// <summary>
	/// The tile that this track section ultimately leads to. 
	/// MUST BE SET WHEN TRACK IS CONSTRUCTED
	/// </summary>
	public int destinationCoordinateX, destinationCoordinateY;


	/// <summary>
	/// The direction the train is facing when it exits the section
	/// </summary>
	public Constants.Rotation exitDirection;



	/// <summary>
	/// The direction the train must face when entering the track section
	/// Else: Derail
	/// </summary>
	public Constants.Rotation entryDirection; 



	/// <summary>
	/// Can a train on this path continue?
	/// </summary>
	public Constants.MovementState signal; 


	public int signalDirection;




} 