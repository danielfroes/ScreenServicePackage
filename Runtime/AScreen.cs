using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public abstract class AScreen : MonoBehaviour
    {
        [SerializeField] bool _isStackable = true;
        public bool IsStackable => _isStackable;
        public virtual void OnShow(object[] args) { }
        public virtual void OnHide() { }

        public void CloseScreen()
        {
            ServiceLocator.Get<ScreenService>().Close(this);
        }
    }
}
