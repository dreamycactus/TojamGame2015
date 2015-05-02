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

    private GameObject m_playerOne;
    private GameObject m_playerTwo;

    public float m_approachDist = 10.0f;
    public float m_shootDist = 5.0f;

    private float m_shootTimer = 0;
    public float m_shootRepeatTime = 1f;

	// Use this for initialization
	void Start () {
        m_currentState = SoldierState.Idle;

        m_rb = gameObject.GetComponent<Rigidbody2D>();

        m_playerOne = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
        //m_playerTwo = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();

        Physics2D.IgnoreCollision(m_playerOne.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(m_playerTwo.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
        
        switch (m_currentState)
        {
            case SoldierState.Idle:

                if (Mathf.Abs(transform.position.x - m_playerOne.transform.position.x) < m_approachDist)
                {
                    if (transform.position.x - m_playerOne.transform.position.x < 0)
                    {
                        m_direction = Enums.EnemyApproachDirection.Right;
                    }
                    else
                    {
                        m_direction = Enums.EnemyApproachDirection.Left;
                    }
                    
                    m_currentState = SoldierState.Approach;
                }

                /*
                if (Mathf.Abs(transform.position.x - m_playerTwo.transform.position.x) < m_approachDist)
                {
                    if (transform.position.x - m_playerTwo.transform.position.x < 0)
                    {
                        m_direction = Enums.EnemyApproachDirection.Right;
                    }
                    else
                    {
                        m_direction = Enums.EnemyApproachDirection.Left;
                    }
                    
                    m_currentState = SoldierState.Approach;
                }
                 */

                break;
            case SoldierState.Approach:

                ApplyMovement(false);

                //Checks distance and enters next state if close
                if ((Mathf.Abs(transform.position.x - m_playerOne.transform.position.x) < m_shootDist) /*|| (Mathf.Abs(transform.position.x - m_playerTwo.transform.position.x) < m_shootDist)*/)
                {
                    if (m_direction == Enums.EnemyApproachDirection.Left)
                        m_direction = Enums.EnemyApproachDirection.Right;
                    else
                        m_direction = Enums.EnemyApproachDirection.Left;

                    //m_rb.velocity = new Vector2(0.0f, 0.0f);    //Stops soldier in place to shoot
                    m_shootTimer = 0.1f;
                    m_currentState = SoldierState.Shoot;    //Switch to shooting state
                }
                break;

            case SoldierState.Shoot:

                ApplyMovement(true);
                
                if (m_shootTimer > 0)
                {
                    m_shootTimer -= Time.deltaTime;

                    if (m_shootTimer <= 0)
                    {
                        //Shoot from Emitter
                        Debug.Log("Soldier is shoot");
                        m_shootTimer = m_shootRepeatTime;
                    }
                }

                break;

            case SoldierState.Death:

                break;
        }
        
	}

    private void EnterDeathState()
    {
        Debug.Log("Soldier is kill");


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
}
