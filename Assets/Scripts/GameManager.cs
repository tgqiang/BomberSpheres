using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float GAME_DURATION;
	float timeElapsed;

	public Text gameOver;

	int p1DeathCount;
	int p1ItemCount;
	public Text p1Death;
	public Text p1Item;

	int p2DeathCount;
	int p2ItemCount;
	public Text p2Death;
	public Text p2Item;

	public GameObject item;

	// Use this for initialization
	void Start () {
		SpawnNewItem ();
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= GAME_DURATION) {
			gameOver.enabled = true;
			Time.timeScale = 0;
		} else {
			p1Death.text = "Deaths: " + p1DeathCount;
			p1Item.text = "Items Taken: " + p1ItemCount;

			p2Death.text = "Deaths: " + p2DeathCount;
			p2Item.text = "Items Taken: " + p2ItemCount;
		}
	}

	public void UpdatePlayerDeath(int playerNum) {
		if (playerNum == 0) {
			p1DeathCount += 1;
		} else if (playerNum == 1) {
			p2DeathCount += 1;
		}
	}

	public void UpdatePlayerItem(int playerNum) {
		if (playerNum == 0) {
			p1ItemCount += 1;
		} else if (playerNum == 1) {
			p2ItemCount += 1;
		}
	}

	public void NotifyItemDropped(float x, float y) {
		GameObject collectible = Instantiate (item);
		collectible.transform.position = new Vector3 (x, y, 0);
		collectible.SetActive (true);
	}

	public void SpawnNewItem() {
		float x = Random.Range (-45, 45) / 10f;
		float y = Random.Range (-45, 45) / 10f;

		GameObject collectible = Instantiate (item);
		collectible.transform.position = new Vector3 (x, y, 0);
		collectible.SetActive (true);
	}
}
