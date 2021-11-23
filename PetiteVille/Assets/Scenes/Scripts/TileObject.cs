using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;

    private void Awake()
    {
        GameManager.Instance.AddTile(x, y, this);
    }
}
