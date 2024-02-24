using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class NetworkObjectSpawner : MonoBehaviour
{
    [FormerlySerializedAs("characterGameObject")]
    [Header("Object")] 
    [SerializeField] private GameObject networkGameObject;
    [SerializeField] private GameObject instantiatedGameObject;

    private void Awake()
    {
        
    }

    private void Start()
    {
        WorldObjectManager.Instance.SpawnObject(this);
        gameObject.SetActive(false);
    }
    
    public void AttemptToSpawnCharacter()
    {
        if (networkGameObject != null)
        {
            
            instantiatedGameObject = Instantiate(networkGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();

        }
    }
}
