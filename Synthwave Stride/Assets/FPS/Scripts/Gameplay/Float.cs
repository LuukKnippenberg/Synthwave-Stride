using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class Float : MonoBehaviour
    {
        [Header("References")] [Tooltip("Audio source for floating sfx")]
        public AudioSource AudioSource;

        [Tooltip("Particles for floating vfx")] public ParticleSystem[] FloatVfx;

        [Range(0f, 1f)]
        [Tooltip(
            "This will affect how much floating will cancel the gravity value. 0 is not at all, 1 is instant")]
        public float FloatDownwardVelocityCancelingFactor = 0.3f;

        [Header("Durations")] [Tooltip("Time the player can float")]
        public float Duration = 2f;

        [Header("Audio")] [Tooltip("Sound played when floating")]
        public AudioClip FloatSfx;

        bool m_CanFloat;
        PlayerCharacterController m_PlayerCharacterController;
        PlayerInputHandler m_InputHandler;

        // stored ratio for float duration (1 is full, 0 is empty)
        public float DurationRatio { get; private set; }

        public bool IsPlayerGrounded() => m_PlayerCharacterController.IsGrounded;

        void Start()
        {

            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Float>(m_PlayerCharacterController,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Float>(m_InputHandler, this, gameObject);

            DurationRatio = 1f;

            AudioSource.clip = FloatSfx;
            AudioSource.loop = true;
        }

        void Update()
        {
            // float can only be used if not grounded and jump has been pressed again once in-air
            if (IsPlayerGrounded())
            {
                m_CanFloat = false;
            }
            else if (!m_PlayerCharacterController.HasJumpedThisFrame && m_InputHandler.GetJumpInputDown())
            {
                m_CanFloat = true;
            }

            // float usage
            bool floating = m_CanFloat && DurationRatio > 0f &&
                                  m_InputHandler.GetJumpInputHeld();
            if (floating)
            {

                // calculate how much the player is slowed down
                float totalAcceleration = (-m_PlayerCharacterController.CharacterVelocity.y / Time.deltaTime) *
                                      FloatDownwardVelocityCancelingFactor;

                // apply the acceleration to character's velocity
                m_PlayerCharacterController.CharacterVelocity += Vector3.up * totalAcceleration * Time.deltaTime;

                // consume stamina
                DurationRatio = DurationRatio - (Time.deltaTime / Duration);

                for (int i = 0; i < FloatVfx.Length; i++)
                {
                    var emissionModulesVfx = FloatVfx[i].emission;
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
                    if (AudioSource.isPlaying)
                        AudioSource.Stop();
                }

                for (int i = 0; i < FloatVfx.Length; i++)
                {
                    var emissionModulesVfx = FloatVfx[i].emission;
                    emissionModulesVfx.enabled = false;
                }

                // keeps the ratio between 0 and 1
                DurationRatio = Mathf.Clamp01(DurationRatio);

                if (AudioSource.isPlaying)
                    AudioSource.Pause();
            }
        }
    }
}