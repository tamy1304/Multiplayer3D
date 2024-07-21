using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPies : MonoBehaviour
{
    public Controlador personaje;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
            personaje.puedeSaltar = true;
    }

    private void OnTriggerExit(Collider other)
    {
            personaje.puedeSaltar = false;
    }
}
