using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Screens
{
    public class ScreenService : IDisposable
    {
        Canvas _screenCanvas;
        List<ScreenData> _screens;
        readonly Dictionary<Type, ScreenData> _screensByType;
        readonly List<ActiveScreen> _activeScreens = new();

        public ScreenService(List<ScreenData> screens, Canvas canvasPrefab)
        {
            _screens = screens;
            _screenCanvas = Object.Instantiate(canvasPrefab);
            _screensByType = CreateScreensByType(screens);
        }

        Dictionary<Type, ScreenData> CreateScreensByType(IReadOnlyList<ScreenData> screens)
        {
            Dictionary<Type, ScreenData> screensByType = new();
;           foreach (ScreenData screen in _screens)
            {
                Type screenType = screen.Provider.GetScreenType();

                if (screensByType.ContainsKey(screenType))
                {
                    Debug.LogError($"[ERROR] Screen of type {screenType} is already been add");
                }

                screensByType[screenType] = screen;
            }

            return screensByType;
        }

        public void Show<T>(params object[] args) where T : AScreen => ShowAsync(typeof(T), args).Forget();
        public void Show(ScreenType screenType, params object[] args) => ShowAsync(screenType.Type, args).Forget();
        async UniTaskVoid ShowAsync(Type screenType, params object[] args)
        {
            if (!screenType.IsSubclassOf(typeof(AScreen)))
                throw new Exception($"[ERROR] Type {screenType.Name} don't inherit {nameof(AScreen)}");

            if (!_screensByType.ContainsKey(screenType))
                throw new Exception($"[ERROR] Screen of type {screenType} is not registered");

            ScreenData screenData = _screensByType[screenType];

            AScreen screenPrefab = await screenData.Provider.GetScreenPrefab();

            if (!screenPrefab.IsStackable)
            {
                CloseAllScreens();
            }

            AScreen screen = Object.Instantiate(screenPrefab, _screenCanvas.transform);
            screen.OnShow(args);
            _activeScreens.Add(new ActiveScreen
            {
                Instance = screen,
                Provider = screenData.Provider,
            });

        }

        public void Close(AScreen screen)
        {
            ActiveScreen addressableScreen = _activeScreens.Find(active => active.Instance == screen);
            CloseScreen(addressableScreen);
        }

        void CloseScreen(ActiveScreen screen)
        {
            screen.Instance.OnHide();
            Object.Destroy(screen.Instance.gameObject);
            screen.Provider.Release();
            _activeScreens.Remove(screen);
        }

        void CloseAllScreens()
        {
            for (int i = 0; i < _activeScreens.Count; i++)
            {
                ActiveScreen screen = _activeScreens[i];
                _activeScreens[i] = default;
                CloseScreen(screen);
            }

            _activeScreens.Clear();
        }

        public void Dispose()
        {
            CloseAllScreens();
        }

        struct ActiveScreen
        {
            public IScreenProvider Provider;
            public AScreen Instance;
        }

    }
}
