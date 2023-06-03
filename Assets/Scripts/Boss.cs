using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject boomerang;
    //herda componentes de Enemy que sao publicos ou protected (nao herda private)
    void Awake ()
    {

    }

    void ThrowBoomrang()
    {
        if(!isDead)
        {
            anim.SetTrigger("Boomerang");
            GameObject tempBoomerang = Instantiate(boomerang, transform.position, transform.rotation);

            Invoke("ThrowBoomerang", Random.Range())
        }
    }
}
