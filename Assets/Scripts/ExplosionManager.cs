using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ExplosionManager {
	private List<GameObject> m_BulletPool = new List<GameObject>();
	private static ExplosionManager inst = null;

	public static ExplosionManager Inst {
		get {
			if (inst == null) {
				inst = new ExplosionManager();
			}
			return inst;
		}
	}

	private ExplosionManager() {
	}

	public GameObject GetExplosion() {
		if (m_BulletPool.Count == 0) {
			for (int i = 0; i < 10; ++i) {
				GameObject objj = UnityEngine.Object.Instantiate(Resources.Load("Explosion")) as GameObject;
				objj.SetActive(false);
				m_BulletPool.Add(objj);
			}
		}
		GameObject obj2 = m_BulletPool.First();
		obj2.SetActive(true);
		m_BulletPool.RemoveAt(0);
		return obj2;
	}

	public void FreeExplosion(GameObject bullet) {
		bullet.SetActive(false);
		m_BulletPool.Add(bullet);
	}
}
