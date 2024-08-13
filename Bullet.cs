using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "BorderBullet") {
            Destroy(gameObject);
        }
    }
}
