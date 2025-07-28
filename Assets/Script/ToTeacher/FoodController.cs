using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodController : MonoBehaviour
{
    public enum E_FoodSizeType
    {
        None = 0,
        Small = 1,
        Middle = 2,
        Large = 3,
    }

    [SerializeField]
    private SpriteRenderer _sprFood;
    [SerializeField]
    private SpriteRenderer _sprLight;

    private float _foodWeight = 0.5f;
    public float FoodWeight => _foodWeight;

    private E_FoodSizeType _foodType = E_FoodSizeType.None;
    public float FoodSize => (float)_foodType * 0.125f;

    [SerializeField]
    private float _popFadeAnimationTimeSec = 0.5f;
    private float _popFadeAnimationFrameTimeSec = 0f;
    private bool _popFaded = false;

    [SerializeField]
    [Range(-0.125f, 0.125f)]
    private float _moveAnimationRadius = 0.125f;

    [SerializeField]
    [Range(1.5f, 5f)]
    private float _moveAnimationFrameTimeSpan = 1.5f;

    private float _moveAnimationFrameTimeSec = 0f;

    private Vector2 defPosition = Vector2.zero;

    private void Start()
    {
        defPosition = this.transform.position;
        this.transform.localScale = Vector3.zero;
        _moveAnimationRadius = UnityEngine.Random.Range(-0.125f, 0.125f);
        _moveAnimationFrameTimeSpan = UnityEngine.Random.Range(5f, 10f);
    }

    private void Update()
    {
        _moveAnimationFrameTimeSec += Time.deltaTime;

        if (_moveAnimationFrameTimeSec > _moveAnimationFrameTimeSpan)
        {
            _moveAnimationFrameTimeSec -= _moveAnimationFrameTimeSpan;
        }

        float per = _moveAnimationFrameTimeSec / _moveAnimationFrameTimeSpan;

        // 光のつよさ　COSINE-WAVEをフェード関数として利用
        float a = Mathf.Cos((per * 2f - 1f) * Mathf.PI + 1f) * 0.5f;

        var c = _sprLight.color;
        c.a = a;
        _sprLight.color = c;

        // 新規Pop時のアニメーション　colorのAlphaで実装だと面倒なのでScaleで実装
        if (_popFaded == false)
        {
            _popFadeAnimationFrameTimeSec += Time.deltaTime;

            if (_popFadeAnimationFrameTimeSec < _popFadeAnimationTimeSec)
            {
                this.transform.localScale
                 = FoodSize * Vector3.one * _popFadeAnimationFrameTimeSec / _popFadeAnimationTimeSec;
            }
            else
            {
                this.transform.localScale = FoodSize * Vector3.one;
                _popFaded = true;
            }
        }

        // 位置を微妙に移動
        float thetaPer = per * 2f * Mathf.PI;
        this.transform.position = defPosition
                                + new Vector2(Mathf.Cos(thetaPer) * _moveAnimationRadius
                                            , Mathf.Sin(thetaPer) * _moveAnimationRadius);
    }

    private void OnDestroy()
    {
        FieldFoodPopper.FoodAted();
    }

    public void SetRandomWeight()
    {
        SetWeight((FoodController.E_FoodSizeType)UnityEngine.Random.Range(1, 3));
    }

    public void SetWeight(E_FoodSizeType type)
    {
        _foodType = type;

        switch (type)
        {
            case E_FoodSizeType.Small:
                _foodWeight = 0.5f;
                break;
            case E_FoodSizeType.Middle:
                _foodWeight = 2f;
                break;
            case E_FoodSizeType.Large:
                _foodWeight = 4f;
                break;
        }
    }

    public void SetRandomColor()
    {
        SetColor(new Color(UnityEngine.Random.Range(0.5f, 1f)
                            , UnityEngine.Random.Range(0.5f, 1f)
                            , UnityEngine.Random.Range(0.5f, 1f)));
    }

    public void SetColor(Color color)
    {
        _sprFood.color = color;
        _sprLight.color = color;
    }
}
