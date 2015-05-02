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
	GameObject targetPlayer;
	float idealHeight;
	public float sinKY= 0.1f;
	public float sinKX = 0.05f;
	public float elapsedTime;
	public float speed = 0.1f;
	private BulletEmitter bEmitter;
    public float m_disappearDist = 20.0f;

	void Start() {
		body = this.gameObject.GetComponent<Rigidbody2D>();
		state = GriffinState.Idle;
		GameObject[] players = new GameObject[2];
		players[0] = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
		players[1] = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();


		if (Mathf.Abs(transform.position.x - players[0].transform.position.x) < Mathf.Abs(transform.position.x - players[1].transform.position.x))
		{
			targetPlayer = players[0];
			speed = -Math.Abs(speed);
		}
		else
		{
			targetPlayer = players[1];
			speed = Math.Abs(speed);
		}

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

				if (Mathf.Abs(transform.position.x - targetPlayer.transform.position.x) > m_disappearDist)
				{
					Destroy(gameObject);    //Destroys enemy if player gets too far
				}
				break;
			case GriffinState.Shoot:
				break;
			case GriffinState.Death:
				GameObject exp = Instantiate(Resources.Load("Explosion")) as GameObject;
				exp.transform.position = transform.position;
				Destroy(this.gameObject);
				break;
		}
	}
	void OnCollisionEnter2D(Collision2D col) {
		state = GriffinState.Death;
	}
}
