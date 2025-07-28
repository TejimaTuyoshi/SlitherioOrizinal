using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _moveSpeed = 0.5f;

    [SerializeField]
    private SpriteRenderer _prefabBody = null;

    private List<SpriteRenderer> _listSprBody = null;

    private Camera _mainCamera = null;

    [SerializeField]
    private Transform _eyeLeft;

    [SerializeField]
    private Transform _eyeRight;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        this.transform.localScale = Vector3.one * 0.5f;

        for (int i = 0; i < 10; i++)
        {

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouseScreenPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = mouseScreenPos - this.transform.position;

        float angle = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg;
        Quaternion qTarget = Quaternion.Euler(0f, 0f, angle);
        _eyeLeft.transform.rotation = qTarget;
        _eyeRight.transform.rotation = qTarget;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qTarget, 0.05f);

        //360度の方向を取得
        float moveAngleDegree = this.transform.rotation.eulerAngles.z;
        //一周360度から一周2πに変換
        float moveAngleRadius = Mathf.Deg2Rad * moveAngleDegree;
        //向いている方向の単位ベクトルを取得
        float nomalX = Mathf.Cos(moveAngleRadius);
        float nomalY = Mathf.Sin(moveAngleRadius);

        // 移動
        Vector3 move = new Vector3(nomalX,nomalY,0f) * _moveSpeed;
        this.transform.position += move;
    }

    private void LateUpdate()
    {
        _mainCamera.transform.position = this.transform.position;
    }
}
