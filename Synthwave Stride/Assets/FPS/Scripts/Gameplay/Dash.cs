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
        public float DashVelocity = 30f;

        [Header("Durations")]
        [Tooltip("Time it takes to dash")]
        public float Duration = 0.2f;

        [Tooltip("Delay after last use before being able to dash again")]
        public float RechargeDelay = 5f;

        [Header("Audio")]
        [Tooltip("Sound played when using the dash")]
        public AudioClip DashSfx;

        public bool m_IsDashing { get; private set; }
        PlayerCharacterController m_PlayerCharacterController;
        PlayerInputHandler m_InputHandler;
        float m_LastTimeOfUse;
        bool m_JustUsed;
        bool m_CanDash;

        public UnityEvent<float> OnRecharging;

        // stored ratio for dash resource (1 is full, 0 is empty)
        public float CurrentRechargeRatio;
        public float CurrentDashingRatio;

        public bool IsPlayerGrounded() => m_PlayerCharacterController.IsGrounded;

        void Start()
        {
            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Dash>(m_PlayerCharacterController,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Dash>(m_InputHandler, this, gameObject);

            CurrentRechargeRatio = 1f;
            OnRecharging.Invoke(CurrentRechargeRatio);
            CurrentDashingRatio = 1f;

            AudioSource.clip = DashSfx;
            AudioSource.loop = false;
            m_CanDash = true;
        }

        void Update()
        {
            // dash is used when the player presses the sprint button
            if (m_InputHandler.GetSprintInputHeld())
            {
                m_CanDash = false;
            }

            // dash usage
            m_IsDashing = !m_CanDash && CurrentDashingRatio > 0f;
            if (m_IsDashing)
            {
                m_JustUsed = true;
                // store the last time of use for refill delay
                m_LastTimeOfUse = Time.time;

                // apply new velocity
                m_PlayerCharacterController.CharacterVelocity = transform.forward * DashVelocity;

                // consume fuel
                CurrentDashingRatio = CurrentDashingRatio - (Time.deltaTime / Duration);
                CurrentRechargeRatio = 0f;
                OnRecharging.Invoke(CurrentRechargeRatio);

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
                    if (IsPlayerGrounded())
                    {
                        m_PlayerCharacterController.CharacterVelocity = transform.forward * m_PlayerCharacterController.MaxSpeedOnGround;
                    }
                    else
                    {
                        m_PlayerCharacterController.CharacterVelocity = transform.forward * m_PlayerCharacterController.MaxSpeedInAir;
                    }
                    m_JustUsed = false;
                    m_IsDashing = false;
                }

                // refill the meter over time
                if (CurrentRechargeRatio < 1f)
                {
                    CurrentRechargeRatio = CurrentRechargeRatio + (Time.deltaTime / RechargeDelay);
                }
                else
                {
                    CurrentDashingRatio = 1f;
                    m_CanDash = true;
                }
                CurrentRechargeRatio = Mathf.Clamp01(CurrentRechargeRatio);
                OnRecharging.Invoke(CurrentRechargeRatio);

                for (int i = 0; i < DashVfx.Length; i++)
                {
                    var emissionModulesVfx = DashVfx[i].emission;
                    emissionModulesVfx.enabled = false;
                }
            }
        }
    }
}