using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 6f;

    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;
    public float bulletLifetime = 3f;

    public bool destroyOnDeath = false;
    public static event Action onPlayerDied;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    private Bullet activePlayerBullet;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        float move = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) move -= 1f;
            if (Keyboard.current.dKey.isPressed) move += 1f;
        }

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
        if (isDead) return;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (activePlayerBullet != null) return;

        if (bulletPrefab == null || shootOffsetTransform == null) return;

        GameObject shotObj = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);

        Bullet bullet = shotObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.direction = Vector2.up;
            bullet.owner = Bullet.Owner.Player;

            activePlayerBullet = bullet;
            bullet.onDestroyed += OnPlayerBulletDestroyed;
        }

        Destroy(shotObj, bulletLifetime);

        if (anim != null)
            anim.SetTrigger("Shot Trigger");
    }

    private void OnPlayerBulletDestroyed(Bullet b)
    {
        if (activePlayerBullet == b)
            activePlayerBullet = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDieFromEnemyBullet(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDieFromEnemyBullet(other.gameObject);
    }

    private void TryDieFromEnemyBullet(GameObject hit)
    {
        if (isDead) return;

        Bullet bullet = hit.GetComponent<Bullet>();
        if (bullet == null) return;

        if (bullet.owner != Bullet.Owner.Enemy) return;

        Destroy(hit);
        Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        onPlayerDied?.Invoke();

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;


        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
    }
}