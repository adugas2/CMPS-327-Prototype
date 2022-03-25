using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform target;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 15)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}