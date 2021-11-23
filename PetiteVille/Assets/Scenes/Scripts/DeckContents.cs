using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckContents : MonoBehaviour
{
    private int size = 14;
    public GameObject deckContents;

    private Text houseAmount;
    private Text parkAmount;
    private Text riverAmount;
    private Text roadAmount;
    private int nbHouse = 0;
    private int nbRoad = 0;
    private int nbRiver = 0;
    private int nbPark = 0;


    // Start is called before the first frame update
    void Start()
    {
        houseAmount = deckContents.transform.Find("House").transform.Find("amount").GetComponent<Text>();
        parkAmount = deckContents.transform.Find("Park").transform.Find("amount").GetComponent<Text>();
        riverAmount = deckContents.transform.Find("River").transform.Find("amount").GetComponent<Text>();
        roadAmount = deckContents.transform.Find("Road").transform.Find("amount").GetComponent<Text>();

        //Tile[] contents = new Tile[size];
        //Tetris[] forms = new Tetris[size];
        Queue<Tile> contents = new Queue<Tile>();
        Queue<Tetris> forms = new Queue<Tetris>();

        List<Tile> acceptedTiles = new List<Tile> { Tile.House, Tile.Park, Tile.River, Tile.Road };
        List<Tetris> acceptedTetris = new List<Tetris> { Tetris.L, Tetris.Line, Tetris.L_Inverted, Tetris.S, Tetris.Square, Tetris.S_Inverted, Tetris.T };

        for (int i = 0; i < size; i++)
        {
            contents.Enqueue(acceptedTiles[Random.Range(0, acceptedTiles.Count)]);
            forms.Enqueue(acceptedTetris[Random.Range(0, acceptedTetris.Count)]);
        }

        setDeckContent(contents);
        showDeckValues();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setDeckContent(Queue<Tile> tile_queue)
    {

        foreach (Tile t in tile_queue)
        {
            switch (t)
            {
                case Tile.House:
                    nbHouse++;
                    break;
                case Tile.Road:
                    nbRoad++;
                    break;
                case Tile.River:
                    nbRiver++;
                    break;
                case Tile.Park:
                    nbPark++;
                    break;
                default:
                    break;
            }
        }
    }

    private void showDeckValues()
    {
        houseAmount.text = "×" + nbHouse;
        parkAmount.text = "×" + nbRoad;
        riverAmount.text = "×" + nbRiver;
        roadAmount.text = "×" + nbPark;
    }
}
