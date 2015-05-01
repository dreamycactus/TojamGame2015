using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    private int m_maxHealth;
    private int m_currentHealth;

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
    }


}
