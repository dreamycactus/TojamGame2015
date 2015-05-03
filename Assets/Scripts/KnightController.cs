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
		MOVE_DURATION = 2,
		WINDDOWN = 1,
		LASER_MIN = 2,
		LASER_MAX = 3;


	// [WINDUP, ATTACK_LEN, WINDDOWN]
	readonly float[] JUMP			= { 0.1f, 0.2f };
	readonly float[] PUNCH			= { 0.5f, 0.5f, 0.5f };
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
	private GameObject m_Camera;
	private float m_size;
	private Vector3 m_camOffset;
	private float m_camDepth = -10;
	private Animator m_animator;
	float extentX;

	void Start() {
		body = gameObject.GetComponent<Rigidbody2D>();
		m_animator = gameObject.GetComponent<Animator>();
		m_camOffset = new Vector3(0, 1.5f, m_camDepth);
		body.fixedAngle = true;
		m_size = 0.55f;
	}

	public GameObject PlayerCamera {
		set { m_Camera = value; }
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Floor" && state == State.INAIR) {
			state = State.FREE;
			body.drag = 3.0f;
		}
	}
	void Update() {
		//keep up with camera
		m_camOffset.x = transform.position.x;
		m_Camera.transform.position = m_camOffset;

		extentX = Input.GetAxis("L_XAxis_1");
	}
	void FixedUpdate() {
		switch(state) {
			case State.FREE:
				body.AddForce(new Vector2(extentX*1000, 0));
				m_animator.SetFloat("RunSpeed", Mathf.Abs(extentX));
				if (extentX > 0.5f && transform.localScale.x > 0) {
					FlipSprite();
				} else if (extentX < -0.5f && transform.localScale.x < 0) {
					FlipSprite();
				}
				if (Input.GetButtonDown("A_1")) {
					StartMove();
					state = State.INAIR;
					body.drag = 0.1f;
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
					if (isLeft) {
						m_animator.SetTrigger("PunchLeft");
					} else {
						m_animator.SetTrigger("PunchRight");
					}
					isLeft = !isLeft;
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
				extentX = Input.GetAxis("L_XAxis_1");
				if (body.velocity.magnitude < Constants.KNIGHT_MAX_SPEED) {
					body.AddForce(new Vector2(extentX * 20.0f, 0), ForceMode2D.Impulse);
				}
				if (Input.GetButtonUp("A_1") && moveTimer < JUMP[WINDUP] && !movedUsed) {
					jumpPower = 10;
					body.AddForce(new Vector2(0, 20 * jumpPower), ForceMode2D.Impulse);
					movedUsed = true;
					m_animator.SetBool("Jumping", true);
				}
				if (moveTimer > JUMP[WINDUP] && !movedUsed) {
					jumpPower = 15;
					body.AddForce(new Vector2(0, 20 * jumpPower), ForceMode2D.Impulse);
					movedUsed = true;
					m_animator.SetBool("Jumping", true);
				}
				break;
			case State.INAIR_LASERING:
				break;
		}
		moveTimer += Time.deltaTime;
	}

	void StartMove() {
		moveTimer = 0;
		movedUsed = false;
		laserEnd = 0;
		jumpPower = 0;
	}

	private void FlipSprite() {
		transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
