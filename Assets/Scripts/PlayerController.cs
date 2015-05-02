/*
 * Player controller class to move the player
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	#region Public Variables
    public float m_ForceMultiplier = 5.0f;
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    // state machine code 
    public enum CharacterStateNames
    {
        NullState = 0,
        IdleState = 0,

    }
    private Dictionary<CharacterStateNames, PlayerBase> m_gameStateDictionary = new Dictionary<CharacterStateNames, PlayerBase>();
    private PlayerBase m_currentGameState = null;
    private CharacterStateNames m_currentGameStateIndex = CharacterStateNames.NullState;
    private CharacterStateNames m_nextGameStateIndex = CharacterStateNames.NullState;
    private bool m_initialised = false;

    //oer private vars
    private GameObject m_Camera;
    private Rigidbody2D m_rb;

    private Vector3 m_camOffset;
    private float m_camDepth = -10;
    
    //input placeholder bools
    private bool m_upKey = false;
    private bool m_downKey = false;
    private bool m_rightKey = false;
    private bool m_leftKey  = false;
    private bool m_chompKey = false;
    private bool m_jumpKey = false;
    private bool m_shootKey = false;
    private bool m_blockKey = false;

    #endregion

    #region Accessors
    public GameObject PlayerCamera
    {
        set { m_Camera = value; }
    }
    #endregion

    #region Unity Defaults
    void Awake ()
    {
        m_rb = gameObject.GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () 
    {
        m_camOffset = new Vector3(0, 3.5f, m_camDepth); 

	}
	// Update is called once per frame
	void Update () 
    {
        //get if grounded
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, 10))
            print("There is something in front of the object!");
        

        //GetInputs really shitty
        if (Input.GetKeyDown(KeyCode.RightArrow))
            m_rightKey = true;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            m_rightKey = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            m_rightKey = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            m_rightKey = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            m_rightKey = true;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            m_rightKey = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            m_rightKey = true;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            m_rightKey = false;




        //keep up with camera
        m_Camera.transform.position = transform.position + m_camOffset;
     
	}
    // FixedUpdate is called on a timer
    void FixedUpdate()
    {
        //this is placeholder code

        // State machine shenanigans 
        if (!m_initialised)
            return;

        if (m_currentGameState != null)
            m_currentGameState.UpdateState();

        if (m_nextGameStateIndex != CharacterStateNames.NullState)
        {
            if (m_currentGameState != null)
                m_currentGameState.ExitState(m_nextGameStateIndex);
            m_currentGameState = m_gameStateDictionary[m_nextGameStateIndex];
            m_currentGameState.EnterState(m_currentGameStateIndex);
            m_currentGameStateIndex = m_nextGameStateIndex;
            m_nextGameStateIndex = CharacterStateNames.NullState;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_rb.AddForce(Vector2.right * m_ForceMultiplier);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_rb.AddForce(-Vector2.right * m_ForceMultiplier);
        }
        //bugs

    }

    #endregion

    #region Public Methods

    public void Init()
    {
        // Initialise the bookstateDictionary
        m_gameStateDictionary.Add(CharacterStateNames.IdleState, new IdlePlayer(this));

        //start the state machine
        ChangeGameState(CharacterStateNames.IdleState); //starts in the idle


        m_initialised = true;

    }

    //Change the game state (occurs on next frame)
    public void ChangeGameState(CharacterStateNames nextState)
    {
        if (!m_gameStateDictionary.ContainsKey(nextState))
            return;

        m_nextGameStateIndex = nextState;
    }

    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}

//abstract base class
public abstract class PlayerBase
{
    protected PlayerController m_cont;

    public PlayerBase()
    {

    }

    public PlayerBase(PlayerController p_cont)
    {
        m_cont = p_cont;
    }


    public virtual void EnterState(PlayerController.CharacterStateNames p_prevState)
    {

    }

    public virtual void UpdateState()
    {

    }

    public virtual void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//while player is standing still
public class IdlePlayer : PlayerBase
{
    public IdlePlayer(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
       
    }

    public override void UpdateState()
    {
        

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//walking state
public class WalkState : PlayerBase
{
    public WalkState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {

    }

    public override void UpdateState()
    {

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}