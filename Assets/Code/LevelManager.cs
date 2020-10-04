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
    }

    List<RoomNode> rooms_;
    RoomNode current_room_;
    RoomNode previous_room_;
    List<RoomNode> next_rooms_;

    float room_size = 45.0f;
    float enemy_value = 0.0f;
    float box_value = 0.0f;

    GameObject map_empty_object;
    GameObject level_floor;

    public GameObject player_ = null;
    GameObject current_level = null;
    List<GameObject> next_levels;

    GameObject prefab_current_level = null;
    GameObject prefab_next_level = null;
    GameObject player_prefab_ = null;
    Object[] room_prefabs_;
    ResourceRequest req_next_level_load;

    bool finished_room = true;
    bool should_open_doors = false;

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
            new_level.transform.localPosition = current_level.transform.localPosition + relative_pos;

        new_level.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        new_level.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        return new_level;
    }

    void LoadNextLevel()
    {

        List<int> next_ids = new List<int>();
        List<Vector3> relative_positions = new List<Vector3>();

        if(current_room_.room_id_north_ != 0 && current_room_.room_id_north_ != previous_room_.id()) {
            next_ids.Add(current_room_.room_id_north_);
            relative_positions.Add(new Vector3(0.0f, 0.0f, room_size));
        }
        if (current_room_.room_id_south_ != 0 && current_room_.room_id_south_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_south_);
            relative_positions.Add(new Vector3(0.0f, 0.0f, -room_size));
        }
        if (current_room_.room_id_east_ != 0 && current_room_.room_id_east_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_east_);
            relative_positions.Add(new Vector3(room_size, 0.0f, 0.0f));
        }
        if (current_room_.room_id_west_ != 0 && current_room_.room_id_west_ != previous_room_.id())
        {
            next_ids.Add(current_room_.room_id_west_);
            relative_positions.Add(new Vector3(-room_size, 0.0f, 0.0f));
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

        should_open_doors = true;

        return;
    }

    void DestroyUnusedLevels()
    {
        GameObject standing_level = null;
        int counter = 0;
        foreach (GameObject level in next_levels)
        {
            if (Vector3.Distance(level.transform.position, player_.transform.position) <= room_size*0.5f)
            {
                standing_level = level;
            }
            else
            {
                Destroy(level);
            }

            if (standing_level == null) counter++;
        }

        next_levels.Clear();

        Destroy(current_level, 0.5f);
        current_level = standing_level;

        previous_room_ = current_room_;
        current_room_ = next_rooms_[counter];
        next_rooms_.Clear();

        finished_room = true;
    }

    void ActiveDoors(RoomNode room, GameObject level)
    {
        GameObject plane = level.transform.GetChild(0).gameObject;

        bool dir_active = room.room_id_north_ == 0;
        plane.transform.Find("NWall").gameObject.SetActive(dir_active);
        plane.transform.Find("NDoor").gameObject.SetActive(!dir_active);

        dir_active = room.room_id_south_ == 0;
        plane.transform.Find("SWall").gameObject.SetActive(dir_active);
        plane.transform.Find("SDoor").gameObject.SetActive(!dir_active);

        dir_active = room.room_id_east_ == 0;
        plane.transform.Find("EWall").gameObject.SetActive(dir_active);
        plane.transform.Find("EDoor").gameObject.SetActive(!dir_active);

        dir_active = room.room_id_west_ == 0;
        plane.transform.Find("WWall").gameObject.SetActive(dir_active);
        plane.transform.Find("WDoor").gameObject.SetActive(!dir_active);
    }

    void FirstLevel()
    {

        next_rooms_ = new List<RoomNode>();
        next_levels = new List<GameObject>();

        current_room_ = rooms_[0];
        previous_room_ = current_room_;
        prefab_current_level = current_room_.room_prefab_;
        current_level = InstantiateCustomLevel(prefab_current_level, new Vector3());
        ActiveDoors(current_room_, current_level);

        //player_prefab_ = (GameObject)Resources.Load("Prefabs/FPSPLayer", typeof(GameObject));

        //player_ = Instantiate(player_prefab_);
        player_.transform.position = current_level.transform.position + new Vector3(0.0f,10.0f,0.0f);
    }

    void LoadRooms()
    {
        room_prefabs_ = Resources.LoadAll("Prefabs/Rooms", typeof(GameObject));

        int id_counter = 1000;
        RoomNode first_room  = new RoomNode(id_counter++);
        RoomNode second_room = new RoomNode(id_counter++);
        RoomNode third_room  = new RoomNode(id_counter++);
        RoomNode fourth_room = new RoomNode(id_counter++);
        RoomNode fifth_room = new RoomNode(id_counter++);

        first_room.room_prefab_ = (GameObject) FindRoomType("emptyRoom");
        second_room.room_prefab_ = (GameObject)FindRoomType("Room3");
        third_room.room_prefab_ = (GameObject) FindRoomType("Room5");
        fourth_room.room_prefab_ = (GameObject)FindRoomType("emptyRoom");
        fifth_room.room_prefab_ = (GameObject)FindRoomType("emptyRoom");

        first_room.ConN(ref second_room);
        first_room.ConS(ref third_room);
        first_room.ConE(ref fourth_room);
        first_room.ConW(ref fifth_room);

        rooms_ = new List<RoomNode>();
        rooms_.Add(first_room);
        rooms_.Add(third_room);
        rooms_.Add(fourth_room);
        rooms_.Add(fifth_room);
        rooms_.Add(second_room);

        RoomNode previous_room = rooms_[rooms_.Count - 1];

        //First Passageway
        for (int i = 0; i < 2; ++i)
        {
            RoomNode new_room = new RoomNode(id_counter++);

            if(i == 1) new_room.room_prefab_ = (GameObject)FindRoomType("ComputerRoom");
            else new_room.room_prefab_ = (GameObject)FindRoomType("emptyRoom");

            new_room.ConS(ref previous_room);

            rooms_[rooms_.Count - 1] = previous_room;

            rooms_.Add(new_room);
            previous_room = new_room;
        }
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
        if (finished_room && next_levels.Count == 0) {
            LoadNextLevel();
            finished_room = false;
        }

        if (next_levels.Count != 0)
        {

            if (Vector3.Distance(current_level.transform.position, player_.transform.position) >= room_size)
            {
                DestroyUnusedLevels();
            }
        }
    }
}
