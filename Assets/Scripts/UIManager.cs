/*
 * This class is responsible for the management of both
 * UI and HUD elements
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


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
	Image DragonHPBar;
	Image KnightHPBar;

	Health DHealth;
	Health KHealth;
	// Use this for initialization
	void Start () {
		p1 = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
		p2 = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();
		//uis = GameObject.FindGameObjectsWithTag("UI");
		DragonHPBar = GameObject.FindGameObjectWithTag("DHealthBar").GetComponent<Image>();
		KnightHPBar = GameObject.FindGameObjectWithTag("KHealthBar").GetComponent<Image>();
		DHealth = p1.GetComponent<Health>();
		KHealth = p2.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
		DragonHPBar.fillAmount = (float)DHealth.currentHealth / (float)DHealth.maxHealth;
		KnightHPBar.fillAmount = (float)KHealth.currentHealth / (float)KHealth.maxHealth;
	}

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
