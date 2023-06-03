using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            //chama funcao de atacar inimigo ao interagir
            enemy.TookDamage(damage);
        }
        
    }
}
