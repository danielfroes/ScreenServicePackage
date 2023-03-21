using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Screens
{
    public class DefaultScreenProvider : IScreenProvider
    {
        [SerializeField] AScreen _screenPrefab;

        public async UniTask<AScreen> GetScreenPrefab()
        {
            await UniTask.Yield();
            return _screenPrefab;
        }

        public Type GetScreenType() => _screenPrefab.GetType();        

        public void Release()
        {
        }

    }

}
