/*
 * Player controller class to move the player
 *
 */
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	#region Public Variables
    public float m_ForceMultiplier = 5.0f;
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    private GameObject m_Camera;
    private Rigidbody2D m_rb;
    
    #endregion

    #region Accessors
    public GameObject PlayerCamera
    {
        set { m_Camera = value; }
    }
    #endregion

    #region Unity Defaults
    void Awake ()
    {
        m_rb = gameObject.GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () 
    {
	
	}
	// Update is called once per frame
	void Update () 
    {
	    
	}
    // FixedUpdate is called on a timer
    void FixedUpdate()
    {
        //this is placeholder code
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_rb.AddForce(Vector2.right * m_ForceMultiplier);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_rb.AddForce(-Vector2.right * m_ForceMultiplier);
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
