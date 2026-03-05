using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.up;
    
    public enum Owner { Player, Enemy }
    public Owner owner = Owner.Player;
    public Action<Bullet> onDestroyed;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }
}