using CodeFighter.UI.Base;
using QFramework;

namespace Main.Scripts.UI.Panel
{
    public class UIMainPanelData : UIPanelData
    {
    }

    public partial class UIMainPanel : UIEnhancedPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIMainPanelData ?? new UIMainPanelData();
        }
    }
}
