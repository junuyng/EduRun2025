using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 5f;
    public float offscreenX = -10f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= offscreenX)
        {
            gameObject.SetActive(false);
        }
    }
}