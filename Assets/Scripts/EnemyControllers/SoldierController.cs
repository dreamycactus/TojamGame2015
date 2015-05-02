using UnityEngine;
using System.Collections;

public class SoldierController : MonoBehaviour {

    private enum SoldierState
    {
        Idle = 0,
        Approach = 1,
        Shoot = 2
    }

    SoldierState m_currentState;

    Rigidbody2D m_rb;

    public float m_maxSpeed;
    public float m_acceleration = 10.0f;
    public Enums.EnemyApproachDirection m_direction;

    private GameObject m_playerOne;
    private GameObject m_playerTwo;

    public float m_approachDist = 10.0f;

	// Use this for initialization
	void Start () {
        m_currentState = SoldierState.Idle;

        m_rb = gameObject.GetComponent<Rigidbody2D>();

        m_playerOne = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
        m_playerTwo = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();

        Physics2D.IgnoreCollision(m_playerOne.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(m_playerTwo.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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

                break;
            case SoldierState.Approach:

                if (Mathf.Abs(m_rb.velocity.x) < m_maxSpeed)
                {
                    if (m_direction == Enums.EnemyApproachDirection.Left)
                        m_rb.AddForce(Vector2.right * -10.0f);
                    else
                        m_rb.AddForce(Vector2.right * 10.0f);
                }
                break;
        }
        
	}


}
