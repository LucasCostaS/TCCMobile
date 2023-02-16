using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour
{
  public GameObject menuInicial, menuFases, Botao, state;

  private void Update()
  {
    if (Application.platform == RuntimePlatform.Android)
    {
      if (Input.GetKey(KeyCode.Escape))
      {
        if (SceneManager.GetActiveScene().buildIndex > 0)
          VoltarMenu();
        else
          BotaoSair();
      }
    }
    else if (Application.platform == RuntimePlatform.WindowsPlayer)
    {
      if (Input.GetKey(KeyCode.Escape))
      {
        if (SceneManager.GetActiveScene().buildIndex > 0)
          VoltarMenu();
        else
          BotaoSair();
      }

    }
  }

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

  public void desfazer()
  {
    state.GetComponent<StateController10>().Desfazer();
  }
}