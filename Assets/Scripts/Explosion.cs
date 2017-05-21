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

	void OnTriggerEnter2D(Collider2D other) {
		Player affectedPlayer = other.gameObject.GetComponent<Player> ();
		if (affectedPlayer != null && toTrigger) {
			if (affectedPlayer.playerNum != ownerPlayer) {
				affectedPlayer.Die ();
			}
		}
	}

	public void TriggerExplosion(float chargeVal) {
		float totalScaling = DEFAULT_SCALE + chargeVal;
		gameObject.transform.localScale = new Vector3 (totalScaling, totalScaling, 1);
		toTrigger = true;
	}
}
