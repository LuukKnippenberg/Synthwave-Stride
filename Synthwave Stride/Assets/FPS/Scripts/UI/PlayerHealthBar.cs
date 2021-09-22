using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        [Tooltip("Canvas group that contains the whole UI for player health")]
        public CanvasGroup MainCanvasGroup;

        Health m_PlayerHealth;

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, PlayerHealthBar>(
                playerCharacterController, this);

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHealthBar>(m_PlayerHealth, this,
                playerCharacterController.gameObject);
        }

        void Update()
        {
            MainCanvasGroup.gameObject.SetActive(false);

            // update health bar value
            HealthFillImage.fillAmount = m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth;
        }
    }
}