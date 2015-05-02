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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BulletEmitter : MonoBehaviour
{
	List<BulletPattern> bPatterns;
	public List<String> Patterns;
    public bool m_autoFire = false;

	public Vector2 Target;
	public BulletEmitter ()
	{
	}

	public void Awake() {
		Refresh();
	}

	public void Refresh() {
		bPatterns = new List<BulletPattern>();
		Debug.Log("hello" + Patterns.Count);
		foreach (var p in Patterns) {
			switch(p) {
				case "line":
					bPatterns.Add(new BulletLinePattern(Constants.NORMAL_BULLET_PERIOD, Constants.NORMAL_BULLET_SPEED));
					break;
				case "sine":
					bPatterns.Add(new BulletLinePattern(Constants.NORMAL_BULLET_PERIOD, Constants.NORMAL_BULLET_SPEED));
					break;
				default:
					break;
			}
		}
	}

	public void Update() {
		if (m_autoFire)
        {
            foreach (var p in bPatterns)
            {
                p.Step(this.gameObject.transform.position.GetVector2(), Target);
            }
        }
	}

    public void ToggleAutoFire()
    {
        m_autoFire = !m_autoFire;
    }
}

