using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
class BulletLinePattern : BulletPattern {
	public float Period;
	private float m_cooldown;
	public float Speed;
	public float AngleOffset = 0.0f;
	public BulletLinePattern(float period, float speed) {
		Period = period;
		Speed = speed;
	}
	override public void Step(Vector2 spawn, Vector2 target) {
		m_cooldown -= Time.deltaTime;
		if (m_cooldown < 0.0f) {
			m_cooldown = Period;
			GameObject bullet = BulletManager.Inst.GetBullet();
			bullet.transform.position = new Vector3(spawn.x, spawn.y, 0.0f);
			bullet.GetComponent<Rigidbody2D>().velocity = Vector2.MoveTowards(spawn, target, Speed).Rotated(AngleOffset);
			bullet.GetComponent<Bullet>().m_currentLife = 5.0f;
		}
	}
}
