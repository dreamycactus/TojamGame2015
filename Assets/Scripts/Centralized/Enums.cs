/*
 * Keeps all publicly accesible Enums for 
 */

using UnityEngine;
using System.Collections;

public class Enums : MonoBehaviour {

    public enum GameStateNames
    {
        GS_00_NULL = -1,
        GS_01_MENU = 1, //MAIN MENU
        GS_02_LOADING = 2, //LOADING INTO THE GAME
        GS_03_INPLAY = 3, // PLAYING THE GAME
        GS_04_FINALE = 4, // FINAL GAME MATCH STATE
        GS_05_LEAVING = 5 // POST GAME CLEAN UP
    };

}
