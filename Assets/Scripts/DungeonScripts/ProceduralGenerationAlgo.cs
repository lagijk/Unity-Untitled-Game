using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgo 
{
    // RandomWalk method that uses HashSet to remove duplicates of the same field
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength) {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++) {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;

    }

    // RandomWalk method that creates corrdiors based on corrdiorLength and direction its facing
    public static List<Vector2Int> RandomWalkCorrdior(Vector2Int startPosition, int corrdiorLength) {
        // List is used instead of HashSet b/c there won't be duplicates, and we
        // need reference to last position, and using a list is more efficient for that
        List<Vector2Int> corrdior = new List<Vector2Int>();

        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corrdior.Add(currentPosition);

        for (int i = 0; i < corrdiorLength; i++) {
            currentPosition += direction;
            corrdior.Add(currentPosition);
        }
        return corrdior;
    }

    // Binary Space Partioning method 
    public static List<BoundsInt> BinarySpacePartition(BoundsInt spaceToSplit, int minWidth, int minHeight) {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while(roomsQueue.Count > 0) {
            var room = roomsQueue.Dequeue();
            // Discard rooms that are too small
            if (room.size.y >= minHeight && room.size.x >= minWidth) {

                // Checks if we can split horizontally
                if (Random.value < 0.5f) {
                    if (room.size.y >= minHeight * 2) {
                        SplitHorizontal(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2) {
                        SplitVertical(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight) {
                        roomsList.Add(room);
                    }
                }

                // Checks if we can split horizontally
                else {
                    if (room.size.x >= minWidth * 2) {
                        SplitVertical(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2) {
                        SplitHorizontal(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight) {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    // Method for splitting the room vertically
    private static void SplitVertical(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room) {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), 
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    // Method for splitting the room horizontally
    private static void SplitHorizontal(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room) {
        var ySplit = Random.Range(1, room.size.y); //Splitting room equally, (minHeight, roomsize.y - minHeight)
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), 
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}


// Class for walking in a random direction
public static class Direction2D {
    // The list of possible directions
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int> {
        new Vector2Int(0, 1), // Up direction
        new Vector2Int(1, 0), // Right direction
        new Vector2Int(0, -1), // Down direction
        new Vector2Int(-1, 0) // Left direction
    };

     public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int> {
        new Vector2Int(1, 1), // Up-Right direction
        new Vector2Int(1, -1), // Right-Down direction
        new Vector2Int(-1, -1), // Down-Left direction
        new Vector2Int(-1, 1) // Left-Up direction
    };

    // Order of directions in clockwise
    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int> {
        new Vector2Int(0, 1), // Up direction
        new Vector2Int(1, 1), // Up-Right direction
        new Vector2Int(1, 0), // Right direction
        new Vector2Int(1, -1), // Right-Down direction
        new Vector2Int(0, -1), // Down direction
        new Vector2Int(-1, -1), // Down-Left direction
        new Vector2Int(-1, 0), // Left direction
        new Vector2Int(-1, 1) // Left-Up direction
    };

    //Method for choosing the random direction
    public static Vector2Int GetRandomCardinalDirection() {
        return cardinalDirectionList[UnityEngine.Random.Range(0, cardinalDirectionList.Count)];
    }
}
