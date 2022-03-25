using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    public float multiplier = 1.5f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Collect(other);
        }
    }

    void Collect(Collider2D player)
    {
        PlayerHealth stats = player.GetComponent<PlayerHealth>();
        stats.maxHealth *= multiplier;
        Destroy(gameObject);
    }
}
