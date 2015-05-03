using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour {
	private bool m_isDetroyed;
	public float m_fallSpeed = 1.5f;
	public float m_shakeFactor = 1.0f;
	public float m_liveTime = 2.0f;
	private float m_liveTimer = 0.0f;
	private float m_explodeTimer = 0.0f;
	public float m_explodeFrequency = 0.5f;
	private ExplosionManager explosionManager;
	private Collider2D collider;

	// Use this for initialization
	void Start () {
		explosionManager = ExplosionManager.Inst;
		collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (m_isDetroyed) {
			float fally = transform.position.y - Time.deltaTime * m_fallSpeed;
			float fallx = transform.position.x + Time.deltaTime * Random.Range(-m_shakeFactor, m_shakeFactor);
			transform.position = new Vector3(fallx, fally, 1f);

			m_liveTimer -= Time.deltaTime;
			m_explodeTimer -= Time.deltaTime;

			if (m_explodeTimer <= 0) {
				GameObject exp = explosionManager.GetExplosion();
				float expx = transform.position.x + transform.localScale.x * Random.Range(-0.5f, 0.5f);
				float expy = transform.position.y + transform.localScale.y * Random.Range(-0.5f, 0.5f);
				exp.transform.position = new Vector2(expx, expy);
				m_explodeTimer = m_explodeFrequency;
			}

			if (m_liveTimer <= 0) {
				Destroy(gameObject);
			}
		}
	}

	private void EnterDeathState() {
		m_isDetroyed = true;
		collider.enabled = false;
		m_liveTimer = m_liveTime;
	}
}
