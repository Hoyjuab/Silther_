using Unity.VisualScripting;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public Transform networkOwner;
    public Transform followTransform;

    [SerializeField] private float deleyTime = 0.01f;
    [SerializeField] private float distance = 0f;
    [SerializeField] private float moveStep = 20f;

    private Vector3 _targetPosition;

    private void Update()
    {
        _targetPosition = followTransform.position - followTransform.up * distance;
        _targetPosition += (transform.position - _targetPosition) * deleyTime;
        _targetPosition.z = 0f;

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * moveStep);

        Vector3 dir = _targetPosition - transform.position;
        if (dir != Vector3.zero)
        {
            transform.up = Vector3.Lerp(transform.up, dir.normalized, Time.deltaTime * moveStep);
        }
    }
}
