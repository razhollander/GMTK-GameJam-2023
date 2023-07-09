using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
        [SerializeField] bool _isSetActive = false;

        private IEnumerator Start()
        {
            var particles = GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(particles.main.duration*particles.sim);
            
            if (_isSetActive)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    
}
