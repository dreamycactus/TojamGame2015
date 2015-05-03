using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public int m_maxHealth = 100;
    private int m_currentHealth;

    // Use this for initialization
    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

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

    public void TakeDamage(int p_dmg)
    {
        m_currentHealth -= p_dmg;
		Debug.Log(m_currentHealth);

        if (m_currentHealth < 0)
        {
            gameObject.SendMessage("EnterDeathState");
        }
    }


}
