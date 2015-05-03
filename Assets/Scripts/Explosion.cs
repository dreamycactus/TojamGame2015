using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Explosion : UnityEngine.MonoBehaviour {
	public float m_currentLife;
	public Explosion() {
		m_currentLife = 5.0f;
	}

	public void Free() {
		m_currentLife = 0;
		if (this.gameObject != null) {
			ExplosionManager.Inst.FreeExplosion(this.gameObject);
		}
	}

	public void Update() {
		if (m_currentLife > 0) {
			m_currentLife -= Time.deltaTime;
			if (m_currentLife < 0) {
				Free();
			}
		}
	}
}
