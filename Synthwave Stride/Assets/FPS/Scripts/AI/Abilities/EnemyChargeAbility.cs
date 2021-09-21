using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.AI { 

    public class EnemyChargeAbility : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float chargeSpeed;
        [SerializeField] private float damage;
        [SerializeField] private Vector3 targetPosition;
        private Vector3 startPosition;

        [SerializeField] private Transform player;

        private Vector3 velocity = Vector3.zero;

        [SerializeField] public bool charging;

        EnemyController m_EnemyController;
        EnemyMobile enemyMobile;

        private Vector3 dir;

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
            //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, chargeSpeed);
            //m_EnemyController.OrientTowards(targetPosition);

            Vector3.RotateTowards(transform.position, dir, 1f, 1f);

            Debug.Log(Vector3.Distance(transform.position, targetPosition));

            if (Vector3.Distance(transform.position, targetPosition) < 3.8f)
            {
                //charging = false;
                //enemyMobile.AiState = EnemyMobile.AIState.Patrol;
            }
        }

        public void StartDash()
        {
            startPosition = transform.position;
            targetPosition = player.position;

            dir = (this.transform.position - targetPosition).normalized;
            charging = true;
        }

    }
}