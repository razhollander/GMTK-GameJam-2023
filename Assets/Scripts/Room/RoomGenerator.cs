using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Room _room;
    [SerializeField] private Vector2Int _numOfRooms = new Vector2Int(3,3);
    [SerializeField] private float minusDelta =0.7f;
    
    [ContextMenu("Generate rooms")]
    public void GenerateRooms()
    {
        GameObject roomsParent = gameObject;//new GameObject("RoomsParent");
        
        var roomSize = _room.RoomSprite.bounds.size.x;

        for (int i = _numOfRooms.x-1; i >=0; i--)
        {
            for (int j = _numOfRooms.y-1; j >= 0 ; j--)
            {
                var room = Instantiate(_room.gameObject, roomsParent.transform);
                room.GetComponent<Room>().Id = new Vector2Int(i, j);
                room.name = "Room_" + room.GetComponent<Room>().Id.x+"_"+room.GetComponent<Room>().Id.y;
                room.transform.localPosition = new Vector3(i * roomSize - (i*minusDelta), j * roomSize - (j*minusDelta));
                room.GetComponent<Room>().SetWalls();
            }
        }
    }
    
    [ContextMenu("Generate things in rooms")]
    public void GenerateThingsInRooms()
    {
        var rooms = GetComponentsInChildren<Room>();
        Shuffle(rooms);
        var enemiesByRooms = new Dictionary<Vector2Int, List<Enemy>>();

        for (int i = 0; i < rooms.Length; i++)
        {
            var templates = rooms[i].GetRoomTemplates();
            
            enemiesByRooms.Add(rooms[i].Id, templates.Select(x=>x.EnemyInTemplate).ToList());
        }

        var chosenEnemyForEachRoom = new Dictionary<Vector2Int, Enemy>();
        while (chosenEnemyForEachRoom.Count==0 || IsHigherThan2(chosenEnemyForEachRoom))
        {
            chosenEnemyForEachRoom.Clear();
            foreach (var enemies in enemiesByRooms)
            {
                var chosenEnemyForRoom = Random.Range(0, enemies.Value.Count);
                chosenEnemyForEachRoom.Add(enemies.Key, enemies.Value[chosenEnemyForRoom]);
            }
        }

        foreach (var enemy in chosenEnemyForEachRoom)
        {
            Debug.Log("CHOSEN! room: "+enemy.Key + ", enemy: "+enemy.Value);
        }
        
        
        
        //var enemies = new List<Enemy>();
        //var numOfEnemies = new Dictionary<Enemy, int>();
//
        //foreach (var e in Enum.GetValues(typeof(Enemy)))
        //{
        //    enemies.Add((Enemy)e);
        //}
        //
        //Shuffle(enemies);
        //int 
        
    }

    public bool IsHigherThan2(Dictionary<Vector2Int, Enemy> dict)
    {
        var counter = new Dictionary<Enemy, int>();

        foreach (var e in Enum.GetValues(typeof(Enemy)))
        {
            counter.Add((Enemy)e,0);
        }
        
        foreach (var i in dict)
        {
            counter[i.Value]++;
            Debug.Log(counter[i.Value]);
            if (counter[i.Value] > 2)
                return true;
        }

        return false;
    }
    public void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}
