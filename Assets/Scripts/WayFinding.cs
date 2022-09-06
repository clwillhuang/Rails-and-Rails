using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to every track piece. Stores ALL paths through this track piece, so it can be assigned to the surrounding tiles on construction. 
/// </summary>
[System.Serializable]
public class WayFinding : MonoBehaviour  {

	/// <summary>
	/// All possible paths through this tile.
	/// </summary>
	public List<Path> trainWayPoints; 

	/// <summary>
	/// The rotation of this track piece. 
	/// </summary>
	private Constants.Rotation rotation; 

	/// <summary>
	/// The type of track piece.
	/// </summary>
	private Constants.TrackPiece trackPiece;

	/// <summary>
	/// Origin tiles of this track piece.
	/// </summary>
	private int originX, originY;

	/// <summary>
	/// Updates the tiles connections by adding junctions. 
	/// </summary>
	/// <param name="_rotation">Rotation of the track piece.</param>
	/// <param name="_trackPiece">Track piece type.</param>
	/// <param name="_originX">The origin tile of the piece, x.</param>
	/// <param name="_originY">The origin tile of the piece, y</param>
	public void setTrackDown(Constants.Rotation _rotation, Constants.TrackPiece _trackPiece, int _originX, int _originY) { 

		rotation = _rotation;
		trackPiece = _trackPiece; 
		originX = _originX;
		originY = _originY; 

		foreach (Path path in trainWayPoints) {
			path.entryDirection = (Constants.Rotation)(((int)path.entryDirection + (int)rotation) % 4); 
			path.exitDirection = (Constants.Rotation)(((int)path.exitDirection + (int)rotation) % 4); 
		}

		// Update tile connections depending on track piece and track rotation

		if (trackPiece == Constants.TrackPiece.StraightHorizontal) {
			if (rotation == Constants.Rotation.Up) {

				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX;
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX;
				trainWayPoints [1].originCoordinateY = originY; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [1]); 

			}
		} else if (trackPiece == Constants.TrackPiece.DoubleStraight) {
			if (rotation == Constants.Rotation.Up) {

				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX;
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX, originY + 1).addPathToJunction (trainWayPoints [1]); 

			} else if (rotation == Constants.Rotation.Right) {

				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2;
				trainWayPoints [0].destinationCoordinateY = originY;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY).addPathToJunction (trainWayPoints [1]); 

			} else if (rotation == Constants.Rotation.Down) {

				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX, originY - 1).addPathToJunction (trainWayPoints [1]); 

			} else if (rotation == Constants.Rotation.Left) {

				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY).addPathToJunction (trainWayPoints [1]); 

			}
		} else if (trackPiece == Constants.TrackPiece.StraightVertical) {
			if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1;
				trainWayPoints [0].destinationCoordinateY = originY;

				trainWayPoints [1].originCoordinateX = originX;
				trainWayPoints [1].originCoordinateY = originY; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [1]);
			}
		} else if (trackPiece == Constants.TrackPiece.Curve) {

			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;
				 
				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1;
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2;
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			}
		} else if (trackPiece == Constants.TrackPiece.SBendLeft) {
			
			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1; 
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2; 
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			}
//			GridBuilder.current.getGridTile (trainWayPoints [0].destinationCoordinateX, trainWayPoints [0].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [1].destinationCoordinateX, trainWayPoints [1].destinationCoordinateY).SetColor (Color.cyan);
		} else if (trackPiece == Constants.TrackPiece.SBendRight) {
			
			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1; 
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunctionStraight (trainWayPoints [1]);
			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2; 
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunctionStraight (trainWayPoints [1]);
			}

		} else if (trackPiece == Constants.TrackPiece.SBendLeftJunction) {
			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				trainWayPoints [2].originCoordinateX = originX + 1;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX + 1;
				trainWayPoints [2].destinationCoordinateY = originY - 2;

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX + 1;
				trainWayPoints [3].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX + 1, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY - 1; 
				trainWayPoints [2].destinationCoordinateX = originX - 2;
				trainWayPoints [2].destinationCoordinateY = originY - 1;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX + 1;
				trainWayPoints [3].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY - 1).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1; 
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				trainWayPoints [2].originCoordinateX = originX - 1;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX - 1;
				trainWayPoints [2].destinationCoordinateY = originY + 2;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX - 1;
				trainWayPoints [3].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX - 1, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2; 
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY + 1; 
				trainWayPoints [2].destinationCoordinateX = originX + 2;
				trainWayPoints [2].destinationCoordinateY = originY + 1; 

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX - 1;
				trainWayPoints [3].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY + 1).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [3]);
			}
//			GridBuilder.current.getGridTile (trainWayPoints [0].destinationCoordinateX, trainWayPoints [0].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [1].destinationCoordinateX, trainWayPoints [1].destinationCoordinateY).SetColor (Color.cyan);
//			GridBuilder.current.getGridTile (trainWayPoints [2].destinationCoordinateX, trainWayPoints [2].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [3].destinationCoordinateX, trainWayPoints [3].destinationCoordinateY).SetColor (Color.cyan);
		} else if (trackPiece == Constants.TrackPiece.SBendRightJunction) {
			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				trainWayPoints [2].originCoordinateX = originX - 1;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX - 1;
				trainWayPoints [2].destinationCoordinateY = originY - 2;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX - 1;
				trainWayPoints [3].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX - 1, originY).addPathToJunction (trainWayPoints [2]); 
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY + 1; 
				trainWayPoints [2].destinationCoordinateX = originX - 2;
				trainWayPoints [2].destinationCoordinateY = originY + 1;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX + 1;
				trainWayPoints [3].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY + 1).addPathToJunction (trainWayPoints [2]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1; 
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				trainWayPoints [2].originCoordinateX = originX + 1;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX + 1;
				trainWayPoints [2].destinationCoordinateY = originY + 2;

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX + 1;
				trainWayPoints [3].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX + 1, originY).addPathToJunction (trainWayPoints [2]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [3]);
			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2; 
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY - 1; 
				trainWayPoints [2].destinationCoordinateX = originX + 2;
				trainWayPoints [2].destinationCoordinateY = originY - 1;

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX - 1;
				trainWayPoints [3].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY - 1).addPathToJunction (trainWayPoints [2]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [3]);
			}
//			GridBuilder.current.getGridTile (trainWayPoints [0].destinationCoordinateX, trainWayPoints [0].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [1].destinationCoordinateX, trainWayPoints [1].destinationCoordinateY).SetColor (Color.cyan);
//			GridBuilder.current.getGridTile (trainWayPoints [2].destinationCoordinateX, trainWayPoints [2].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [3].destinationCoordinateX, trainWayPoints [3].destinationCoordinateY).SetColor (Color.cyan);
		} 

		else if (trackPiece == Constants.TrackPiece.ThreeWayJunction) {

			if (rotation == Constants.Rotation.Up) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 2;
				trainWayPoints [0].destinationCoordinateY = originY - 1;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY + 1;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX + 2;
				trainWayPoints [2].destinationCoordinateY = originY - 1;

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX;
				trainWayPoints [3].destinationCoordinateY = originY + 1;

				trainWayPoints [4].originCoordinateX = originX + 1;
				trainWayPoints [4].originCoordinateY = originY - 1; 
				trainWayPoints [4].destinationCoordinateX = originX - 2; 
				trainWayPoints [4].destinationCoordinateY = originY - 1;

				trainWayPoints [5].originCoordinateX = originX - 1;
				trainWayPoints [5].originCoordinateY = originY - 1; 
				trainWayPoints [5].destinationCoordinateX = originX + 2;
				trainWayPoints [5].destinationCoordinateY = originY - 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [3]);
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [4]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [5]);

			} else if (rotation == Constants.Rotation.Right) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX - 1;
				trainWayPoints [0].destinationCoordinateY = originY + 2;

				trainWayPoints [1].originCoordinateX = originX - 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX + 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX - 1;
				trainWayPoints [2].destinationCoordinateY = originY - 2;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY - 1; 
				trainWayPoints [3].destinationCoordinateX = originX + 1;
				trainWayPoints [3].destinationCoordinateY = originY;

				trainWayPoints [4].originCoordinateX = originX - 1;
				trainWayPoints [4].originCoordinateY = originY - 1; 
				trainWayPoints [4].destinationCoordinateX = originX - 1; 
				trainWayPoints [4].destinationCoordinateY = originY + 2;

				trainWayPoints [5].originCoordinateX = originX - 1;
				trainWayPoints [5].originCoordinateY = originY + 1; 
				trainWayPoints [5].destinationCoordinateX = originX - 1;
				trainWayPoints [5].destinationCoordinateY = originY - 2;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [3]);
				GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [4]);
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [5]);

			} else if (rotation == Constants.Rotation.Down) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 2;
				trainWayPoints [0].destinationCoordinateY = originY + 1;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY + 1; 
				trainWayPoints [1].destinationCoordinateX = originX;
				trainWayPoints [1].destinationCoordinateY = originY - 1;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX - 2;
				trainWayPoints [2].destinationCoordinateY = originY + 1;

				trainWayPoints [3].originCoordinateX = originX - 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX;
				trainWayPoints [3].destinationCoordinateY = originY - 1;

				trainWayPoints [4].originCoordinateX = originX - 1;
				trainWayPoints [4].originCoordinateY = originY + 1; 
				trainWayPoints [4].destinationCoordinateX = originX + 2; 
				trainWayPoints [4].destinationCoordinateY = originY + 1;

				trainWayPoints [5].originCoordinateX = originX + 1;
				trainWayPoints [5].originCoordinateY = originY + 1; 
				trainWayPoints [5].destinationCoordinateX = originX - 2;
				trainWayPoints [5].destinationCoordinateY = originY + 1;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [3]);
				GridBuilder.current.getGridTile (originX - 1, originY + 1).addPathToJunction (trainWayPoints [4]);
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [5]);

			} else if (rotation == Constants.Rotation.Left) {
				trainWayPoints [0].originCoordinateX = originX;
				trainWayPoints [0].originCoordinateY = originY; 
				trainWayPoints [0].destinationCoordinateX = originX + 1;
				trainWayPoints [0].destinationCoordinateY = originY - 2;

				trainWayPoints [1].originCoordinateX = originX + 1;
				trainWayPoints [1].originCoordinateY = originY - 1; 
				trainWayPoints [1].destinationCoordinateX = originX - 1;
				trainWayPoints [1].destinationCoordinateY = originY;

				trainWayPoints [2].originCoordinateX = originX;
				trainWayPoints [2].originCoordinateY = originY; 
				trainWayPoints [2].destinationCoordinateX = originX + 1;
				trainWayPoints [2].destinationCoordinateY = originY + 2;

				trainWayPoints [3].originCoordinateX = originX + 1;
				trainWayPoints [3].originCoordinateY = originY + 1; 
				trainWayPoints [3].destinationCoordinateX = originX - 1;
				trainWayPoints [3].destinationCoordinateY = originY;

				trainWayPoints [4].originCoordinateX = originX + 1;
				trainWayPoints [4].originCoordinateY = originY + 1; 
				trainWayPoints [4].destinationCoordinateX = originX + 1; 
				trainWayPoints [4].destinationCoordinateY = originY - 2;

				trainWayPoints [5].originCoordinateX = originX + 1;
				trainWayPoints [5].originCoordinateY = originY - 1; 
				trainWayPoints [5].destinationCoordinateX = originX + 1;
				trainWayPoints [5].destinationCoordinateY = originY + 2;

				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]); 
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [1]);
				GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [2]);
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [3]);
				GridBuilder.current.getGridTile (originX + 1, originY + 1).addPathToJunction (trainWayPoints [4]);
				GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [5]);
			}
//			GridBuilder.current.getGridTile (trainWayPoints [0].destinationCoordinateX, trainWayPoints [0].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [1].destinationCoordinateX, trainWayPoints [1].destinationCoordinateY).SetColor (Color.cyan);
//			GridBuilder.current.getGridTile (trainWayPoints [2].destinationCoordinateX, trainWayPoints [2].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [3].destinationCoordinateX, trainWayPoints [3].destinationCoordinateY).SetColor (Color.cyan);
//			GridBuilder.current.getGridTile (trainWayPoints [4].destinationCoordinateX, trainWayPoints [4].destinationCoordinateY).SetColor (Color.cyan); 
//			GridBuilder.current.getGridTile (trainWayPoints [5].destinationCoordinateX, trainWayPoints [5].destinationCoordinateY).SetColor (Color.cyan);

		} else if (trackPiece == Constants.TrackPiece.FourWayJunction) {
			trainWayPoints [0].originCoordinateX = originX;
			trainWayPoints [0].originCoordinateY = originY; 
			trainWayPoints [0].destinationCoordinateX = originX - 2;
			trainWayPoints [0].destinationCoordinateY = originY - 1;

			trainWayPoints [1].originCoordinateX = originX - 1;
			trainWayPoints [1].originCoordinateY = originY - 1; 
			trainWayPoints [1].destinationCoordinateX = originX;
			trainWayPoints [1].destinationCoordinateY = originY + 1;

			trainWayPoints [2].originCoordinateX = originX;
			trainWayPoints [2].originCoordinateY = originY; 
			trainWayPoints [2].destinationCoordinateX = originX + 2;
			trainWayPoints [2].destinationCoordinateY = originY - 1;

			trainWayPoints [3].originCoordinateX = originX + 1;
			trainWayPoints [3].originCoordinateY = originY - 1; 
			trainWayPoints [3].destinationCoordinateX = originX;
			trainWayPoints [3].destinationCoordinateY = originY + 1;

			trainWayPoints [4].originCoordinateX = originX + 1;
			trainWayPoints [4].originCoordinateY = originY - 1; 
			trainWayPoints [4].destinationCoordinateX = originX - 2; 
			trainWayPoints [4].destinationCoordinateY = originY - 1;

			trainWayPoints [5].originCoordinateX = originX - 1;
			trainWayPoints [5].originCoordinateY = originY - 1; 
			trainWayPoints [5].destinationCoordinateX = originX + 2;
			trainWayPoints [5].destinationCoordinateY = originY - 1;

			trainWayPoints [6].originCoordinateX = originX;
			trainWayPoints [6].originCoordinateY = originY; 
			trainWayPoints [6].destinationCoordinateX = originX;
			trainWayPoints [6].destinationCoordinateY = originY - 3;

			trainWayPoints [7].originCoordinateX = originX;
			trainWayPoints [7].originCoordinateY = originY - 2; 
			trainWayPoints [7].destinationCoordinateX = originX;
			trainWayPoints [7].destinationCoordinateY = originY + 1;

			trainWayPoints [8].originCoordinateX = originX;
			trainWayPoints [8].originCoordinateY = originY - 2; 
			trainWayPoints [8].destinationCoordinateX = originX + 2;
			trainWayPoints [8].destinationCoordinateY = originY - 1;

			trainWayPoints [9].originCoordinateX = originX + 1;
			trainWayPoints [9].originCoordinateY = originY - 1; 
			trainWayPoints [9].destinationCoordinateX = originX;
			trainWayPoints [9].destinationCoordinateY = originY - 3;

			trainWayPoints [10].originCoordinateX = originX - 1;
			trainWayPoints [10].originCoordinateY = originY - 1; 
			trainWayPoints [10].destinationCoordinateX = originX;
			trainWayPoints [10].destinationCoordinateY = originY - 3;

			trainWayPoints [11].originCoordinateX = originX;
			trainWayPoints [11].originCoordinateY = originY - 2; 
			trainWayPoints [11].destinationCoordinateX = originX - 2;
			trainWayPoints [11].destinationCoordinateY = originY - 1;


			GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [0]);
			GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [1]);
			GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [2]);
			GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [3]);
			GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [4]);
			GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [5]);
			GridBuilder.current.getGridTile (originX, originY).addPathToJunction (trainWayPoints [6]);
			GridBuilder.current.getGridTile (originX, originY - 2).addPathToJunction (trainWayPoints [7]);
			GridBuilder.current.getGridTile (originX, originY - 2).addPathToJunction (trainWayPoints [8]);
			GridBuilder.current.getGridTile (originX + 1, originY - 1).addPathToJunction (trainWayPoints [9]);
			GridBuilder.current.getGridTile (originX - 1, originY - 1).addPathToJunction (trainWayPoints [10]);
			GridBuilder.current.getGridTile (originX, originY - 2).addPathToJunction (trainWayPoints [11]);

		}
	}

	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	public void OnMouseDown() { 

		Debug.Log ("Hello!"); 

		if (StateManager.current.SelectionMode == Constants.SelectionMode.Demolish) { 

			if (trackPiece == Constants.TrackPiece.StraightHorizontal) {
				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 

				}
			} else if (trackPiece == Constants.TrackPiece.StraightVertical) {
				if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
				}
			} else if (trackPiece == Constants.TrackPiece.Curve) {

				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction ();
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction ();

				}
			} else if (trackPiece == Constants.TrackPiece.SBendLeft) {

				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction ();

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction (); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction ();

				}

			} else if (trackPiece == Constants.TrackPiece.SBendRight) {

				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Down) {
					
					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction();
				}

			} else if (trackPiece == Constants.TrackPiece.SBendLeftJunction) {
				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX + 1, originY).RemoveJunction();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY - 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY).RemoveJunction();
				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY + 1).RemoveJunction();
				}

			} else if (trackPiece == Constants.TrackPiece.SBendRightJunction) {
				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY).RemoveJunction(); 

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY + 1).RemoveJunction(); 

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX + 1, originY).RemoveJunction(); 

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY - 1).RemoveJunction(); 
				}
			} else if (trackPiece == Constants.TrackPiece.ThreeWayJunction) {

				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY + 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction(); 
					GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction ();
					GridBuilder.current.getGridTile (originX + 1, originY + 1).RemoveJunction ();
				}


			} else if (trackPiece == Constants.TrackPiece.FourWayJunction) {

				GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
				GridBuilder.current.getGridTile (originX - 1, originY - 1).RemoveJunction();
				GridBuilder.current.getGridTile (originX + 1, originY - 1).RemoveJunction();
				GridBuilder.current.getGridTile (originX, originY - 2).RemoveJunction();

			} else if (trackPiece == Constants.TrackPiece.DoubleStraight) {
				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY + 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
					GridBuilder.current.getGridTile (originX + 1, originY).RemoveJunction();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
					GridBuilder.current.getGridTile (originX, originY - 1).RemoveJunction();

				} else if (rotation == Constants.Rotation.Left) {


					GridBuilder.current.getGridTile (originX, originY).RemoveJunction();
					GridBuilder.current.getGridTile (originX - 1, originY).RemoveJunction();

				}
			}


			// Check if nothing is obstructing construction
			if (trackPiece == Constants.TrackPiece.StraightVertical || trackPiece == Constants.TrackPiece.StraightHorizontal) {
				// Nothing else to check
			} else if (trackPiece == Constants.TrackPiece.Curve) {

				if (rotation == Constants.Rotation.Up) {

						GridBuilder.current.getGridTile(originX-1, originY-1).setEmpty(); 
						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(0);
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(2); 

					
				} else if (rotation == Constants.Rotation.Right) {

						GridBuilder.current.getGridTile(originX-1, originY+1).setEmpty(); 
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(1); 
						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(3);

				} else if (rotation == Constants.Rotation.Down) {
						GridBuilder.current.getGridTile(originX+1, originY+1).setEmpty();
						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(0); 
			
				} else if (rotation == Constants.Rotation.Left) {
						GridBuilder.current.getGridTile(originX+1, originY-1).setEmpty();
						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(1);
						GridBuilder.current.getGridTile(originX+1,originY).setEmptyCorner(3); 
				}  
			} else if (trackPiece == Constants.TrackPiece.SBendLeft) {

				if (rotation == Constants.Rotation.Up) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(1);
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(3);
						GridBuilder.current.getGridTile(originX + 1, originY - 1).setEmpty();

				} else if (rotation == Constants.Rotation.Right) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(0); 
						GridBuilder.current.getGridTile(originX-1, originY).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX - 1, originY - 1).setEmpty();
					
				} else if (rotation == Constants.Rotation.Down) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(3); 
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(1); 
						GridBuilder.current.getGridTile(originX - 1, originY + 1).setEmpty(); 
					
				} else if (rotation == Constants.Rotation.Left) {
					
						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX + 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(0); 
		
				}  
			} else if (trackPiece == Constants.TrackPiece.SBendRight) {
				if (rotation == Constants.Rotation.Up) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(0); 
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX - 1, originY - 1).setEmpty(); 
					
				} else if (rotation == Constants.Rotation.Right) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(3);
						GridBuilder.current.getGridTile(originX - 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(1); 

				} else if (rotation == Constants.Rotation.Down) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(2);
						GridBuilder.current.getGridTile(originX + 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(0);

				} else if (rotation == Constants.Rotation.Left) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(1);
						GridBuilder.current.getGridTile(originX + 1, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(3);

				}   
			} else if (trackPiece == Constants.TrackPiece.SBendLeftJunction) {
				if (rotation == Constants.Rotation.Up) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(1);
						GridBuilder.current.getGridTile(originX + 1, originY).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY - 1).setEmpty();

				} else if (rotation == Constants.Rotation.Right) {
					
						GridBuilder.current.getGridTile(originX, originY - 1).setEmpty(); 
						GridBuilder.current.getGridTile(originX-1, originY).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX - 1, originY - 1).setEmpty();
					
				} else if (rotation == Constants.Rotation.Down) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(3); 
						GridBuilder.current.getGridTile(originX - 1, originY).setEmpty(); 
						GridBuilder.current.getGridTile(originX - 1, originY + 1).setEmpty(); 

				} else if (rotation == Constants.Rotation.Left) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmpty(); 
						GridBuilder.current.getGridTile(originX + 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(0); 

				}  
			} else if (trackPiece == Constants.TrackPiece.SBendRightJunction) {
				if (rotation == Constants.Rotation.Up) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmptyCorner(0); 
						GridBuilder.current.getGridTile(originX - 1, originY).setEmpty(); 
						GridBuilder.current.getGridTile(originX - 1, originY - 1).setEmpty(); 

				} else if (rotation == Constants.Rotation.Right) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(1); 

				} else if (rotation == Constants.Rotation.Down) {

						GridBuilder.current.getGridTile(originX, originY + 1).setEmptyCorner(2);
						GridBuilder.current.getGridTile(originX + 1, originY + 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmpty();

				} else if (rotation == Constants.Rotation.Left) {

						GridBuilder.current.getGridTile(originX, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(3);

				}         
			} else if (trackPiece == Constants.TrackPiece.ThreeWayJunction) {
				if (rotation == Constants.Rotation.Up) {
						GridBuilder.current.getGridTile(originX, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX + 1, originY - 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(2); 
						GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(3);

				} else if (rotation == Constants.Rotation.Right) {

						GridBuilder.current.getGridTile(originX - 1, originY- 1).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY).setEmpty();
						GridBuilder.current.getGridTile(originX - 1, originY+ 1).setEmpty();
						GridBuilder.current.getGridTile(originX, originY- 1).setEmptyCorner(0); 
						GridBuilder.current.getGridTile(originX,originY+1).setEmptyCorner(3); 
					
				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile(originX - 1, originY+ 1).setEmpty();
					GridBuilder.current.getGridTile(originX, originY+ 1).setEmpty();
					GridBuilder.current.getGridTile(originX + 1, originY+ 1).setEmpty();
					GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(1); 
					GridBuilder.current.getGridTile(originX+1,originY).setEmptyCorner(0); 

				} else if (rotation == Constants.Rotation.Left) {

					GridBuilder.current.getGridTile(originX + 1, originY- 1).setEmpty();
					GridBuilder.current.getGridTile(originX + 1, originY).setEmpty();
					GridBuilder.current.getGridTile(originX + 1, originY+ 1).setEmpty();
					GridBuilder.current.getGridTile(originX, originY- 1).setEmptyCorner(1); 
					GridBuilder.current.getGridTile(originX,originY+1).setEmptyCorner(2); 

				}   
			} else if (trackPiece == Constants.TrackPiece.FourWayJunction) {

					GridBuilder.current.getGridTile(originX, originY- 1).setEmpty();
					GridBuilder.current.getGridTile(originX - 1, originY- 1).setEmpty();
					GridBuilder.current.getGridTile(originX + 1, originY- 1).setEmpty();
					GridBuilder.current.getGridTile(originX, originY- 2).setEmpty();
					GridBuilder.current.getGridTile (originX - 1, originY - 2).setEmptyCorner (1);
					GridBuilder.current.getGridTile(originX - 1, originY).setEmptyCorner(2);
					GridBuilder.current.getGridTile(originX + 1, originY- 2).setEmptyCorner(0); 
					GridBuilder.current.getGridTile(originX + 1, originY).setEmptyCorner(3); 

			} else if (trackPiece == Constants.TrackPiece.DoubleStraight) {
				if (rotation == Constants.Rotation.Up) {

					GridBuilder.current.getGridTile (originX, originY).setEmpty();
					GridBuilder.current.getGridTile (originX, originY + 1).setEmpty();

				} else if (rotation == Constants.Rotation.Right) {

					GridBuilder.current.getGridTile (originX, originY).setEmpty();
					GridBuilder.current.getGridTile (originX + 1, originY).setEmpty();

				} else if (rotation == Constants.Rotation.Down) {

					GridBuilder.current.getGridTile (originX, originY).addPathToJunctionStraight (trainWayPoints [0]); 
					GridBuilder.current.getGridTile (originX, originY - 1).setEmpty();

				} else if (rotation == Constants.Rotation.Left) {
					

					GridBuilder.current.getGridTile (originX, originY).setEmpty(); 
					GridBuilder.current.getGridTile (originX - 1, originY).setEmpty();

				}
			}

			GridBuilder.current.getGridTile(originX, originY).setEmpty (); 
			GridBuilder.current.getGridTile (originX, originY).straightTrack = false; 
			GridBuilder.current.getGridTile (originX, originY).PlayDes (); 

			Destroy(gameObject);
		}
	}

}
