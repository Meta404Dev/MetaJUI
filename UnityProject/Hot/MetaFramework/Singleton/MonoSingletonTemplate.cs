using UnityEngine;

namespace MetaFramework.Singleton
{
    public abstract class MonoSingletonTemplate<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T Instance
        {
            get
            {
                return m_instance;
            }
        }


        protected virtual void Awake()
        {
            m_instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            m_instance = null;
        }
    }
}