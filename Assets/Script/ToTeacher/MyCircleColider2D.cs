using System.Collections.Generic;
using UnityEngine;

public class MyCircleColider2D : MonoBehaviour
{
    [SerializeField] Vector2 _offset = Vector2.zero;
    public Vector2 Offset => _offset; 
    [SerializeField] float _radius = 0.5f;
    public float Radius => _radius * this.transform.lossyScale.x;
    static List<MyCircleColider2D> _listColiders = new List<MyCircleColider2D>(1024);

    bool _isColed = false;
    public bool IsColed => _isColed;
    bool _needCheckCol = true;

    private void Awake()
    {
        _listColiders.Add(this);
    }

    private void OnDestroy()
    {
        _listColiders.Remove(this);
    }

    void Update()
    {
        if (this._isColed) Destroy(this.gameObject);//テスト

        this._isColed = false;
        if (this._needCheckCol == false) { return; }
        var a = this;
        foreach (var b in _listColiders)
        {
            if (a == b){  continue; }//自分自身と比べるのを防ぐ

            Vector2 PositionA = (Vector2)a.transform.position + a.Offset;
            Vector2 PositionB = (Vector2)b.transform.position + b.Offset;

            float sqrtMagnitude = (PositionA - PositionB).sqrMagnitude;

            float AddictiveRadius = a.Radius + b.Radius;

            if (sqrtMagnitude <= AddictiveRadius * AddictiveRadius)
            {
                //当たっている場合
                _isColed = true;
                b.SetColed();//相手にも当たったことを知らせる。
            }
        }
    }

    private void LateUpdate()
    {
        _needCheckCol = true;

    }

    public void SetColed()
    {
        _isColed = true;
        _needCheckCol = false;
    }
}
