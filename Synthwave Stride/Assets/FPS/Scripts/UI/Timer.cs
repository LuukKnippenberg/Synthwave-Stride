using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Unity.FPS.UI
{
    public class Timer : MonoBehaviour
    {
        [Tooltip("Image component representing floating")]
        public Image TimerFillImage;

        [Tooltip("Component to animate the color when floating")]
        public FillBarColorChange FillBarColorChange;

        [Tooltip("Total time the player has to survive")]
        public float TotalTime;

        float m_TimeLeft;
        float m_TimeLeftRatio;

        public UnityEvent OnTimerRanOut;
        bool stopTimer = false;

        void Awake()
        {
            FillBarColorChange.Initialize(1f, 0f);
            m_TimeLeft = TotalTime;
            m_TimeLeftRatio = 1f;
        }

        void Update()
        {
            if (!stopTimer)
            {
                m_TimeLeft -= (Time.deltaTime / TotalTime);
                m_TimeLeftRatio = m_TimeLeft / TotalTime;
                m_TimeLeftRatio = Mathf.Clamp01(m_TimeLeftRatio);

                TimerFillImage.fillAmount = m_TimeLeftRatio;
                FillBarColorChange.UpdateVisual(m_TimeLeftRatio);

                if (m_TimeLeft <= 0)
                {
                    OnTimerRanOut.Invoke();
                }
            }
            
        }

        public void AddTime(float time)
        {
            m_TimeLeft += time;
            m_TimeLeft = Mathf.Clamp(m_TimeLeft, 0f, TotalTime);
        }

        public void StopTimer()
        {
            stopTimer = true;
        }
    }
}