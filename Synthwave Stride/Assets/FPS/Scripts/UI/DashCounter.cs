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

        void Awake()
        {
            FillBarColorChange.Initialize(1f, 0f);
        }

        public void UpdateCounter(float percentage)
        {
            DashFillImage.fillAmount = percentage;
            FillBarColorChange.UpdateVisual(percentage);
        }
    }
}