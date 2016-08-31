using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCore : MonoBehaviour {
    static public float km = 0.0f;
    public float liftingSpeed = 1.0f;
    public Text counter;
    public GameObject finalText;
    public GameObject player;
    private PlayerControl playerControl;
    static public bool liftingStart = false;
    private bool playerStarted = false;
    public GameObject sun;
    public GameObject mainMenu;
    public Text credits;
    private Animator creditsShow;
    public GameObject[] enemiesParent = new GameObject[5];
    public Enemy[][] enemies = new Enemy[5][];
    public GameObject ship;
    private bool shipDropped = false;
    public GameObject pauseMenu;
    static public float[] checkpoints = { 0.1f, 50.0f, 100.0f, 170.0f, 200.0f, 250.0f, 330.0f, 400.0f };
    [HideInInspector]
    static public int lastCheckpoint = 0;
    public float startPos;
    private bool paused = false;

    [Header("Skybox Blend Manager")]
    public Material[] skyBoxes;
    public Color[] fogColors;
    [Range(0, 1.0f)]
    public float blend = 0.0f;
    private int currentSky = 0;
    public bool changeSky = false;
    public float changeSkySpeed = 1.0f;

    [Header("Music Manager")]
    private AudioSource audioSrc;
    public AudioClip[] musics;
    private int currentMusic = 0;

    void Start() {
        RenderSettings.skybox = skyBoxes[currentSky];
        playerControl = player.GetComponent<PlayerControl>();
        creditsShow = credits.GetComponent<Animator>();
        blend = 1.0f;
        RenderSettings.skybox.SetFloat("_Blend", blend);
        counter.color = new Color(1.0f,1.0f, 1.0f,0);
        finalText.SetActive(false);
        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = musics[currentMusic];
        audioSrc.Play();
        for (int i = 0; i < 5; i++) {
            enemies[i] = enemiesParent[i].GetComponentsInChildren<Enemy>();
        }
    }

    void nextMusic() {
        audioSrc.volume = 0;
        currentMusic++;
        audioSrc.clip = musics[currentMusic];
        audioSrc.Play();
    }

    void setSky() {
        if (changeSky) {
            if (blend < 1.0f) {
                blend += 0.1f * changeSkySpeed * Time.deltaTime;
                RenderSettings.skybox.SetFloat("_Blend", blend);
            }
            else if (currentSky < skyBoxes.Length - 1) {
                blend = 1.0f;
                RenderSettings.skybox.SetFloat("_Blend", blend);

                currentSky++;
                RenderSettings.skybox = skyBoxes[currentSky];

                blend = 0;
                RenderSettings.skybox.SetFloat("_Blend", 0);

                changeSky = false;
            }
        }
    }

    void shipDrop() {
        AudioSource shipAudioSrc = ship.GetComponent<AudioSource>();
        if (ship.transform.position.y == 125.0f)
            shipAudioSrc.Play();
        ship.transform.Translate(Random.Range(-5.0f,5.0f) * Time.deltaTime,0, 10.0f * Time.deltaTime);
        if (ship.transform.position.y < 60) {
            Destroy(ship);
            shipDropped = true;
        }
    }

    void startGame() {
        if (playerStarted && shipDropped) {
            liftingSpeed += 0.05f * Time.deltaTime;

            if (transform.position.y > startPos) {
                transform.Translate((-transform.up * liftingSpeed) * Time.deltaTime);
                player.transform.Translate((transform.up * liftingSpeed) * Time.deltaTime);
                if (blend > 0)
                    blend -= 0.01f;
                if (km < 100.0f) {
                    creditsShow.SetBool("creditsShow", true);
                    RenderSettings.fogColor = fogColors[0];
                }
                RenderSettings.skybox.SetFloat("_Blend", blend);
            }
            else {
                transform.position = new Vector3(0, startPos, 0);
                player.transform.position = new Vector3(0, startPos, 6.3f);
                playerControl.animatorEvents.setGameStart();
                if (playerControl.animatorEvents.swimming) {
                    liftingStart = true;
                    liftingSpeed = 7.0f;
                    nextMusic();
                }
            }
        }
        else if (playerStarted && !shipDropped) {
            shipDrop();
        }
        else if (Input.anyKeyDown && !playerStarted) {
            counter.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            playerStarted = true;
            mainMenu.SetActive(false);
        }
    }

    void pause() {
        Time.timeScale = 0;
        audioSrc.volume = 0.5f;
        pauseMenu.SetActive(true);
    }

    public void exitPause() {
        Time.timeScale = 1.0f;
        audioSrc.volume = 1.0f;
        pauseMenu.SetActive(false);
        paused = false;
    }

    public void quitGame() {
        Debug.Log("quit");
        Application.Quit();
    }

    void hideSun() {
        if (km < 100.0f) {
            sun.SetActive(false);
        }
        else {
            sun.SetActive(true);
            RenderSettings.fogColor = fogColors[1];
        }
    }

    void showFinalText() {
        finalText.SetActive(true);
    }

    void returnToCheckpoint() {
        if (transform.position.y > checkpoints[lastCheckpoint]) {
            player.transform.position = new Vector3(0, 600.0f, 6.3f);
            liftingStart = false;
            liftingSpeed = 20.0f;
            transform.Translate((-transform.up * liftingSpeed) * Time.deltaTime);
            audioSrc.pitch = -1.0f;
            for (int i = 0; i < 5; i++) {
                foreach (Enemy e in enemies[i]) {
                    e.reset();
                }
            }
        }
        else {
            audioSrc.pitch = 1.0f;
            liftingSpeed = 5.0f;
            liftingStart = true;
            playerControl.died = false;
            player.transform.position = new Vector3(0, checkpoints[lastCheckpoint], 6.3f);
        }
    }

    void Update() {
        if (liftingStart) {
            if (audioSrc.volume < 1.0f)
                audioSrc.volume += 0.01f;
            if (km < 500.0f)
                transform.Translate((transform.up * liftingSpeed) * Time.deltaTime);
            if (km > 70.0f && currentSky == 0) {
                changeSky = true;
            }
            else if (km > 200.0f && currentSky == 1) {
                changeSky = true;
                RenderSettings.fogColor = fogColors[2];
            }
            else if (km > 300.0f && currentSky == 2) {
                changeSky = true;
                RenderSettings.fogColor = fogColors[3];
            }
            else if (km > 400.0f && currentSky == 3) {
                changeSky = true;
                RenderSettings.fogColor = fogColors[4];
            }
            else if (km >= 500.0f) {
                transform.position = new Vector3(0, 500.0f, 0);
                playerControl.animatorEvents.setOrbit();
                audioSrc.Stop();
                Invoke("showFinalText", 10);
            }
            if (km > 110.0f)
                sun.transform.Translate(0, 0, 0.2f);
        }
        else if (!playerControl.died) {
            startGame();
        }

        if (playerControl.died)
            returnToCheckpoint();

        if (Input.GetButtonDown("Cancel") && liftingStart) {
            paused = true;
        }

        if (paused) {
            pause();
        }

        else {
            exitPause();
        }
        
        if (transform.position.y >= 0)
            km = transform.position.y;
        counter.text = Mathf.Floor(km) + "km";

        if (lastCheckpoint < checkpoints.Length - 1 && liftingStart) {
            if (GameCore.km > checkpoints[lastCheckpoint + 1]) {
                lastCheckpoint++;
            }
        }

        hideSun();
        setSky();
    }
}
