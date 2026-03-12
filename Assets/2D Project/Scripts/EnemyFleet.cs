using UnityEngine;

public class EnemyFleet : MonoBehaviour
{
    public float stepX = 0.3f;
    public float stepDown = 0.4f;
    public float startInterval = 0.7f;
    public float minInterval = 0.1f;
    public float leftLimit = -7f;
    public float rightLimit = 7f;

    private float timer;
    private int direction = 1;

    private int initialEnemyCount;

    void Start()
    {
        initialEnemyCount = transform.childCount;
        timer = startInterval;

        ApplyAnimationSpeed(timer);
    }

    void Update()
    {
        if (transform.childCount == 0) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        MoveFleet();

        timer = GetCurrentInterval();
        ApplyAnimationSpeed(timer);
    }

    void MoveFleet()
    {
        float leftMost = float.MaxValue;
        float rightMost = float.MinValue;

        foreach (Transform child in transform)
        {
            if (child == null) continue;

            float x = child.position.x;
            if (x < leftMost) leftMost = x;
            if (x > rightMost) rightMost = x;
        }

        bool hitLeft = leftMost <= leftLimit && direction == -1;
        bool hitRight = rightMost >= rightLimit && direction == 1;

        if (hitLeft || hitRight)
        {
            transform.position += Vector3.down * stepDown;

            direction *= -1;
        }
        else
        {
            transform.position += Vector3.right * direction * stepX;
        }
    }

    float GetCurrentInterval()
    {
        int alive = transform.childCount;

        float progress = 1f - (alive / (float)initialEnemyCount);

        return Mathf.Lerp(startInterval, minInterval, progress);
    }

    void ApplyAnimationSpeed(float interval)
    {
        if (interval <= 0f) return;

        float speed = startInterval / interval;

        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator a in animators)
        {
            if (a == null) continue;

            AnimatorStateInfo state = a.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("EnemyDeath"))
                continue;

            a.speed = speed;
        }
    }
}