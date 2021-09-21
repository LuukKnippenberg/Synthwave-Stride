using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.AI { 

    public class EnemyChargeAbility : MonoBehaviour
    {
        [SerializeField] private Transform chargeDamagePosition;
        [SerializeField] [Range(0.0f, 1.0f)] private float chargeSpeed;
        [SerializeField] private float chargeDistance;
        [SerializeField] private float damage;
        [SerializeField] private float damageDistance;
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
            m_EnemyController.OrientTowards(targetCalc);
            //Vector3.RotateTowards(transform.position, dir, 1f, 1f);

            Debug.Log(Vector3.Distance(chargeDamagePosition.position, player.position));
            if(Vector3.Distance(chargeDamagePosition.position, player.position) < damageDistance)
            {
                player.GetComponent<Health>().TakeDamage(damage, gameObject);
            }

            if (Vector3.Distance(transform.position, targetCalc) < 3.8f)
            {
                charging = false;
            }
            else if ((startTime + elapsedTime) > (startTime + resetTime))
            {
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