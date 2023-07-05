using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
        [SerializeField] bool _isSetActive = false;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
            
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
