using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")] 
    [SerializeField] private GameObject characterGameObject;
    [SerializeField] private GameObject instantiatedGameObject;

    private void Awake()
    {
        
    }

    private void Start()
    {
        WorldAIManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }
    
    public void AttemptToSpawnCharacter()
    {
        if (characterGameObject != null)
        {
            
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();

        }
    }
}
