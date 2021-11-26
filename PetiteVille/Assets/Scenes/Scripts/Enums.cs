using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tetris
{
    Square,
    Line,
    T,
    L,
    L_Inverted,
    S,
    S_Inverted
}

public enum Tile
{
    Empty,
    House,
    Road,
    River,
    Park,
    Factory,
    Mountain
}


//If we add missions we need to leave the last two as Daily Shuffle and Random Shuffle for a % operation made in dontDestroy.cs
public enum Missions
{
    IndustrialArea,
    neighborhood,
    //Island,
    //HabitatedIsland,
    //Aqueduct,
    //Highways,
    DailyShuffle,
    RandomShuffle
}