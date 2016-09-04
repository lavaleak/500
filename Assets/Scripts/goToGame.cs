using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class goToGame : MonoBehaviour {
	public void goTo () {
        SceneManager.LoadScene(1);
	}
}
