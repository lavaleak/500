using UnityEngine;
using System.Collections;

public class AnimatorEvents : MonoBehaviour {
    [HideInInspector]
    public bool gameStart, swimming, flying, orbit, movingX;
    [HideInInspector]
    public Animator anim;
    public GameObject flyingTurbine;
    public GameObject swimmingTurbine;

    void Start() {
        anim = GetComponent<Animator>();
        gameStart = anim.GetBool("gameStart");
        swimming = anim.GetBool("swimming");
        flying = anim.GetBool("flying");
        orbit = anim.GetBool("orbit");
        movingX = anim.GetBool("movingX");
        flyingTurbine.SetActive(false);
        swimmingTurbine.SetActive(false);
    }

    public void setGameStart() {
        gameStart = true;
        anim.SetBool("gameStart", gameStart);
    }

    public void setSwimming() {
        swimming = true;
        flying = false;
        flyingTurbine.SetActive(false);
        swimmingTurbine.SetActive(true);
        anim.SetBool("swimming", swimming);
    }

    public void setFlying() {
        flying = true;
        swimmingTurbine.SetActive(false);
        flyingTurbine.SetActive(true);
        anim.SetBool("flying", flying);
    }

    public void setOrbit() {
        orbit = true;
        flyingTurbine.SetActive(false);
        anim.SetBool("orbit", orbit);
    }

    public void isMoving() {
        movingX = true;
        anim.SetBool("movingX", movingX);
    }

    public void stopMoving() {
        movingX = false;
        anim.SetBool("movingX", movingX);
    }

    public void reset() {
        stopMoving();
        orbit = false;
        flying = false;
        swimming = false;
        gameStart = false;
        swimmingTurbine.SetActive(false);
        flyingTurbine.SetActive(false);
        anim.SetBool("orbit", orbit);
        anim.SetBool("flying", flying);
        anim.SetBool("swimming", swimming);
        anim.SetBool("gameStart", gameStart);
    }
}