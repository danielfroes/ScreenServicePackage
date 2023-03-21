using System;
using UnityEngine;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Utils;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Screens
{
    public class AddressableScreenProvider : IScreenProvider
    {
        [OnValueChanged(nameof(UpdateTypeName))][SerializeField] AssetReferenceT<GameObject> _reference;
        [ReadOnly][SerializeField] string _typeName;
        void UpdateTypeName() => UpdateTypeNameAsync().Forget();

        AddressableAsset<AScreen> _addressableScreen;

        async UniTaskVoid UpdateTypeNameAsync()
        {
            if (!_reference.RuntimeKeyIsValid())
            {
                _typeName = string.Empty;
                return;
            }

            AddressableAsset<AScreen> addressableAsset = new(_reference);
            AScreen screen = await addressableAsset.Load();
            _typeName = screen.GetType().FullName;
            addressableAsset.Release();
        }


        public async UniTask<AScreen> GetScreenPrefab()
        {
            _addressableScreen = new(_reference);
            return await _addressableScreen.Load();
        }

        public Type GetScreenType()
        {
            Type type = Type.GetType(_typeName);
            if (type == null)
                throw new Exception($"[ERROR] Type {_typeName} was not found. Check if the class or namespace " +
                    $"name has changed since the last time SO was updated.");
            return type;
        }

        public void Release()
        {
            _addressableScreen.Release();
        }

    }

}
