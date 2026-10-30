using UnityEngine;

public enum BallType
{
    Normal,
    Fire,
    Bomb
}

public class BallMovement : MonoBehaviour
{
    public float fallSpeed = 7f;
    public BallType ballType = BallType.Normal;

    public float hitWidth = 1.3f;
    public float hitHeight = 1.8f;

    private bool returning = false;

    void Start()
    {
        float randomX = Random.Range(-2.3f, 2.3f);
        transform.position = new Vector3(randomX, 5.5f, 1f);

        GameManager manager = FindAnyObjectByType<GameManager>();

        float extraSpeed = 0f;

        if (manager != null)
        {
            extraSpeed = Mathf.Min(manager.score / 1000f * 0.25f, 5f);
        }

        fallSpeed = Random.Range(7f, 9f) + extraSpeed;

        if (ballType == BallType.Fire)
        {
            fallSpeed += 1.5f;
        }
    }

    void Update()
    {
        if (returning == false)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            CheckForForgivingHit();
        }
        else
        {
            transform.position += Vector3.up * fallSpeed * Time.deltaTime;
        }

        if (transform.position.y < -7f)
        {
            GameManager manager = FindAnyObjectByType<GameManager>();

            if (manager != null && ballType != BallType.Bomb)
            {
                manager.LoseLife();
            }

            Destroy(gameObject);
        }

        if (transform.position.y > 8f)
        {
            Destroy(gameObject);
        }
    }

    void CheckForForgivingHit()
    {
        PlayerMovement paddle = FindAnyObjectByType<PlayerMovement>();

        if (paddle != null && paddle.isSwinging)
        {
            float horizontalDistance =
                Mathf.Abs(transform.position.x - paddle.transform.position.x);

            float verticalDistance =
                Mathf.Abs(transform.position.y - paddle.transform.position.y);

            if (horizontalDistance <= hitWidth &&
                verticalDistance <= hitHeight)
            {
                ReturnBall();
            }
        }
    }

    void ReturnBall()
    {
        if (returning)
        {
            return;
        }

        GameManager manager = FindAnyObjectByType<GameManager>();

        if (ballType == BallType.Bomb)
        {
            if (manager != null)
            {
                manager.LoseLife();
            }

            Destroy(gameObject);
            return;
        }

        returning = true;
        fallSpeed += 4f;

        if (manager != null)
        {
            if (ballType == BallType.Fire)
            {
                manager.PlayGameSound(manager.fireballHitSound);
                manager.AddScore(300);
            }
            else
            {
                manager.PlayGameSound(manager.pickleballHitSound);
                manager.AddScore(100);
            }
        }
    }
}