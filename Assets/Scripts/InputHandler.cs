/*
 * This class is to be used to override the unity default input manager
 * not sure how we want to do this though
 */
using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	#region Public Variables
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    PlayerController m_player1;
    PlayerController m_player2;

    #endregion

    #region Accessors
    #endregion

    #region Unity Defaults
    
	// Use this for initialization
	void Start () {
        m_player1 = Managers.GetInstance().GetPlayerManager().GetPlayerOne().GetComponent<PlayerController>();
        m_player2 = Managers.GetInstance().GetPlayerManager().GetPlayerTwo().GetComponent<PlayerController>();   
	}
	
	// Update is called once per frame
	void Update () {

        /****************************************************
         * PLAYER 1 INPUT
         *///////////////////////////////////////////////////

        
        //GetInputs really shitty
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_player1.m_Direction = 1;
            m_player1.m_rightKey = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
            m_player1.m_rightKey = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_player1.m_Direction = -1;
            m_player1.m_leftKey = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            m_player1.m_leftKey = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            m_player1.m_upKey = true;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            m_player1.m_upKey = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            m_player1.m_downKey = true;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            m_player1.m_downKey = false;

        if (Input.GetKeyDown(KeyCode.Space))
            m_player1.m_jumpKey = true;
        if (Input.GetKeyUp(KeyCode.Space))
            m_player1.m_jumpKey = false;

        if (Input.GetKeyDown(KeyCode.Z))
            m_player1.m_chompKey = true;
        if (Input.GetKeyUp(KeyCode.Z))
            m_player1.m_chompKey = false;

        if (Input.GetKeyDown(KeyCode.X))
            m_player1.m_shootKey = true;
        if (Input.GetKeyUp(KeyCode.X))
            m_player1.m_shootKey = false;



        /****************************************************
         * PLAYER 2 INPUT
         *///////////////////////////////////////////////////

        if (Input.GetButtonDown("A_Button")) //A
            m_player2.m_jumpKey = true;
        if (Input.GetButtonUp("A_Button"))
            m_player2.m_jumpKey = false;

        if (Input.GetButtonDown("B_Button")) //B
            m_player2.m_shootKey = true;
        if (Input.GetButtonUp("B_Button"))
            m_player2.m_shootKey = false;

        if (Input.GetButtonDown("X_Button")) //X
            m_player2.m_chompKey = true;
        if (Input.GetButtonUp("X_Button"))
            m_player2.m_chompKey = false;


        if (Input.GetAxis("Horizontal_Dpad") == 1) { //Right input == true
            m_player2.m_rightKey = true; m_player2.m_leftKey = false;
            m_player2.m_Direction = 1;
        }
        else if (Input.GetAxis("Horizontal_Dpad") == -1){ //Left input = true
            m_player2.m_leftKey = true; m_player2.m_rightKey = false;
            m_player2.m_Direction = -1;
        }
        else
        {
            m_player2.m_leftKey = false; m_player2.m_rightKey = false; 
        }
        
        if (Input.GetAxis("Vertical_Dpad") == 1) //Right input == true
        {
            m_player2.m_upKey = true; m_player2.m_downKey = false;
        }
        else if (Input.GetAxis("Vertical_Dpad") == -1) //Left input = true
        {
            m_player2.m_upKey = false; m_player2.m_downKey = true;
        }
        else
        {
            m_player2.m_upKey = false; m_player2.m_downKey = false;
        }

	}

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
