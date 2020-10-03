using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    enum LevelDifficulty{
        LevelDifficulty_Low = 0,
        LevelDifficulty_Medium,
        LevelDifficulty_High,
    };

    enum LevelType {
        LevelType_ShortRange,
        LevelType_LongRange,
    }

    float enemy_value = 0.0f;
    float box_value = 0.0f;

    GameObject map_empty_object;
    GameObject level_floor;

    GameObject current_level = null;
    GameObject next_level = null;
    ResourceRequest req_next_level_load;

    public GameObject level_prefab = null;


    IEnumerator LoadNextLevelCor()
    {

        req_next_level_load = Resources.LoadAsync("./Prefabs/Level", typeof(GameObject));

        while (!req_next_level_load.isDone)
        {
            yield return null;
        }

        //Modify Level with random values
        Vector3 current_size = current_level.transform.Find("LevelFloor").localScale;

        next_level = (GameObject)req_next_level_load.asset;
        next_level.transform.localPosition = current_level.transform.localPosition + new Vector3(0.0f,0.0f, current_size.z);

        Invoke("DestroyPreviousLevel", 2.0f);
    }

    void DestroyPreviousLevel()
    {
        Destroy(current_level);
        current_level = next_level;
        next_level = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        //current_level = (GameObject)Resources.Load("Prefabs/Level", typeof(GameObject));
        current_level = Instantiate(level_prefab);
        Invoke("LoadNextLevelCor", 2.0f);
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update() {

    }
}
