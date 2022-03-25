using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedUp : MonoBehaviour
{
    public float multiplier = 20f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other);
        }
    }

    void Collect(Collider2D player)
    {
        Movement stats = player.GetComponent<Movement>();
        stats.runSpeed += multiplier;
        Destroy(gameObject);
    }
}