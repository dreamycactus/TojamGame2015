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
    public float m_jumpSpeed = 15.0f;
    public float m_jumpDuration = 0.3f;
    public float m_chomSpeed = 10.0f;
    public float m_chompDuration = 0.1f;
    public float m_shootDuration = 0.2f;
    public float m_stompPauseDuration = 0.1f;
    public float m_stompSpeed = 25.0f;

    public int m_Direction = 1;
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

    public bool m_grounded = false;
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
        CrouchState,
        ChompState,
        ShootState,
        StompState
    }
    private Dictionary<CharacterStateNames, PlayerBase> m_gameStateDictionary = new Dictionary<CharacterStateNames, PlayerBase>();
    private PlayerBase m_currentGameState = null;
    private CharacterStateNames m_currentGameStateIndex = CharacterStateNames.NullState;
    private CharacterStateNames m_nextGameStateIndex = CharacterStateNames.NullState;
    private bool m_initialised = false;

    //oer private vars
    private GameObject m_Camera;
    private float m_size;
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
        m_camOffset = new Vector3(0, 1.5f, m_camDepth);
        m_rb.fixedAngle = true;
        m_size = 0.55f;
        Init();

	}
	// Update is called once per frame
	void Update () 
    {

        //keep up with camera
        m_camOffset.x = transform.position.x;
        m_Camera.transform.position = m_camOffset;
     
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        foreach (ContactPoint2D tact in coll.contacts)
        {
            if (tact.normal == Vector2.up)
                m_grounded = true;
        }
    }


    // FixedUpdate is called on a timer
    void FixedUpdate()
    {
        //get if grounded
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, m_size, 9);
        //if (hit.collider != null)
        //    m_grounded = true;
        //else
        //    m_grounded = false;


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

        m_grounded = false;
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
        m_gameStateDictionary.Add(CharacterStateNames.ChompState, new ChompState(this));
        m_gameStateDictionary.Add(CharacterStateNames.ShootState, new ShootState(this));
        m_gameStateDictionary.Add(CharacterStateNames.StompState, new StompState(this));
        
        //start the state machine
        ChangePlayerState(CharacterStateNames.IdleState); //starts in the idle


        m_initialised = true;

    }

    //Change the game state (occurs on next frame)
    public void ChangePlayerState(CharacterStateNames nextState)
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
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.WalkState);
        if (m_cont.m_jumpKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.JumpState);
        }
        if (m_cont.m_downKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.CrouchState);
        }
        if (m_cont.m_shootKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        }
        if (m_cont.m_chompKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ChompState);
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
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.JumpState);
        }
        if (m_cont.m_downKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.CrouchState);
        }
        if (m_cont.m_shootKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        }
        if (m_cont.m_chompKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ChompState);
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
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        }
            
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//jumping state
public class JumpState : PlayerBase
{
    private float m_jumpTime;
    public JumpState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("In Jumping State");
        if (p_prevState != PlayerController.CharacterStateNames.ShootState)
            m_jumpTime = m_cont.m_jumpDuration;
        else
            m_jumpTime = 0;
        
            
    }

    public override void UpdateState()
    {
        m_jumpTime -= Time.fixedDeltaTime;

        if(m_jumpTime > 0)
        {
            Vector2 temp = m_cont.m_rb.velocity;
            temp.y = m_cont.m_jumpSpeed;
            m_cont.m_rb.velocity = temp;
        }
        if (m_cont.m_shootKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        }
        if (m_cont.m_chompKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.StompState);
        }


        if(m_cont.m_grounded && m_jumpTime < 0)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        }
        
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//Crouching state
public class CrouchState : PlayerBase
{
    Vector3 m_scale;

    public CrouchState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }
    
    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("In Crouching State");
        m_scale = m_cont.transform.localScale;
        m_scale.y *= 0.5f;
        m_cont.transform.localScale = m_scale;

    }

    public override void UpdateState()
    {
        if (m_cont.m_chompKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ChompState);
        }
        if (m_cont.m_shootKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        }
        


        if (!m_cont.m_downKey)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        }
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        m_scale.y *= 2.0f;
        m_cont.transform.localScale = m_scale;
    }
}

//while chomping around state
public class ChompState : PlayerBase
{
    private float m_chompTime;
    private PlayerController.CharacterStateNames m_lastState;
    public ChompState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_chompTime = m_cont.m_chompDuration;
        m_lastState = p_prevState;
        Debug.Log("Chomp");
    }

    public override void UpdateState()
    {
        m_chompTime -= Time.fixedDeltaTime;
        if (m_chompTime > 0)
        {
            Vector2 temp = m_cont.m_rb.velocity;
            temp.x = m_cont.m_chomSpeed*m_cont.m_Direction;
            m_cont.m_rb.velocity = temp;
        }
        
        if(m_chompTime <= 0)
        {
            m_cont.m_rb.velocity = Vector2.zero;
            m_cont.ChangePlayerState(m_lastState);
        }

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}

//while on the ground shooting
public class ShootState : PlayerBase
{
    private float m_ShootTime;
    private PlayerController.CharacterStateNames m_laststate;
    public ShootState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("Shooting a fireball");
        m_ShootTime = m_cont.m_shootDuration;
        m_laststate = p_prevState;
    }

    public override void UpdateState()
    {
        m_ShootTime -= Time.fixedDeltaTime;
        if (m_ShootTime > 0)
        {
            //SHOOTING DIRECTIONAL CODE GOES HERE
        }
        // NO MOVEMENT;
        m_cont.m_rb.velocity = Vector2.zero;
        if (m_ShootTime <= 0)
            m_cont.ChangePlayerState(m_laststate);

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }

}

public class StompState : PlayerBase
{
    private float m_stompTime;
    private bool flag;

    public StompState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }
    
    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_stompTime = m_cont.m_stompPauseDuration;
        flag = false;
        Debug.Log("Stomping");
    }

    public override void UpdateState()
    {
        m_stompTime -= Time.fixedDeltaTime;
        if (m_stompTime > 0)
        {
            m_cont.m_rb.velocity = Vector2.zero;
        }
        if (m_stompTime < 0 && !flag)
        {
            m_cont.m_rb.velocity = new Vector2(0, m_cont.m_stompSpeed);
            flag = true;
        }


        if (m_cont.m_grounded) //when hits the ground
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }
}