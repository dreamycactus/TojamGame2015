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
        

    }
    private Dictionary<CharacterStateNames, PlayerBase> m_gameStateDictionary = new Dictionary<CharacterStateNames, PlayerBase>();
    private GameStateBase m_currentGameState = null;
    private CharacterStateNames m_currentGameStateIndex = CharacterStateNames.NullState;
    private CharacterStateNames m_nextGameStateIndex = CharacterStateNames.NullState;
    private bool m_initialised = false;

    // movement
    private GameObject m_Camera;
    private Rigidbody2D m_rb;
    
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
	
	}
	// Update is called once per frame
	void Update () 
    {
        //GetInputs


	    
	}
    // FixedUpdate is called on a timer
    void FixedUpdate()
    {
        //this is placeholder code
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
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}


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