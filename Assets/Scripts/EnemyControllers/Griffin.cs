using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Griffin : MonoBehaviour {
	public enum GriffinState {
		Idle = 0,
		Approach = 1,
		Shoot = 2,
		Death = 3,
	}
	Rigidbody2D body;
	GriffinState state;
	int playerIndex;
	GameObject targetPlayer;
	float idealHeight;
	public float sinKY= 0.1f;
	public float sinKX = 0.05f;
	public float elapsedTime;
	public float speed = 0.1f;
	private BulletEmitter bEmitter;

	void Start() {
		body = this.gameObject.GetComponent<Rigidbody2D>();
		state = GriffinState.Idle;
		var mid = GameObject.FindGameObjectWithTag("mid");
		var players = GameObject.FindGameObjectsWithTag("Player");

		if (transform.position.x < mid.transform.position.x) {
			playerIndex = 0;
			speed = -Math.Abs(speed);
		} else {
			playerIndex = 1;
			speed = Math.Abs(speed);
		}
		targetPlayer = players[playerIndex];
		idealHeight = transform.position.y;
		bEmitter = gameObject.GetComponent<BulletEmitter>();
	}

	void Update() {
		switch(state) {
			case GriffinState.Idle:
				if ((targetPlayer.transform.position - transform.position).magnitude < Constants.ENEMY_DETECTION_RADIUS) {
					state = GriffinState.Approach;
					body.velocity = new Vector2(speed, 0);
					bEmitter.ToggleAutoFire();
					//TODO Play griffin sound
					bEmitter.Target = new Vector2(speed * 1000000, transform.position.y);

				}
				break;
			case GriffinState.Approach:
				elapsedTime += Time.deltaTime;
				//float x = transform.localPosition.x + speed;
				float y = sinKY * Mathf.Sin(elapsedTime * sinKX) + idealHeight;
				transform.position = new Vector3(transform.position.x, y, 0);
				break;
			case GriffinState.Shoot:
				break;
			case GriffinState.Death:
				GameObject exp = Instantiate(Resources.Load("Explosion")) as GameObject;
				exp.transform.position = transform.position;
				break;
		}
	}
	void OnCollisionEnter(Collision col) {
		state = GriffinState.Death;
	}
}
