using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class BulletPattern {
	public virtual void Step(Vector2 Spawn, Vector2 target) { }
}
