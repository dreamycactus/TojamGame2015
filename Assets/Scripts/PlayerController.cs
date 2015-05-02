/*
 * Player controller class to move the player
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	#region Public Variables
    public float m_movementMultiplier = 5.0f;
    public float m_maxSpeed = 5.0f;
    public Rigidbody2D m_rb;


    //input placeholder bools
    public bool m_upKey = false;
    public bool m_downKey = false;
    public bool m_rightKey = false;
    public bool m_leftKey = false;
    public bool m_chompKey = false;
    public bool m_jumpKey = false;
    public bool m_shootKey = false;
    public bool m_blockKey = false;

    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    // state machine code 
    public enum CharacterStateNames
    {
        NullState = 0,
        IdleState,
        WalkState,
        JumpState,
        CrouchState

    }
    private Dictionary<CharacterStateNames, PlayerBase> m_gameStateDictionary = new Dictionary<CharacterStateNames, PlayerBase>();
    private PlayerBase m_currentGameState = null;
    private CharacterStateNames m_currentGameStateIndex = CharacterStateNames.NullState;
    private CharacterStateNames m_nextGameStateIndex = CharacterStateNames.NullState;
    private bool m_initialised = false;

    //oer private vars
    private GameObject m_Camera;

    private Vector3 m_camOffset;
    private float m_camDepth = -10;

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
        m_rb.fixedAngle = true;
        Init();

	}
	// Update is called once per frame
	void Update () 
    {
        //get if grounded
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, fwd, 1, 9))
            Debug.Log("Touching");

        //GetInputs really shitty
        if (Input.GetKeyDown(KeyCode.RightArrow))
            m_rightKey = true;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            m_rightKey = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            m_leftKey = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            m_leftKey = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            m_upKey = true;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            m_upKey = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            m_downKey = true;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            m_downKey = false;

        if (Input.GetKeyDown(KeyCode.Space))
            m_jumpKey = true;
        if (Input.GetKeyUp(KeyCode.Space))
            m_jumpKey = false;

        //keep up with camera
        m_Camera.transform.position = transform.position + m_camOffset;
     
	}
    // FixedUpdate is called on a timer
    void FixedUpdate()
    {
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

    }

    #endregion

    #region Public Methods

    public void Init()
    {
        // Initialise the bookstateDictionary
        m_gameStateDictionary.Add(CharacterStateNames.IdleState, new IdlePlayer(this));
        m_gameStateDictionary.Add(CharacterStateNames.WalkState, new WalkState(this));
        m_gameStateDictionary.Add(CharacterStateNames.JumpState, new JumpState(this));
        m_gameStateDictionary.Add(CharacterStateNames.CrouchState, new CrouchState(this));
        
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
        Debug.Log("In Idle State");
    }

    public override void UpdateState()
    {
        if (m_cont.m_rightKey || m_cont.m_leftKey) //moving left or right
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.WalkState);
        if (m_cont.m_jumpKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.JumpState);
        }
        if (m_cont.m_downKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.CrouchState);
        }


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
        Debug.Log("In Walking State");
    }

    public override void UpdateState()
    {
        //CHECK IF GROUNDED


        //input
        if(m_cont.m_rightKey)
        {
            m_cont.m_rb.velocity = new Vector2(m_cont.m_movementMultiplier, m_cont.m_rb.velocity.y);
        }
        if (m_cont.m_leftKey)
        {
            m_cont.m_rb.velocity = new Vector2(-1.0f*m_cont.m_movementMultiplier, m_cont.m_rb.velocity.y);
        }
        if (m_cont.m_jumpKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.JumpState);
        }
        if (m_cont.m_downKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.CrouchState);
        }

        //Clamp speed
        if (Mathf.Abs(m_cont.m_rb.velocity.x) > m_cont.m_maxSpeed)
        {
            float spd = m_cont.m_rb.velocity.x;
            spd = Mathf.Clamp(spd, -m_cont.m_maxSpeed, m_cont.m_maxSpeed);
            m_cont.m_rb.velocity = new Vector2(spd, m_cont.m_rb.velocity.y);
        }

        //standing still
        if (!m_cont.m_rightKey && !m_cont.m_leftKey && !m_cont.m_jumpKey && !m_cont.m_downKey)
        {
            m_cont.m_rb.velocity = new Vector2(0, m_cont.m_rb.velocity.y);
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.IdleState);
        }
            
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//jumping state
public class JumpState : PlayerBase
{
    public JumpState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("In Jumping State");
    }

    public override void UpdateState()
    {


        if (!m_cont.m_rightKey && !m_cont.m_leftKey && !m_cont.m_jumpKey && !m_cont.m_downKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.IdleState);
        }
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//Crouching state
public class CrouchState : PlayerBase
{
    public CrouchState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("In Crouching State");
    }

    public override void UpdateState()
    {



        if (!m_cont.m_rightKey && !m_cont.m_leftKey && !m_cont.m_jumpKey && !m_cont.m_downKey)
        {
            m_cont.ChangeGameState(PlayerController.CharacterStateNames.IdleState);
        }
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}