using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace RishUI.Deprecated.Editor
{
    public static class TransformDebug
    {
        private const int Width = 175;
        private const int Height = 130;
        private const int Border = 10;
        private const int Margin = 25;
        
        private static GUIStyle minStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            clipping = TextClipping.Overflow,
            alignment = TextAnchor.MiddleRight,
            normal = new GUIStyleState
            {
                textColor = Color.black
            }
        };
        
        private static GUIStyle maxStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            clipping = TextClipping.Overflow,
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState
            {
                textColor = Color.black
            }
        };
        
        private static GUIStyle centeredStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            clipping = TextClipping.Overflow,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState
            {
                textColor = Color.black
            }
        };
        
        public static void Draw(float width, RishNode node, bool clean = false, bool world = false)
        {
            var area = new Rect(0, 0, width, Height + Border * 2);

            var transform = node.Component.Local;
            if (world)
            {
                var parent = node.Parent;
                while (parent != null && !(parent.Component is AppComponent))
                {
                    var parentTransform = parent.Component.Local;
                    transform = parentTransform * transform;
                    parent = parent.Parent;
                }
            }
            
            GUILayout.BeginArea(area);
            
            var parentRect = new Rect((area.width - Width) * 0.5f, Border, Width, Height);
            
            Rect anchorsRect;
            Rect localRect;
            if(clean)
            {
                anchorsRect = new Rect(parentRect.position + Vector2.one * Margin, parentRect.size - Vector2.one * Margin * 2);
                localRect = new Rect(anchorsRect.position + Vector2.one * Margin, anchorsRect.size - Vector2.one * Margin * 2);
            }
            else
            {
                anchorsRect = new Rect(parentRect.x  + parentRect.width * transform.min.x, parentRect.y + parentRect.height * (1 - transform.max.y), parentRect.width - parentRect.width * (1 - transform.max.x) - parentRect.width * transform.min.x, parentRect.height - parentRect.height * (1 - transform.max.y) - parentRect.height * transform.min.y);
                localRect = new Rect(anchorsRect.x + parentRect.width * (transform.left / (Width * 3)), anchorsRect.y + parentRect.height * (transform.top / (Height * 3)), anchorsRect.width - parentRect.width * (transform.left / (Width * 3)) - parentRect.width * (transform.right / (Width * 3)), anchorsRect.height - parentRect.height * (transform.top / (Height * 3)) - parentRect.height * (transform.bottom / (Height * 3)));
            }
            
            EditorGUI.DrawRect(area, new Color(0.682f, 0.506f, 0.322f, 1f));
            EditorGUI.DrawRect(parentRect, new Color(0.89f, 0.765f, 0.506f, 1f));
            EditorGUI.DrawRect(anchorsRect, new Color(0.718f, 0.769f, 0.498f, 1f));
            EditorGUI.DrawRect(localRect, new Color(0.529f, 0.698f, 0.737f, 1f));

            if (clean)
            {
                var minX = new Rect(parentRect.xMin, anchorsRect.center.y, anchorsRect.xMin - parentRect.xMin, 0);
                var maxX = new Rect(anchorsRect.xMax, anchorsRect.center.y, parentRect.xMax - anchorsRect.xMax, 0);
                var maxY = new Rect(anchorsRect.center.x, parentRect.yMin, 0, anchorsRect.yMin - parentRect.yMin);
                var minY = new Rect(anchorsRect.center.x, anchorsRect.yMax, 0, parentRect.yMax - anchorsRect.yMax);

                EditorGUI.LabelField(minX, transform.min.x.ToString(CultureInfo.InvariantCulture), centeredStyle);
                EditorGUI.LabelField(maxX, (1 - transform.max.x).ToString(CultureInfo.InvariantCulture), centeredStyle);
                EditorGUI.LabelField(maxY, (1 - transform.max.y).ToString(CultureInfo.InvariantCulture), centeredStyle);
                EditorGUI.LabelField(minY, transform.min.y.ToString(CultureInfo.InvariantCulture), centeredStyle);
            }
            else
            {
                var minRect = new Rect(anchorsRect.xMin, anchorsRect.yMax, 0, 0);
                var maxRect = new Rect(anchorsRect.xMax, anchorsRect.yMin, 0, 0);

                EditorGUI.LabelField(minRect, transform.min.ToString(), minStyle);
                EditorGUI.LabelField(maxRect, transform.max.ToString(), maxStyle);
            }

            var leftRect = new Rect(anchorsRect.xMin, localRect.center.y, localRect.xMin - anchorsRect.xMin, 0);
            var rightRect = new Rect(localRect.xMax, localRect.center.y, anchorsRect.xMax - localRect.xMax, 0);
            var topRect = new Rect(localRect.center.x, anchorsRect.yMin, 0, localRect.yMin - anchorsRect.yMin);
            var bottomRect = new Rect(localRect.center.x, localRect.yMax, 0, anchorsRect.yMax - localRect.yMax);

            EditorGUI.LabelField(leftRect, transform.left.ToString(CultureInfo.InvariantCulture), centeredStyle);
            EditorGUI.LabelField(rightRect, transform.right.ToString(CultureInfo.InvariantCulture), centeredStyle);
            EditorGUI.LabelField(topRect, transform.top.ToString(CultureInfo.InvariantCulture), centeredStyle);
            EditorGUI.LabelField(bottomRect, transform.bottom.ToString(CultureInfo.InvariantCulture), centeredStyle);

            GUILayout.EndArea();
            
            GUILayout.Space(area.height);
        }
    }
}