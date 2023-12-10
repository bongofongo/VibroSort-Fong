using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum ObjectCategory {
    Concrete,
    Squishy,
    Chalky,
    Bruisy,
    Alien
}

public class ObjectGenerator : MonoBehaviour
{
    public int numObjsToSpawn = 20;
    public int minPerCat = 2;

    public Scoreboard scoreboard;
    private int leftToSpawn;

    public Material[] categoryMaterials;

    public GameObject[] bins;
    public GameObject[] objectPrefabs;

    private Collider spawnCollider;


    // Start is called before the first frame update
    void Start()
    {
        spawnCollider = this.GetComponent<Collider>();
    }

    public void AssignBins() {
        System.Random random = new System.Random();
        GameObject[] shuffledBins = bins.OrderBy(x => random.Next()).ToArray();
        for (int i = 0; i < shuffledBins.Length; i++) {
            shuffledBins[i].GetComponent<Renderer>().material = categoryMaterials[i];
            shuffledBins[i].transform.GetChild(0).GetComponent<BinCheck>().binCategory = (ObjectCategory)i;
            SpatialHapticSource shs = shuffledBins[i].transform.GetChild(0).AddComponent<SpatialHapticSource>;
        }
    }

    public void InstantiateObjects() {
        scoreboard.SetNumObjectsToSort(numObjsToSpawn);
        leftToSpawn = numObjsToSpawn;

        // Assures
        for (int i = 0; i < minPerCat; i++) {
            foreach (ObjectCategory oCat in (ObjectCategory[]) System.Enum.GetValues(typeof(ObjectCategory))) {
                leftToSpawn -= 1;
                Quaternion randomStartRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
                Vector3 randomStartPos = RandomPointInBounds(spawnCollider.bounds);
                GameObject currObj = Instantiate(objectPrefabs[(int)oCat], randomStartPos, randomStartRot);
            }
        }
        while (leftToSpawn > 0) {
            leftToSpawn -= 1;
            Quaternion randomStartRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            Vector3 randomStartPos = RandomPointInBounds(spawnCollider.bounds);
            GameObject currObj = Instantiate(objectPrefabs[Random.Range(0,4)], randomStartPos, randomStartRot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
