using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WorldAIManager : Singleton<WorldAIManager>
{
    [Header("Character")] 
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<GameObject> spawnedInCharacters;

    
    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }
        
    }

    /*
    private void SpawnAllCharacter()
    {
        foreach(var character in aiCharacterSpawners)
        {
            character.AttemptToSpawnCharacter();
        }
    }
    */
    private void DespawnAllCharacter()
    {
        foreach (var character in spawnedInCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    private void DisableAllCharacter()
    {
        foreach (var character in spawnedInCharacters)
        {
            character.SetActive(false);
        }
    }
}
