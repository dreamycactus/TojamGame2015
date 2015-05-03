/*
 * This class is responsible for the management of both
 * UI and HUD elements
 */
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	#region Public Variables
	#endregion

	#region Protected Variables
	#endregion

	#region Private Variables
	#endregion

	#region Accessors
	#endregion

	#region Unity Defaults

	GameObject p1;
	GameObject p2;
	GameObject[] uis;
	// Use this for initialization
	void Start () {
		p1 = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
		p2 = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();
		uis = GameObject.FindGameObjectsWithTag("UI");
	}
	
	// Update is called once per frame
	void Update () {
		uis[0].GetComponent<UnityEngine.UI.Text>().text = "Health: " + p1.GetComponent<Health>().currentHealth;
		uis[1].GetComponent<UnityEngine.UI.Text>().text = "Health: " + p2.GetComponent<Health>().currentHealth;
	}

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
