using Assets.Scripts.Utils;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    [Serializable]
    public struct ScreenType
    {
        [Dropdown("ScreenTypes")][SerializeField] string _screenType;

        public Type Type => Type.GetType(_screenType);

        List<string> ScreenTypes
        {
            get
            {
                var types = typeof(AScreen).
                    Assembly.GetTypes().
                    Where(t => t.IsSubclassOf(typeof(AScreen)) && !t.IsAbstract).ToList();

                if (types.IsNullOrEmpty())
                    return null;

                return types.ConvertAll(type => type.FullName);
            }
        }
    }
}
