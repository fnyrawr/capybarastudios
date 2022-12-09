using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Image foregroundImage, backgroundImage;
    private float parentWidth;
    void Awake() {
        parentWidth = GetComponent<RectTransform>().rect.width;
    }

    void LateUpdate()
    {
        Vector3 dir = (target.position - Camera.main.transform.position).normalized;
        bool show = Vector3.Dot(dir, Camera.main.transform.forward) <= 0f;
        foregroundImage.enabled = !show;
        backgroundImage.enabled = !show;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealtBar(float percent) {
        float width = parentWidth * percent;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
