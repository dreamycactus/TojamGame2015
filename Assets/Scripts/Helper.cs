using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
public static class Vector2Extension {
	public static Vector2 Rotated(this Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
public static class Vector3Extension {
	public static Vector2 GetVector2(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}
}