using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_CtrlParticles : MonoBehaviour
{

    public ParticleSystem Particules;
    public GameObject VectorObj;
    public float Rate;
    public float Speed;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ForceOverLifetimeModule forceVector;



    // Start is called before the first frame update
    void Start()
    {
        emission = Particules.emission;
        forceVector = Particules.forceOverLifetime;
        VectorObj.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        emission.rateOverTime = Rate;
        forceVector.x = VectorObj.transform.right.x * Speed;
        forceVector.y = VectorObj.transform.right.y * Speed;
        forceVector.z = VectorObj.transform.right.z * Speed;

    }


}
