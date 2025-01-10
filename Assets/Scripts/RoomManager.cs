using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 20;
    [SerializeField] private int minRooms = 15;


    int roomWidth = 20;
    int roomHeight = 12;

    [SerializeField] int gridSizeX = 11;
    [SerializeField] int gridSizeY = 11;
    
    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    
    private int[,] roomGrid;

    private int roomCount;

    [SerializeField] private int maxRoomString;

    private int roomString;

    private bool generationComplete = false;

    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject merchantRoom;
    [SerializeField] GameObject recoveryRoom;
    [SerializeField] GameObject miniBossRoom;
    [SerializeField] GameObject bossRoom;

    private int generationAttempts = 0;
    private const int maxAttempts = 1000;


    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        

        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;


            if(gridX > 0 && roomGrid[gridX - 1, gridY] == 0)
            {
               // No neighbor to the left
               TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            }
            if (gridX < gridSizeX - 1 && roomGrid[gridX + 1, gridY] == 0)
            {
               // No neigbor to the right
               TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            }
            if (gridY > 0 && roomGrid[gridX, gridY - 1] == 0)
            {
               // No neighbor below
               TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            }
            if (gridY < gridSizeY - 1 && roomGrid[gridX, gridY + 1] == 0)
            {
               // No neighbor above
               TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            }

        }
        else if(roomCount < minRooms)
        {
            //Debug.Log("RoomCount was less than the minimum amount of rooms. Trying again");
            RegenerateRooms();
        } 
        else if(roomString > maxRoomString)
        {
            //Debug.Log($"RoomString was, {roomString}. Trying again");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            //Debug.Log($"Generation complete, {roomCount} rooms created");
            generationComplete = true;

            FinalizeDungeonGeneration();
        }

    }

    private void ReplaceRoomWithPrefab(GameObject targetRoom, GameObject prefab)
    {      
        if (targetRoom != null)
        {
            Instantiate(prefab, targetRoom.transform.position, Quaternion.identity);
            
            roomObjects.Remove(targetRoom);
            Destroy(targetRoom);
        }
        else
        {
            Debug.LogError("Not enough rooms to replace with the specified prefab.");
        }
    }

    private void FinalizeDungeonGeneration()
    {
        //PlaceEnermies();
        //PlaceItems();
        Debug.Log("Dungeon finalization complete.");
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }
    
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;


        if (x >= gridSizeX || y >= gridSizeY || x < 0 || y < 0)
            return false;
        
        if (roomCount >= maxRooms)
            return false;

        if (roomIndex != Vector2Int.zero && Random.value < 0.5f)
           return false;

        int adjacentRoomCount = CountAdjacentRooms(roomIndex);
        if (adjacentRoomCount == 0 && Random.value < 0.9f)
            return false;

        if (adjacentRoomCount > 1)
            return false;

        if (roomGrid[x, y] != 0)
            return false;

        if (CountRoomString(roomIndex) >= maxRoomString)
            return false;

        if (roomGrid[roomIndex.x, roomIndex.y] == 0)
        {
            roomQueue.Enqueue(roomIndex);
            roomGrid[roomIndex.x, roomIndex.y] = 1;
            roomCount++;
        }

        if (generationAttempts >= maxAttempts)
        {
            Debug.LogError("Maximum room generation attempts reached. Stopping the process.");
            return false;
        }

        generationAttempts++;

        Vector3 roomPosition = GetPositionFromGridIndex(roomIndex);
        
        var newRoom = Instantiate(roomPrefab,GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        OpenDoors(newRoom, x, y);

        return Random.value < 0.1f;
    }

    // Clear all rooms and try again
    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);

        Debug.Log("Room generation complete. Final room grid: ");

        UpdateAllAdjacentCounts();
    }

    // Updating room counts after regeneration
    private void UpdateAllAdjacentCounts()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (roomGrid[x, y] != 0)
                {
                    Vector2Int roomIndex = new Vector2Int(x, y);
                    int adjacentROoms = CountAdjacentRooms(roomIndex);
                }
            }
        }
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        //Neighbors
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        // Determine which doors to open based on the direction
        if(x > 0 && roomGrid[x - 1, y] != 0)
        {
            // Neighboring room to the left
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }
        if(x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) 
        {
            // Neighboring room to the right
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            // Neighboring room below
            newRoomScript.OpenDoor(Vector2Int.down);
            bottomRoomScript.OpenDoor(Vector2Int.up);
        }
        
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            // Neighboring room above
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript.OpenDoor(Vector2Int.down);
        }


    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if(roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++; // Left neighbor
        if (x < gridSizeX - 1 && roomGrid[x + 1,y] != 0) count++; // Right neighbor
        if (y > 0 && roomGrid[x, y - 1] != 0) count++; // Bottom neighbor
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++; // Top neighbor

        return count;
    }

    private void AttachNodes()
    {
        if (x > 0 && roomGrid[x - 1, y] != 0) count++; // Left neighbor
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++; // Right neighbor
        if (y > 0 && roomGrid[x, y - 1] != 0) count++; // Bottom neighbor
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++; // Top neighbor
    }

    private int CountRoomString(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int countH = 0;
        int countV = 0;

        // Counting Horizontally
        for (int i = x - 1; i >= 0 && roomGrid[i, y] != 0; i--) countH++;
        for (int i = x + 1; i < gridSizeX && roomGrid[i, y] != 0; i++) countH++;

        // Counting Vertically
        for (int i = y - 1; i >= 0 && roomGrid[x, i] != 0; i--) countV++;
        for (int i = y + 1; i < gridSizeY && roomGrid[x, i] != 0; i++) countV++;

        roomString = Mathf.Max(countH, countV);
        return Mathf.Max(countH, countV);
    }
    
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private Vector2Int GetRoomGridPosition(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt(worldPosition.x / roomWidth + gridSizeX / 2);
        int gridY = Mathf.RoundToInt(worldPosition.y / roomHeight + gridSizeX / 2);

        return new Vector2Int(gridX, gridY);
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}
