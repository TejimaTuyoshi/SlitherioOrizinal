using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.5f;

    [SerializeField] private SpriteRenderer _prefabBody = null;
    private Camera _mainCamera = null;
    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 mouseScreenPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = mouseScreenPos - this.transform.position;
        float angle = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg;
        Quaternion qTarget = Quaternion.Euler(0f, 0f, angle);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qTarget, 0.05f);

        float moveAngleDegree = this.transform.rotation.eulerAngles.z;
        float moveAngleRadius = Mathf.Deg2Rad * moveAngleDegree;
        float nomalX = Mathf.Cos(moveAngleRadius);
        float nomalY = Mathf.Sin(moveAngleRadius);

        Vector3 move = new Vector3(nomalX, nomalY, 0f) * _moveSpeed;
        this.transform.position += move;
    }
    private void LateUpdate()
    {
        _mainCamera.transform.position = this.transform.position;
    }
}
