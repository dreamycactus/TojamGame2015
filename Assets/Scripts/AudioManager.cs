/*
 * This class is for controling and playing audio
 */

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	#region Public Variables
    
    
    public AudioSource soundPlayer;
    public AudioSource musicPlayer;
    public AudioClip[] clips;
    public AudioClip[] musics;
    #endregion

    #region Protected Variables
    #endregion

    #region Private Variables
    private static AudioManager m_instance;
    #endregion

    #region Accessors
    public static AudioManager GetInstance()
    {
        if (m_instance == null)
            m_instance = Managers.GetInstance().gameObject.AddComponent<AudioManager>();

        return m_instance;
    }
    #endregion

    #region Unity Defaults
    void Awake()
    {
        soundPlayer = new AudioSource();
        musicPlayer = new AudioSource();
    }

    public void PlayMusic(int tracknum)
    {
        musicPlayer.clip = musics[tracknum];
        musicPlayer.Play();
    }


    public void PlayClip(int clipnum)
    {
        
        soundPlayer.clip = clips[clipnum];
        soundPlayer.Play();
    }
    #endregion
}
    