using Main.Scripts.UI.Base;
using QFramework;
using UnityEngine.InputSystem;

namespace Main.Scripts.UI.Panel
{
    public class UIMainPanelData : UIPanelData
    {
    }

    public partial class UIMainPanel : UIEnhancedInputPanel
    {
        private GameInputSystem _input;

        protected override void OnBindInput()
        {
            _input = new GameInputSystem();
            _input.Enable();
            _input.UI.Cancel.performed += OnCancel;
        }

        protected override void OnUnbindInput()
        {
            if (_input == null) return;
            _input.UI.Cancel.performed -= OnCancel;
            _input.Disable();
            _input.Dispose();
            _input = null;
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIMainPanelData ?? new UIMainPanelData();
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            UIKitEx.CloseCurrentPanel();
        }
    }
}