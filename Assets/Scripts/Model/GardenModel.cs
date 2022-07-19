using System;
using UnityEngine;



public class GardenModel
{

    public Vector3 position { get; set; }    
    public int width { get; set; }
    public int height { get; set; }
    public float growTime { get; set; }
    public bool isCuted { get; set; }
    public float meshStep { get; set; }


    public GardenModel(Vector3 position, int width, int height, float growSpeed, bool isCuted, float meshStep)
    {
        this.position = position;
        this.width = width;
        this.height = height;
        this.growTime = growSpeed;
        this.isCuted = isCuted;
        this.meshStep = meshStep;
    }

}
