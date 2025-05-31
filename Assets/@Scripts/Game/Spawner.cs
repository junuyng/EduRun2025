using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string objectTag = "Obstacle"; 
    public float spawnInterval = 2f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0f);
            ObjectPoolManager.Instance.SpawnFromPool(objectTag, spawnPos);
        }
    }
}