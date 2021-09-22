using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class BubblePickup : Pickup
    {
        [Header("Parameters")] [Tooltip("Amount of time to add on pickup")]
        public float TimeAmount;

        public UnityEvent<float> OnTimePickup;

        protected override void OnPicked(PlayerCharacterController player)
        {
            OnTimePickup.Invoke(TimeAmount);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}