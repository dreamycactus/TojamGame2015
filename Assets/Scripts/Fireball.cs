/*
 * For the Dragon's fireball Attack
 */
using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	#region Public Variables
    public float m_currentLife = 3.0f;
	private int m_dmg = 1;
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

	public void Update() {
		if (m_currentLife > 0)
        {
            m_currentLife -= Time.deltaTime;
		    if (m_currentLife < 0) {
                Destroy(this.gameObject);
		    }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(m_dmg);
            Destroy(this.gameObject);
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
