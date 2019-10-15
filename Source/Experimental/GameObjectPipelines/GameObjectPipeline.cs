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

        public PipelineElements.AddComponent<T> AddComponent<T>() where T : Component
        {
            var element = new PipelineElements.AddComponent<T>();
            _pipelineElements.Add(element);
            return element;
        }

        public PipelineElements.MissingComponent<T> MissingComponent<T>() where T : Component
        {
            var element = new PipelineElements.MissingComponent<T>();
            _pipelineElements.Add(element);
            return element;
        }

        public PipelineElements.SetLayer SetLayer()
        {
            var element = new PipelineElements.SetLayer();
            _pipelineElements.Add(element);
            return element;
        }

        public PipelineElements.SetName SetName()
        {
            var element = new PipelineElements.SetName();
            _pipelineElements.Add(element);
            return element;
        }

        public PipelineElements.SetTag SetTag()
        {
            var element = new PipelineElements.SetTag();
            _pipelineElements.Add(element);
            return element;
        }

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
        }

        public class MissingComponent<T> : IPipelineElement where T : Component
        {
            public void Execute(GameObject gameObject)
                => gameObject.MissingComponent<T>();

            public override bool Equals(object obj)
                => obj is MissingComponent<T>;
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
    }
}