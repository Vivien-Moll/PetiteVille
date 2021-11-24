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
    private bool partOfTheBoard  = true;
    [SerializeField]
    private bool partOfTetrisPreview = false;
    [SerializeField]
    private bool partOfUniquePreview = false;

    public float randomFlower { get; private set; } = 0f;

    //[HideInInspector]
    public Tile tile = Tile.Empty;

    private void Awake()
    {
        randomFlower = Random.Range(0f, 1f);
    }

    public bool isPartOfBoard()
    {
        return partOfTheBoard;
    }

    public bool isPartOfTetris()
    {
        return partOfTetrisPreview;
    }

    public bool isPartOfUnique()
    {
        return partOfUniquePreview;
    }

    private void Start()
    {
        if (partOfTheBoard)
        {
            GameManager.Instance.AddTile(_x, _y, this);
        }
        else if (partOfTetrisPreview)
        {
            DeckContents.Instance.AddTileToTetrisPreview(_x, _y, this);
        }
        else if (partOfUniquePreview)
        {
            DeckContents.Instance.SetTilePreviewUnique(this);
        }
    }
}
