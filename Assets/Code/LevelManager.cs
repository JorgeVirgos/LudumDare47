using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    enum LevelDifficulty : int{
        Low = 0,
        Medium,
        High,
        Default,
    };

    enum LevelType : int {
        ShortRange = 0,
        LongRange,
        Default,
    }

    enum CardinalDirection : int
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }

    struct RoomNode
    {
        public RoomNode(int id)
        {
            room_id_ = id;

            room_prefab_ = null;
            room_id_north_ = 0;
            room_id_south_ = 0;
            room_id_west_  = 0;
            room_id_east_  = 0;
            level_type_ = (int)LevelType.Default;
            level_difficulty_ = (int)LevelDifficulty.Default;
            direction_locked = null;
        }

        public void AddLockedDoor(CardinalDirection dir, int key_num)
        {
            if (direction_locked == null) direction_locked = new Dictionary<CardinalDirection, int>();
            direction_locked.Add(dir, key_num);
        }


        public int id() { return room_id_; }
        public void ConN(ref RoomNode room) { room_id_north_ = room.id(); room.room_id_south_ = id(); }
        public void ConS(ref RoomNode room) { room_id_south_ = room.id(); room.room_id_north_ = id(); }
        public void ConW(ref RoomNode room) { room_id_west_ =  room.id(); room.room_id_east_  = id(); }
        public void ConE(ref RoomNode room) { room_id_east_ =  room.id(); room.room_id_west_  = id(); }

        public int room_id_;
        public GameObject room_prefab_;
        public int level_type_;
        public int level_difficulty_;

        public int room_id_north_;
        public int room_id_south_;
        public int room_id_west_;
        public int room_id_east_;

        public Dictionary<CardinalDirection, int> direction_locked;
    }

    List<RoomNode> rooms_;
    RoomNode current_room_;
    RoomNode previous_room_;
    List<RoomNode> next_rooms_;

    float room_size = 24.0f;
    float enemy_value = 0.0f;
    float box_value = 0.0f;

    GameObject map_empty_object;
    GameObject level_floor;

    public GameObject player_ = null;
    GameObject current_level = null;
    int current_corridor = -1;
    GameObject[] corridor_by_dir;
    List<GameObject> next_levels;
    List<GameObject> corridors;

    GameObject prefab_current_level = null;
    GameObject prefab_next_level = null;
    GameObject prefab_corridor_ = null;
    GameObject player_prefab_ = null;
    Object[] room_prefabs_;
    ResourceRequest req_next_level_load;

    bool entered_room = false;
    bool finished_room = true;
    bool should_open_doors = false;
    bool load_next_level = true;

    public GameObject PistolEnemy, RifleEnemy, ShotgunEnemy;
    public int num = 3;
    List<GameObject> spawnpoints = new List<GameObject>();
    List<GameObject> boxes = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();

    //IEnumerator LoadNextLevelCor()
    //{

    //    //req_next_level_load = Resources.LoadAsync("Prefabs/Level", typeof(GameObject));

    //    //while (!req_next_level_load.isDone)
    //    //{
    //    //    yield return null;
    //    //}

    //    next_room_ = FindRoom(current_room_.room_id_north_);
    //    if (next_room_.room_prefab_ == null) return null;


    //    prefab_next_level = next_room_.room_prefab_;
    //    next_level = Instantiate(prefab_next_level);
    //    next_level.transform.localPosition = current_level.transform.localPosition + new Vector3(0.0f, 0.0f, room_size);
    //    next_level.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
    //    should_open_doors = true;

    //    return null;
    //}

    GameObject InstantiateCustomLevel(GameObject level_prefab, Vector3 relative_pos)
    {
        GameObject new_level = Instantiate(level_prefab, transform);

        if(current_level != null)
            new_level.transform.position = current_level.transform.position + relative_pos * 2.0f;

        new_level.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        new_level.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        return new_level;
    }

    GameObject GetUnusedCorridor()
    {
        for(int i = 0; i < 4; ++i)
        {
            if (!corridors[i].activeSelf)
            {
                corridors[i].SetActive(true);
                return corridors[i];
            }
        }

        return null;
    }


    public void GetSpawnPointsAndBoxes(GameObject c)
    {
        Transform t = c.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == "spawner")
            {
                spawnpoints.Add(tr.gameObject);
            }
            if (tr.tag == "removableBox")
            {
                boxes.Add(tr.gameObject);
            }
        }



    }
    public void removeBoxes()
    {

        for (int i = 0; i < boxes.Count; i++)
        {
            int chance = UnityEngine.Random.Range(2, 5);
            if (UnityEngine.Random.Range(0, chance) == 0)
            {
                Destroy(boxes[i]);
            }
        }

    }

    public Vector3 getRandomSpawnPoint()
    {
        Vector3 pos = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Count)].transform.position;
        return new Vector3(pos.x, 1.5f, pos.z);
    }
    void Spawn(GameObject e1, GameObject e2)
    {
        for (int i = 0; i < num; i++)
        {

            switch (UnityEngine.Random.Range(1, 3))
            {
                case 1:
                    enemies.Add(Instantiate(e1, getRandomSpawnPoint(), Quaternion.identity));

                    break;
                case 2:
                    enemies.Add(Instantiate(e2, getRandomSpawnPoint(), Quaternion.identity));
                    break;

            }

        }

    }

    void SpawnIntoRoom()
    {
        if (current_level.name != "emptyRoom")
        {
            GetSpawnPointsAndBoxes(current_level.transform.Find("suelo").gameObject);
            if (current_level.transform.Find("suelo").tag == "shortRangeRoom")
            {
                Spawn(PistolEnemy, ShotgunEnemy);
            }
            else
            {
                Spawn(PistolEnemy, RifleEnemy);
            }

        }

    }

    void StartSpawn()
    {
        if (current_level.tag == "Untagged") return;

        SpawnIntoRoom();
        if (current_level.tag == "RandomRoom")
        {
            removeBoxes();
        }
    }

    void LoadNextLevel()
    {
        load_next_level = false;

        List<int> next_ids = new List<int>();
        List<Vector3> relative_positions = new List<Vector3>();

        for (int i = 0; i < 4; ++i) corridor_by_dir[i] = null;

        GameObject corridor = null;
        if(current_room_.room_id_north_ != 0 && current_room_.room_id_north_ != previous_room_.id()) {
            next_ids.Add(current_room_.room_id_north_);
            relative_positions.Add(new Vector3(0.0f, 0.0f, room_size));

            corridor = GetUnusedCorridor();
            if (corridor != null)
            {
                corridor.transform.position = current_level.transform.position + new Vector3(0.0f, 0.0f, 1.0f) * room_size;
                corridor.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                corridor_by_dir[0] = corridor;
            }
        }
        if (current_room_.room_id_south_ != 0 && current_room_.room_id_south_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_south_);
            relative_positions.Add(new Vector3(0.0f, 0.0f, -room_size));

            corridor = GetUnusedCorridor();
            if (corridor != null)
            {
                corridor.transform.position = current_level.transform.position + new Vector3(0.0f, 0.0f, -1.0f) * room_size;
                corridor.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                corridor_by_dir[1] = corridor;
            }
        }
        if (current_room_.room_id_east_ != 0 && current_room_.room_id_east_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_east_);
            relative_positions.Add(new Vector3(room_size, 0.0f, 0.0f));

            corridor = GetUnusedCorridor();
            if (corridor != null)
            {
                corridor.transform.position = current_level.transform.position + new Vector3(1.0f, 0.0f, 0.0f) * room_size;
                corridor.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                corridor_by_dir[2] = corridor;
            }
        }
        if (current_room_.room_id_west_ != 0 && current_room_.room_id_west_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_west_);
            relative_positions.Add(new Vector3(-room_size, 0.0f, 0.0f));

            corridor = GetUnusedCorridor();
            if (corridor != null)
            {
                corridor.transform.position = current_level.transform.position + new Vector3(-1.0f, 0.0f, 0.0f) * room_size;
                corridor.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                corridor_by_dir[3] = corridor;
            }
        }



        int counter = 0;
        foreach (int id in next_ids)
        {
            RoomNode next_room_;

            next_room_ = FindRoom(id);
            if (next_room_.id() == 0) continue;
            prefab_next_level = next_room_.room_prefab_;

            GameObject new_level = InstantiateCustomLevel(prefab_next_level, relative_positions[counter]);
            ActiveDoors(next_room_, new_level);

            next_rooms_.Add(next_room_);
            next_levels.Add(new_level);

            counter++;
        }

        StartSpawn();
        should_open_doors = true;

        return;
    }

    void DestroyUnusedLevels()
    {
        spawnpoints.Clear();
        foreach (GameObject enemy in enemies) Destroy(enemy);
        enemies.Clear();

        GameObject standing_level = null;
        int counter = 0;
        int target_pos = 0;
        float least_distance = 10000000.0f;
        foreach (GameObject level in next_levels)
        {
            float dist = Vector3.Distance(level.transform.position, player_.transform.position);
            if (dist <= least_distance)
            {
                target_pos = counter;
                standing_level = level;
                least_distance = dist;
            }

            counter++;
        }

        foreach (GameObject level in next_levels)
            if (level != standing_level)
            {
                Destroy(level);
            }

        next_levels.Clear();

        Destroy(current_level, 0.5f);
        current_level = standing_level;

        previous_room_ = current_room_;
        current_room_ = next_rooms_[target_pos];
        next_rooms_.Clear();


        counter = 0;
        target_pos = 0;
        least_distance = 1000000.0f;
        foreach (GameObject corridor in corridors)
        {
            float dist = Vector3.Distance(corridor.transform.position, player_.transform.position);
            if (dist <= least_distance)
            {
                target_pos = counter;
                standing_level = corridor;
                least_distance = dist;
            }

            counter++;
        }

        current_corridor = target_pos;
        for (int i = 0; i < 4; ++i) if(i != current_corridor) corridors[i].SetActive(false);

        load_next_level = true;
    }

    void ActiveDoors(RoomNode room, GameObject level)
    {
        GameObject plane = level.transform.GetChild(0).gameObject;
        string[] directions = new string[4]
        {
            "N","S", "E", "W",
        };
        int[] ids = new int[4]
        {
            room.room_id_north_,room.room_id_south_,room.room_id_east_,room.room_id_west_
        };

        for(int i = 0; i < 4; ++i)
        {
            bool dir_active = ids[i] == 0;
            plane.transform.Find(directions[i] + "Wall").gameObject.SetActive(dir_active);

            GameObject door = plane.transform.Find(directions[i] + "Door").gameObject;
            door.SetActive(!dir_active);

            int key_value = 0;
            if(room.direction_locked != null && room.direction_locked.TryGetValue((CardinalDirection)i, out key_value)){
                door.transform.GetChild(0).gameObject.GetComponent<DoorBehaviour>().RequiredKey = 
                    (PickableObject.KeyNumber)key_value;
                //if(corridor_by_dir[i] != null)
                //    corridor_by_dir[i].transform.GetChild(0).Find("SDoor").GetChild(0).gameObject.GetComponent<DoorBehaviour>().RequiredKey =
                //        (PickableObject.KeyNumber)key_value;
            }
            //else
            //{
            //    if (corridor_by_dir[i] != null)
            //        corridor_by_dir[i].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<DoorBehaviour>().RequiredKey =
            //            PickableObject.KeyNumber.kKeyNumberNone;
            //}


            if (door.transform.childCount > 0)
            {
                door.transform.GetChild(0).gameObject.SetActive(current_room_.id() == ids[i]);
            }
        }

    }

    void FirstLevel()
    {

        next_rooms_ = new List<RoomNode>();
        next_levels = new List<GameObject>();
        corridor_by_dir = new GameObject[4];

        current_room_ = rooms_[0];
        previous_room_ = current_room_;
        prefab_current_level = current_room_.room_prefab_;
        current_level = InstantiateCustomLevel(prefab_current_level, new Vector3());
        ActiveDoors(current_room_, current_level);

        //player_prefab_ = (GameObject)Resources.Load("Prefabs/FPSPLayer", typeof(GameObject));
        prefab_corridor_ = (GameObject)Resources.Load("Prefabs/Corridor", typeof(GameObject));

        corridors = new List<GameObject>();
        for (int i = 0; i < 4; ++i) {
            corridors.Add(Instantiate(prefab_corridor_, transform));
            corridors[i].transform.position = new Vector3(0.0f, -100.0f, 0.0f);
            corridors[i].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            corridors[i].SetActive(false);
        }

        //player_ = Instantiate(player_prefab_);
        player_.transform.position = current_level.transform.position + new Vector3(0.0f,3.0f,0.0f);
    }

    string RandomRoom()
    {
        return "Room" + Random.Range(1, 11).ToString();
    }

    int CreatePassageway(ref List<RoomNode> rooms_list, ref int id_counter, int first, CardinalDirection dir, int count)
    {
        List<RoomNode> passage = new List<RoomNode>();
        //Route 1
        for (int i = 0; i < count; ++i)
        {
            RoomNode room = new RoomNode(id_counter++);
            room.room_prefab_ = (GameObject)FindRoomType(RandomRoom());

            if (i > 0)
            {
                RoomNode prev_room = passage[i - 1];
                switch (dir)
                {
                    case CardinalDirection.North: prev_room.ConN(ref room); break;
                    case CardinalDirection.South: prev_room.ConS(ref room); break;
                    case CardinalDirection.East: prev_room.ConE(ref room); break;
                    case CardinalDirection.West: prev_room.ConW(ref room); break;
                }

                passage[i - 1] = prev_room;
            }
            else
            {
                RoomNode prev_room = rooms_list[first];
                switch (dir)
                {
                    case CardinalDirection.North: prev_room.ConN(ref room); break;
                    case CardinalDirection.South: prev_room.ConS(ref room); break;
                    case CardinalDirection.East: prev_room.ConE(ref room); break;
                    case CardinalDirection.West: prev_room.ConW(ref room); break;
                }
                rooms_list[first] = prev_room;
            }
            passage.Add(room);
        }

        int first_room = rooms_list.Count;
        foreach (RoomNode room in passage) rooms_list.Add(room);

        return first_room;
    }

    void LoadRooms()
    {
        rooms_ = new List<RoomNode>();
        room_prefabs_ = Resources.LoadAll("Prefabs/Rooms", typeof(GameObject));

        int id_counter = 1000;
        RoomNode starting_room  = new RoomNode(id_counter++);
        RoomNode first_room  = new RoomNode(id_counter++);
        RoomNode computer_room = new RoomNode(id_counter++);
        computer_room.room_prefab_ = (GameObject)FindRoomType("ComputerRoom");

        starting_room.room_prefab_ = (GameObject) FindRoomType("emptyRoom");
        first_room.room_prefab_ = starting_room.room_prefab_;


        first_room.ConS(ref starting_room);
        rooms_.Add(starting_room);
        rooms_.Add(first_room);

        int locked_count = CreatePassageway(ref rooms_, ref id_counter, 1, CardinalDirection.East, 3);
        RoomNode locked_room = rooms_[locked_count];
        locked_room.AddLockedDoor(CardinalDirection.West, 2);
        rooms_[locked_count] = locked_room;

        CreatePassageway(ref rooms_, ref id_counter, 1, CardinalDirection.North, 3);
        RoomNode last = rooms_[rooms_.Count - 1];
        last.room_prefab_ = (GameObject)FindRoomType("spawnerRoomKey1");
        computer_room.ConS(ref last);
        rooms_[rooms_.Count - 1] = last;

        int route_to_key_1 = CreatePassageway(ref rooms_, ref id_counter, 1, CardinalDirection.West, 1);

        RoomNode weapon_room = rooms_[route_to_key_1];
        weapon_room.AddLockedDoor(CardinalDirection.East, 1);
        weapon_room.room_prefab_ = (GameObject)FindRoomType("emptyRoom");
        CreatePassageway(ref rooms_, ref id_counter, route_to_key_1, CardinalDirection.North, 5);

        RoomNode armor_room = rooms_[rooms_.Count - 2]; armor_room.room_prefab_ = (GameObject)FindRoomType("spawnerRoomArmor");
        rooms_[rooms_.Count - 2] = armor_room;

        RoomNode key_room = rooms_[rooms_.Count - 1]; armor_room.room_prefab_ = (GameObject)FindRoomType("spawnerRoomKey2");
        computer_room.ConE(ref key_room);
        rooms_[rooms_.Count - 1] = armor_room;


        rooms_.Add(computer_room);







    }

    RoomNode FindRoom(int id)
    {
        foreach (RoomNode room in rooms_)
        {
            if (room.room_id_ == id) {
                return room;
            }
        }

        return new RoomNode();
    }

    Object FindRoomType(string name)
    {
        foreach (Object prefab in room_prefabs_)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }

        return room_prefabs_[0];
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadRooms();
        FirstLevel();
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update() {
        if (load_next_level) {
            LoadNextLevel();
        }

        if (next_levels.Count != 0 && entered_room)
        {

            if (Vector3.Distance(current_level.transform.position, player_.transform.position) >= room_size)
            {
                DestroyUnusedLevels();
                entered_room = false;
            }
        }

        if(next_levels.Count != 0 && 
            Vector3.Distance(current_level.transform.position, player_.transform.position) <= room_size * 0.5f)
        {
            entered_room = true;
        }

    }
}
