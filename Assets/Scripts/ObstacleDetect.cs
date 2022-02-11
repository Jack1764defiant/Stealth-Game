using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetect : MonoBehaviour
{
    public GameObject obj; // Assign in inspector
    public ParticleSystem dissapear;
    bool active = true;

    public void Start()
    {
        obj.SetActive(true);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && active)
        {
            dissapear.Play();
            obj.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
