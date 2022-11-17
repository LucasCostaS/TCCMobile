using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragnDropStock : MonoBehaviour
{
    private Vector3 screenPoint, offset;
    private string tipo;
    private int limite;
    private StateController2 controlador;
    private SpriteRenderer rend;

    public GameObject prefab;
    public GameObject pai;
    public GameObject state;

    private void Start()
    {
        controlador = state.GetComponent<StateController2>();
    }

}
