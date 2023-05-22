using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAreaMark : MonoBehaviour {
    public Color color = Color.green;
    private List<SceneConfig> loadedScenes = new List<SceneConfig>();
    /*****************************************************************
     * 
     *      lifecycle
     * 
     *****************************************************************/
    private void OnDrawGizmos() {
        Gizmos.color = color;
        foreach(SceneConfig cfg in loadedScenes)
            DrawOneScene(cfg);
    }
    /*****************************************************************
     * 
     *      public method
     * 
     *****************************************************************/
    public void Add(SceneConfig cfg) {
        loadedScenes.Add(cfg);
    }
    public void Remove(SceneConfig cfg) {
        loadedScenes.Remove(cfg);
    }
    /*****************************************************************
     * 
     *      private method
     * 
     *****************************************************************/
    private void DrawOneScene(SceneConfig cfg) {
        if(cfg == null)
            return;
        Vector3 center = new Vector3(cfg.Area.center.x, cfg.Area.center.y, 0f);
        Vector3 size = new Vector3(cfg.Area.size.x, cfg.Area.size.y, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}
