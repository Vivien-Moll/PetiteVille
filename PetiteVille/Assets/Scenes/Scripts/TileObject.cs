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

    [SerializeField]
    public bool PartOfTheBoard { get; private set; } = true;
    [SerializeField]
    public bool PartOfTetrisPreview { get; private set; } = false;
    [SerializeField]
    public bool PartOfUniquePreview { get; private set; } = false;

    //[HideInInspector]
    public Tile tile = Tile.Empty;

    private void Start()
    {
        if (PartOfTheBoard)
        {
            GameManager.Instance.AddTile(_x, _y, this);
        }
        else if (PartOfTetrisPreview)
        {
            DeckContents.Instance.AddTileToTetrisPreview(_x, _y, this);
        }
        else if (PartOfUniquePreview)
        {
            DeckContents.Instance.SetTilePreviewUnique(this);
        }
    }
}
