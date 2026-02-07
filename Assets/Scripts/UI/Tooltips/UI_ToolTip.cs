using System.Data;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (show == false)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetRect); 
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0;

        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float veritcalHalf = rect.sizeDelta.y / 2f;
        float topY = targetPosition.y + veritcalHalf;
        float bottomY = targetPosition.y - veritcalHalf;

        if (topY > screenTop)
            targetPosition.y = screenTop - veritcalHalf - offset.y;
        else if(bottomY < screenBottom)
            targetPosition.y = screenBottom + veritcalHalf + offset.y;


        rect.position = targetPosition;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}
