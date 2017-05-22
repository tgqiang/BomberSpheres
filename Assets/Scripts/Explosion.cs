using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public static float DEFAULT_SCALE = 3f;
	public static float MAX_SCALE = 8f;
	public static float DIFFERENCE_SCALE = MAX_SCALE - DEFAULT_SCALE;

	public float ATTACK_DURATION;

	SpriteRenderer explosionRenderer;

	int ownerPlayer;
	public bool toTrigger;
	public bool isActive;
	float triggerDuration;

	// Use this for initialization
	void Start () {
		ownerPlayer = GetComponentInParent<Player> ().playerNum;
		explosionRenderer = GetComponent<SpriteRenderer> ();
		toTrigger = false;
		isActive = false;
		triggerDuration = 0;
	}

	void Update () {
		explosionRenderer.enabled = toTrigger;

		if (toTrigger) {	// if explosion attack is triggered

			triggerDuration += Time.deltaTime;

			if (triggerDuration >= ATTACK_DURATION) {	// if attack duration is over
				toTrigger = false;
				explosionRenderer.enabled = toTrigger;
				triggerDuration = 0;
				gameObject.transform.localScale = new Vector3 (DEFAULT_SCALE, DEFAULT_SCALE, 1);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		//print (other.gameObject);

		Player affectedPlayer = other.gameObject.GetComponent<Player> ();
		if (toTrigger && isActive && affectedPlayer != null) {
			if (affectedPlayer.playerNum != ownerPlayer) {
				isActive = false;
				affectedPlayer.Die ();
			}
		}
	}

	public void TriggerExplosion(float chargeVal, bool voided=false) {
		if (voided) {
			toTrigger = true;
			gameObject.transform.localScale = new Vector3 (0, 0, 1);
		} else {
			toTrigger = true;
			isActive = true;
			float totalScaling = DEFAULT_SCALE + chargeVal;
			gameObject.transform.localScale = new Vector3 (totalScaling, totalScaling, 1);
		}
	}
}
