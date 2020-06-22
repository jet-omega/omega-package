using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.Tools
{
    internal interface IPipelineElement
    {
        void Execute(GameObject gameObject);
    }

    internal class GameObjectPipeline
    {
        private List<IPipelineElement> _pipelineElements = new List<IPipelineElement>();

        public void RemoveElement(IPipelineElement element)
            => _pipelineElements.Remove(element);

        public void AddElement(IPipelineElement element)
            => _pipelineElements.Add(element);

        public IEnumerable<IPipelineElement> GetElements()
            => _pipelineElements;

        public void Execute(GameObject gameObject)
        {
            _pipelineElements.ForEach(e => e.Execute(gameObject));
        }
    }

    internal static class PipelineElements
    {
        public class AddComponent<T> : IPipelineElement where T : Component
        {
            public void Execute(GameObject gameObject)
                => gameObject.AddComponent<T>();

            public override bool Equals(object obj)
                => obj is AddComponent<T>;

            public override int GetHashCode()
            {
                var type = typeof(T);
                var typeHashCode = type.GetHashCode();
                return typeHashCode << 16 | typeHashCode >> 16;
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        public class MissingComponent<T> : IPipelineElement where T : Component
        {
            public void Execute(GameObject gameObject)
                => gameObject.MissingComponent<T>();

            public override bool Equals(object obj)
                => obj is MissingComponent<T>;
            
            public override int GetHashCode()
            {
                var type = typeof(T);
                var typeHashCode = type.GetHashCode();
                return ~(typeHashCode << 16 | typeHashCode >> 16);
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        public class SetParent : IPipelineElement
        {
            private Transform _parent;
            private bool _worldPositionStays;

            public void Set(Transform parent, bool worldPositionStays)
            {
                _parent = parent;
                _worldPositionStays = worldPositionStays;
            }
            
            public void Execute(GameObject gameObject)
            {
                gameObject.transform.SetParent(_parent, _worldPositionStays);
            }
        }

        public class SetName : IPipelineElement
        {
            private string _name;

            public string Name
            {
                get => _name;
                set => _name = value;
            }

            public void Execute(GameObject gameObject)
                => gameObject.name = _name;
        }

        public class SetLayer : IPipelineElement
        {
            private int _layer;

            public int Layer
            {
                get => _layer;
                set => _layer = value;
            }
            
            public void Execute(GameObject gameObject)
                => gameObject.layer = _layer;
        }

        public class SetTag : IPipelineElement
        {
            private string _tag;

            public string Tag
            {
                get => _tag;
                set => _tag = value;
            }

            public void Execute(GameObject gameObject)
                => gameObject.tag = _tag;
        }

        public class Custom : IPipelineElement
        {
            private readonly Action<GameObject> _action;

            public Custom(Action<GameObject> action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            public void Execute(GameObject gameObject)
            {
                _action(gameObject);
            }

            protected bool Equals(Custom other)
            {
                return other != null && other._action == _action;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Custom);
            }

            public override int GetHashCode()
            {
                return _action.GetHashCode();
            }
        }
    }
}