using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLength : NetworkBehaviour
{
    [SerializeField] private GameObject tailPerfab;

    public NetworkVariable<ushort> Length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public static event System.Action<ushort> ChangedLengthEvent;

    private List<GameObject> _tails;
    private Transform _lastTail;
    private Collider2D _collider2D;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _tails = new List<GameObject>();
        _lastTail = transform;
        _collider2D = GetComponent<Collider2D>();
        if (!IsServer)Length.OnValueChanged += LengthChangedEvent;
    }

    [ContextMenu("Add Length")] 
    public void AddLength()
    {
        Length.Value += 1;
        LengthChanged();
    }

    private void LengthChanged()
    {
       //InstantiateTail();

        if (!IsOwner) return;
        ChangedLengthEvent?.Invoke(Length.Value);
    }

    private void LengthChangedEvent(ushort previousValue, ushort newValue)
    {
        Debug.Log("LengthChanged Callback");
        LengthChanged();
    }

    private void InsantiateTail()
    {
        GameObject tailGameObject = Instantiate(tailPerfab, transform.position, Quaternion.identity);
        tailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -Length.Value;
        if (tailGameObject.TryGetComponent(out Tail tail))
        {
            tail.networkOwner = transform;
            tail.followTransform = _lastTail;
            _lastTail = tailGameObject.transform;
            Physics2D.IgnoreCollision(tailGameObject.GetComponent<Collider2D>(), _collider2D);
        }
        _tails.Add(tailGameObject);
    }
}
