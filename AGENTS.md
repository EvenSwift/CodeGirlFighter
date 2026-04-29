# CodeFighter 项目架构

## 框架仓库
git@github.com:EvenSwift/CodeFighterFramework.git
详细文档见框架仓库 `INTEGRATION.md` 或 `README.md`

## Git Subtree 工作流

框架代码放在 `Assets/CodeFighterFramework/`，通过 git subtree 管理：

```bash
# 首次引入框架
git subtree add --prefix=Assets/CodeFighterFramework \
    git@github.com:EvenSwift/CodeFighterFramework.git main --squash

# 拉取框架更新
git subtree pull --prefix=Assets/CodeFighterFramework \
    git@github.com:EvenSwift/CodeFighterFramework.git main --squash

# 将本地的框架修改推送回框架仓库
git subtree push --prefix=Assets/CodeFighterFramework \
    git@github.com:EvenSwift/CodeFighterFramework.git main
```

## 目录约定

- `Assets/CodeFighterFramework/` ← Git Subtree（框架，可双向同步）
- `Assets/Main/Scripts/Game/` ← 游戏业务代码（不受 subtree 管理）
- `Assets/Main/Addressable/Prefabs/UI/` ← UI 面板预制体

## 关键约定

- `GameManager.SetControllers()` 是 `protected virtual`，游戏子类在此注册控制器
- 游戏枚举（AudioMusic/AudioSfx 等）定义在游戏层，框架使用 `string` 参数
- `Assets/CodeFighterFramework/CodeFighter.Framework.asmdef` 引用 `QFramework`/`UIKit`/`AudioKit` 程序集
