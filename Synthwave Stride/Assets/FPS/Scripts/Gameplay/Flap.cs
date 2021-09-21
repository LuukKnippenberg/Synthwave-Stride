using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class Flap : MonoBehaviour
    {
        [Header("References")] [Tooltip("Audio source for flapping sfx")]
        public AudioSource AudioSource;

        [Tooltip("Particles for flapping vfx")] public ParticleSystem[] FlapVfx;

        [Range(0f, 1f)]
        [Tooltip(
            "This will affect how much flapping will cancel the gravity value. 0 is not at all, 1 is instant")]
        public float FlapDownwardVelocityCancelingFactor = 0.3f;

        [Header("Durations")] [Tooltip("Time the player can flap")]
        public float Duration = 2f;

        [Header("Audio")] [Tooltip("Sound played when flapping")]
        public AudioClip FlapSfx;

        bool m_CanFlap;
        PlayerCharacterController m_PlayerCharacterController;
        PlayerInputHandler m_InputHandler;

        // stored ratio for flap duration (1 is full, 0 is empty)
        public float DurationRatio { get; private set; }

        public bool IsPlayerGrounded() => m_PlayerCharacterController.IsGrounded;

        void Start()
        {

            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Flap>(m_PlayerCharacterController,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Flap>(m_InputHandler, this, gameObject);

            DurationRatio = 1f;

            AudioSource.clip = FlapSfx;
            AudioSource.loop = true;
        }

        void Update()
        {
            // flap can only be used if not grounded and jump has been pressed again once in-air
            if (IsPlayerGrounded())
            {
                m_CanFlap = false;
            }
            else if (!m_PlayerCharacterController.HasJumpedThisFrame && m_InputHandler.GetJumpInputDown())
            {
                m_CanFlap = true;
            }

            // flap usage
            bool flapping = m_CanFlap && DurationRatio > 0f &&
                                  m_InputHandler.GetJumpInputHeld();
            if (flapping)
            {

                // calculate how much the player is slowed down
                float totalAcceleration = (-m_PlayerCharacterController.CharacterVelocity.y / Time.deltaTime) *
                                      FlapDownwardVelocityCancelingFactor;

                // apply the acceleration to character's velocity
                m_PlayerCharacterController.CharacterVelocity += Vector3.up * totalAcceleration * Time.deltaTime;

                // consume stamina
                DurationRatio = DurationRatio - (Time.deltaTime / Duration);

                for (int i = 0; i < FlapVfx.Length; i++)
                {
                    var emissionModulesVfx = FlapVfx[i].emission;
                    emissionModulesVfx.enabled = true;
                }

                if (!AudioSource.isPlaying)
                    AudioSource.Play();
            }
            else
            {
                // refill the meter if the player lands
                if (m_PlayerCharacterController.IsGrounded)
                {
                    DurationRatio = Duration;
                }

                for (int i = 0; i < FlapVfx.Length; i++)
                {
                    var emissionModulesVfx = FlapVfx[i].emission;
                    emissionModulesVfx.enabled = false;
                }

                // keeps the ratio between 0 and 1
                DurationRatio = Mathf.Clamp01(DurationRatio);

                if (AudioSource.isPlaying)
                    AudioSource.Stop();
            }
        }
    }
}