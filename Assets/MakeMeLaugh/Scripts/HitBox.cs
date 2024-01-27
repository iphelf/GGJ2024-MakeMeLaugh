using System;
using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    public class HitBox : MonoBehaviour
    {
        public event EventHandler<PlayerControl> OnHitPlayer;

        private void OnTriggerEnter(Collider other)
        {
            var playerControl = other.GetComponent<PlayerControl>();
            if (playerControl != null)
                OnHitPlayer?.Invoke(this, playerControl);
        }
    }
}