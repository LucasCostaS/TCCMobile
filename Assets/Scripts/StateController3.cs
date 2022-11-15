using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateController3 : MonoBehaviour
{

    public GameObject r1, r2, r3, r4, prefab1, pecas, vitoria;
    private float movX1, movY1, rotZ, movX2, movX3, movY3, newFPS;
    private bool teste1, teste2, teste3, criar1, parte1, parte2, anim;
    public bool click;

    private float fps = 90f;

    // Start is called before the first frame update
    void Start()
    {
        movX1 = (r4.transform.localPosition.x - r3.transform.localPosition.x);
        movY1 = (r4.transform.localPosition.y - r3.transform.localPosition.y);
        movX2 = (r2.transform.localPosition.x - r4.transform.localPosition.x);
        movX3 = (r2.transform.localPosition.x - r1.transform.localPosition.x);
        movY3 = (r2.transform.localPosition.y - r1.transform.localPosition.y);
        criar1 = true;
        parte1 = true;
        parte2 = true;
        anim = true;
        click = true;
        newFPS = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        newFPS = 1.0f / Time.smoothDeltaTime;
        if (newFPS != float.PositiveInfinity)
            fps = Mathf.Lerp(fps, newFPS, 0.005f);
        //UnityEngine.Debug.Log((fps));

        if (r4 != null)
        {
            equivalente1();
        }
        if (r3 != null)
        {
            equivalente2();
        }
        if (r2 != null)
        {
            equivalente3();
        }
        if (r2 == null)
        {
            checarVitoria();
        }
    }

    private void equivalente1()
    {
        if (r3.GetComponent<Resistores3>().modificado == true && r4.GetComponent<Resistores3>().modificado == true && parte1 == true)
        {
            if (criar1)
            {
                Instantiate(prefab1, r3.transform.position, Quaternion.identity, r3.transform.parent);
                criar1 = false;
            }
            teste1 = r3.transform.localPosition.y > r4.transform.localPosition.y;
            teste3 = !teste2 && r3.transform.localPosition.x <= r4.transform.localPosition.x;
            teste2 = !teste1 && r3.transform.rotation.eulerAngles.z < 90;

            if (teste1)
                r3.transform.Translate(0f, (movY1 / fps), 0f, Space.World);
            if (teste2)
                r3.transform.Rotate(0f, 0f, 0.5f);
            if (teste3)
                r3.transform.Translate((movX1 / fps), 0f, 0f, Space.World);


            if (!teste1 && !teste2 && !teste3)
            {
                r3.transform.localPosition = r4.transform.localPosition;
                r3.GetComponent<Resistores3>().resistencia = r3.GetComponent<Resistores3>().resistencia + r4.GetComponent<Resistores3>().resistencia;
                r4.SetActive(false);
                r4 = null;
                r3.GetComponent<Resistores3>().reduzido = true;
                parte1 = false;
                //r3.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText(r3.GetComponent<Resistores3>().resistencia.ToString());
            }

        }
    }

    private void equivalente2()
    {
        if (parte1 == false && r2.GetComponent<Resistores3>().modificado == true)
        {
            GameObject[] linha = new GameObject[] { r3, pecas.transform.GetChild(6).gameObject, pecas.transform.GetChild(9).gameObject, pecas.transform.GetChild(19).gameObject };
            teste1 = r3.transform.localPosition.x > r2.transform.localPosition.x;
            pecas.transform.GetChild(3).gameObject.SetActive(false);
            pecas.transform.GetChild(4).gameObject.SetActive(false);
            pecas.transform.GetChild(20).gameObject.SetActive(false);
            pecas.transform.GetChild(8).gameObject.SetActive(false);
            pecas.transform.GetChild(16).gameObject.SetActive(false);
            pecas.transform.GetChild(17).gameObject.SetActive(false);
            pecas.transform.GetChild(18).gameObject.SetActive(false);
            if (teste1)
            {
                foreach (GameObject item in linha)
                {
                    item.transform.Translate(movX2 / fps, 0f, 0f, Space.World);
                }
            }
            else
            {
                foreach (GameObject item in linha)
                {
                    item.transform.localPosition = new Vector3(r2.transform.localPosition.x, item.transform.localPosition.y, 0f);
                }
                r2.GetComponent<Resistores3>().resistencia = 1 / ((1 / r3.GetComponent<Resistores3>().resistencia) + (1 / r2.GetComponent<Resistores3>().resistencia));
                r3.SetActive(false);
                r2.GetComponent<Resistores3>().reduzido = true;
                r3 = null;
                parte2 = false;
                criar1 = true;
            }

        }
    }

    private void equivalente3()
    {
        if (r1.GetComponent<Resistores3>().modificado == true && r2.GetComponent<Resistores3>().modificado == true && parte2 == false)
        {
            if (criar1)
            {
                Instantiate(prefab1, r1.transform.position, Quaternion.identity, r1.transform.parent);
                criar1 = false;
            }
            teste1 = r1.transform.localPosition.y > r2.transform.localPosition.y;
            teste3 = !teste2 && r1.transform.localPosition.x <= r2.transform.localPosition.x;
            teste2 = !teste1 && r1.transform.rotation.eulerAngles.z < 90;

            if (teste1)
                r1.transform.Translate(0f, movY3 / fps, 0f, Space.World);
            if (teste2)
                r1.transform.Rotate(0f, 0f, 0.5f);
            if (teste3)
                r1.transform.Translate(movX3 / fps, 0f, 0f, Space.World);


            if (!teste1 && !teste2 && !teste3)
            {
                r1.transform.localPosition = r2.transform.localPosition;
                r1.GetComponent<Resistores3>().resistencia = r1.GetComponent<Resistores3>().resistencia + r2.GetComponent<Resistores3>().resistencia;
                r2.SetActive(false);
                r2 = null;
                r1.GetComponent<Resistores3>().reduzido = true;
                //parte1 = false;
                //r3.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText(r3.GetComponent<Resistores3>().resistencia.ToString());
            }

        }
    }

    private void checarVitoria()
    {
        if (r1.GetComponent<Resistores3>().resistencia == 3)
        {
            vitoria.SetActive(true);
            if (anim)
            {
                pecas.transform.Translate((2.56f / fps), 0f, 0f);
                if (pecas.transform.position.x >= 6.144f)
                {
                    pecas.transform.position = new Vector3(6.144f, 0f, 0f);
                    for (int i = 0; i < pecas.transform.childCount; i++)
                    {
                        if (pecas.transform.GetChild(i).childCount == 1)
                            pecas.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
                        else
                            pecas.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    anim = false;
                }
            }

        }
    }

    public void resetar()
    {
        if (r2 == null && vitoria.activeSelf == false)
        {
            pecas.transform.GetChild(pecas.transform.childCount - 1).gameObject.SetActive(false);
            r2 = pecas.transform.GetChild(11).gameObject;
            r2.SetActive(true);
            r1.transform.localPosition = pecas.transform.GetChild(pecas.transform.childCount - 1).transform.localPosition;
            r1.transform.Rotate(0f, 0f, -90f);
            r1.GetComponent<Resistores3>().resistencia = r1.GetComponent<Resistores3>().resistencia - r2.GetComponent<Resistores3>().resistencia;
            r1.GetComponent<Resistores3>().reduzido = false;
            r1.GetComponent<Resistores3>().modificado = false;
            Destroy(pecas.transform.GetChild(pecas.transform.childCount - 1).gameObject);
            criar1 = true;
            return;
        }

        if (r3 == null && r2 != null)
        {
            r3 = pecas.transform.GetChild(5).gameObject;
            GameObject[] linha = new GameObject[] { r3, pecas.transform.GetChild(6).gameObject, pecas.transform.GetChild(9).gameObject, pecas.transform.GetChild(19).gameObject };
            r2.GetComponent<Resistores3>().resistencia = (r2.GetComponent<Resistores3>().resistencia * r3.GetComponent<Resistores3>().resistencia) / (r2.GetComponent<Resistores3>().resistencia + r3.GetComponent<Resistores3>().resistencia);
            r3.SetActive(true);
            r2.GetComponent<Resistores3>().reduzido = false;
            r2.GetComponent<Resistores3>().modificado = false;
            pecas.transform.GetChild(3).gameObject.SetActive(true);
            pecas.transform.GetChild(4).gameObject.SetActive(true);
            pecas.transform.GetChild(20).gameObject.SetActive(true);
            pecas.transform.GetChild(8).gameObject.SetActive(true);
            pecas.transform.GetChild(16).gameObject.SetActive(true);
            pecas.transform.GetChild(17).gameObject.SetActive(true);
            pecas.transform.GetChild(18).gameObject.SetActive(true);
            foreach (GameObject item in linha)
            {
                item.transform.localPosition = new Vector3(pecas.transform.GetChild(18).localPosition.x + 5.12f, item.transform.localPosition.y, 0f);
            }

            parte2 = true;
            criar1 = true;
            return;
        }

        if (r4 == null && r3 != null)
        {
            r4 = pecas.transform.GetChild(12).gameObject;
            r3.transform.localPosition = pecas.transform.GetChild(pecas.transform.childCount - 1).localPosition;
            r3.transform.Rotate(0f, 0f, -90f);
            Destroy(pecas.transform.GetChild(pecas.transform.childCount - 1).gameObject);
            r3.GetComponent<Resistores3>().resistencia = r3.GetComponent<Resistores3>().resistencia - r4.GetComponent<Resistores3>().resistencia;
            r4.SetActive(true);
            r3.GetComponent<Resistores3>().reduzido = false;
            r3.GetComponent<Resistores3>().modificado = false;
            r4.GetComponent<Resistores3>().reduzido = false;
            r4.GetComponent<Resistores3>().modificado = false;
            parte1 = true;
            criar1 = true;
            return;
        }
    }
}