using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DetectMousePosition : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    [SerializeField]
    private DeckContents deck;

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
        CheckTileTetris(GameManager.Instance.pattern);
    }

    private void CheckIndividualTile()
    {

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

            //Debug.Log(res.x.ToString() + res.y.ToString());

            if (res.tile != Tile.Empty)
            {
                res.gameObject.GetComponent<Image>().color = Color.red;
            }
            else
            {
                res.gameObject.GetComponent<Image>().color = Color.green;
            }

            break;
        }
    }

    private void CheckTileTetris(bool[,] pattern)
    {
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
                col = Color.red;
            }
            else
            {
                col = Color.green;
            }

            foreach(Vector2Int ofs in offsets)
            {
                if ((res.x + ofs.x >= 0) && (res.x + ofs.x <= 10) && (res.y + ofs.y >= 0) && (res.y + ofs.y <= 10))
                {
                    GameManager.Instance.board[res.x + ofs.x, res.y + ofs.y].GetComponent<Image>().color = col;
                }
            }

            break;
        }
    }
}