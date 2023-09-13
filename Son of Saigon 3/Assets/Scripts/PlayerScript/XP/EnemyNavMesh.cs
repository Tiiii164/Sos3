using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;

namespace CodeMonkey.HealthSystemCM
{
    public class EnemyNavMesh : MonoBehaviour, IGetHealthSystem
    {
        [SerializeField] int HP = 100;
        [SerializeField] int DamageStat = 15;
        public Animator animator;
        [SerializeField] XPTracker XPTracker;
        //private AudioSource enemyAudio;

        private HealthSystem healthSystem;

        //public AudioClip SpiderChaseAudioClip;
        //public AudioClip SpiderChaseAudioClip2;
        //public AudioClip ChaseThemeAudioClip;
        //public AudioClip SpiderPatrolAudioClip;
        //private bool hasPlayedTheme = false;


        public void Awake()
        {
            healthSystem = new HealthSystem(HP);
            healthSystem.OnDead += HealthSystem_OnDead;
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }





        // Start is called before the first frame update
        public void Start()
        {
            //enemyAudio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public void Update()
        {
            // Check if the animator is in the "Chase" state
            /* if (animator.GetCurrentAnimatorStateInfo(0).IsName("ChaseState") && !hasPlayedTheme)
             {
                 // Play the ChaseThemeAudioClip
                 //enemyAudio.PlayOneShot(SpiderChaseAudioClip);
                 //enemyAudio.PlayOneShot(SpiderChaseAudioClip2);
                 hasPlayedTheme = true;
             }else
             {
                 hasPlayedTheme = false;
             }*/
        }

        public void Damage(int damageAmount)
        {
            /*if (HP <= 0)
           {
               //play death animation
               animator.SetTrigger("Die");
           }
           else
           {
               //play get hit animation
               animator.SetTrigger("Damaged");
           }*/
            healthSystem.Damage(damageAmount);
            Debug.Log("Enemy Damaged");
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterStats player))
            {
                player.Damage(DamageStat);
                //enemy.Damage(5);
            }
        
        }

        private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {

            //animator.SetTrigger("Damaged");
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            animator.SetTrigger("Die");
            XPTracker.AddXP(200);
            Destroy(gameObject, 5);
        }

        public HealthSystem GetHealthSystem()
        {
            return healthSystem;
        }
    }
}


    
 
