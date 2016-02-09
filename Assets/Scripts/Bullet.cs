using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private int speed = 150;
    private Transform bulletTransform;

    void Start() {
        bulletTransform = transform;
    }

    void Update() {
        bulletTransform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    void OnBecameInvisible() {
        SimplePool.Despawn(gameObject);
    }

/*	private int speed = 75;
    private Rigidbody2D body;

    private BulletType bullet;
    private enum BulletType {
        BASIC = 0,
        ROCKET,
        GRENADELAUNCHER
    }

    void Start () {
        body = GetComponent<Rigidbody2D>();
        //body.velocity = new Vector2(0, speed);

        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y).normalized;
        Vector2 currPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 shootDir = mousePosition - currPosition;

        body.velocity = new Vector2(shootDir.x * speed, shootDir.y * speed);

        /*
        switch (Weapon.weapon) {
            case Weapon.WeaponType.BASIC:
                bullet = BulletType.BASIC;
                break;
            case Weapon.WeaponType.ROCKET:
                bullet = BulletType.ROCKET;
                break;
            case Weapon.WeaponType.GRENADELAUNCHER:
                bullet = BulletType.GRENADELAUNCHER;
                break;
        }
        
    }

    void OnBecameInvisible() {
        SimplePool.Despawn(gameObject);
    }

    void OnEnable() {
        Start();
    }
 */

}
