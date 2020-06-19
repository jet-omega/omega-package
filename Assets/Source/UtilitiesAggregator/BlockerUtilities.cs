using UnityEngine;
using UnityEngine.UI;

namespace Omega.Package.Internal
{
    public sealed class BlockerUtilities
    {
        public BlockerData CreateBlockerForRectTransform(RectTransform target)
        {
            var blockerGameObject = new GameObject("Blocker");
            
            var blockerRectTransform = blockerGameObject.AddComponent<RectTransform>();
            blockerRectTransform.SetParent(target.parent, false);
            blockerRectTransform.anchorMin = Vector3.zero;
            blockerRectTransform.anchorMax = Vector3.one;
            blockerRectTransform.sizeDelta = Vector2.zero;
            
            var blockerImage = blockerGameObject.AddComponent<Image>();
            var blockerButton = blockerGameObject.AddComponent<Button>();
            
            blockerImage.color = Color.clear;
            
            blockerRectTransform.SetAsLastSibling();
            target.SetAsLastSibling(); // That makes blocker be the second from the end
            
            return new BlockerData(blockerRectTransform, blockerButton, blockerImage);
        }
        
        public readonly struct BlockerData
        {
            public readonly RectTransform RectTransform;
            public readonly Button Button;
            public readonly Image Image;

            public BlockerData(RectTransform rectTransform, Button button, Image image)
            {
                RectTransform = rectTransform;
                Button = button;
                Image = image;
            }
        }
    }
}