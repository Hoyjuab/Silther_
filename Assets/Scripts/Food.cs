using Unity.Netcode;
using UnityEngine;

public class Food : NetworkBehaviour
{

    public GameObject prefab;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!NetworkManager.Singleton.IsServer) return;

        if (prefab == null) return;

        if (col.TryGetComponent(out PlayerLength playerLength))
            playerLength.AddLength();
        else if (col.TryGetComponent(out Tail tail))
            tail.networkOwner.GetComponent<PlayerLength>().AddLength();

        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}
