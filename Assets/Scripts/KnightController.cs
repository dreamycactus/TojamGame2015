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
		INAIR_ROCKET,
		HURT
	}
	const int WINDUP = 0,
		MOVE_DURATION = 2,
		WINDDOWN = 1,
		LASER_MIN = 2,
		LASER_MAX = 3,
		OFFSETX = 3,
		OFFSETY = 4,
		WIDTH = 4,
		HEIGHT = 5;
	enum Move {
		PUNCH,
		JUMP,
		LASER,
		ROCKET,
		NONE
	}

	// [WINDUP, ATTACK_LEN, WINDDOWN]
	readonly float[] JUMP			= { 0.2f, 0.1f };
	readonly float[] PUNCH			= { 0.01f, 0.5f, 0.5f, 0.5f, -0.2f };
	readonly float[] ROK_PUNCH		= { 0.2f, 0.2f, 0.3f, 0.5f, -0.4f, 0.5f, 0.5f };
	readonly float[] LASER			= { 0.2f, 0.2f, 1.0f, 2.0f };
	readonly float HURT				= 0.2f;

	bool isInAir = false;
	bool inputLocked = false;
	float moveTimer = 0;
	bool lockedOnPlayer = false;
	State state = State.FREE;
	bool isLeft = false;
	float laserEnd = 0;
	bool movedUsed = false;
	bool landed = true;
	Rigidbody2D body;
	float jumpPower;
	private GameObject m_Camera;
	private float m_size;
	private Vector3 m_camOffset;
	private float m_camDepth = -10;
	private Animator m_animator;
	float extentX;
	Move queuedMove;

	GameObject pHitbox;

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
		if (state == State.INAIR) {
			foreach (ContactPoint2D tact in col.contacts) {
				if (state == State.INAIR && tact.normal == Vector2.up && movedUsed) {
					m_animator.SetBool("Jumping", false);
					landed = true;
					moveTimer = 0;
				}
			}
		}
	}
	void OnCollisionStay2D(Collision2D coll) {
		
	}
	void Update() {
		//keep up with camera
		m_camOffset.x = transform.position.x;
		m_Camera.transform.position = m_camOffset;

		extentX = Input.GetAxis("L_XAxis_1");
		if (queuedMove == Move.NONE && state == State.FREE) {
			if (Input.GetButtonDown("A_1")) {
				state = State.INAIR;
				StartMove();
			} else if (Input.GetButtonDown("B_1")) {
				queuedMove = Move.ROCKET;
			} else if (Input.GetButtonDown("X_1")) {
				queuedMove = Move.PUNCH;
			}
		}
		if (state == State.INAIR && !movedUsed) {
			if (Input.GetButtonUp("A_1")) {
				queuedMove = Move.JUMP;
				jumpPower = 16;
			}
			if (moveTimer > JUMP[0]) {
				queuedMove = Move.JUMP;
				jumpPower = 20;
			}
		}
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
				switch(queuedMove) {
					case Move.JUMP:
						break;
					case Move.LASER:
						StartMove();
						state = State.LASERING;
						queuedMove = Move.NONE;
						break;
					case Move.PUNCH:
						StartMove();
						state = State.PUNCHING;
						queuedMove = Move.NONE;
						break;
					case Move.ROCKET:
						StartMove();
						state = State.ROCKET_PUNCHING;
						queuedMove = Move.NONE;
						break;
					default:
						break;
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
					pHitbox = Instantiate(Resources.Load("K_PUNCH_BOX", typeof(GameObject))) as GameObject;
					pHitbox.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
					var col = pHitbox.GetComponent<BoxCollider2D>();
					col.offset = new Vector2(-Math.Sign(transform.localScale.x) * PUNCH[OFFSETX], PUNCH[OFFSETY]);
					var hitbox = pHitbox.GetComponent<Hitbox>();
					hitbox.l_shootingPlayer = col;
					hitbox.ttl = -200;
					movedUsed = true;
					if (isLeft) {
						m_animator.SetTrigger("PunchLeft");
					} else {
						m_animator.SetTrigger("PunchRight");
					}
					isLeft = !isLeft;
				}
				if (moveTimer > PUNCH[MOVE_DURATION]) {
					Destroy(pHitbox);
				}
				if (moveTimer > PUNCH[WINDDOWN]) {
					state = State.FREE;
				}
				break;
			case State.ROCKET_PUNCHING:
				if (moveTimer > ROK_PUNCH[WINDUP] && !movedUsed) {
					pHitbox = Instantiate(Resources.Load("K_PUNCH_BOX", typeof(GameObject))) as GameObject;
					pHitbox.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
					var col = pHitbox.GetComponent<BoxCollider2D>();
					col.offset = new Vector2(-Math.Sign(transform.localScale.x) * ROK_PUNCH[OFFSETX], ROK_PUNCH[OFFSETY]);
					col.size = new Vector2(ROK_PUNCH[WIDTH], ROK_PUNCH[HEIGHT]);
					var hitbox = pHitbox.GetComponent<Hitbox>();
					hitbox.l_shootingPlayer = col;
					hitbox.ttl = 10;
					pHitbox.GetComponent<Rigidbody2D>().velocity = new Vector2(-Math.Sign(transform.localScale.x)*20, 0) + body.velocity;
					movedUsed = true;
				}
				if (moveTimer > ROK_PUNCH[MOVE_DURATION]) {

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
				body.drag = 0.1f;
				if (queuedMove == Move.JUMP) {
					if (Math.Sign(extentX) != -Math.Sign(transform.localScale.x) && jumpPower > 17) {
						body.AddForce(new Vector2(20 * jumpPower * extentX, 14 * jumpPower), ForceMode2D.Impulse);
					} else {
						body.AddForce(new Vector2(10 * jumpPower * extentX, 20 * jumpPower), ForceMode2D.Impulse);
					}
					landed = false;
					queuedMove = Move.NONE;
					movedUsed = true;
					m_animator.SetBool("Jumping", true);
				}
				if (body.velocity.magnitude < Constants.KNIGHT_MAX_SPEED && !landed && movedUsed) {
					body.AddForce(new Vector2(extentX * 15.0f, 0), ForceMode2D.Impulse);
				}
				if (landed && movedUsed && moveTimer > JUMP[WINDDOWN]) {
					state = State.FREE;
					body.drag = 3.0f;
				}
				break;
			case State.INAIR_ROCKET:
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
