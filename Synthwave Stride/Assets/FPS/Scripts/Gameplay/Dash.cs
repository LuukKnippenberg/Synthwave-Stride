using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class Dash : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Audio source for dash sfx")]
        public AudioSource AudioSource;

        [Tooltip("Particles for dash vfx")] public ParticleSystem[] DashVfx;

        [Header("Parameters")]
        [Tooltip("The strength with which the jetpack pushes the player up")]
        public float DashAcceleration = 7f;

        [Header("Durations")]
        [Tooltip("Time it takes to dash")]
        public float Duration = 0.5f;

        [Tooltip("Delay after last use before being able to dash again")]
        public float RechargeDelay = 5f;

        [Header("Audio")]
        [Tooltip("Sound played when using the dash")]
        public AudioClip DashSfx;

        bool m_IsDashing;
        PlayerCharacterController m_PlayerCharacterController;
        PlayerInputHandler m_InputHandler;
        float m_LastTimeOfUse;
        bool m_JustUsed;

        // stored ratio for dash resource (1 is full, 0 is empty)
        public float CurrentFillRatio { get; private set; }

        void Start()
        {
            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Dash>(m_PlayerCharacterController,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Dash>(m_InputHandler, this, gameObject);

            CurrentFillRatio = 1f;

            AudioSource.clip = DashSfx;
            AudioSource.loop = false;
        }

        void Update()
        {
            // dash is used when the player presses the sprint button
            if (m_InputHandler.GetSprintInputHeld())
            {
                m_IsDashing = true;
            }

            // dash usage
            bool dashIsInUse = m_IsDashing && CurrentFillRatio > 0f;
            if (dashIsInUse)
            {
                m_JustUsed = true;
                // store the last time of use for refill delay
                m_LastTimeOfUse = Time.time;

                // apply the acceleration to character's velocity
                m_PlayerCharacterController.CharacterVelocity = transform.forward * DashAcceleration;

                // consume fuel
                CurrentFillRatio = CurrentFillRatio - (Time.deltaTime / Duration);

                for (int i = 0; i < DashVfx.Length; i++)
                {
                    var emissionModulesVfx = DashVfx[i].emission;
                    emissionModulesVfx.enabled = true;
                }

                if (!AudioSource.isPlaying)
                    AudioSource.Play();
            }
            else
            {
                // slow down after dash
                if (m_JustUsed)
                {
                    m_PlayerCharacterController.CharacterVelocity = transform.forward * 5f;
                    m_JustUsed = false;
                }

                // refill the meter over time
                if (Time.time - m_LastTimeOfUse >= RechargeDelay)
                {
                    CurrentFillRatio = 1f;
                    m_IsDashing = false;
                }

                for (int i = 0; i < DashVfx.Length; i++)
                {
                    var emissionModulesVfx = DashVfx[i].emission;
                    emissionModulesVfx.enabled = false;
                }

                // keeps the ratio between 0 and 1
                CurrentFillRatio = Mathf.Clamp01(CurrentFillRatio);

                if (AudioSource.isPlaying)
                    AudioSource.Stop();
            }
        }
    }
}