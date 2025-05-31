using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum eUIPosition
{
    UI,
    Popup,
    Navigator
}

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private List<Transform> parents;

    private Dictionary<System.Type, GameObject> openUI = new Dictionary<System.Type, GameObject>();
    private Dictionary<System.Type, GameObject> closeUI = new Dictionary<System.Type, GameObject>();

    public UIBase FrontUI;

    protected override void Init()
    {
        base.Init();

        if (parents == null) parents = new List<Transform>();
        if (parents.Count == 0)
        {
            List<Transform> trList = new List<Transform>()
            {
                Instantiate(ResourceManager.Instance.LoadAsset<GameObject>("UI/@UI")).transform,
                Instantiate(ResourceManager.Instance.LoadAsset<GameObject>("UI/@Popup")).transform,
                Instantiate(ResourceManager.Instance.LoadAsset<GameObject>("UI/@Navigation")).transform
            };
            SetParents(trList);
            foreach (var tr in trList) DontDestroyOnLoad(tr.gameObject);
        }
    }

    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
    }

    public T Show<T>(params object[] param) where T : UIBase
    {
        System.Type type = typeof(T);

        bool isOpen = false;
        var ui = Get<T>(out isOpen);

        if (ui == null)
        {
            Logger.ErrorLog($"{type} does not exit.");
            return null;
        }

        if (isOpen)
        {
            Logger.ErrorLog($"{type} is already open.");
            return ui;
        }

        var siblingIndex = parents[(int)ui.uiPosition].childCount;
        ui.gameObject.transform.parent = parents[(int)ui.uiPosition];
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.opened?.Invoke(param);
        ui.SetActive(ui.uiOptions.isActiveOnLoad);
        ui.uiOptions.isActiveOnLoad = true;

        FrontUI = ui;
        openUI[type] = ui.gameObject;

        return (T)ui;
    }

    public void Hide<T>(params object[] param) where T : UIBase
    {
        System.Type type = typeof(T);

        bool isOpen = false;
        var ui = Get<T>(out isOpen);
        eUIPosition up = ui.uiPosition;

        if (isOpen)
        {
            openUI.Remove(type);
            ui.closed.Invoke(param);

            if (ui.uiOptions.isDestroyOnHide)
            {
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
                closeUI[type] = ui.gameObject;
                ui.transform.SetAsFirstSibling();
            }
            
            FrontUI = null;
            var lastChild = parents[(int)up].GetChild(parents[(int)up].childCount - 1);
            if (lastChild)
            {
                UIBase baseUI = lastChild.gameObject.GetComponent<UIBase>();
                FrontUI = baseUI.gameObject.activeInHierarchy ? baseUI : null;
            }
        }
    }


    public T Get<T>(out bool isOpened) where T : UIBase
    {
        System.Type type = typeof(T);

        UIBase ui = null;
        isOpened = false;

        if (openUI.ContainsKey(type))
        {
            ui = openUI[type].GetComponent<UIBase>();
            isOpened = true;
        }
        else if (closeUI.ContainsKey(type))
        {
            ui = closeUI[type].GetComponent<UIBase>();
            closeUI.Remove(type);
        }
        else
        {
            var prefab = Resources.Load("UI/" + typeof(T).ToString()) as GameObject;
            GameObject go = Instantiate(prefab);
            ui = go.GetComponent<UIBase>();
        }

        return (T)ui;
    }

    public UIBase IsOpened<T>() where T : UIBase
    {
        var uiType = typeof(T);
        return openUI.ContainsKey(uiType) ? openUI[uiType].GetComponent<UIBase>() : null;
    }

    public bool ExistOpenUI()
    {
        return FrontUI != null;
    }

    public UIBase GetCurrentFrontUI()
    {
        return FrontUI;
    }

    public static void ShowIndicator()
    {

    }

    public static void HideIndicator()
    {

    }
}