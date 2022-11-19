using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController2 : MonoBehaviour
{

    public bool spawn;
    private Vector2 lugar = new Vector2(0.0f, 0.0f);
    private Collider2D vet, vet1;
    private bool[] posicaoCorreta = new bool[16];
    public GameObject vitoria, sombra, pecas, stock;
    public PosicaoSnap posicaoSnap;

    void Start()
    {
        spawn = true;
        posicaoSnap = pecas.GetComponent<PosicaoSnap>();
    }

    void Update()
    {

        vet = Physics2D.OverlapCircle(lugar, 0.1f);
        spawn = (vet == null) || (vet.transform.localPosition != new Vector3(0f, 0f, 0f));

        checarPosicao();
        
        checarVitoria();

    }

    private void checarPosicao()
    {
        double angulo = 0;
        for (int i = 0; i < posicaoCorreta.Length; i++)
        {
            vet1 = Physics2D.OverlapCircle(posicaoSnap.posicao[i], 0.01f);

            if (vet1 != null)
                angulo = Math.Ceiling(vet1.gameObject.transform.eulerAngles.z);

            switch (i)
            {
                case 0:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && angulo == 0);
                    break;
                case 1:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "ResistorPrefab" && angulo == 0);
                    break;
                case 2:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (angulo == 0 || angulo == 180));
                    break;
                case 3:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && angulo == 270);
                    break;
                case 4:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaTPrefab" && angulo == 90);
                    break;
                case 5:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "FontePrefab" && angulo == 0);
                    break;
                case 6:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && angulo == 270);                 
                    break;
                case 7:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (angulo == 90 || angulo == 270));
                    break;
                case 8:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (angulo == 90 || angulo == 270));
                    break;
                case 9:
                    posicaoCorreta[i] = (vet1 == null);
                    break;
                case 10:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "ResistorPrefab" && angulo == 90);
                    break;
                case 11:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (angulo == 90 || angulo == 270));
                    break;
                case 12:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && angulo == 90);
                    break;
                case 13:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (angulo == 0 || angulo == 180));
                    break;
                case 14:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaTPrefab" && angulo == 180);
                    break;
                case 15:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && angulo == 180);
                    break;
                default:
                    break;
            }
        }  
    }

    private void checarVitoria()
    {
        int cont = 0;
        
        for (int i = 0; i < posicaoCorreta.Length; i++)
        {
            
            if (posicaoCorreta[i] == true)
            {
                cont += 1;
            }
                

        }
        if (cont >= 16)
        {
            vitoria.SetActive(true);
            sombra.SetActive(false);
            stock.SetActive(false);
            int k = pecas.transform.childCount;
            for (int i = 0; i < k; i++)
            {
                Destroy(pecas.transform.GetChild(i).GetChild(0).GetComponent<DragnDrop>());
                pecas.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
                pecas.transform.GetChild(i).GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0f);
                pecas.transform.GetChild(i).GetChild(0).localPosition = new Vector3(pecas.transform.GetChild(i).GetChild(0).localPosition.x * 0.5f, pecas.transform.GetChild(i).GetChild(0).localPosition.y * 0.5f, 0f);

            }

            spawn = false;
        }
    }

}
