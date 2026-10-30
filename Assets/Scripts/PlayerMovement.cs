using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public bool isSwinging = false;

    private float swingTimer = 0f;
    private Quaternion normalRotation;

    void Start()
    {
        normalRotation = transform.rotation;
    }

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move = 1f;
        }

        Vector3 newPosition = transform.position;
        newPosition.x += move * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -2.5f, 2.5f);

        transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            isSwinging = true;
            swingTimer = 0.55f;
            transform.rotation = normalRotation * Quaternion.Euler(0f, 0f, -25f);
        }

        if (isSwinging)
        {
            swingTimer -= Time.deltaTime;

            if (swingTimer <= 0)
            {
                isSwinging = false;
                transform.rotation = normalRotation;
            }
        }
    }
}