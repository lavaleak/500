using UnityEngine;
using System.Collections;

public class WhiteFade : MonoBehaviour {

    static public bool fadeIsOver = false;

    public void setFade() {
        fadeIsOver = true;
        gameObject.SetActive(false);
    }
}
