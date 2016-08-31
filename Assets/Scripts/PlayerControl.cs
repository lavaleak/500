using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    public float speed = 7.0f;
    public float rotationSpeed = 700.0f;
    public GameObject character;
    [HideInInspector]
    public AnimatorEvents animatorEvents;
    static public float cameraLimit = 3.0f;
    [HideInInspector]
    public bool died = false;

    /*
    void Awake() {
        Camera cam = Camera.main;
        if (cam.aspect > 1.5) { // 16:10, 16:9
            cameraLimit = 7.0f;
        }

        else if (cam.aspect <= 1.5) { // 3:2, 4:3, 5:4 
            cameraLimit = 5.5f;
        }

        else if (cam.aspect > 2) { // 21:9
            cameraLimit = 9.0f;
        }
    }
    */

    void Start() {
        animatorEvents = character.GetComponent<AnimatorEvents>();
    }

    void OnTriggerEnter(Collider other) {
        died = true;
    }

    void turn() {
        float rotY = character.transform.eulerAngles.y;
        float transX = Input.GetAxis("Horizontal") * speed;
        float transY = Input.GetAxis("Vertical") * speed;
        float rotateY = Input.GetAxis("Horizontal") * rotationSpeed;
      
        transX *= Time.deltaTime;
        transY *= Time.deltaTime;
        rotateY *= Time.deltaTime;

        // Check the side character is faced
        if (transX > 0) {
            if (rotY < 110 && rotY > 70) {
                rotateY = 0;
            }
            if (transform.position.x >= cameraLimit) {
                transX = 0;
            } 
        }
        
        if (transX < 0) {
            if (rotY < 290 && rotY > 250) {
                rotateY = 0;
            }
            if (transform.position.x <= -cameraLimit) {
                transX = 0;
            }
        }

        // Limit movement in Y
        if (transY > 0 && transform.position.y >= 6.58 + GameCore.km)
            transY = 0;
        else if (transY < 0 && transform.position.y <= -0.55 + GameCore.km)
            transY = 0;

        // Play moving animation
        if (transX != 0)
            animatorEvents.isMoving();
        else
            animatorEvents.stopMoving();
        
        character.transform.Rotate(0, rotateY, 0);
        transform.Translate(transX, transY, 0);
    }

    void Update() {
        if (animatorEvents.swimming && !animatorEvents.orbit) {
            turn();
        }

        if (transform.position.y > 105.0f) {
            animatorEvents.setFlying();
        }
        else if (GameCore.liftingStart) {
            animatorEvents.setSwimming();
        }
    }
}

