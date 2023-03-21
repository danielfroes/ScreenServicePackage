using System;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Screens
{
    public interface IScreenProvider
    {
        UniTask<AScreen> GetScreenPrefab();
        Type GetScreenType();
        void Release();
    }

}
