using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pointsWorth = 10;
    public GameObject bulletPrefab;
    public float shootChancePerSecond = 0.2f;

    public delegate void EnemyDiedFunc(float points);
    public static event EnemyDiedFunc onEnemyDied;

    private int bulletLayer;
    private static Bullet activeEnemyBullet;

    void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }

    void Update()
    {
        TryShoot();
    }

    void TryShoot()
    {
        if (activeEnemyBullet != null) return;

        if (Random.value < shootChancePerSecond * Time.deltaTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        GameObject shotObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bullet = shotObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.direction = Vector2.down;
            activeEnemyBullet = bullet;
            bullet.onDestroyed += OnEnemyBulletDestroyed;
        }

        Destroy(shotObj, 5f);
    }

    private void OnEnemyBulletDestroyed(Bullet b)
    {
        if (activeEnemyBullet == b)
            activeEnemyBullet = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == bulletLayer)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            onEnemyDied?.Invoke(pointsWorth);
        }
    }
}