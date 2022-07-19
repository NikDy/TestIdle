using System;
using System.Collections.Generic;
using UnityEngine;


public class GardenController
{
    private GardenModel gardenModel;
    private GardenUpdate gardenUpdate;

    private Dictionary<Collider, GameObject> collidersGrid = new Dictionary<Collider, GameObject>();
    private Dictionary<BoxCollider, float> collidersCutedTime = new Dictionary<BoxCollider, float>();

    public GardenController(GardenModel gardenModel, GardenUpdate gardenUpdate)
    {
        this.gardenModel = gardenModel;
        this.gardenUpdate = gardenUpdate;        
        BuildGarden();
        PlayerUpdate.HitEvent += OnHit;
        EntityHandler.FixedUpdateEvent += OnFixedUpdate;
    }

    private void OnHit(Collider collider)
    {
        GameObject gameObject;
        collidersGrid.TryGetValue(collider, out gameObject);
        if (gameObject != null)
        {
            collidersGrid[(BoxCollider)collider] = gardenUpdate.SetShortMesh(collidersGrid[(BoxCollider)collider].gameObject);
            collidersCutedTime[(BoxCollider)collider] = Time.time;
            collider.enabled = false;
            gardenUpdate.createStack(collidersGrid[(BoxCollider)collider].gameObject.transform.position);
        }
        
    }

    private void OnFixedUpdate()
    {
        if(collidersCutedTime.Count > 0)
        { 
            foreach(var timestamp in collidersCutedTime)
            {
                if(Time.time - timestamp.Value >= gardenModel.growTime)
                {
                    collidersGrid[timestamp.Key] = gardenUpdate.SetLongMesh(collidersGrid[timestamp.Key].gameObject);
                    timestamp.Key.enabled = true;
                }
            }
        }
    }


    private void BuildGarden()
    {
        for (int i = 0; i < gardenModel.width; i++)
        {
            for (int j = 0; j < gardenModel.height; j++)
            {
                var newSupplingWithCollider = gardenUpdate.AddSuppling(i * gardenModel.meshStep, j * gardenModel.meshStep, gardenModel.isCuted);
                collidersGrid.Add(newSupplingWithCollider.Item2, newSupplingWithCollider.Item1);
                collidersCutedTime.Add(newSupplingWithCollider.Item2, gardenModel.growTime);
            }
        }
    }




}
