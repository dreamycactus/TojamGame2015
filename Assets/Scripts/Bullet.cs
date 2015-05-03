//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float m_currentLife;
	public int m_dmg = 1;
	public bool m_explodeOnImpact = false;
	public Bullet ()
	{
	}
		
	public void Free() {
        m_currentLife = 0;
        
        if (this.gameObject != null) {
			BulletManager.Inst.FreeBullet(this.gameObject);
		}
	}

	public void Update() {
		if (m_currentLife > 0)
        {
            m_currentLife -= Time.deltaTime;
		    if (m_currentLife < 0) {
			    Free();
		    }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(m_dmg, gameObject.GetComponent<Collider2D>());
            Free();
			if (m_explodeOnImpact) {
				GameObject exp = ExplosionManager.Inst.GetExplosion();
				exp.transform.position = transform.position;
			}
            Debug.Log("Player Hit");
        }
        else if (other.tag != "Enemy")
        {
			if (m_explodeOnImpact) {
				GameObject exp = ExplosionManager.Inst.GetExplosion();
				exp.transform.position = transform.position;
			}
            Free();
        }
        
    }
}

