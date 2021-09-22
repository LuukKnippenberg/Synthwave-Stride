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


        void Awake()
        {
            FillBarColorChange.Initialize(1f, 0f);
        }

        public void UpdateCounter(float percentage)
        {
            FloatFillImage.fillAmount = percentage;
            FillBarColorChange.UpdateVisual(percentage);
        }
    }
}