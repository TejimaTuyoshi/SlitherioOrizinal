using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameObject _player;
    Vector3 _mousePos;
    Transform _rotate;
    void Start()
    {
        _player = this.gameObject;
        _rotate = this.gameObject.transform;
    }

    void Update()
    {
        _mousePos = Input.mousePosition;
        _rotate = _player.transform;
        _rotate.rotation = Quaternion.Euler(0,0,_mousePos.z);
    }
}
