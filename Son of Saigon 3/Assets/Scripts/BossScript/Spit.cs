using System.Collections;
using UnityEngine;

namespace LlamAcademy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Spit : MonoBehaviour
    {
        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(DelayDisable());
        }

        private IEnumerator DelayDisable()
        {
            if (Wait == null)
            {
                Wait = new WaitForSeconds(AutoDestroyTime);
            }

            yield return null;
            
            Rigidbody.AddForce(transform.forward * Force);
            
            yield return Wait;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // apply damage 
            //gameObject.SetActive(false);
        }
        
        
        [SerializeField]
        private float AutoDestroyTime = 2f;

        [SerializeField] private float Force = 100;
        
        private WaitForSeconds Wait;
        private Rigidbody Rigidbody;
    }
}
