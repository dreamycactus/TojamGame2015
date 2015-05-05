using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public int m_maxHealth = 100;
    private int m_currentHealth;
    private bool blocking;

	private float endGameTimer = 0;

    

    // Use this for initialization
    void Start()
    {
        m_currentHealth = m_maxHealth;
        blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (endGameTimer > 0) {
			endGameTimer -= Time.deltaTime;

			if (endGameTimer <= 0) {
				Application.LoadLevel("MainMenu");
			}
		}
    }

    public bool Blocking
    {
        set { blocking = value; }
    }

    public int currentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    public int maxHealth
    {
        get { return m_maxHealth; }
        set { m_maxHealth = value; }
    }

    public void TakeDamage(int p_dmg, Collider2D col)
    {
		if (gameObject.tag == "Player") {
			Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
			float force = 200 * p_dmg / 5 * Mathf.Sign(transform.position.x- col.transform.position.x);
			body.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
		}
        if(blocking)
        {
            Debug.Log("blocked");
        }
        else 
        {
			m_currentHealth -= p_dmg;
            Debug.Log(m_currentHealth);

            if( gameObject.tag == "Player" && m_currentHealth > 0)
            {
                 gameObject.SendMessage("ApplyDamage", p_dmg);
            }

            if (m_currentHealth <= 0)
            {
				if (gameObject.tag == "Player") {
					endGameTimer = 5.0f;
					gameObject.SendMessage("EnterDeathState");
				} else {
					gameObject.SendMessage("EnterDeathState");
				}
            }
        }

    }


}
