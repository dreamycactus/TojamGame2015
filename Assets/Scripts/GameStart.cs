using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        Application.targetFrameRate = 60;
        gameObject.AddComponent<Managers>();
        DontDestroyOnLoad(this);

        Managers.GetInstance().Init();
    }
}
