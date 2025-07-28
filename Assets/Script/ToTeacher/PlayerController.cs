using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _moveSpeed = 0.5f;

    [SerializeField]
    private SpriteRenderer _prefabBody = null;

    private List<SpriteRenderer> _listSprBody = new List<SpriteRenderer>();

    private Camera _mainCamera = null;

    [SerializeField]
    private Transform _eyeLeft;

    [SerializeField]
    private Transform _eyeRight;

    int _point = 1;

    [SerializeField] MyCircleColider2D _colider = null;

    List<Vector2> _lastPos = new List<Vector2>();

    void GrowUp(MyCircleColider2D target)
    {
        var f = target.GetComponent<FoodController>();
        if (f)
        {
            _point += (int)f?.FoodWeight;
            Destroy(f.gameObject);

            var sqr = Instantiate<SpriteRenderer>(_prefabBody, this.transform.position, Quaternion.identity);
            _listSprBody.Add(sqr);
            _lastPos.Add(this.transform.position);
            sqr.transform.SetParent(this.transform, false);
            sqr.transform.localScale = _prefabBody.transform.localScale;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        this.transform.localScale = Vector3.one * 0.5f;
        _point = 1;
        _colider.SetCallback(GrowUp);

        //頭の位置を更新する変数をリストに追加
        _lastPos.Add(this.transform.position);
        //体の位置を更新する変数をリストに追加
        _listSprBody.Add(_prefabBody);
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

        for (int i = _lastPos.Count - 1; i >= 0; --i)
        {
            if (i == 0)
            {
                _lastPos[i] = this.transform.position;
            }
            else
            {
                _lastPos[i] = _lastPos[i - 1];
            }
        }

        for (int i = 0; i < _listSprBody.Count; i++)
        {
            var spr = _listSprBody[i];
            spr.transform.position = _lastPos[i];

            spr.sortingOrder = _listSprBody.Count - i + 1;
        }
    }

    private void LateUpdate()
    {
        _mainCamera.transform.position = this.transform.position;
    }
}
