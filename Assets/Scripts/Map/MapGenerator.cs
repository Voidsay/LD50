using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] [Min(5)] int gridWidth, gridHeight;
    [SerializeField] float cellWidth, cellHeight;
    [SerializeField] WallCollection collection;

    [SerializeField] GameObject startPrefab;
    [SerializeField] Wall fixerWall;

    private Connections[,] grid;
    private List<Vector2Int> spawnQueue = new List<Vector2Int>();
    public List<Vector2Int> streetTiles = new List<Vector2Int>();

    public List<Wall> weighted;

    Vector2Int cur, next;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    void Generate()
    {
        grid = new Connections[gridWidth, gridHeight];

        //Initial starting room
        Vector2Int cell = new Vector2Int(Random.Range(2, gridWidth - 2), Random.Range(2, gridHeight - 2));

        InstantiateAt(startPrefab, cell, Quaternion.identity);
        SetGrid(cell + Vector2Int.up, Connections.dd | Connections.dl | Connections.dr | Connections.du | Connections.placed);
        spawnQueue.Add(cell + Vector2Int.down);
        spawnQueue.Add(cell + Vector2Int.left);
        spawnQueue.Add(cell + Vector2Int.right);

        SetGrid(cell, Connections.dd | Connections.bl | Connections.br | Connections.du | Connections.placed);
        spawnQueue.Add(cell + Vector2Int.up + Vector2Int.left);
        spawnQueue.Add(cell + Vector2Int.up + Vector2Int.right);

        SetGrid(cell + 2 * Vector2Int.up, Connections.dd | Connections.fl | Connections.fr | Connections.fu | Connections.placed);
        spawnQueue.Add(cell + 2 * Vector2Int.up + Vector2Int.left);
        spawnQueue.Add(cell + 2 * Vector2Int.up + Vector2Int.right);
        spawnQueue.Add(cell + 3 * Vector2Int.up);

        //spawn all other rooms
        while (spawnQueue.Count > 0)
        {
            GenerateRoom();
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            //yield return new WaitForSeconds(0.1f);
        }
        //yield return null;
    }

    void AddToQueue(Vector2Int cell)
    {
        if (!spawnQueue.Contains(cell) && GetConnections(cell) == Connections.none)//TODO could be simplified
        {
            spawnQueue.Add(cell);
        }
    }

    bool SetGrid(Vector2Int pos, Connections connection)
    {
        if (pos.x < 0 || pos.x >= gridWidth || pos.y < 0 || pos.y >= gridHeight)
        {
            return false;
        }
        grid[pos.x, pos.y] |= connection;
        return true;
    }

    Connections GetConnections(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridWidth || pos.y < 0 || pos.y >= gridHeight)
        {
            return Connections.border;
        }
        return grid[pos.x, pos.y];
    }

    void GenerateRoom()
    {
        Vector2Int cell = spawnQueue[0];

        List<Wall> walls = new List<Wall>();
        if (Check(cell + Vector2Int.up, Connections.wd) ||
            Check(cell + Vector2Int.right, Connections.wl) ||
            Check(cell + Vector2Int.down, Connections.wu) ||
            Check(cell + Vector2Int.left, Connections.wr))
        {
            Debug.Log("StreetTile");
            InstantiateAt(collection.streetFloor, cell, Quaternion.Euler(-90, 0, 0));
            //only spawn street compliant walls
            walls.AddRange(collection.walls.FindAll(wall => wall.streetType));
        }
        else
        {
            Debug.Log("RoomTile");
            InstantiateAt(collection.floor, cell, Quaternion.Euler(-90, 0, 0));
            //only spawn room compliant walls
            walls.AddRange(collection.walls.FindAll(wall => !wall.streetType));
        }
        SetGrid(cell, Connections.placed);

        weighted = new List<Wall>();
        foreach (var wall in walls)
        {
            for (int i = 0; i < wall.weight; i++)
            {
                weighted.Add(wall);
            }
        }
        cur = cell;

        SpawnWall(cell, cell + Vector2Int.up, 0, weighted);
        SpawnWall(cell, cell + Vector2Int.right, 1, weighted);
        SpawnWall(cell, cell + Vector2Int.down, 2, weighted);
        SpawnWall(cell, cell + Vector2Int.left, 3, weighted);

        AddToQueue(cell + Vector2Int.up);
        AddToQueue(cell + Vector2Int.right);
        AddToQueue(cell + Vector2Int.down);
        AddToQueue(cell + Vector2Int.left);

        spawnQueue.Remove(cell);
    }

    void SpawnWall(Vector2Int cell, Vector2Int nextPos, int rotation, List<Wall> weight)
    {
        List<Wall> weightCopy = new List<Wall>(weight);
        Wall selected;
        next = nextPos;
        Debug.Log(rotation + " " + GetConnections(nextPos) + " " + nextPos);


        if (spawnQueue.Contains(nextPos) && !streetTiles.Contains(nextPos))
        {
            Debug.Log("not in queue");
            weightCopy.RemoveAll(wall => (wall.connection & Connections.wu) > 0);
        }
        else if (streetTiles.Contains(nextPos) || GetConnections(nextPos) == Connections.border)
        {
            Debug.Log("have to place windows");
            weightCopy.RemoveAll(wall => (wall.connection & Connections.wu) <= 0);
        }
        else
        {
            Debug.Log("all goes");
        }


        if (GetConnections(nextPos) == Connections.none || GetConnections(nextPos) == Connections.border)
        {
            if (weightCopy.Count == 0)
            {
                weightCopy.Add(fixerWall);
            }
            selected = weightCopy[Random.Range(0, weightCopy.Count)];
            SetGrid(cell, (Connections)((int)selected.connection << rotation));
            if ((selected.connection & Connections.wu) > 0)
            {
                streetTiles.Add(nextPos);
            }
            rotation = (rotation + 2) * 90;
            InstantiateAt(selected.prefab, cell, Quaternion.Euler(-90, rotation, 0));
        }
    }

    bool Check(Vector2Int pos, Connections connection)
    {
        return (GetConnections(pos) & connection) > 0;
    }

    void InstantiateAt(GameObject prefab, Vector2Int position, Quaternion rotation)
    {
        Instantiate(prefab, new Vector3(position.x * cellWidth, 0, position.y * cellHeight), rotation);
    }

    private void OnDrawGizmos()
    {
        Vector3 origin, right, forward;
        origin = transform.position;
        right = Vector3.right * gridWidth * cellWidth;
        forward = Vector3.forward * gridHeight * cellHeight;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + right);
        Gizmos.DrawLine(origin, origin + forward);
        Gizmos.DrawLine(origin + right, origin + right + forward);
        Gizmos.DrawLine(origin + forward, origin + right + forward);

        Gizmos.color = Color.green;
        foreach (var item in spawnQueue)
        {
            Gizmos.DrawSphere(new Vector3(item.x * cellWidth, 0, item.y * cellHeight), 1f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(cur.x * cellWidth, 1, cur.y * cellHeight), 1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(new Vector3(next.x * cellWidth, 1, next.y * cellHeight), 1f);
    }
}
