using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed = 1f;
    public float resetPositionX;   
    public float destinationX;     

    private float currentX;

    void Update()
    {
        currentX = transform.position.x;
        currentX += speed * Time.deltaTime;
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        if (currentX <= destinationX)
        {
            Debug.Log("Resetting background position.");
            currentX = resetPositionX;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }
    }
}