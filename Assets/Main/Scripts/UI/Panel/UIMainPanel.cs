using Main.Scripts.UI.Base;
using QFramework;

namespace Main.Scripts.UI.Panel
{
    public class UIMainPanelData : UIPanelData
    {
    }

    public partial class UIMainPanel : UIEnhancedInputPanel
    {
        #region InputLifecycle

        protected override void OnBindInput()
        {
        }

        protected override void OnUnbindInput()
        {
        }

        #endregion

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIMainPanelData ?? new UIMainPanelData();
        }
    }
}