using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
		
	void OnTriggerEnter2D(Collider2D other) {
		Player affectedPlayer = other.gameObject.GetComponent<Player> ();
		if (affectedPlayer != null) {
			affectedPlayer.hasItem = true;
			Destroy (this.gameObject);
		}
	}

}
