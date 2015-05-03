using UnityEngine;
using System.Collections;

public class Kamikaze : MonoBehaviour {

    private enum KamikazeState
    {
        Idle = 0,
        Approach = 1,
        Death = 2
    }

    KamikazeState m_currentState;

    Rigidbody2D m_rb;

    public float m_maxSpeed = 5.0f;
    public float m_acceleration = 10.0f;
    public Enums.EnemyApproachDirection m_direction;

    private GameObject m_target;

    public float m_approachDist = 10.0f;
    public float m_disappearDist = 10.0f;

    private Animator m_animator;

	public int m_explodeDamage = 10;

	// Use this for initialization
	void Start () {
        m_currentState = KamikazeState.Idle;

        m_rb = gameObject.GetComponent<Rigidbody2D>();

        GameObject l_playerOne = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
        GameObject l_playerTwo = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();

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
            case KamikazeState.Idle:

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
                    
                    m_currentState = KamikazeState.Approach;
					AudioManager.GetInstance().PlayClip(8);

				}

				break;
            case KamikazeState.Approach:

				ApplyMovement();

				if (Mathf.Abs(transform.position.x - m_target.transform.position.x) > m_disappearDist)
				{
					Destroy(gameObject);    //Destroys enemy if player gets too far
				}
                break;

            case KamikazeState.Death:

				GameObject exp = ExplosionManager.Inst.GetExplosion();
				exp.transform.position = transform.position;
				Destroy(this.gameObject);
                break;
        }
        
	}

    private void EnterDeathState()
    {
        Debug.Log("Soldier is kill");

		m_currentState = KamikazeState.Death;
    }

    private void ApplyMovement()
    {
            if (Mathf.Abs(m_rb.velocity.x) < m_maxSpeed)
            {
                if (m_direction == Enums.EnemyApproachDirection.Left)
                    m_rb.AddForce(Vector2.right * -10.0f);
                else
                    m_rb.AddForce(Vector2.right * 10.0f);
            }
    }

	private void FlipSprite()
	{
		transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.tag == "Player")
		{
			col.gameObject.GetComponent<Health>().TakeDamage(m_explodeDamage, gameObject.GetComponent<Collider2D>());
			
			m_currentState = KamikazeState.Death;
		}
		
	}
}
