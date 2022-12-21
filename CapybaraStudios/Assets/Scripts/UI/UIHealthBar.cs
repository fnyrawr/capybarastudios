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
    public void SetHealtBar(float percent) {
        //float width = parentWidth * percent;
        //foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        foregroundImage.rectTransform.localScale = new Vector3(-percent,1,1);
    }
}
