using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int _vida;
    void Start()
    {
        
    }

    void Update()
    {
        if(_vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
