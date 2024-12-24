using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisual : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]    
    private TileBase floorTile, wallTop, wallBottom, wallSiderLeft, wallSideRight, wallFull,
    wallInnerCornerDownLeft, wallInnerCornerDownRight, 
    wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft; //change to array for random tiles

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    // Method for creating tilemaps
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile) {
        foreach(var position in positions) {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    // Method for creating a single tile
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position) {
        // Gets the tilePosition in unity to paint tile
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    //Methods that clears the tilemaps to prevent duplication
    public void Clear() {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    // Method for creating a single wall tile
    internal void PaintSingleBasicWall(Vector2Int position, string binaryType) {
        int typeAsInt = Convert.ToInt32(binaryType, 2); //Converts binary string into binary number
        TileBase tile = null;

        if (WallTypes.wallTop.Contains(typeAsInt)) {
            tile = wallTop;
        }
        else if (WallTypes.wallSideRight.Contains(typeAsInt)) {
            tile = wallSideRight;
        }
        else if (WallTypes.wallSideLeft.Contains(typeAsInt)) {
            tile = wallSiderLeft;
        }
        else if (WallTypes.wallBottm.Contains(typeAsInt)) {
            tile = wallBottom;
        }
        else if (WallTypes.wallFull.Contains(typeAsInt)) {
            tile = wallFull;
        }

        if (tile != null) 
            PaintSingleTile(wallTilemap, tile, position);
        
    }


    // Method for creating single corner wall tile
    internal void PaintSingleCornerWall(Vector2Int position, string binaryType) {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypes.wallInnerCornerDownLeft.Contains(typeAsInt)) {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypes.wallInnerCornerDownRight.Contains(typeAsInt)) {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt)) {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypes.wallDiagonalCornerDownRight.Contains(typeAsInt)) {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypes.wallDiagonalCornerUpRight.Contains(typeAsInt)) {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt)) {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypes.wallFullEightDirections.Contains(typeAsInt)) {
            tile = wallFull;
        }
        else if (WallTypes.wallBottmEightDirections.Contains(typeAsInt)) {
            tile = wallBottom;
        }

        if (tile != null) {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

}
