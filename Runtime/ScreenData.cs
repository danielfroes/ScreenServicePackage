using UnityEngine;
using Assets.Game.Scripts.Utils;

namespace Assets.Scripts.Screens
{
    [CreateAssetMenu(fileName = "Screen Data", menuName = "Screen Service/Screen Data")]
    public class ScreenData : ScriptableObject
    {
        [SerializeReference][SerializeReferenceMenu] IScreenProvider _provider = new DefaultScreenProvider();
        public IScreenProvider Provider => _provider;
    }

}
