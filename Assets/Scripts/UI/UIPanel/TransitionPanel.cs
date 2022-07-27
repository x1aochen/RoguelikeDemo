using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 淡入淡出效果在过场面板调用的原因：
/// 将来可能会有的需求：过场读条、过场立绘、过场增加可供玩家选择的新状态Buff
/// </summary>
namespace UI
{
    public class TransitionPanel : BasePanel
    {
        private FadeController fade;

        private const string preName = "FadePanel";
        public TransitionPanel() : base(preName)
        {
        }

        public override void OnStart()
        {
            base.OnStart();
            fade = CodeHelper.FindChild(uiObj.transform, "Effect").GetComponent<FadeController>();
        }

        public void StartFadeIn()
        {
            fade.StartCoroutine(fade.FadeIn());
        }

        public void StartFadeOut()
        {
            fade.StartCoroutine(fade.FadeOut());
        }

        public override void OnDisable()
        {
            base.OnDisable();
            fade.StopAllCoroutines();
        }
    }
}
