using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	private float m_lifeTimer = 0;
	public float m_timeToLive = 3;

	// Use this for initialization
	void Start () {
		m_lifeTimer = m_timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
		m_lifeTimer -= Time.deltaTime;

		if (m_lifeTimer <= 0)
		{
			Destroy(gameObject);
		}
	}
}
