﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
class BulletLinePattern : BulletPattern {
	public float Period;
	private float m_cooldown;
	private float smallCooldown;
	public int burstCount;
	private int burstIndex = 0;
	public float Speed;
	public float AngleOffset = 0.0f;
	public BulletLinePattern(BulletEmitter em, int burstCount, float angle, float period, float speed) {
		Period = period;
		Speed = speed;
		this.burstCount = burstCount;
		AngleOffset = angle;
	}
	override public void Step(Vector2 spawn, Vector2 target) {
		if (burstIndex >= burstCount) {
			m_cooldown -= Time.deltaTime;
		} else {
			if (smallCooldown < 0.0f) {
				smallCooldown = Constants.NORMAL_BULLET_SMALL_PERIOD;
				burstIndex++;
				GameObject bullet = BulletManager.Inst.GetBullet();
				bullet.transform.position = new Vector3(spawn.x, spawn.y, 0.0f);
				bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(AngleOffset, Vector3.forward) * (Vector2)(Vector3.Normalize(target - spawn) * Speed);
				bullet.GetComponent<Bullet>().m_currentLife = 5.0f;
			}
			smallCooldown -= Time.deltaTime;
		}
		if (m_cooldown < 0) {
			m_cooldown = Period;
			burstIndex = 0;
		}
	}
}
