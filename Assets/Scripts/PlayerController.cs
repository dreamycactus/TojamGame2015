/*
 * Player controller class to move the player
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	#region Public Variables
   

    public float m_movementMultiplier; //movement force multiplier
    public float m_maxSpeed; //a speed cap for horizontal movement
    public float m_jumpMagnitude; //jump force multiplier
    public float m_jumpSpeed; //jump speed cap
    public float m_jumpDuration; //how long you're jumping for
    public float m_chomSpeed; //how fast you move while chomping
    public float m_chompDuration; //how long a chomp state is
    public float m_chompWaitTime; //how long you wait at the end of a chomp
    public float m_shootDuration; //how long a shoot animation state is
    public float m_ShootDelayTime; //how long you wait before firing
    public float m_bulletSpeed;//how fast the bullets fly
    public float m_stompPauseDuration; //how long you hang in the air before stomping
    public float m_stompForwardMagnitude; //the force multiplier forward motion on stomping;
    public float m_stompSpeed;//how fast you fall when stomping
    public float m_blockDuration; // length of block
    public float m_maxBlockCoolDown;// how long between blocks (max time)
    public float m_blockCooldown;  //inner cooldown used by class, starts at maxBlockCoolDown
    public float m_dashSpeed; //how fast you dash
    public float m_dashDuration; //how long the dash lasts
    public float m_maxDashCoolDown; // cooldown between dashes
    public float m_dashCooldown; //inner cooldown used by clss, starts at maxDashCooldown
    public float m_riseDuration; //how long you are rising for
    public float m_riseSpeedX; // vertical rise speed
    public float m_riseSpeedY; // horizontal rise speed
    public float m_hurtTimer; // long the hurt stun lasts

    public int m_Direction = 1; //direcitonal facing
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
    public bool m_dashKey = false;

    public bool m_grounded = false;

    public Animator m_animator;

    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    // state machine code 
    public enum CharacterStateNames
    {
        NullState = 0,
        IdleState = 1,
        WalkState = 2,
        JumpState = 3,
        CrouchState = 4,
        ChompState = 5,
        ShootState = 6,
        StompState = 7,
        BlockState = 8,
        DashState = 9,
        RisingState = 10,
        HurtState = 11
    }
    private Dictionary<CharacterStateNames, PlayerBase> m_gameStateDictionary = new Dictionary<CharacterStateNames, PlayerBase>();
    private PlayerBase m_currentGameState = null;
    public CharacterStateNames m_currentGameStateIndex = CharacterStateNames.NullState;
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
//////////////////////////////////////////////////////////////////////////
        m_movementMultiplier = 1000.0f; //movement speed multiplier
        m_maxSpeed = 10.0f; //a speed cap for horizontal movement
        m_jumpMagnitude = 5000f; ; //jump force multiplier
        m_jumpSpeed = 15.0f; //how fast you jump up
        m_jumpDuration = 0.3f; //how long you're jumping for
        m_chomSpeed = 10.0f; //how fast you move while chomping
        m_chompDuration = 0.3f; //how long a chomp state is
        m_chompWaitTime = 0.2f; //how long you wait at the end of a chomp
        m_shootDuration = 0.4f; //how long a shoot animation state is
        m_ShootDelayTime = 0.2f; //how long you wait before firing
        m_bulletSpeed = 20.0f; //how fast the bullets fly
        m_stompPauseDuration = 0.1f; //how long you hang in the air before stomping
        m_stompForwardMagnitude = 5.0f; //the force multiplier forward motion on stomping;
        m_stompSpeed = 25.0f; //how fast you fall when stomping
        m_blockDuration = 0.5f; // length of block
        m_maxBlockCoolDown = 0.5f; // how long between blocks (max time)
        m_dashSpeed = 15.0f; //how fast you dash
        m_dashDuration = 0.5f;
        m_maxDashCoolDown = 1.0f; // cooldown between dashes
        m_riseDuration = 0.5f; //how long you are rising for
        m_riseSpeedX = 15.0f; // vertical rise speed
        m_riseSpeedY = 15.0f; // horizontal rise speed
        m_hurtTimer = 0.3f; // long the hurt stun lasts
//////////////////////////////////////////////////////////////////////////
        m_camOffset = new Vector3(0, 1.5f, m_camDepth);
        m_rb.fixedAngle = true;
        m_size = 0.55f;
        m_animator = gameObject.GetComponent<Animator>();
        m_blockCooldown = m_maxBlockCoolDown;
        m_dashCooldown = m_maxDashCoolDown;
        Init();

	}
	// Update is called once per frame
	void Update () 
    {
        // Multiply the player's x local scale by -1
        Vector3 l_facing = transform.localScale;
        l_facing.x = m_Direction;
        transform.localScale = l_facing;

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
        //block cooldown countdown
        m_blockCooldown -= Time.fixedDeltaTime;
        m_dashCooldown -= Time.fixedDeltaTime;

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
        m_gameStateDictionary.Add(CharacterStateNames.BlockState, new BlockState(this));
        m_gameStateDictionary.Add(CharacterStateNames.DashState, new DashState(this));
        m_gameStateDictionary.Add(CharacterStateNames.RisingState, new RisingState(this));
        m_gameStateDictionary.Add(CharacterStateNames.HurtState, new HurtState(this));

        //start the state machine
        ChangePlayerState(CharacterStateNames.IdleState); //starts in the idle

        m_initialised = true;

    }

    //Change the game state (occurs on next frame)
    public void ChangePlayerState(CharacterStateNames nextState)
    {
        if (!m_gameStateDictionary.ContainsKey(nextState))
            return;
        m_animator.SetInteger(HashIDs.State, (int)nextState);
        if (m_nextGameStateIndex == CharacterStateNames.HurtState)
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
        Debug.Log("Idling");
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

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
        if (m_cont.m_blockKey && m_cont.m_blockCooldown < 0)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.BlockState);
        }
        if (m_cont.m_dashKey && m_cont.m_dashCooldown < 0)
        {
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.DashState);
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
        Debug.Log("Walking");
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);
        //input
        /// new code
        float l_accelerationMultiplier = 1 - (m_cont.m_rb.velocity.magnitude / m_cont.m_maxSpeed); 
        ///
        if(m_cont.m_rightKey)
        {
            m_cont.m_rb.AddForce(new Vector2(m_cont.m_movementMultiplier * l_accelerationMultiplier, 0.0f));
            //m_cont.m_rb.velocity = new Vector2(m_cont.m_movementMultiplier, m_cont.m_rb.velocity.y);
        }
        if (m_cont.m_leftKey)
        {
            m_cont.m_rb.AddForce(new Vector2(-m_cont.m_movementMultiplier * l_accelerationMultiplier, 0.0f));
            //m_cont.m_rb.velocity = new Vector2(-1.0f * m_cont.m_movementMultiplier, m_cont.m_rb.velocity.y);
        }
        if (m_cont.m_jumpKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.JumpState);
        if (m_cont.m_downKey && m_cont.m_grounded)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.CrouchState);
        if (m_cont.m_shootKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        if (m_cont.m_chompKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ChompState);
        if (m_cont.m_blockKey && m_cont.m_blockCooldown < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.BlockState);
        if (m_cont.m_dashKey && m_cont.m_dashCooldown < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.DashState);

        //Clamp speed
       

        //if (Mathf.Abs(m_cont.m_rb.velocity.x) > m_cont.m_maxSpeed)
        //{
        //    float spd = m_cont.m_rb.velocity.x;
        //    spd = Mathf.Clamp(spd, -m_cont.m_maxSpeed, m_cont.m_maxSpeed);
        //    m_cont.m_rb.velocity = new Vector2(spd, m_cont.m_rb.velocity.y);
        //}

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
        if (p_prevState != PlayerController.CharacterStateNames.ShootState)
            m_jumpTime = m_cont.m_jumpDuration;
        else
            m_jumpTime = 0;
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);
        m_jumpTime -= Time.fixedDeltaTime;

        /// new code
        float l_accelerationMultiplier = 1 - (m_cont.m_rb.velocity.magnitude / m_cont.m_jumpSpeed);
        ///
        if(m_jumpTime > 0)
        {
            m_cont.m_rb.AddForce(new Vector2(0.0f, m_cont.m_jumpMagnitude * l_accelerationMultiplier));
            //Vector2 temp = m_cont.m_rb.velocity;
            //temp.y = m_cont.m_jumpSpeed;
            //m_cont.m_rb.velocity = temp;
        }
        if (m_cont.m_shootKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        if (m_cont.m_chompKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.StompState);

        if(m_cont.m_grounded && m_jumpTime < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
    }
}

//Crouching state
public class CrouchState : PlayerBase
{
    private BoxCollider2D m_col;
    public CrouchState(PlayerController p_cont)
    {
        m_cont = p_cont;
        m_col = m_cont.GetComponent<BoxCollider2D>();
    }
    
    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("In Crouching State");
        m_col.size = new Vector2(4.0f, 2.0f);
        m_col.offset = new Vector2(0.0f, -1.2f);
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);
        if (m_cont.m_chompKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ChompState);
        if (m_cont.m_shootKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.ShootState);
        if (m_cont.m_jumpKey && m_cont.m_grounded)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.RisingState);
        if (m_cont.m_blockKey && m_cont.m_blockCooldown < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.BlockState);
        if (m_cont.m_dashKey && m_cont.m_dashCooldown < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.DashState);

        if (!m_cont.m_downKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        m_col.size = new Vector2(3.0f, 4.0f);
        m_col.offset = new Vector2(0f, -0.2f);
    }
}

//while chomping around state
public class ChompState : PlayerBase
{
    private float m_chompTime;
    private float m_waitTime;
    private PlayerController.CharacterStateNames m_lastState;
    private BoxCollider2D m_col;
    private bool m_hasChomped = false;
    private GameObject m_hitbox;
    public ChompState(PlayerController p_cont)
    {
        m_cont = p_cont;
        m_col = m_cont.GetComponent<BoxCollider2D>();
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_chompTime = m_cont.m_chompDuration;
        m_waitTime = m_cont.m_chompWaitTime;
        m_lastState = p_prevState;
        m_col.size = new Vector2(4.0f, 2.0f);
        m_col.offset = new Vector2(0.0f, -1.2f);
        m_hasChomped = false;

        Debug.Log("Chomp");
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

        m_chompTime -= Time.fixedDeltaTime;
        
        if (m_chompTime > 0)
        {
            /// new code
            float l_accelerationMultiplier = 1 - (m_cont.m_rb.velocity.magnitude / m_cont.m_chomSpeed);
            m_cont.m_rb.AddForce(new Vector2(m_cont.m_Direction*m_cont.m_movementMultiplier * l_accelerationMultiplier, 0.0f));
            ///
            //Vector2 temp = m_cont.m_rb.velocity;
            //temp.x = m_cont.m_chomSpeed*m_cont.m_Direction;
            //m_cont.m_rb.velocity = temp;

            if (m_hasChomped == false)
            {
                m_hitbox = Object.Instantiate(Resources.Load("ChompBox", typeof(GameObject))) as GameObject;
                
                if (m_lastState == PlayerController.CharacterStateNames.CrouchState)
                    m_hitbox.transform.position = m_cont.transform.position + new Vector3(m_cont.m_Direction * 3.0f, -1, 0);
                else
                    m_hitbox.transform.position = m_cont.transform.position + new Vector3(m_cont.m_Direction * 2.1f, 0, 0);

                m_hitbox.GetComponent<ChompStomp>().l_shootingPlayer = m_cont.GetComponent<BoxCollider2D>();
                m_hitbox.GetComponent<ChompStomp>().Init();
                
                m_hasChomped = true;
            }

        }
        if(m_chompTime <= 0)
        {
            m_cont.m_rb.velocity = Vector2.zero;
            m_waitTime -= Time.fixedDeltaTime;
        }
        if(m_waitTime <= 0)
            m_cont.ChangePlayerState(m_lastState);

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        m_col.size = new Vector2(3.0f, 4.0f);
        m_col.offset = new Vector2(0f, -0.2f);
        Object.Destroy(m_hitbox);
    }
}

//while on the ground shooting
public class ShootState : PlayerBase
{
    private float m_ShootTime;
    private float m_ShootDelayTime;
    private PlayerController.CharacterStateNames m_laststate;
    private bool m_hasShot;
    private Vector2 m_shootDirection;
    public ShootState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        Debug.Log("Shooting a fireball");
        m_ShootTime = m_cont.m_shootDuration;
        m_ShootDelayTime = m_cont.m_ShootDelayTime;
        m_laststate = p_prevState;
        m_hasShot = false;
        m_shootDirection = new Vector2(m_cont.m_Direction, 0.0f);
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);
        m_ShootTime -= Time.fixedDeltaTime;
        m_ShootDelayTime -= Time.fixedDeltaTime;

        if (m_ShootTime > 0)
        {
            //Get shot direction
            
            if(m_cont.m_upKey)
                m_shootDirection.y = 1.0f;
            else if (m_cont.m_downKey && !m_cont.m_grounded)
                m_shootDirection.y = -1.0f;
            else
                m_shootDirection.y = 0.0f;

            //shoot bullet
            if (!m_hasShot && m_ShootDelayTime < 0)
            {
                GameObject bullet = Object.Instantiate(Resources.Load("FireBall", typeof(GameObject))) as GameObject;
                bullet.transform.position = m_cont.transform.position + new Vector3(m_cont.m_Direction * 2.0f, 0.6f, 0);
                if (m_cont.m_Direction > 0 && m_shootDirection.y > 0)
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                if (m_cont.m_Direction > 0 && m_shootDirection.y < 0)
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
                if (m_cont.m_Direction < 0 && m_shootDirection.y > 0)
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
                if (m_cont.m_Direction < 0 && m_shootDirection.y < 0)
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));

                bullet.GetComponent<Fireball>().l_shootingPlayer = m_cont.GetComponent<BoxCollider2D>();
                bullet.GetComponent<Fireball>().Init();
                bullet.GetComponent<Rigidbody2D>().velocity = m_shootDirection*m_cont.m_bulletSpeed;
                m_hasShot = true;
            }
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

//while butt stomping 
public class StompState : PlayerBase
{
    private float m_stompTime;
    private bool flag;
    private GameObject m_hitbox;
    private bool m_hasStomped;
    private PlayerController.CharacterStateNames m_lastState;
    
    public StompState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }
    
    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_stompTime = m_cont.m_stompPauseDuration;
        flag = false;
        m_hasStomped = false;
        m_lastState = p_prevState;
        Debug.Log("Stomping");
    }

    public override void UpdateState()
    {
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);
        m_stompTime -= Time.fixedDeltaTime;
        if (m_stompTime > 0)
            m_cont.m_rb.velocity = Vector2.zero;
        if (m_hasStomped == false)
        {
            m_hitbox = Object.Instantiate(Resources.Load("ChompBox", typeof(GameObject))) as GameObject;
            m_hitbox.transform.position = m_cont.transform.position + new Vector3(0, -1, 0);
            m_hitbox.GetComponent<ChompStomp>().l_shootingPlayer = m_cont.GetComponent<BoxCollider2D>();
            m_hitbox.GetComponent<ChompStomp>().Init();

            m_hasStomped = true;
        }
        if(m_hitbox != null)
            m_hitbox.transform.position = m_cont.transform.position + new Vector3(0, -2, 0);

        if (m_stompTime < 0 && !flag)
        {
            m_cont.m_rb.velocity = new Vector2(0, m_cont.m_stompSpeed);
            m_cont.m_rb.AddForce(new Vector2(m_cont.m_Direction * m_cont.m_movementMultiplier*5f, 0.0f));
            flag = true;
        }


        if (m_cont.m_grounded) //when hits the ground
        {
            GameObject buttSmoke = Object.Instantiate(Resources.Load("ButtSmoke", typeof(GameObject))) as GameObject;
            buttSmoke.transform.position = m_cont.transform.position + new Vector3(0, -1.35f, 0);
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        }
            
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        Object.Destroy(m_hitbox);
    }
}

//while blocking
public class BlockState : PlayerBase
{
    private float m_blockTime;
    public BlockState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_cont.GetComponent<Health>().Blocking = true;
        m_blockTime = m_cont.m_blockDuration;
        Debug.Log("Blocking");
    }

    public override void UpdateState()
    {
        m_blockTime -= Time.fixedDeltaTime;
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

        if (!m_cont.m_blockKey)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
        if (m_blockTime <= 0) // out of time
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);

    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        m_cont.GetComponent<Health>().Blocking = false;
        m_cont.m_blockCooldown = m_cont.m_maxBlockCoolDown;
    }

}

//While Dashing
public class DashState : PlayerBase
{
    private float m_dashTime;
    private int m_direction;
    public DashState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_dashTime = m_cont.m_dashDuration;
        m_direction = m_cont.m_Direction;
        Debug.Log("Dashing!");
    }

    public override void UpdateState()
    {
        m_dashTime -= Time.fixedDeltaTime;
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

        if (m_dashTime > 0)
        {
            /// new code
            float l_accelerationMultiplier = 1 - (m_cont.m_rb.velocity.magnitude / m_cont.m_dashSpeed);
            m_cont.m_rb.AddForce(new Vector2(m_direction * m_cont.m_stompForwardMagnitude * l_accelerationMultiplier * 3f, 0.0f));
            ///
            //Vector2 temp = m_cont.m_rb.velocity;
            //temp.x = m_cont.m_dashSpeed * m_direction;
            //m_cont.m_rb.velocity = temp;
        }

        if (m_dashTime <= 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
        m_cont.m_dashCooldown = m_cont.m_maxDashCoolDown;
    }
}

//While doing a Rising Attack
public class RisingState : PlayerBase
{
    private float m_riseTime;
    private int m_direction;
    public RisingState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }
    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_riseTime = m_cont.m_riseDuration;
        m_direction = m_cont.m_Direction;
        Debug.Log("Rising!");
    }

    public override void UpdateState()
    {
        m_riseTime -= Time.fixedDeltaTime;
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

        if(m_riseTime > 0)
        {
            Vector2 temp = new Vector2(m_cont.m_riseSpeedX * m_direction, m_cont.m_riseSpeedY);
            m_cont.m_rb.velocity = temp;
        }
        if (m_riseTime <= 0)
        {
            Vector2 temp = new Vector2(0.0f, m_cont.m_rb.velocity.y);
            m_cont.m_rb.velocity = temp;
        }

        if (m_riseTime <= 0 && m_cont.m_grounded)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
    }

    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {
    }
}

//while being hurt
public class HurtState : PlayerBase
{
    private int m_direction = -1;
    private float m_hurtTimer;
    public HurtState(PlayerController p_cont)
    {
        m_cont = p_cont;
    }

    public override void EnterState(PlayerController.CharacterStateNames p_prevState)
    {
        m_direction = m_cont.m_Direction;
        m_hurtTimer = m_cont.m_hurtTimer;
    }
    public override void UpdateState()
    {
        m_hurtTimer -= Time.fixedDeltaTime;
        m_cont.m_animator.SetInteger(HashIDs.State, (int)m_cont.m_currentGameStateIndex);

        if (m_hurtTimer > 0)
        {
            /// new code
            float l_accelerationMultiplier = 1 - (m_cont.m_rb.velocity.magnitude / m_cont.m_maxSpeed);
            m_cont.m_rb.AddForce(new Vector2(-1f * m_cont.m_Direction * m_cont.m_movementMultiplier * l_accelerationMultiplier, 0.5f* (-1f * m_cont.m_Direction * m_cont.m_movementMultiplier * l_accelerationMultiplier)));
            ///
        }

        if (m_hurtTimer < 0)
            m_cont.ChangePlayerState(PlayerController.CharacterStateNames.IdleState);
    }
    public override void ExitState(PlayerController.CharacterStateNames p_nextState)
    {

    }


}