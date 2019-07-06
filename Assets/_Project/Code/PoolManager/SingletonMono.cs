/*---------------------------------------------------------------
 * 作者：evesgf    创建时间：2016-8-2 12:01:49
 * 修改：evesgf    修改时间：2016-8-2 12:01:54
 *
 * 版本：V0.0.1
 * 
 * 描述：接收MonoBehaviour生命周期的单例的模板
 * 1.约束脚本实例对象的个数。 
 * 2.约束GameObject的个数。 
 * 3.接收MonoBehaviour生命周期。
 * 4.销毁单例和对应的GameObject。
 ---------------------------------------------------------------*/

using UnityEngine;
using System.Collections;

namespace GPL
{
    public abstract class SingletonMono<T> : MonoBehaviour where T: SingletonMono<T>
    {
        private static T _instance = null;

        public static T Create()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.Log("<color=silver><SingletonMono></color>" + "More than 1!");
                    return _instance;
                }
                
                if (_instance == null)
                {
                    string instanceName = typeof(T).Name;
                    Debug.Log("<color=silver><SingletonMono></color>" + "Instance Name: " + instanceName);
                    GameObject instanceGO = GameObject.Find(instanceName);

                    if (instanceGO == null)
                        instanceGO = new GameObject(instanceName);
                    _instance = instanceGO.AddComponent<T>();
                    DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
                    Debug.Log("<color=silver><SingletonMono></color>" + "Add New Singleton " + _instance.name + " in Game!");
                }
                else
                {
                    DontDestroyOnLoad(_instance);  //保证实例不会被释放
                    Debug.Log("<color=silver><SingletonMono></color>" + "Already exist: " + _instance.name);
                }
            }
            return _instance;
        }

        /// <summary>
        /// 获取单例
        /// 此处不提供自动创建，旨在明确单例创建时间
        /// </summary>
        public static  T Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public static void Destroy()
        {
            T[] instanceList =FindObjectsOfType(typeof(T)) as T[];
            if (instanceList!=null && instanceList.Length >0)
            {
                for (int i = 0; i < instanceList.Length; i++)
                {
                    Destroy(instanceList[i].gameObject);
                }
            }

            _instance = null;
            return;
        }
    }
}
