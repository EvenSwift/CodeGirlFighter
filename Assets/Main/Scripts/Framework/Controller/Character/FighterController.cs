using Cysharp.Threading.Tasks;
using Main.Scripts.Entity;
using Main.Scripts.Framework.Controller.Base;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Model.Data;
using UnityEngine;

namespace Main.Scripts.Framework.Controller.Character
{
    /// <summary>
    /// 格斗者控制器，负责角色的创建、回收和生命周期管理
    /// </summary>
    public class FighterController : SyncPureController
    {
        /// <summary>
        /// 角色预制体 Addressable 路径
        /// </summary>
        private const string CHARACTER_PREFAB_PATH = "Assets/Main/Addressable/Prefabs/Character/CharacterPre.prefab";

        /// <summary>
        /// 场景中所有角色实例的父容器名称
        /// </summary>
        private const string CHARACTERS_ROOT_NAME = "Characters";

        /// <summary>
        /// 角色父容器，所有角色实例挂在此 GameObject 下
        /// </summary>
        private Transform _charactersRoot;

        /// <summary>
        /// 玩家角色
        /// </summary>
        public CharacterEntityView Player { get; private set; }

        /// <summary>
        /// 对手角色
        /// </summary>
        public CharacterEntityView Opponent { get; private set; }

        protected override async UniTask OnInitialize(IInitContext context)
        {
            await base.OnInitialize(context);

            // 创建角色父容器，避免角色对象直接散落在场景根目录
            CreateCharactersRoot();

            await LoadCharacters();
        }

        /// <summary>
        /// 创建角色父容器空对象
        /// </summary>
        private void CreateCharactersRoot()
        {
            var root = new GameObject(CHARACTERS_ROOT_NAME);
            Object.DontDestroyOnLoad(root);
            _charactersRoot = root.transform;
        }

        /// <summary>
        /// 加载双方角色
        /// 玩家在左侧（不需要翻转），对手在右侧（需要翻转）
        /// 具体位置由 CharacterViewBase.CalculatePosition() 根据 Sprite 尺寸动态计算
        /// </summary>
        private async UniTask LoadCharacters()
        {
            // 初始位置给原点即可，实际位置由 View 层根据 Sprite 尺寸计算
            Player = await CreateCharacter(Vector3.zero, false);
            Opponent = await CreateCharacter(Vector3.zero, true);
        }

        /// <summary>
        /// 创建单个角色实例，挂到父容器下
        /// </summary>
        /// <param name="position">初始位置（View 层会在 Sprite 加载后重新计算）</param>
        /// <param name="isFlipped">是否翻转（true = 右侧对手，翻转向左）</param>
        private async UniTask<CharacterEntityView> CreateCharacter(Vector3 position, bool isFlipped)
        {
            var data = new CharacterData(configId: 1, position, isFlipped: isFlipped);
            var context = new CharacterEntityViewInitContext(this, data);

            var view = await this.InstantiateAsync<CharacterEntityView>(
                CHARACTER_PREFAB_PATH,
                context: context
            );

            // 将角色实例挂到父容器下
            view.transform.SetParent(_charactersRoot);

            return view;
        }

        protected override void OnRelease()
        {
            if (Player != null)
            {
                Object.Destroy(Player.gameObject);
                Player = null;
            }

            if (Opponent != null)
            {
                Object.Destroy(Opponent.gameObject);
                Opponent = null;
            }

            if (_charactersRoot != null)
            {
                Object.Destroy(_charactersRoot.gameObject);
                _charactersRoot = null;
            }
        }
    }
}