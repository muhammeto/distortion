using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
