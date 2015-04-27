/*
 * Managers singleton, everything should more or less be accesible from here
 */

using UnityEngine;
using System.Collections;

public class Managers : MonoBehaviour
{

    #region Public Variables
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    private static Managers m_instance = null;
    private GameProperties m_gameProperties;
    private GameStateManager m_gameStateManager;
    private AudioManager m_audioManager;
    private InputHandler m_inputHandler;
    private UIManager m_uiManager;
    private PlayerManager m_playerManager;

    #endregion

    #region Accessors
    public static Managers GetInstance()
    {
        return m_instance;
    }

    public GameProperties GetGameProperties()
    {
        return m_gameProperties;
    }

    public GameStateManager GetGameStateManager()
    {
        return m_gameStateManager;
    }

    public AudioManager GetAudioManager()
    {
        return m_audioManager;
    }

    public InputHandler GetInputHandler()
    {
        return m_inputHandler;
    }

    public UIManager GetUIManager()
    {
        return m_uiManager;
    }
    
    public PlayerManager GetPlayerManager()
    {
        return m_playerManager;
    }
    #endregion

    #region Unity Defaults
    public void Awake()
    {
        m_instance = this;
    }
    #endregion

    #region Public Methods
    public void Init()
    {
        m_gameProperties = GetComponent<GameProperties>();

        m_gameStateManager = gameObject.AddComponent<GameStateManager>();
        m_gameStateManager.Init();
        m_audioManager = gameObject.AddComponent<AudioManager>();
        m_inputHandler = gameObject.AddComponent<InputHandler>();
        m_uiManager = gameObject.AddComponent<UIManager>();
        m_playerManager = gameObject.AddComponent <PlayerManager>();

    }
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
      
}
