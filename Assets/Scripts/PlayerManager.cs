/*
 * This class is responsible for spawning both players
 * and handling their access for ... things
 */
using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	#region Public Variables
    private GameObject[] m_playerCameras;
    private GameObject[] m_playerCharacters;
    private MonoBehaviour[] m_playerControllers;
        
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    #endregion

    #region Accessors
    public GameObject GetPlayerOne()
    {
        return m_playerCharacters[0];
    }
    public GameObject GetPlayerTwo()
    {
        return m_playerCharacters[1];
    }
    public GameObject GetLeftCamera()
    {
        return m_playerCameras[0];
    }
    public GameObject GetRightCamera()
    {
        return m_playerCameras[1];
    }
    #endregion

    #region Unity Defaults
    void Awake()
    {
        m_playerCameras = new GameObject[2];
        m_playerCharacters = new GameObject[2];
        m_playerControllers = new MonoBehaviour[2];
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #endregion

    #region Public Methods
    public void Init()
    {
        SpawnCameras();
        SpawnPlayers();

    }
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    //spawn cameras + initialization
    private void SpawnCameras()
    {
        GameObject Cameraroot = GameObject.Instantiate(Managers.GetInstance().GetGameProperties().Cameras_prefab);
        m_playerCameras[0] = Cameraroot.transform.GetChild(0).gameObject;
        m_playerCameras[1] = Cameraroot.transform.GetChild(1).gameObject;
    }

    //spawn playerSprites + initialization
    private void SpawnPlayers()
    {
        m_playerCharacters[0] = GameObject.Instantiate(Managers.GetInstance().GetGameProperties().Player1_prefab);
		m_playerCharacters[0].transform.position = new Vector2(-20, 0);
        m_playerCharacters[0].GetComponent<PlayerController>().PlayerCamera = m_playerCameras[0];
        m_playerControllers[0] = m_playerCharacters[0].GetComponent<PlayerController>();

        m_playerCharacters[1] = GameObject.Instantiate(Managers.GetInstance().GetGameProperties().Knight_prefab);
		m_playerCharacters[1].transform.position = new Vector2(20, 0);
        m_playerCharacters[1].GetComponent<KnightController>().PlayerCamera = m_playerCameras[1];
        m_playerControllers[1] = m_playerCharacters[1].GetComponent<KnightController>();
        //m_playerControllers[1].PlayerCamera = m_playerCameras[1];

	}
    #endregion
}
