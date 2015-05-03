using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UI : MonoBehaviour {
	public float barDisplay= 50;
	Vector2 pos  = new Vector2(20,40);
	Vector2 size = new Vector2(60,20);
	Texture2D progressBarEmpty;
	Texture2D progressBarFull;
	void OnGUI() {

		// draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty);

		// draw the filled-in part:
		GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
		GUI.EndGroup();

		GUI.EndGroup();

	}
}

