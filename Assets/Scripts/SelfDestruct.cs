/*
 *
 *
 */
using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	#region Public Variables
    private float timer = 0.2f;
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    
    #endregion

    #region Accessors
    #endregion

    #region Unity Defaults
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(gameObject);
	}

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
