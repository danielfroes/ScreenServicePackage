using Assets.Scripts.Screens;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Initialization
{
    public class ScreenServiceInitializer : IInitializationStep
    {
        public Canvas _canvasPrefab;
        public List<ScreenData> _screens;

        public void Run()
        {
            ServiceLocator.Register(new ScreenService(_screens, _canvasPrefab));
        }

        public void Dispose()
        {
            ServiceLocator.Unregister<ScreenService>();
        }
    }
}