using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private int poolSize = 20;

    [SerializeField] private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem ps = Instantiate(particlePrefab);
            //ps.transform.SetParent(transform, false);
           
            // ParticleSystem ps = Instantiate(particlePrefab/*, transform*/);
            ps.gameObject.SetActive(false);
            pool.Enqueue(ps);
        }
    }

    public ParticleSystem Get()
    {
        ParticleSystem ps;
        if (pool.Count > 0)
        {
            ps = pool.Dequeue();
        }
        else
        {
            ps = Instantiate(particlePrefab);
        }



       // ps.transform.SetParent(transform, false);      
        ps.gameObject.SetActive(true);
        return ps;
    }
    public void Return(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }
}
