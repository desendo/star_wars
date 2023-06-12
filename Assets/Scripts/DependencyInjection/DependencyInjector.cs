using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DependencyInjection
{
    internal class DependencyInjector
    {
        private readonly DependencyContainer _container;
        private readonly HashSet<object> _injected = new HashSet<object>();

        internal DependencyInjector(DependencyContainer container)
        {
            _container = container;
        }

        internal void Inject(object target)
        {
            if(_injected.Contains(target))
                return;

            var type = target.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(InjectAttribute)))
                {
                    InvokeMethod(method, target);
                }
            }

            _injected.Add(target);
        }

        private void InvokeMethod(MethodInfo method, object target)
        {
            var parameters = method.GetParameters();
            var args = GetArguments(parameters);
            method.Invoke(target, args);
        }

        internal object[] GetArguments(ParameterInfo[] parameters)
        {
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var type = parameter.ParameterType;
                object arg = null;
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    var itemType = type.GetGenericArguments().Single();
                    var cachedObjectsList = _container.GetList(itemType) as List<object>;
                    var genericListType = typeof(List<>).MakeGenericType(new Type[] {itemType});
                    var genericListInstance = (IList) Activator.CreateInstance(genericListType);

                    if (cachedObjectsList != null)
                        foreach (var o in cachedObjectsList)
                            genericListInstance.Add(o);

                    arg = genericListInstance;
                }
                else
                {
                    arg = _container.Get(type);
                }

                if (arg == null)
                {
                    Debug.LogWarning($"cant resolve by type {type}");
                }


                args[i] = arg;
            }

            return args;
        }
    }
}
