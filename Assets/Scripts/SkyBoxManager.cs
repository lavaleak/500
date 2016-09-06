using UnityEngine;
using System.Collections;

public class SkyBoxManager : MonoBehaviour {
    public Material[] skyBoxes;
    public Color[] fogColors;
    [Range(0, 1.0f)]
    public float blend = 1.0f;
    private int currentSky = 0;
    public bool changeSky = false;
    public float changeSkySpeed = 1.0f;

    void Start () {
        blend = 1.0f;
        RenderSettings.skybox = skyBoxes[currentSky];
        RenderSettings.skybox.SetFloat("_Blend", blend);
        RenderSettings.fogColor = fogColors[1];
    }

    public void reset() {
        currentSky = 0;
        blend = 1.0f;
        changeSky = false;
        changeSkySpeed = 1.0f;
        RenderSettings.skybox.SetFloat("_Blend", blend);
        RenderSettings.skybox = skyBoxes[currentSky];
        RenderSettings.fogColor = fogColors[1];
    }

    public void setSky() {
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

    void setFog() {
        if (GameCore.km > 98.0f) {
            RenderSettings.fogColor = fogColors[1];
        }
        else {
            RenderSettings.fogColor = fogColors[0];
        }
    }

    public void startGame() {
        if (blend > 0) {
            blend -= 0.01f;
        }
        RenderSettings.skybox.SetFloat("_Blend", blend);
    }
    
    void Update () {
        if (GameCore.liftingStart && !changeSky) {
            if (GameCore.km > 70.0f && currentSky == 0) {
                changeSky = true;
            }
            else if (GameCore.km > 200.0f && currentSky == 1) {
                changeSky = true;
            }
            else if (GameCore.km > 300.0f && currentSky == 2) {
                changeSky = true;
            }
            else if (GameCore.km > 400.0f && currentSky == 3) {
                changeSky = true;
            }
        }

        setFog();
        setSky();
    }
}
