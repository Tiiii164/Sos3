using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using LlamAcademy.FSM;

namespace CodeMonkey.HealthSystemCM
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody bulletRigidbody;
        private float bulletSpeed = 20f;
        //[SerializeField] private CharacterStats characterStats;

        private CharacterStats characterStats;
        private void Awake()
        {
            bulletRigidbody = GetComponent<Rigidbody>();
            
        }
        // Update is called once per frame
        void Start()
        {
            characterStats = CharacterStats.Instance;
            bulletRigidbody.velocity = transform.up * bulletSpeed;
          
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyNavMesh enemy))
            {
                enemy.Damage(characterStats.DamageStat);
               
                Destroy(gameObject);
            }
            else if (other.TryGetComponent(out BossHealthSystem boss))
            {
                boss.Damage(characterStats.DamageStat);
              
                Destroy(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }

}
