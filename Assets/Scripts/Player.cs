using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Player : MonoBehaviour {

	public float START_SPEED;
	public float MIN_SPEED;
	public float speed;
	float speedDeductionInterval;

	float cooldown;

	float initX;
	float initY;

	public int playerNum;

	public bool hasItem;
	public float ITEM_HOLD_TARGET;
	float itemHeldDuration;

	public int deathCount;
	public int itemCount;

	public GameObject playerExplosionObject;
	float explosionCharge;
	float chargeLimit;
	bool voided;
	Explosion explosion;
	Energy playerChargeEnergy;

	Rigidbody2D rb2d;

	GameManager gameManager;

	// Use this for initialization
	void Start () {
		initX = transform.position.x;
		initY = transform.position.y;
		itemHeldDuration = 0;
		explosionCharge = 0;
		cooldown = 0;
		voided = false;
		speedDeductionInterval = (speed - MIN_SPEED) / 50f;		// 50 is magic number
		explosion = playerExplosionObject.GetComponent<Explosion> ();
		rb2d = GetComponent<Rigidbody2D> ();
		playerChargeEnergy = GetComponent<Energy> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//print ("Player " + playerNum + " attack: " + voided);

		float deltaTime = Time.deltaTime;

		if (cooldown > 0) {
			cooldown -= deltaTime;
		} else {
			cooldown = 0;
		}

		UpdateControllerForPlayer ();

		if (hasItem) {
			itemHeldDuration += deltaTime;

			if (itemHeldDuration >= ITEM_HOLD_TARGET) {
				itemCount += 1;

				hasItem = false;
				itemHeldDuration = 0;

				// TODO: inform game-manager to spawn new item on map
				gameManager.UpdatePlayerItem(playerNum);
				gameManager.SpawnNewItem ();
			}
		}
	}

	void UpdateControllerForPlayer() {
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null) {
			// do nothing
		} else {
			UpdateMovementWithInputDevice (inputDevice);
			TriggerExplosionWithInputDevice (inputDevice);
		}
	}

	void UpdateMovementWithInputDevice(InputDevice inputDevice) {
		if (cooldown <= 0) {
			float x = inputDevice.LeftStickX.Value;
			float y = inputDevice.LeftStickY.Value;

			Vector2 movement = new Vector2 (x, y).normalized;

			rb2d.MovePosition (new Vector2 (transform.position.x + movement.x * speed, transform.position.y + movement.y * speed));
		}
	}

	void TriggerExplosionWithInputDevice(InputDevice inputDevice) {
		if (inputDevice.RightTrigger.IsPressed && !voided && cooldown <= 0) {
			//print ("Charging.");
			if (explosionCharge < Explosion.DIFFERENCE_SCALE) {
				explosionCharge += 0.1f;
				playerChargeEnergy.Modify (0.1f);
				speed -= speedDeductionInterval;
			} else {
				//print ("Charging maxed.");
				explosionCharge = Explosion.DIFFERENCE_SCALE;
				chargeLimit += Time.deltaTime;
				speed = MIN_SPEED;

				if (chargeLimit >= 2.0f) {
					//print ("Overcharging.");
					voided = true;
					explosionCharge = 0;
					playerChargeEnergy.Set (0);
					chargeLimit = 0;
					cooldown = explosion.ATTACK_DURATION;
				}
			}
		}

		if (inputDevice.RightTrigger.IsPressed && voided) {
			//print ("Explosion charge voided.");
			speed = START_SPEED;
		}

		if (inputDevice.RightTrigger.WasReleased) {
			//print ("Right trigger released.");
			if (voided) {
				//print ("Charging reset.");
				voided = false;
				speed = START_SPEED;
				cooldown = explosion.ATTACK_DURATION;
			} else if (cooldown <= 0) {
				//print ("Releasing explosion.");
				explosion.TriggerExplosion (explosionCharge);
				explosionCharge = 0;
				playerChargeEnergy.Set (0);
				speed = START_SPEED;
				cooldown = explosion.ATTACK_DURATION;
			}
		}
	}

	public void Die() {
		deathCount += 1;
		gameManager.UpdatePlayerDeath (playerNum);
		if (hasItem) {
			hasItem = false;
			gameManager.NotifyItemDropped (transform.position.x, transform.position.y);
		}
		transform.position = new Vector3 (initX, initY, 0);
	}
}
