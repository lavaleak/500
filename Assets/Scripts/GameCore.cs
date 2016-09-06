using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameCore : MonoBehaviour {
    static public float km = 0.0f;
    static public bool liftingStart = false;
    static public float[] checkpoints = { 0.1f, 50.0f, 100.0f, 170.0f, 200.0f, 250.0f, 327.0f, 390.0f };
    static public int lastCheckpoint = 0;
    public GameObject finalText;
    public GameObject player;
    public GameObject sun;
    public GameObject mainMenu;
    public GameObject ship;
    public GameObject pauseMenu;
    public Text counter;
    public Text credits;
    public GameObject arrowsUpDown;
    public GameObject arrowsLeftRight;
    public float startPos;
    public float liftingSpeed = 1.0f;
    public GameObject[] enemiesParent = new GameObject[5];
    public Enemy[][] enemies = new Enemy[5][];
    public AudioClip[] musics;
    public Animator[] arrowsAnimators;
    private Animator creditsShow;
    private SkyBoxManager skyboxMngr;
    private PlayerControl playerControl;
    private bool playerStarted = false;
    private bool shipDropped = false;
    private bool paused = false;
    private bool reseted = false;
    private float finalTimer = 0;
    private AudioSource audioSrc;
    private int currentMusic = 0;
    private bool showUpDown = true;
    private bool showLeftRight = true;
    private bool changeMusic = false;

    void Start() {
        playerControl = player.GetComponent<PlayerControl>();
        creditsShow = credits.GetComponent<Animator>();
        skyboxMngr = GetComponent<SkyBoxManager>();
        counter.color = new Color(1.0f,1.0f, 1.0f,0);
        finalText.SetActive(false);
        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = musics[currentMusic];
        audioSrc.Play();
        for (int i = 0; i < 5; i++) {
            enemies[i] = enemiesParent[i].GetComponentsInChildren<Enemy>();
        }
    }

    void reset() {
        transform.position = new Vector3(0.0f,105.0f,0.0f);
        sun.transform.position = new Vector3(168.03f, 101.0f, 673.0f);
        player.transform.position = new Vector3(0.0f, -105.0f, 6.3f);
        ship.transform.position = new Vector3(0.0f, 125.0f, 12.5f);
        skyboxMngr.reset();
        changeMusic = false;
        liftingStart = false;
        playerStarted = false;
        shipDropped = false;
        lastCheckpoint = 0;
        km = 0.0f;
        liftingSpeed = 5.0f;
        startPos = 0.0f;
        currentMusic = 0;
        creditsShow.SetBool("reset", true);
        creditsShow.SetBool("creditsShow", false);
        counter.color = new Color(1.0f, 1.0f, 1.0f, 0);
        finalText.SetActive(false);
        mainMenu.SetActive(true);
        audioSrc.loop = true;
        audioSrc.clip = musics[currentMusic];
        audioSrc.Play();
        playerControl.animatorEvents.reset();
        for (int i = 0; i < 5; i++) {
            foreach (Enemy e in enemies[i]) {
                e.reset();
            }
        }
        finalTimer = 0;
        reseted = false;
    }

    void nextMusic() {
        if (changeMusic) {
            audioSrc.volume = 0;
            currentMusic++;
            audioSrc.clip = musics[currentMusic];
            audioSrc.Play();
            changeMusic = false;
        }
    }

    void shipDrop() {
        AudioSource shipAudioSrc = ship.GetComponent<AudioSource>();
        if (ship.transform.position.y == 125.0f)
            shipAudioSrc.Play();
        ship.transform.Translate(Random.Range(-7.5f,7.5f) * Time.deltaTime,0, 10.0f * Time.deltaTime);
        if (ship.transform.position.y < 60) {
            ship.transform.position = new Vector3(1000.0f, 125.0f, 12.5f);
            shipAudioSrc.Stop();
            shipDropped = true;
        }
    }

    void startGame() {
        if (playerStarted && shipDropped) {
            liftingSpeed += 0.2f * Time.deltaTime;

            if (transform.position.y > startPos) {
                transform.Translate((-transform.up * liftingSpeed) * Time.deltaTime);
                player.transform.Translate((transform.up * liftingSpeed) * Time.deltaTime);
                skyboxMngr.startGame();
                if (km < 100.0f) {
                    creditsShow.SetBool("creditsShow", true);
                }
            }
            else {
                transform.position = new Vector3(0, startPos, 0);
                player.transform.position = new Vector3(0, startPos, 6.3f);
                playerControl.animatorEvents.setGameStart();
                if (playerControl.animatorEvents.swimming) {
                    liftingStart = true;
                    liftingSpeed = 5.0f;
                    changeMusic = true;
                }
            }
        }
        else if (playerStarted && !shipDropped) {
            shipDrop();
        }
        else if (Input.anyKeyDown && !playerStarted && WhiteFade.fadeIsOver) {
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

    void underWater() {
        if (km < 100.0f) {
            sun.SetActive(false);
        }
        else {
            sun.SetActive(true);
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

    void tutorial() {
        if (showLeftRight && liftingStart) {
            arrowsLeftRight.SetActive(true);
            arrowsAnimators[0].SetBool("tutorialStart",true);
            arrowsAnimators[1].SetBool("tutorialStart", true);
            showLeftRight = false;
        }
        if (showUpDown && liftingStart && km > 25.0f) {
            arrowsUpDown.SetActive(true);
            arrowsAnimators[2].SetBool("tutorialStart", true);
            arrowsAnimators[3].SetBool("tutorialStart", true);
            showUpDown = false;
        }
    }

    void Update() {
        if (liftingStart) {
            if (audioSrc.volume < 1.0f)
                audioSrc.volume += 0.01f;
            if (km < 500.0f)
                transform.Translate((transform.up * liftingSpeed) * Time.deltaTime);
            else if (km >= 500.0f) {
                transform.position = new Vector3(0, 500.0f, 0);
                playerControl.animatorEvents.setOrbit();
                if (audioSrc.loop) {
                    changeMusic = true;
                }
                audioSrc.loop = false;
                if (finalTimer >= 7.0f)
                    showFinalText();
                if (finalTimer >= 25.0f)
                    reseted = true;
                finalTimer += 1.0f * Time.deltaTime;
            }
            if (km > 110.0f)
                sun.transform.Translate(0, 0, 0.2f);
        }
        else if (!playerControl.died) {
            startGame();
        }

        if (playerControl.died)
            returnToCheckpoint();

        if (Input.GetButtonDown("Menu") && liftingStart) {
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

        if (reseted) {
            reset();
            creditsShow.SetBool("reset", false);
        }

        nextMusic();
        underWater();
        tutorial();
    }
}
