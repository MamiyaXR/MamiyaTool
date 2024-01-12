using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRender : MaskableGraphic {
    [SerializeField] protected List<Vector2> points = new List<Vector2>();
    [SerializeField] protected float width;
    /***********************************************************
     * 
     *      lifecycle
     * 
     ***********************************************************/
    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();
        if(points != null && points.Count > 1) {
            for(int i = 0; i < points.Count - 1; ++i)
                vh.AddUIVertexQuad(GetRectangleQuad(color, points[i], points[i + 1], width));
        }
    }
    /***********************************************************
     * 
     *      private method
     * 
     ***********************************************************/
    private UIVertex[] GetRectangleQuad(Color color, Vector2 begin, Vector2 end, float width) {
        float length = (end - begin).magnitude;
        float sin = length == 0f ? 0f : (end.y - begin.y) / length;
        float cos = length == 0f ? 0f : (end.x - begin.x) / length;
        float offsetX = width / 2f * sin;
        float offsetY = width / 2f * cos;
        Vector2 p1 = begin + new Vector2(offsetX, -offsetY);
        Vector2 p2 = begin + new Vector2(-offsetX, offsetY);
        Vector2 p3 = end + new Vector2(-offsetX, offsetY);
        Vector2 p4 = end + new Vector2(offsetX, -offsetY);
        return GetRectangleQuad(color, p1, p2, p3, p4);
    }
    private UIVertex[] GetRectangleQuad(Color color, params Vector2[] points) {
        UIVertex[] vertexs = new UIVertex[points.Length];
        for(int i = 0; i < vertexs.Length; ++i)
            vertexs[i] = GetUIVertex(points[i], color);
        return vertexs;
    }
    private UIVertex GetUIVertex(Vector2 point, Color color) {
        UIVertex vertex = new UIVertex() {
            position = point,
            color = color,
            uv0 = Vector2.zero,
        };
        return vertex;
    }
}
