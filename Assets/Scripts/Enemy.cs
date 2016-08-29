using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour {
    private AudioSource audioSrc;
    private Animator anim;
    private Vector3 initPos;
    public float triggerKm = 0.0f;
    public string animPropertyName;
    private bool triggered = false;

    void Start() {
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.pitch = Random.Range(0.5f, 1.0f);
        initPos = transform.position;
    }

    void doSomething() {
        if (GameCore.km >= triggerKm && !triggered) {
            anim.SetBool(animPropertyName, true);
            audioSrc.Play();
            triggered = true;
        }
    }

    public void reset() {
        anim.SetBool(animPropertyName, false);
        transform.position = initPos;
        audioSrc.Stop();
        audioSrc.time = 0;
        triggered = false;
    }
	
	void Update () {
        if (GameCore.liftingStart) {
            doSomething();
        }
	}
}
