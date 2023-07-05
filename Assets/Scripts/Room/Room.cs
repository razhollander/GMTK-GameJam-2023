using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Id;
    public SpriteRenderer RoomSprite;

    [SerializeField] private GameObject leftwall_with_door;
    [SerializeField] private GameObject leftwall_with_NO_door;

    [SerializeField] private GameObject topwall_with_door;
    [SerializeField] private GameObject topwall_with_NO_door;

    [SerializeField] private GameObject rightwall_with_door;
    [SerializeField] private GameObject rightwall_with_NO_door;
    
    [SerializeField] private GameObject bottomwall_with_door;
    [SerializeField] private GameObject bottomwall_with_NO_door;

    // Start is called before the first frame update
    public RoomTemplate[] GetRoomTemplates()
    {
        return GetComponentsInChildren<RoomTemplate>();
    }

    public void SetWalls()
    {
        if (Id.x == 0)
        {
            leftwall_with_door.SetActive(false);
            leftwall_with_NO_door.SetActive(true);
        }
        
        if (Id.x == 2)
        {
            rightwall_with_door.SetActive(false);
            rightwall_with_NO_door.SetActive(true);
        }
        
        if (Id.y == 0)
        {
            bottomwall_with_door.SetActive(false);
            bottomwall_with_NO_door.SetActive(true);
        }
        
        if (Id.y == 2)
        {
            topwall_with_door.SetActive(false);
            topwall_with_NO_door.SetActive(true);
        }
    }
}
