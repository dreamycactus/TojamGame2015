using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public int m_maxHealth = 100;
    private int m_currentHealth;
    private bool blocking;

    

    // Use this for initialization
    void Start()
    {
        m_currentHealth = m_maxHealth;
        blocking = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        if(blocking)
        {
            Debug.Log("blocked");
        }
        else 
        {
			m_currentHealth -= p_dmg;
            Debug.Log(m_currentHealth);

            if( col.tag == "Player")
            {
                PlayerController cont = gameObject.GetComponent<PlayerController>();
                if (cont != null)
                {
                    cont.ChangePlayerState(PlayerController.CharacterStateNames.HurtState);
                }
            }

            if (m_currentHealth <= 0)
            {
                gameObject.SendMessage("EnterDeathState");
            }
        }

    }


}
