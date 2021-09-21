using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class FlapCounter : MonoBehaviour
    {
        [Tooltip("Image component representing flapping")]
        public Image FlapFillImage;

        [Tooltip("Canvas group that contains the whole UI for flapping")]
        public CanvasGroup MainCanvasGroup;

        [Tooltip("Component to animate the color when using flap")]
        public FillBarColorChange FillBarColorChange;

        Flap m_Flap;

        void Awake()
        {
            m_Flap = FindObjectOfType<Flap>();
            DebugUtility.HandleErrorIfNullFindObject<Flap, FlapCounter>(m_Flap, this);

            FillBarColorChange.Initialize(1f, 0f);
        }

        void Update()
        {
            FlapFillImage.fillAmount = m_Flap.DurationRatio;
            FillBarColorChange.UpdateVisual(m_Flap.DurationRatio);
        }
    }
}