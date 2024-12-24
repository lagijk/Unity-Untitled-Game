using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    // A method that creates walls by taking in a HashSet and TileMapVisual as parameters
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisual tileMapVisual) {
        // variable used for standard walls (not corners)
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);

        CreateBasicWall(tileMapVisual, basicWallPositions, floorPositions);
        CreateCornerWalls(tileMapVisual, cornerWallPositions, floorPositions);


    }

    // Method that allows creation of basic walls using binary numbers
    private static void CreateBasicWall(TileMapVisual tileMapVisual, HashSet<Vector2Int> basicWallPositions, 
    HashSet<Vector2Int> floorPositions) {
        foreach (var position in basicWallPositions) {
            string neighborsBinaryType = "";

            foreach (var direction in Direction2D.cardinalDirectionList) {
                var neighborPosition = position + direction;
                if (floorPositions.Contains(neighborPosition)) {
                    neighborsBinaryType += "1";
                }
                else {
                    neighborsBinaryType += "0";
                }
            }
            tileMapVisual.PaintSingleBasicWall(position, neighborsBinaryType);
        }
    }

    // Method that creates corner walls using 8 bit binary values from the Direction2D class
    private static void CreateCornerWalls(TileMapVisual tileMapVisual, HashSet<Vector2Int> cornerWallPositions,
    HashSet<Vector2Int> floorPositions) {
        foreach (var position in cornerWallPositions) {
            string neighborsBinaryType = "";

            foreach (var direction in Direction2D.eightDirectionsList) {
                var neighborPosition = position + direction;
                if (floorPositions.Contains(neighborPosition)) {
                    neighborsBinaryType += "1";
                }
                else {
                    neighborsBinaryType += "0";
                }
            }
            tileMapVisual.PaintSingleCornerWall(position, neighborsBinaryType);
        }
    }

    public static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList) {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        // first foreach finds each position
        foreach (var position in floorPositions) {

            // nested foreach finds the direction of each position
            foreach(var direction in directionList) {

                // checks if there is an empty floor tile next to the tile, 
                // if yes, it should be a wall tile
                var neighborPosition = position + direction;
                if (floorPositions.Contains(neighborPosition) == false) 
                    wallPositions.Add(neighborPosition);
            }
        }
        return wallPositions;
    }

}
