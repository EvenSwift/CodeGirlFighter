using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using QFramework;

namespace Main.Scripts.UI.Panel
{
    public class UIStartPanelData : UIPanelData
    {
    }

    public partial class UIStartPanel : UIEnhancedInputPanel
    {
        private GameInputSystem _input;

        #region InputLifecycle

        protected override void OnBindInput()
        {
            _input = new GameInputSystem();
            _input.Enable();
            _input.UI.Submit.performed += OnSubmit;
        }

        protected override void OnUnbindInput()
        {
            if (_input == null) return;
            _input.UI.Submit.performed -= OnSubmit;
            _input.Disable();
            _input.Dispose();
            _input = null;
        }

        #endregion

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        #region UIEvent

        private void OnBtnStartClick()
        {
            if (BtnStart != null && BtnStart.IsInteractable())
                BtnStart.onClick?.Invoke();
        }

        private void OnBtnSavesClick()
        {
            if (BtnSaves != null && BtnSaves.IsInteractable())
                BtnSaves.onClick?.Invoke();
        }

        private void OnBtnSettingsClick()
        {
            if (BtnSettings != null && BtnSettings.IsInteractable())
                BtnSettings.onClick?.Invoke();
        }

        #endregion

        #region UIInput

        private void OnSubmit(InputAction.CallbackContext context)
        {
            OnBtnStartClick();
        }

        #endregion
    }
}