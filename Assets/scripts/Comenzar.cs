using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comenzar : MonoBehaviour
{
  private bool lookingAtThis;
  void Update()
  {
    if (lookingAtThis && Input.GetButtonDown("BotonX")) {
      Debug.Log("Comenzar 2");
      SceneManager.LoadScene("Demo_Scene");
    }
  }
  // public void OnPointerClick()
  // {
  //   Debug.Log("Comenzar");
  //   Application.LoadLevel("Demo_Scene");
  // }
  public void OnPointerEnter()
  {
    lookingAtThis = true;
  }

  public void OnPointerExit()
  {
    lookingAtThis = false;
  }
}
