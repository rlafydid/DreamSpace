using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;

    public Transform target;
    
    void Awake () {
        
        const float radius = 1;
        var random = new System.Random(1);
    
        for (int i = 0; i < spawnCount; i++) {
            
            var angle = Random.Range(0, 2 * Mathf.PI);
            var xOffset = radius * Mathf.Cos(angle);
            var yOffset = radius * Mathf.Sin(angle);

            var offset = new Vector3(xOffset, 0, yOffset);
            
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            pos.y = 0;
            // Vector3 pos = transform.position;
            Boid boid = Instantiate (prefab);
            boid.transform.position = pos;
            var dir = Quaternion.AngleAxis(Random.Range(0, 5), Vector3.up) * Vector3.forward;
            var forward = Random.insideUnitSphere;
            forward.y = 0;
            boid.transform.forward = forward;

            boid.SetColour (colour);
        }
    }

    public void OnGUI()
    {
        if (GUILayout.Button("跟随"))
        {
            BoidManager.Instance.StartFollow(target);
        }
        if (GUILayout.Button("停止"))
        {
            BoidManager.Instance.StopFollow();
        }
    }

    void Spawn()
    {
        const float radius = 0.5f;
        var angle = Random.Range(0, 2 * Mathf.PI);
        var xOffset = radius * Mathf.Cos(angle);
        var yOffset = radius * Mathf.Sin(angle);

        var offset = new Vector3(xOffset, 0, yOffset);
            
        // Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
        Vector3 pos = target != null ? target.position + offset : transform.position;
        Boid boid = Instantiate (prefab);
        boid.transform.position = pos;
        var dir = Quaternion.AngleAxis(Random.Range(0, 5), Vector3.up) * Vector3.forward;
        var forward = Random.insideUnitSphere;
        forward.y = 0;
        boid.transform.forward = forward;
        // boid.forward = Vector3.forward;
        boid.SetColour (colour);

        // if (target == null)
            // target = boid;
    }

    private void OnDrawGizmos () {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected () {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos ();
        }
    }

    void DrawGizmos () {

        Gizmos.color = new Color (colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}