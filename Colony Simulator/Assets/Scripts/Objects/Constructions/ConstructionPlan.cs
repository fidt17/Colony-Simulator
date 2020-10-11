using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConstructionPlan : StaticObject, IPlacable {

    public new ConstructionScriptableObject data { get; protected set; }
    
    public ConstructionPlan() : base() {}

    private Job _constructionJob;
    private List<Job> _haulingJobs = new List<Job>();

    private void CreateConstructionJob() {
        Debug.Log("CONSTRUCTION JOB");
    }

    private void CreateHaulingJobs() {
        foreach (Ingredient ingredient in data.ingredients) {
            CreateHaulingJobForIngredient(ingredient);
        }
    }

    private void CreateHaulingJobForIngredient(Ingredient ingredient) {
        Type ingredientType = Factory.GetTypeDerivativeOf<Item>(ingredient.itemName);
        for (int i = ingredient.count; i > 0; i--) {
            SearchAndHaulJob job = new SearchAndHaulJob(ingredientType, position);
            JobSystem.GetInstance().AddJob(job);
            _haulingJobs.Add(job);

            job.JobResultHandler += HandleHaulJobResult;
        }
    }

    //FIX THIS// item count
    private void CalculateNewIngredientsValues(Item newItem) {
        for (int i = data.ingredients.Count - 1; i >= 0; i--) {
            Ingredient ingredient = data.ingredients[i];
            ingredient.count -= 1;
            if (ingredient.count <= 0) {
                data.ingredients.Remove(ingredient);
            }
        }

        if (data.ingredients.Count == 0) {
            CreateConstructionJob();
        }
    }

    private void HandleHaulJobResult(object source, EventArgs e) {
        (source as Job).JobResultHandler -= HandleHaulJobResult;

        bool result = (e as Job.JobResultEventArgs).result;
        if (result == true) {
            Debug.Log("RECALCULATING INGREDIENTS VALUES");
            CalculateNewIngredientsValues((source as SearchAndHaulJob).Item);
        } else {
            Debug.Log("CREATE NEW HAUL JOB BECAUSE PREVIOUS ONE FAILED");
        }
    }

    private void DeleteJob() {
        foreach (Job job in _haulingJobs) {
            job?.DeleteJob();
        }
        _haulingJobs.Clear();

        _constructionJob?.DeleteJob();
        _constructionJob = null;
    }

    #region IPrefab

    public override void SetData(PrefabScriptableObject data) {
        this.data = data as ConstructionScriptableObject;
        isTraversable = false;
    } 

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        PutOnTile();

        CreateHaulingJobs();
    }

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion

    #region IPlacable

    public void PutOnTile()      => Utils.TileAt(position).contents.PutStaticObjectOnTile(this, isTraversable);
    public void RemoveFromTile() => Utils.TileAt(position).contents.RemoveStaticObjectFromTile();
    
    #endregion
}