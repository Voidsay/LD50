using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collection", menuName = "Map/Collection")]
public class WallCollection : ScriptableObject
{
    public List<Wall> walls;
    public GameObject floor;
    public GameObject streetFloor;
}
