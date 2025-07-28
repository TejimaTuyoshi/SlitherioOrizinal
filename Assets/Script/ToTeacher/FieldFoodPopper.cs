using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldFoodPopper : MonoBehaviour
{
    [SerializeField]
    private float _popRadius = 10f;

    [SerializeField]
    const int MAX_FOOD_NUMBER = 250;

    [SerializeField]
    private static int _foodCount = 0;

    [SerializeField]
    private float _popTimeSec = 1f;

    private float _popFrameTimeSec = 0f;

    [SerializeField]
    private FoodController _prefabFood;

    private void Start()
    {
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
    }

    public static void FoodAted()
    {
        _foodCount--;
    }

    // Update is called once per frame
    void Update()
    {
        _popFrameTimeSec += Time.deltaTime;
        if (_popFrameTimeSec > _popTimeSec)
        {
            _popFrameTimeSec -= _popTimeSec;

            if (MAX_FOOD_NUMBER <= _foodCount) return;

            float popRadius = UnityEngine.Random.Range(0f, _popRadius);
            float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float popX = Mathf.Cos(theta) * popRadius;
            float popY = Mathf.Sin(theta) * popRadius;
            FoodController food = Instantiate(_prefabFood, new Vector2(popX, popY), Quaternion.identity);
            food.SetRandomWeight();
            food.SetRandomColor();
            food.transform.localScale = food.FoodSize * Vector3.one;

            _foodCount++;
        }
    }
}
