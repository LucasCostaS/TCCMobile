using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController5 : MonoBehaviour
{
    public bool spawn = true;
    public GameObject pecasCriadas;

    void Start()
    {
        
    }

    void Update()
    { 
        spawn = (Physics2D.OverlapCircle(new Vector2(pecasCriadas.transform.localPosition.x, pecasCriadas.transform.localPosition.y), 0.1f, (1 << 6)) == null);
    }
}
