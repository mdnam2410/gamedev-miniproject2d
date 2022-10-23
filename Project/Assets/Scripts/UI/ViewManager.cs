using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewLayer
{
    Main,
    Top
}

public class ViewManager : MonoBehaviour
{
    private ViewManager() { }
    private static ViewManager _instance;
    public static ViewManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ViewManager();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] Transform main;
    [SerializeField] Transform top;

    public void ChangeMain(string path)
    {
        if (main.childCount > 0)
        {
            var child = top.GetChild(0);
            Destroy(child);
        }
        var viewPrefab = Resources.Load<GameObject>(path);
        if (viewPrefab != null)
        {
            var view = Instantiate(viewPrefab);
            view.transform.SetParent(main);
        }
        else
        {
            Debug.LogError($"View prefab {path} is null");
        }
    }

    public void PushTop(string path)
    {
        var viewPrefab = Resources.Load<GameObject>(path);
        if (viewPrefab != null)
        {
            var view = Instantiate(viewPrefab);
            view.transform.SetParent(top);
        }
        else
        {
            Debug.LogError($"View prefab {path} is null");
        }
    }

    public void PopTop()
    {
        if (top.childCount > 0)
        {
            var child = top.GetChild(0);
            Destroy(child.gameObject);
        }
    }
}
