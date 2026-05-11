using Main.Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Main.Scripts.UI.Base
{
    public class UIEnhancedButton : Button
    {
        [SerializeField] private string highlightedSound;
        [SerializeField] private string clickSound;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (IsInteractable())
            {
                DoStateTransition(currentSelectionState, false);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!string.IsNullOrEmpty(clickSound))
            {
                AudioKitEx.PlaySound(clickSound);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (!string.IsNullOrEmpty(highlightedSound))
            {
                AudioKitEx.PlaySound(highlightedSound);
            }
        }

        public void SimulateSelect()
        {
            OnSelect(null);
        }

        public void SimulateDeselect()
        {
            OnDeselect(null);
        }
    }
}
