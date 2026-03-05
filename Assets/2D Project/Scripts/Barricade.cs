using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Barricade : MonoBehaviour
{
    public Sprite[] damageStages;
    public int maxHits = 6;

    private int hits;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        hits = 0;
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet == null) return;

        Destroy(other.gameObject);
        TakeHit();
    }

    private void TakeHit()
    {
        hits++;
        UpdateSprite();

        if (hits >= maxHits)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateSprite()
    {
        if (damageStages == null || damageStages.Length == 0) return;

        int index = Mathf.FloorToInt((hits / (float)maxHits) * (damageStages.Length - 1));
        index = Mathf.Clamp(index, 0, damageStages.Length - 1);

        sr.sprite = damageStages[index];
    }
}