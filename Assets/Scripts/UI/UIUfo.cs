using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIUfo : MonoBehaviour
{
    [SerializeField] private Image _ufoLight;
    [SerializeField] private float _lightSpeed;

    private void OnEnable()
    {
        PlayLightAnim();
    }

    private void PlayLightAnim()
    {
        float alpha = _ufoLight.color.a;
        Color clr = _ufoLight.color;
        if (alpha==0)
        {
            clr.a = 1f;
        }
        else
        {
            clr.a = 0f;
        }
        _ufoLight.DOColor(clr, _lightSpeed).SetEase(Ease.Linear).OnComplete(PlayLightAnim);
    }
}
