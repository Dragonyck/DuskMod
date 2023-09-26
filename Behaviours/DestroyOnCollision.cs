using System;
using UnityEngine;

namespace DuskMod
{
    public class DestroyOnCollision : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider)
            {
                if (!collision.collider.gameObject.IsEnemyOrBoss())
                {
                    return;
                }
            }
            Destroy(base.gameObject);
        }
    }
    public class DestroyOnInvisible : MonoBehaviour
    {
        public void OnBecameInvisible()
        {
            Destroy(base.gameObject);
        }
    }
}
