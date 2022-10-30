using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewLayer
{
    Main,
    Top
}

public class ViewManager : MonoBehaviourSingletonPersistent<ViewManager>
{
    [SerializeField] Transform main;
    [SerializeField] Transform top;

    public BaseView ChangeMain(string path)
    {
        if (main.childCount > 0)
        {
            var child = main.GetChild(0);
            Destroy(child.gameObject);
        }

        var viewPrefab = Resources.Load<GameObject>(path);
        if (viewPrefab != null)
        {
            var view = Instantiate(viewPrefab);
            view.transform.SetParent(main);
            return view.GetComponent<BaseView>();
        }
        else
        {
            Debug.LogError($"View prefab {path} is null");
            return null;
        }
    }

    public BaseView PushTop(string path)
    {
        var viewPrefab = Resources.Load<GameObject>(path);
        if (viewPrefab != null)
        {
            var view = Instantiate(viewPrefab);
            view.transform.SetParent(top);
            return view.GetComponent<BaseView>();
        }
        else
        {
            Debug.LogError($"View prefab {path} is null");
            return null;
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
