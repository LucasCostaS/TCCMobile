using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour
{
  public GameObject menuInicial, menuFases, Botao;

  public void BotaoJogar()
  {
    menuInicial.SetActive(false);
    menuFases.SetActive(true);
  }

  public void BotaoFase()
  {
    int i = int.Parse(Botao.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text);
    SceneManager.LoadScene(i);
  }

  public void VoltarMenu()
  {
    SceneManager.LoadScene(0);
  }

  public void BotaoSair()
  {
    Application.Quit();
  }
}