using UnityEngine;

public class GuardBob : MonoBehaviour
{
    [SerializeField] private float bopHeight = 0.1f;
    [SerializeField] private float bopSpeed = 2f;

    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * bopSpeed) * bopHeight;
        transform.localPosition = startLocalPosition + new Vector3(0f, yOffset, 0f);
    }
}