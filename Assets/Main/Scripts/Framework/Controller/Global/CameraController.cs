using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Controller.Base;
using Main.Scripts.Framework.Controller.Base.Interface;
using QFramework;
using UnityEngine;

namespace Main.Scripts.Framework.Controller.Global
{
    public class CameraController : MonoController
    {
        [SerializeField] private Camera uiCamera;
        public Camera MainCamera { get; private set; }
        public Camera UICamera => uiCamera;

        protected override UniTask OnInitialize(IInitContext context)
        {
            MainCamera = GetComponent<Camera>();
            return UniTask.CompletedTask;
        }

        protected override void OnRelease()
        {
        }
    }
}
