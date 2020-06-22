using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools
{
    public abstract class GameObjectFactory
    {
        private protected readonly GameObjectPipeline _pipeline = new GameObjectPipeline();

        internal GameObjectFactory()
        {
        }
        
        public abstract GameObject Build();

        public virtual GameObject[] Build(int count)
        {
            var instances = new GameObject[count];
            for (int i = 0; i < instances.Length; i++)
                instances[i] = Build();

            return instances;
        }

        public virtual T Build<T>() where T : Component
        {
            return Build().GetComponent<T>() ?? throw new MissingReferenceException();
        }

        public virtual T[] Build<T>(int count) where T : Component
        {
            var instances = new T[count];
            for (int i = 0; i < instances.Length; i++)
                instances[i] = Build<T>();

            return instances;
        }

        private protected T Find<T>() where T : IPipelineElement =>
            (T) _pipeline.GetElements().FirstOrDefault(e => e is T);

        private protected T FindOrCreate<T>() where T : IPipelineElement, new()
        {
            var element = Find<T>();
            if (element == null)
            {
                element = new T();
                _pipeline.AddElement(element);
            }

            return element;
        }
        
        public static GameObjectFactoryNew New() => new GameObjectFactoryNew();
        public static GameObjectFactoryPrefab Prefab(GameObject prefab) => new GameObjectFactoryPrefab(prefab); 
    }

    public sealed class GameObjectFactoryPrefab : GameObjectFactory
    {
        private GameObject _prefab;

        public GameObjectFactoryPrefab(GameObject prefab)
        {
            SetPrefab(prefab);
        }

        public GameObjectFactoryPrefab Custom(Action<GameObject> action)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));
            
            var custom = new PipelineElements.Custom(action);
            _pipeline.AddElement(custom);
            return this;
        }
        
        public GameObjectFactoryPrefab AddComponent<T>() where T : Component
        {
            var element = new PipelineElements.AddComponent<T>();
            _pipeline.AddElement(element);
            return this;
        }

        public GameObjectFactoryPrefab MissingComponent<T>() where T : Component
        {
            var missingComponent = Find<PipelineElements.MissingComponent<T>>();
            if(missingComponent != null)
                throw new Exception();
            
            _pipeline.AddElement(new PipelineElements.MissingComponent<T>());
            return this;
        }

        public GameObjectFactoryPrefab RemoveComponent<T>() where T : Component
        {
            var addComponent = Find<PipelineElements.AddComponent<T>>();
            if (addComponent != null)
                _pipeline.RemoveElement(addComponent);
            else
            {
                var missingComponent = Find<PipelineElements.MissingComponent<T>>();
                if (missingComponent != null)
                    _pipeline.RemoveElement(missingComponent);
            }

            return this;
        }

        public GameObjectFactoryPrefab SetName(string name)
        {
            FindOrCreate<PipelineElements.SetName>().Name = name;
            return this;
        }

        public GameObjectFactoryPrefab SetTag(string tag)
        {
            FindOrCreate<PipelineElements.SetTag>().Tag = tag;
            return this;
        }

        public GameObjectFactoryPrefab SetLayer(int layer)
        {
            FindOrCreate<PipelineElements.SetLayer>().Layer = layer;
            return this;
        }

        public GameObjectFactoryPrefab SetLayer(string layerName)
        {
            SetLayer(LayerMask.NameToLayer(layerName));
            return this;
        }

        public GameObjectFactoryPrefab SetParent(Transform parent, bool worldPositionStays)
        {
            FindOrCreate<PipelineElements.SetParent>().Set(parent, worldPositionStays);
            return this;
        }

        public void SetPrefab(GameObject prefab)
        {
            if (prefab is null)
                throw new ArgumentNullException(nameof(prefab));
            if (!prefab)
                throw new MissingReferenceException(nameof(prefab));

            _prefab = prefab;
        }

        public override GameObject Build()
        {
            var instance = Object.Instantiate(_prefab);
            _pipeline.Execute(instance);
            return instance;
        }
    }

    public sealed class GameObjectFactoryNew : GameObjectFactory
    {
        public GameObjectFactoryNew AddComponent<T>() where T : Component
        {
            var element = new PipelineElements.AddComponent<T>();
            _pipeline.AddElement(element);
            return this;
        }

        public GameObjectFactoryNew RemoveComponent<T>() where T : Component
        {
            var addComponent = Find<PipelineElements.AddComponent<T>>();
            if (addComponent != null)
                _pipeline.RemoveElement(addComponent);

            return this;
        }

        public GameObjectFactoryNew Custom(Action<GameObject> action)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));
            
            var custom = new PipelineElements.Custom(action);
            _pipeline.AddElement(custom);
            return this;
        }

        public GameObjectFactoryNew SetName(string name)
        {
            FindOrCreate<PipelineElements.SetName>().Name = name;
            return this;
        }

        public GameObjectFactoryNew SetTag(string tag)
        {
            FindOrCreate<PipelineElements.SetTag>().Tag = tag;
            return this;
        }

        public GameObjectFactoryNew SetLayer(int layer)
        {
            FindOrCreate<PipelineElements.SetLayer>().Layer = layer;
            return this;
        }

        public GameObjectFactoryNew SetLayer(string layerName)
        {
            SetLayer(LayerMask.NameToLayer(layerName));
            return this;
        }

        public GameObjectFactoryNew SetParent(Transform parent, bool worldPositionStays)
        {
            FindOrCreate<PipelineElements.SetParent>().Set(parent, worldPositionStays);
            return this;
        }

        public override GameObject Build()
        {
            var instance = new GameObject();
            _pipeline.Execute(instance);
            return instance;
        }
    }
}