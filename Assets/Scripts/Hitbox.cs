using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Hitbox : MonoBehaviour {
	public Collider2D l_shootingPlayer;
	public int m_dmg = 1;
	public float ttl = -200;
	public void Init() {
		Physics2D.IgnoreCollision(l_shootingPlayer, GetComponent<Collider2D>());
	}
	void Update() {
		ttl -= Time.deltaTime;
		if (ttl < -100) {
			return;
		}
		if (ttl < 0) {
			Destroy(gameObject);
		}
	}
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other != l_shootingPlayer && (other.tag == "Enemy" || other.tag == "Player")) {
			other.GetComponent<Health>().TakeDamage(m_dmg, gameObject.GetComponent<Collider2D>());
			Destroy(gameObject);
		}

	}
	    
}
