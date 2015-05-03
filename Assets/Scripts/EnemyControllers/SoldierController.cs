using UnityEngine;
using System.Collections;

public class SoldierController : MonoBehaviour {

    private enum SoldierState
    {
        Idle = 0,
        Approach = 1,
        Shoot = 2,
        Death = 3,
    }

    SoldierState m_currentState;

    Rigidbody2D m_rb;

    public float m_maxSpeed = 5.0f;
    public float m_maxStrafeSpeed = 3.0f;
    public float m_acceleration = 10.0f;
    public Enums.EnemyApproachDirection m_direction;

    private GameObject m_target;

    public float m_approachDist = 10.0f;
    public float m_shootDist = 5.0f;
    public float m_disappearDist = 10.0f;

    private BulletEmitter m_bulletEmitter;

    private Animator m_animator;

    private float m_shootTimer = 0;

    public bool m_standShoot = false;

	public bool m_isTank = false;

	public bool m_ignorePlayerCollision = true;

	// Use this for initialization
	void Start () {
        m_currentState = SoldierState.Idle;

        m_rb = gameObject.GetComponent<Rigidbody2D>();

        m_bulletEmitter = gameObject.GetComponent<BulletEmitter>();

        GameObject l_playerOne = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
        GameObject l_playerTwo = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();

		if (m_ignorePlayerCollision)
		{
			Physics2D.IgnoreCollision(l_playerOne.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			Physics2D.IgnoreCollision(l_playerTwo.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}

        m_animator = gameObject.GetComponent<Animator>();

		if (Mathf.Abs(transform.position.x - l_playerOne.transform.position.x) < Mathf.Abs(transform.position.x - l_playerTwo.transform.position.x))
		{
			m_target = l_playerOne;
		}
		else
		{
			m_target = l_playerTwo;
		}
	}
	
	// Update is called once per frame
	void Update () {
        
        switch (m_currentState)
        {
            case SoldierState.Idle:

                if (Mathf.Abs(transform.position.x - m_target.transform.position.x) < m_approachDist)
                {
                    if (transform.position.x - m_target.transform.position.x < 0)
                    {
                        m_direction = Enums.EnemyApproachDirection.Right;
                    }
                    else
                    {
                        m_direction = Enums.EnemyApproachDirection.Left;
                    }
                    
                    m_currentState = SoldierState.Approach;
                }

                break;
            case SoldierState.Approach:

                ApplyMovement(false);

                //Checks distance and enters next state if close
                if ((Mathf.Abs(transform.position.x - m_target.transform.position.x) < m_shootDist))
                {
                    if (m_direction == Enums.EnemyApproachDirection.Left)
                        m_direction = Enums.EnemyApproachDirection.Right;
                    else
                        m_direction = Enums.EnemyApproachDirection.Left;

                    
                    if (m_standShoot)
                    {
                        m_rb.velocity = new Vector2(0.0f, 0.0f);    //Stops soldier in place to shoot
						m_rb.mass = 1000000;
                        m_animator.SetBool("Stand", true);
                    }
                    else
                    {
                        m_animator.SetBool("Strafe", true);
                    }

                    m_currentState = SoldierState.Shoot;    //Switch to shooting state

                    m_bulletEmitter.ToggleAutoFire();

                    m_shootTimer = 0.1f;
                }
                break;

            case SoldierState.Shoot:

                m_bulletEmitter.Target = m_target.transform.position;

                if (m_shootTimer > 0)
                {
                    m_shootTimer -= Time.deltaTime;

                    if (m_shootTimer <= 0)
                    {
                        Debug.Log("Shoot animate");
                        m_animator.SetTrigger("Shoot");
                        m_shootTimer = Constants.NORMAL_BULLET_PERIOD;
                    }
                }

                if (!m_standShoot)
                {
                    ApplyMovement(true);
                }

				if (m_isTank)
				if (transform.position.x - m_target.transform.position.x > 0 && transform.localScale.x < 0)
				{
					if (m_isTank)
					{
						m_bulletEmitter.Patterns[0] = "parabola:1:-45";
						m_bulletEmitter.Refresh();
					}	

					FlipSprite();
				}
				else if (transform.position.x - m_target.transform.position.x < 0 && transform.localScale.x > 0)
				{
					if (m_isTank)
					{
						m_bulletEmitter.Patterns[0] = "parabola:1:45";
						m_bulletEmitter.Refresh();
					}

					FlipSprite();
				}

                if (Mathf.Abs(transform.position.x - m_target.transform.position.x) > m_disappearDist)
                {
                    Destroy(gameObject);    //Destroys enemy if player gets too far
                }

                break;

            case SoldierState.Death:
				GameObject exp = ExplosionManager.Inst.GetExplosion();
				exp.transform.position = transform.position;
				Destroy(this.gameObject);
                break;
        }
        
	}

    private void EnterDeathState()
    {
        Debug.Log("Soldier is kill");

		m_currentState = SoldierState.Death;
    }

    private void ApplyMovement(bool p_isStrafing)
    {
        if (!p_isStrafing)
        {
            if (Mathf.Abs(m_rb.velocity.x) < m_maxSpeed)
            {
                if (m_direction == Enums.EnemyApproachDirection.Left)
                    m_rb.AddForce(Vector2.right * -10.0f);
                else
                    m_rb.AddForce(Vector2.right * 10.0f);
            }
        }
        else
        {
            if (Mathf.Abs(m_rb.velocity.x) < m_maxStrafeSpeed)
            {
                if (m_direction == Enums.EnemyApproachDirection.Left)
                    m_rb.AddForce(Vector2.right * -10.0f);
                else
                    m_rb.AddForce(Vector2.right * 10.0f);
            }
        }
        
    }

	private void FlipSprite()
	{
		transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void OnCollisionEnter2D(Collision2D other)
    {
        m_rb.velocity = new Vector2(0 , m_rb.velocity.y);
    }
}
