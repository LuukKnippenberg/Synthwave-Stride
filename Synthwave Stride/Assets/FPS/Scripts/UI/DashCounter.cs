using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class DashCounter : MonoBehaviour
    {
        [Tooltip("Image component representing the dash")]
        public Image DashFillImage;

        [Tooltip("Canvas group that contains the whole UI for the dash")]
        public CanvasGroup MainCanvasGroup;

        [Tooltip("Component to animate the color when the player has a dash or not")]
        public FillBarColorChange FillBarColorChange;

        Dash m_Dash;

        void Awake()
        {
            m_Dash = FindObjectOfType<Dash>();
            DebugUtility.HandleErrorIfNullFindObject<Dash, DashCounter>(m_Dash, this);

            FillBarColorChange.Initialize(1f, 0f);
        }

        void Update()
        {
            if (m_Dash.m_IsDashing)
            {
                DashFillImage.fillAmount = 0;
                FillBarColorChange.UpdateVisual(0);
            }
            else 
            {
                DashFillImage.fillAmount = 1;
                FillBarColorChange.UpdateVisual(1);
            }
        }
    }
}