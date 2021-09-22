using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class FloatCounter : MonoBehaviour
    {
        [Tooltip("Image component representing floating")]
        public Image FloatFillImage;

        [Tooltip("Canvas group that contains the whole UI for floating")]
        public CanvasGroup MainCanvasGroup;

        [Tooltip("Component to animate the color when floating")]
        public FillBarColorChange FillBarColorChange;

        Float m_Float;

        void Awake()
        {
            m_Float = FindObjectOfType<Float>();
            DebugUtility.HandleErrorIfNullFindObject<Float, FloatCounter>(m_Float, this);

            FillBarColorChange.Initialize(1f, 0f);
        }

        void Update()
        {
            FloatFillImage.fillAmount = m_Float.DurationRatio;
            FillBarColorChange.UpdateVisual(m_Float.DurationRatio);
        }
    }
}