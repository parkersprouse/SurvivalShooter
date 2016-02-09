using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int health = 100;
        public int ammo = 40;
        public bool isAlive = true;
    }

    public PlayerStats stats = new PlayerStats();
    public Sprite[] bloodSpots;
    public GameObject blood;
    public Animator animator;
    public GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("_Manager").GetComponent<GameManager>();
        gameManager.player = this;
    }

    public void DamagePlayer(int dmg) {
        stats.health -= dmg;

        if (stats.health <= 0) {
            blood.GetComponent<SpriteRenderer>().sprite = bloodSpots[Random.Range(0, bloodSpots.Length)];
            GameObject b = Instantiate(blood, transform.position, transform.rotation) as GameObject;
            b.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.8f, 0.8f, 1);
            animator.SetTrigger("BloodFade");
            Destroy(b, 5f);
            gameManager.KillPlayer();
        }
    }

}
