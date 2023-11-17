using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private GameObject npc;
    public GameObject npcRespawn;
    // Start is called before the first frame update
    void Start()
    {
        npc = GameObject.FindGameObjectWithTag("NPC");
    }

    // Update is called once per frame
    void Update()
    {
        if(npc == null) npc = Instantiate(npcRespawn, transform.position, Quaternion.identity);
    }
}
