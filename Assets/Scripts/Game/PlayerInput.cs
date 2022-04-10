using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Ввод от кнпок A и D")]
    [SerializeField] private bool DebugInput = true;
    
    public event FloatEvent HorizontalMove;
    private float _moveX = 0;
    private void Update()
    {
        
        _moveX = 0;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if(Input.GetKey(KeyCode.A))
            {
                _moveX = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _moveX = 1;
            }
        }
        else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _moveX = Input.acceleration.x;
        }
        HorizontalMove?.Invoke(_moveX);
        
    }
}
