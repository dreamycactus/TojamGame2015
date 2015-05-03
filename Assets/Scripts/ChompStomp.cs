/*
 * The Hit box code for the chomps and stomps for the Dragon
 */
using UnityEngine;
using System.Collections;

public class ChompStomp : MonoBehaviour {

    #region Public Variables
    public int m_dmg = 5;
    public Collider2D l_shootingPlayer;

    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    #endregion

    #region Accessors
    #endregion

    #region Unity Defaults
    void Awake()
    {
    }
    public void Init()
    {
        Physics2D.IgnoreCollision(l_shootingPlayer, GetComponent<Collider2D>());
    }

    public void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Health>().TakeDamage(m_dmg);
            Debug.Log("Player Hit");
        }

    }

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
