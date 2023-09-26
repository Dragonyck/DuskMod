using System;
using UnityEngine;

namespace DuskMod
{
   public class AnimEventHandler : MonoBehaviour
    {
        public void EnableCollider()
        {
            base.GetComponent<Collider2D>().enabled = true;
        }
    }
}
