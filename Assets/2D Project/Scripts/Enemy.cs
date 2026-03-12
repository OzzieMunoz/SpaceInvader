using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pointsWorth = 10;
    public GameObject bulletPrefab;
    public float shootChancePerSecond = 0.2f;

    public float shootStepUpDistance = 0.08f;
    public float shootStepUpDuration = 0.06f;

    public static event Action<int> onEnemyDied;

    public static Bullet activeEnemyBullet;

    private int bulletLayer;
    private Animator anim;
    private bool dying = false;

    private Coroutine shootStepRoutine;

    void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (dying) return;

        TryShoot();
    }

    void TryShoot()
    {
        if (activeEnemyBullet != null) return;

        if (UnityEngine.Random.value < shootChancePerSecond * Time.deltaTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        if (shootStepRoutine != null)
            StopCoroutine(shootStepRoutine);

        shootStepRoutine = StartCoroutine(ShootStepRoutine());

        GameObject shotObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bullet = shotObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.owner = Bullet.Owner.Enemy;
            bullet.direction = Vector2.down;

            activeEnemyBullet = bullet;
            bullet.onDestroyed += OnEnemyBulletDestroyed;
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyShoot);

        Destroy(shotObj, 5f);
    }

    private IEnumerator ShootStepRoutine()
    {
        Vector3 offset = Vector3.up * shootStepUpDistance;

        transform.localPosition += offset;

        yield return new WaitForSeconds(shootStepUpDuration);

        transform.localPosition -= offset;

        shootStepRoutine = null;
    }

    private void OnEnemyBulletDestroyed(Bullet b)
    {
        if (activeEnemyBullet == b)
            activeEnemyBullet = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dying) return;

        if (other.gameObject.layer == bulletLayer)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null && bullet.owner == Bullet.Owner.Player)
            {
                Destroy(other.gameObject);
                StartDeath();
            }
        }
    }

    void StartDeath()
    {
        dying = true;

        if (shootStepRoutine != null)
        {
            StopCoroutine(shootStepRoutine);
            shootStepRoutine = null;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXAtPosition(AudioManager.Instance.enemyDeath, transform.position);

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return null;

        float delay = 0.25f;

        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.length > 0.0f)
                delay = stateInfo.length;
        }

        yield return new WaitForSeconds(delay);

        onEnemyDied?.Invoke(pointsWorth);
        Destroy(gameObject);
    }
}