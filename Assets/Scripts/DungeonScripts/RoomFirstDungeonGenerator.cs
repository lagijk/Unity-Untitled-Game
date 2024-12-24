using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : RandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;

    // Offset make sures there are walls to divide the rooms
    [SerializeField]
    [Range(0, 10)]
    private int offSet = 1;

    [SerializeField]
    // Checks if we want to use random walk or binary partioning
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    // Method for creating rooms using binary partioning
    private void CreateRooms() {
        var roomsList = ProceduralGenerationAlgo.BinarySpacePartition
        (new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms) {
            floor = CreateRandomRooms(roomsList);
        }
        else {
            floor = CreateSimpleRooms(roomsList);
        }

        // Connect rooms together by using a list that contains the center point of each rooms
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach(var room in roomsList) {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // Corridors connect to rooms
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        // Paints the tilemaps
        tileMapVisual.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tileMapVisual);
    }

    // A method for creating HashSet of rooms
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList) {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList) {
            for (int col = 0; col < room.size.x - offSet; col++) {
                for (int row = offSet; row < room.size.y - offSet; row++) {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    // Method for connecting corridors to rooms
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters) {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0) {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }


    // Method for finding the cloest point to the center of the current room
    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters) {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters) {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance) {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;

    }


    // Method for creating corridors
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination) {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter; // start position for corridor
        corridor.Add(position);
        
        // Check which direction to travel
        while(position.y != destination.y) {
            // Move up
            if (destination.y > position.y) {
                position += Vector2Int.up;
            }
            // Move down
            else if (destination.y < position.y) {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x) {
            // Move right
            if (destination.x > position.x) {
                position += Vector2Int.right;
            }
            // Move left
            else if (destination.x < position.x) {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;

    }


    // Method that combines randomwalk and binary partioning for room generation
    private HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomsList) {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        for (int i = 0; i < roomsList.Count; i++) {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), 
            Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            // After generating random positions, we need to calculate the offset
            foreach (var position in roomFloor) {
                if (position.x >= (roomBounds.xMin + offSet) && position.x <= (roomBounds.xMax - offSet)
                && position.y >= (roomBounds.yMin - offSet) && position.y <= (roomBounds.yMax - offSet)) {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

}
