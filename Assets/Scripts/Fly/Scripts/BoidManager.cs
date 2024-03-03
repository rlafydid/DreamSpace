using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;
    List<Boid> boids = new();
    public Transform target;

    public static BoidManager Instance;

    private bool _startFollow = false;
    
    public List<Boid> Boids
    {
        get => boids;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        // boids = FindObjectsOfType<Boid> ();
        // foreach (Boid b in boids) {
        //     b.Initialize (settings, target);
        // }
    }

    public void StartFollow(Transform target)
    {
        foreach (var boid in boids)
        {
            boid.StartFollow(target);
        }
    }

    public void StopFollow()
    {
        foreach (var boid in boids)
        {
            boid.StopFollow();
        }
    }
    
    public void Add(Boid boid)
    {
        boids.Add(boid);
        boid.Initialize (settings, target);
    }
    
    void Update () {
        if (boids != null) {

            int numBoids = boids.Count;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Count; i++) {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
                boidData[i].speed = boids[i].speed;
                boidData[i].toTargetDirection = boids[i].ToTargetDirection;
            }

            var boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
            boidBuffer.SetData (boidData);

            compute.SetBuffer (0, "boids", boidBuffer);
            compute.SetInt ("numBoids", boids.Count);
            compute.SetFloat ("viewRadius", settings.perceptionRadius);
            compute.SetFloat ("avoidRadius", settings.avoidanceRadius);

            int threadGroups = Mathf.CeilToInt (numBoids / (float) threadGroupSize);
            compute.Dispatch (0, threadGroups, 1, 1);

            boidBuffer.GetData (boidData);

            for (int i = 0; i < boids.Count; i++) {
                boids[i].avgFlockHeading = boidData[i].flockHeading;
                boids[i].centreOfFlockmates = boidData[i].flockCentre;
                boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;
                
                boids[i].countInFrontCone = boidData[i].countInFrontCone;
                boids[i].sumUnitSpeedInFrontCone = boidData[i].sumUnitSpeedInFrontCone;

                boids[i].UpdateBoid ();
            }

            boidBuffer.Release ();
        }
    }

    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public float speed;
        public int countInFrontCone;
        public float sumUnitSpeedInFrontCone;
        public Vector3 toTargetDirection;
        
        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int)/*后面的后加的三个*/ + sizeof (int) + sizeof(float) * 2 + sizeof(float) * 3;
            }
        }
    }
}