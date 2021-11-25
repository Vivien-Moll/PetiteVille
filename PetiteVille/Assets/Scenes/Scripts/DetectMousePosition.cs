using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DetectMousePosition : MonoBehaviour
{
    public static DetectMousePosition Instance { get; private set; }

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    
    [HideInInspector]
    public bool checkmodeTetris = true;

    [SerializeField] Color colGood = Color.green;
    [SerializeField] Color colBad = Color.red;
    [SerializeField] Text description;
    [SerializeField] Image descriptionSprite;
    [SerializeField] GameObject tetrisContent;
    [SerializeField] GameObject singleton;

    public bool placementValidate { get; private set; } = false;
    public List<Vector2Int> selectedCells { get; private set; } = new List<Vector2Int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        GameManager.Instance.UpdateTiles();
        GameManager.Instance.ClearColors();
        CheckTile();
    }

    private void CheckTile()
    {
        //Reset the placement validation
        placementValidate = false;

        //Reset the selected cells
        selectedCells.Clear();

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            var res = result.gameObject.GetComponent<TileObject>();

            if (res != null)
            {
                checkTileType(res);
                if (res.isPartOfBoard())
                {

                    if (checkmodeTetris)
                    {
                        CheckTileTetris(res, GameManager.Instance.pattern);
                    }
                    else
                    {
                        CheckIndividualTile(res);
                    }
                }
                else if (res.isPartOfUnique())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        checkmodeTetris = false;
                        foreach (Transform child in tetrisContent.transform)
                            child.GetComponent<Image>().color = Color.grey;
                        singleton.GetComponent<Image>().color = Color.white;
                    }
                }
                else if (res.isPartOfTetris())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        checkmodeTetris = true;
                        foreach (Transform child in tetrisContent.transform)
                            child.GetComponent<Image>().color = Color.white;
                        singleton.GetComponent<Image>().color = Color.grey;
                    }
                }

                break;
            }
        }
    }

    private void CheckIndividualTile(TileObject res)
    {
        if (res.tile != Tile.Empty)
        {
            res.gameObject.GetComponent<Image>().color = colBad;
        }
        else
        {
            placementValidate = true;
            selectedCells.Add(new Vector2Int(res.x, res.y));
            res.gameObject.GetComponent<Image>().color = colGood;
        }
    }

    private void CheckTileTetris(TileObject res, bool[,] pattern)
    {
        var empty = true;
        Color col;
        List<Vector2Int> offsets = new List<Vector2Int>();

        for (var _x = 0; _x < 5; _x++)
        {
            for (var _y = 0; _y < 5; _y++)
            {
                if (GameManager.Instance.pattern[_x, _y] == true)
                {
                    offsets.Add(new Vector2Int(_x - 2, _y - 2));

                    if((res.x-2+_x >= 0) && (res.x - 2 + _x <= 10) && (res.y - 2 + _y >= 0) && (res.y - 2 + _y <= 10))
                    {
                        if (GameManager.Instance.board[res.x - 2 + _x, res.y - 2 + _y].tile != Tile.Empty)
                        {
                            empty = false;
                        }
                    }
                    else
                    {
                        empty = false;
                    }
                }
            }
        }

        if (!empty)
        {
            col = colBad;
        }
        else
        {
            placementValidate = true;
            col = colGood;
        }

        foreach(Vector2Int ofs in offsets)
        {
            if ((res.x + ofs.x >= 0) && (res.x + ofs.x <= 10) && (res.y + ofs.y >= 0) && (res.y + ofs.y <= 10))
            {
                if (placementValidate)
                {
                    selectedCells.Add(new Vector2Int(res.x + ofs.x, res.y + ofs.y));
                }

                GameManager.Instance.board[res.x + ofs.x, res.y + ofs.y].GetComponent<Image>().color = col;
            }
        }
    }

    private void checkTileType (TileObject t)
    {
        descriptionSprite.sprite = t.GetComponent<Image>().sprite;
        switch (t.tile)
        {
            case Tile.Empty:
                description.text = "This tile is empty... Time to fill it !";
                break;
            case Tile.House:
                description.text = "+2 points for each non-house tile surrounding a house.";
                break;
            case Tile.Road:
                description.text = "+2 points for each house and factory connected to a road.";
                break;
            case Tile.River:
                description.text = "If a river divides the map, the points of the smallest part are doubled.\nParks don't like to be near Factories.";
                break;
            case Tile.Park:
                description.text = "+2 points for each park tile in your biggest park.\nParks don't like to be near Factories.";
                break;
            case Tile.Factory:
                description.text = "Each factory gives 8 points, -2 for each park and river surrounding a factory.";
                break;
            case Tile.Mountain:
                description.text = "Just a mountain. They enjoy being there :)";
                break;
            default:
                break;
        }
    }
}
