using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform Player;
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.position, .1f);
    }
}
