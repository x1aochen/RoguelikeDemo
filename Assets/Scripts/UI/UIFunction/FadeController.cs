using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeController : MonoBehaviour
    {
        
        private Image backImage;
        private float alpha;
        private void Awake()
        {
            backImage = GetComponent<Image>();
        }

        /// <summary>
        /// 淡出
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeOut()
        {
            alpha = 1;
            backImage.color = new Color(0, 0, 0, alpha);

            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                backImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            //栈顶弹出 过渡面板
            UIManager.instance.Pop();
        }

        /// <summary>
        /// 淡入
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeIn()
        {
            alpha = 0;
            backImage.color = new Color(0, 0, 0, alpha);
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                backImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            //通知更换角色位置
            EventManager.instance.Call(EventName.Transition);
        }

    }
}