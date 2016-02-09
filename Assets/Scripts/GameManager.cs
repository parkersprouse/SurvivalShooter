using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform playerTransform, spawnPoint;
    [HideInInspector] public Player player;

    private Text healthText, ammoText;

    void Start() {
        Instantiate(playerTransform, spawnPoint.position, spawnPoint.rotation);

        if (GameObject.Find("HealthText") != null)
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
        if (GameObject.Find("AmmoText") != null)
            ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevel("MainMenu");
        }

        if (healthText != null)
            healthText.text = "Health: " + player.stats.health;
        if (ammoText != null)
            ammoText.text = "Ammo: " + player.stats.ammo + " / 40";
    }

    public void KillPlayer() {
        player.stats.isAlive = false;
        Destroy(player.gameObject);
        RespawnPlayer();
    }

    public void RespawnPlayer() {
        if (!player.stats.isAlive && spawnPoint != null) {
            player.stats.isAlive = true;
            Instantiate(playerTransform, spawnPoint.position, spawnPoint.rotation);
        }
    }

}
