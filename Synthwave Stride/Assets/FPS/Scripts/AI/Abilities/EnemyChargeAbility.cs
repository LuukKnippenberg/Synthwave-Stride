using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.AI { 

    public class EnemyChargeAbility : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float chargeSpeed;
        [SerializeField] private float chargeDistance;
        [SerializeField] private float damage;
        private Vector3 targetPosition;
        private Vector3 targetCalc;
        private Vector3 startPosition;

        [SerializeField] private Transform player;

        private Vector3 velocity = Vector3.zero;

        [SerializeField] public bool charging;

        EnemyController m_EnemyController;
        EnemyMobile enemyMobile;

        private Vector3 dir;

        private float startTime;
        private float elapsedTime;
        [SerializeField]private float resetTime;

        // Start is called before the first frame update
        void Start()
        {
            enemyMobile = GetComponent<EnemyMobile>();
            m_EnemyController = GetComponent<EnemyController>();
            charging = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (charging)
            {
                Charging();
            }
        }

        private void Charging()
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.SmoothDamp(transform.position, targetCalc, ref velocity, chargeSpeed);
            //m_EnemyController.OrientTowards(targetPosition);
            //Vector3.RotateTowards(transform.position, dir, 1f, 1f);

            if (Vector3.Distance(transform.position, targetCalc) < 3.8f)
            {
                charging = false;
                //enemyMobile.AiState = EnemyMobile.AIState.Patrol;
            }
            else if ((startTime + elapsedTime) > (startTime + resetTime))
            {
                //Its kinda wonky
                charging = false;
            }
        }



        public void StartDash()
        {
            startTime = Time.time;
            elapsedTime = 0;

            startPosition = transform.position;
            targetPosition = player.position;

            dir = (this.transform.position - targetPosition).normalized;

            targetCalc = startPosition + -dir * chargeDistance;
            charging = true;
        }

    }
}