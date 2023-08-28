using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpMensagem : MonoBehaviour
{
    public Text mensagemText;
    void Start()
    {
        StartCoroutine(TimeToDestroy());
    }

    public void SetUp(string mensagem) {
        mensagemText.text = mensagem;
    }

    IEnumerator TimeToDestroy() {

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
