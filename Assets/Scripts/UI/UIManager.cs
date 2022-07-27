using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Stack<BasePanel> uiStack;
        private Dictionary<string, GameObject> uiDic;

        private Transform canvas;
        private const string path = "UIPanel/";

        public override void Init()
        {
            canvas = GameObject.Find("Canvas").transform;
            uiStack = new Stack<BasePanel>();
            uiDic = new Dictionary<string, GameObject>();
        }
        private GameObject LoadPrefab(BasePanel basePanel)
        {
            if (uiDic.ContainsKey(basePanel.name))
                return uiDic[basePanel.name];

            GameObject uiPanel = Resources.Load<GameObject>(path + basePanel.name);

            if (uiPanel == null)
            {
                Debug.LogError("δ�ҵ�UI�����Դ");
                return null;
            }

            GameObject uiGo = GameObject.Instantiate<GameObject>(uiPanel, canvas.transform);
            uiDic.Add(basePanel.name, uiGo);
            basePanel.uiObj = uiGo;
            basePanel.OnStart();
            return uiGo;
        }

        public void Push(BasePanel basePanel)
        {
            if (uiStack.Count > 0)
            {
                uiStack.Peek().OnDisable();
            }
            //��һ�μ������
            GameObject uiObj = LoadPrefab(basePanel);

            uiObj.SetActive(true);

            //��ջ
            if (uiStack.Count == 0)
            {
                uiStack.Push(basePanel);
            }
            else
            {
                //�Է�������ջ
                if (uiStack.Peek().name != basePanel.name)
                {
                    uiStack.Push(basePanel);
                }

            }

            basePanel.OnEnable();
        }
        
        public void Pop()
        {
            if (uiStack.Count == 0) return;
            uiStack.Peek().OnDisable();
            uiDic[uiStack.Peek().name].SetActive(false);
            uiStack.Pop();
        }
    }
}
