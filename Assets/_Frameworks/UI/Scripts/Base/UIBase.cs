using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UIOptions
{
    public bool isActiveOnLoad = true;
    public bool isDestroyOnHide = true;
}

public class UIBase : MonoBehaviour
{
    public eUIPosition uiPosition;
    public UIOptions uiOptions;

    public Animator uiAnim;

    public UnityAction<object[]> opened;
    public UnityAction<object[]> closed;

    private void Awake()
    {
        opened = Opened;
        closed = Closed;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public virtual void HideDirect() { }
    public virtual void Opened(object[] param) 
    {
        if (uiAnim != null) uiAnim.SetTrigger("OpenUI");

        var rectTf = GetComponent<RectTransform>();
        rectTf.localPosition = Vector3.zero;
        rectTf.localScale = Vector3.one;
        rectTf.offsetMin = Vector3.zero;
        rectTf.offsetMax = Vector3.zero;
    }

    public virtual void Closed(object[] param) 
    {
        if (uiAnim != null) uiAnim.SetTrigger("CloseUI");
    }
}