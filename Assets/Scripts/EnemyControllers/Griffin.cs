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

	public bool m_isBomber = false;

	void Start() {
		body = this.gameObject.GetComponent<Rigidbody2D>();
		state = GriffinState.Idle;
		GameObject[] players = new GameObject[2];
		players[0] = Managers.GetInstance().GetPlayerManager().GetPlayerOne();
		players[1] = Managers.GetInstance().GetPlayerManager().GetPlayerTwo();

		Physics2D.IgnoreCollision(players[0].GetComponent<Collider2D>(), GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(players[1].GetComponent<Collider2D>(), GetComponent<Collider2D>());

		bEmitter = gameObject.GetComponent<BulletEmitter>();

		if (Mathf.Abs(transform.position.x - players[0].transform.position.x) < Mathf.Abs(transform.position.x - players[1].transform.position.x))
		{
			targetPlayer = players[0];
			speed = -Math.Abs(speed);
			if (!m_isBomber) {
				bEmitter.Patterns[0] = "line:3:20";
				bEmitter.Refresh();
			} else {
				bEmitter.Patterns[0] = "line:1:-15";
				bEmitter.Refresh();
			}
		}
		else
		{
			targetPlayer = players[1];
			speed = Math.Abs(speed);
			if (!m_isBomber) {
				bEmitter.Patterns[0] = "line:3:-20";
				bEmitter.Refresh();
			} else {
				bEmitter.Patterns[0] = "line:1:15";
				bEmitter.Refresh();
			}
			FlipSprite();
		}

		idealHeight = transform.position.y;
		
	}

	void Update() {
		switch(state) {
			case GriffinState.Idle:
				if ((targetPlayer.transform.position - transform.position).magnitude < Constants.ENEMY_DETECTION_RADIUS) {
					state = GriffinState.Approach;
					body.velocity = new Vector2(speed, 0);
					bEmitter.ToggleAutoFire();
					//TODO Play griffin sound
					if (!m_isBomber) {
						bEmitter.Target = new Vector2(speed * 1000000, transform.position.y);
					} else {
						bEmitter.Target = new Vector2(transform.position.x, -1000);
					}
				}
				break;
			case GriffinState.Approach:
				elapsedTime += Time.deltaTime;
				//float x = transform.localPosition.x + speed;
				float y = sinKY * Mathf.Sin(elapsedTime * sinKX) + idealHeight;
				transform.position = new Vector3(transform.position.x, y, 0);

				if (m_isBomber) {
					bEmitter.Target = new Vector2(transform.position.x, -1000);
				}

				if (Mathf.Abs(transform.position.x - targetPlayer.transform.position.x) > m_disappearDist)
				{
					Destroy(gameObject);    //Destroys enemy if player gets too far
				}
				break;
			case GriffinState.Shoot:
				break;
			case GriffinState.Death:
				GameObject exp = ExplosionManager.Inst.GetExplosion();
				exp.transform.position = transform.position;
				Destroy(this.gameObject);
				break;
		}
	}

	private void EnterDeathState()
	{
		state = GriffinState.Death;
	}

	void OnCollisionEnter2D(Collision2D col) {
		
	}

	private void FlipSprite()
	{
		transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
