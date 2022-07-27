using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class BasePanel
    {
        public string name;
        public GameObject uiObj;

        public BasePanel(string name)
        {
            this.name = name;
        }

        public virtual void OnStart()
        {
            CodeHelper.AddOrGetComponent<CanvasGroup>(uiObj);
        }

        public virtual void OnEnable()
        {
            CodeHelper.AddOrGetComponent<CanvasGroup>(uiObj).interactable = true;
        }
        public virtual void OnDisable()
        {
            CodeHelper.AddOrGetComponent<CanvasGroup>(uiObj).interactable = false;
        }
    }
}