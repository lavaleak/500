using UnityEngine;
using System.Collections;

public class AssetGenerator : MonoBehaviour {
    public Transform[] assets100;
    public Transform[] assets100rare;
    public Transform[] assets200;
    public Transform[] assets200rare;
    public Transform[] assets300;
    public Transform[] assets300rare;
    public Transform[] assets400;
    public Transform[] assets400rare;
    public Transform[] assets500;
    public Transform[] assets500rare;

    void generate(Transform[] array, int amount, float minY, float maxY, 
        float minZ = 15.0f, float maxZ = 30.0f, float minX = -10.0f, float maxX = 10.0f) {
        for (int i = 0; i < amount; i++) {
            Transform temp = Instantiate(array[Random.Range(0, array.Length)]);
            temp.transform.position = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                Random.Range(minZ, maxZ));
            float scale = Random.Range(0.1f, 2.0f);
            temp.transform.localScale = new Vector3(scale, scale, scale);
            float[] mirror = { 1, -1 };
            temp.transform.localScale = new Vector3(
                temp.transform.localScale.x * mirror[Random.Range(0,2)],
                temp.transform.localScale.y,
                temp.transform.localScale.z
                );
            temp.transform.parent = transform;
        }
    }
    
    void Start () {
        generate(assets100, 10, 0, 90);
        generate(assets100rare, 1, 0, 90);

        generate(assets200, 10, 125, 200, 15.0f, 30.0f, -30.0f, 30.0f);
        generate(assets200rare, 1, 125, 200);

        generate(assets300, 10, 200, 300, 15.0f, 30.0f, -30.0f, 30.0f);
        // generate(assets300rare, 2, 200, 300);

        generate(assets400, 200, 300, 400, 20.0f, 50.0f, -30.0f, 30.0f);
        // generate(assets400rare, 2, 300, 400);

        generate(assets500, 400, 400, 600, 20.0f, 50.0f, -30.0f, 30.0f);
        generate(assets500rare, 1, 400, 450);
    }
}
