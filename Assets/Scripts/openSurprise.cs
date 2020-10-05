using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class openSurprise : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
      mc.SetActive(false);
      // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
      SceneManager.LoadScene("Sorpresa");

    }
  }
}


