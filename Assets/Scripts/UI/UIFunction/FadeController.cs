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
        /// ����
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
            //ջ������ �������
            UIManager.instance.Pop();
        }

        /// <summary>
        /// ����
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
            //֪ͨ������ɫλ��
            EventManager.instance.Call(EventName.Transition);
        }

    }
}