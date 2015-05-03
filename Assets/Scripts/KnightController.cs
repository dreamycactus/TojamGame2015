using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class KnightController : MonoBehaviour {
	enum State {
		PUNCHING,
		FREE,
		INAIR,
		ROCKET_PUNCHING,
		LASERING,
		INAIR_LASERING,
		HURT
	}
	const int WINDUP = 0,
		MOVE_DURATION = 3,
		WINDDOWN = 1,
		LASER_MIN = 2,
		LASER_MAX = 3;

	// [WINDUP, ATTACK_LEN, WINDDOWN]
	readonly float[] JUMP			= { 0.1f, 0.2f };
	readonly float[] PUNCH			= { 0.05f, 0.05f, 0.3f };
	readonly float[] ROK_PUNCH		= { 0.05f, 0.05f, 0.3f };
	readonly float[] LASER			= { 0.2f, 0.2f, 1.0f, 2.0f };
	readonly float HURT				= 0.2f;

	bool isInAir = false;
	bool inputLocked = false;
	float moveTimer = 0;
	bool lockedOnPlayer = false;
	State state = State.FREE;
	String bufferedAction;
	bool isLeft = false;
	float laserEnd = 0;
	bool movedUsed = false;
	Rigidbody2D body;
	float jumpPower;

	void Start() {
		body = gameObject.GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Floor" && state == State.INAIR) {
			state = State.FREE;
		}
	}
	void Update() {
	}	
	void FixedUpdate() {
		switch(state) {
			case State.FREE:
				var extent = Input.GetAxis("L_XAxis_1");
					body.AddForce(new Vector2(extent*1000, 0));
				if (Input.GetButtonDown("A_1")) {
					StartMove();
					state = State.INAIR;
				} else if (Input.GetButtonDown("B_1")) {
					StartMove();
					state = State.LASERING;
				} else if (Input.GetButtonDown("X_1")) {
					StartMove();
					state = State.PUNCHING;
				}
				break;
			case State.HURT:
				if (moveTimer > HURT) {
					state = State.FREE;
				}
				break;
			case State.PUNCHING:
				if (moveTimer > PUNCH[WINDUP] && !movedUsed) {
					// Spawn punch
					movedUsed = true;
				}
				if (moveTimer > PUNCH[MOVE_DURATION]) {
					// Destroy punch
				}
				if (moveTimer > PUNCH[WINDDOWN]) {
					state = State.FREE;
				}
				break;
			case State.ROCKET_PUNCHING:
				if (moveTimer > ROK_PUNCH[WINDUP] && !movedUsed) {
					// Spawn punch
					movedUsed = true;
				}
				if (moveTimer > ROK_PUNCH[MOVE_DURATION]) {
					// Destroy punch
				}
				if (moveTimer > ROK_PUNCH[WINDDOWN]) {
					state = State.FREE;
				}
				break;
			case State.LASERING:
				if (moveTimer > LASER[WINDUP] && !movedUsed) {
					// Spawn punch
					movedUsed = true;
				}
				if (moveTimer > LASER[LASER_MIN] && !Input.GetButton("B_1")) {
					// Destroy punch
					state = State.FREE;
				}
				if (moveTimer > LASER[LASER_MAX]) {
					// Destroy punch
					state = State.FREE;
				}

				break;
			case State.INAIR:
				if (Input.GetButtonUp("A_1") && moveTimer < JUMP[WINDUP]) {
					jumpPower = moveTimer / JUMP[WINDUP];
				}
				if (moveTimer > JUMP[WINDUP] && !movedUsed) {
					jumpPower = 1;
					body.AddForce(new Vector2(0, 200 * jumpPower), ForceMode2D.Impulse);
					movedUsed = true;
				}
				break;
			case State.INAIR_LASERING:
				break;
		}
		moveTimer += Time.deltaTime;
	}

	void StartMove() {
		moveTimer = 0;
		isLeft = false;
		movedUsed = false;
		laserEnd = 0;
		jumpPower = 0;
	}
}
