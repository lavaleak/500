using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour {
    private AudioSource audioSrc;
    private Animator anim;
    private Vector3 initPos;
    public bool customTriggerKm = false;
    public float triggerKm = 0.0f;
    public string animPropertyName;
    private bool triggered = false;

    void Start() {
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.pitch = Random.Range(0.75f, 1.0f);
        initPos = transform.position;
        if (!customTriggerKm)
            triggerKm = initPos.y - 5;
    }

    void doSomething() {
        if (GameCore.km >= triggerKm && !triggered && triggerKm > GameCore.checkpoints[GameCore.lastCheckpoint]) {
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
