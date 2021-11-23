using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    [SerializeField]
    private int _x;
    public int x { get { return _x; } private set { _x = value; } }

    [SerializeField]
    private int _y;
    public int y { get { return _y; } private set { _y = value; } }

    //[HideInInspector]
    public Tile tile = Tile.Empty;

    private void Start()
    {
        GameManager.Instance.AddTile(_x, _y, this);
    }
}
