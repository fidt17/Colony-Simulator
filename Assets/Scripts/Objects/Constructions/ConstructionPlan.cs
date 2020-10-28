using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ConstructionPlan : StaticObject, IItemHolder {

    public new ConstructionScriptableObject data { get; protected set; }
    
    public ConstructionPlan() : base() {}

    private Job _constructionJob;
    private List<Job> _haulingJobs = new List<Job>();
    private List<Ingredient> _ingredients;
    private List<Item> _hauingIngredients = new List<Item>();

    public List<Item> items { get; private set; } = new List<Item>();

    public void Build() {
        GameObject.Destroy(gameObject);
        Factory.Create<Construction>("wall", position);
    }

    #region IItemHolder

    public void ItemIn(Item item) {
        items.Add(item);
        item.SetPosition(position);
        item.gameObject.SetActive(false);
    }

    public Item ItemOut(Item item) {
        if (items.Contains(item)) {
            items.Remove(item);
            item.gameObject.SetActive(true);
            return item;
        }
        return null;
    }

    #endregion

    #region IPrefab

    public new void SetData(ConstructionScriptableObject data, Vector2Int position) {
        this.data = data as ConstructionScriptableObject;
        this.position = position;
        isTraversable = true;

        ConfigureIngredients();
        PutOnTile();
    } 

    public override void SetGameObject(GameObject gameObject) {
        base.SetGameObject(gameObject);
        CheckCurrentTileContents();
    }

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion

    private void ConfigureIngredients() {
        _ingredients = new List<Ingredient>();
        foreach(var dataIngredient in data.ingredients) {
            Ingredient newIngredient = new Ingredient();
            newIngredient.itemName = dataIngredient.itemName;
            newIngredient.count = dataIngredient.count;
            _ingredients.Add(newIngredient);
        }
    }

    private void CreateConstructionJob() => JobSystem.GetInstance().AddJob(new BuildJob(this));

    private void CheckCurrentTileContents() {
        Tile currentTile = Utils.TileAt(position.x, position.y);
        //Removing any existing stockpiles from the tile
        StockpileCreator.RemoveStockpileFromTile(currentTile);

        //detecting if there is an item on the tile at the moment
        //if so - creating a haul job that need to be finished before anything else
        //otherwise create haul jobs for ingredients.
        if (currentTile.content.HasItem) {
            Item item = currentTile.content.item;

            //find closest free spot
            Func<Tile, bool> requirementsFunction = delegate(Tile t) {
            if (t == null) {
                return false;
            } else {
                return !t.content.HasItem;
            }
            };
            Tile tile = SearchEngine.FindClosestTileWhere(position, requirementsFunction);
        
            if (tile != null) {
                HaulJob job = new HaulJob(item, tile.position);
                _haulingJobs.Add(job);
                JobSystem.GetInstance().AddJob(job);
                job.JobResultHandler += HaulItemFromConstructionPlanJobHandler;
            } else {
                Debug.LogError("No empty tile to move item to was found. Implement waiting function to try again some time later. P: " + position);
            }
        } else {
            CreateHaulingJobs();
        }
    }

    private void HaulItemFromConstructionPlanJobHandler(object source, EventArgs e) {
        if (source is HaulJob) {
            _haulingJobs.Remove(source as HaulJob);
            if ((e as Job.JobResultEventArgs).result == true) {
                (source as HaulJob).JobResultHandler -= HandleHaulJobResult;
                CreateHaulingJobs();
            }
        }
    }

    private void CreateHaulingJobs() {
        foreach (var ingredient in _ingredients) {
            for (int i = ingredient.count; i > 0; i--) {
                CreateHaulingJobForIngredient(SearchEngine.GetTypeDerivativeOf<Item>(ingredient.itemName));
            }
        }
    }

    private void CreateHaulingJobForIngredient(Type ingredientType) {
        HaulToItemHolderJob job = new HaulToItemHolderJob(ingredientType, this);
        JobSystem.GetInstance().AddJob(job);
        _haulingJobs.Add(job);
        job.JobResultHandler += HandleHaulJobResult;
    }

    //FIX THIS// item count
    private void CalculateNewIngredientsValues(Item newItem) {
        for (int i = _ingredients.Count - 1; i >= 0; i--) {
            _ingredients[i].count--;
            if (_ingredients[i].count <= 0) {
                _ingredients.RemoveAt(i);
            }
        }
    }

    private void HandleHaulJobResult(object source, EventArgs e) {
        if (source is HaulToItemHolderJob) {
            HaulToItemHolderJob job = source as HaulToItemHolderJob;

            job.JobResultHandler -= HandleHaulJobResult;
            _haulingJobs.Remove(job);

            bool result = (e as Job.JobResultEventArgs).result;
            if (result == true) {
                CalculateNewIngredientsValues((source as HaulToItemHolderJob).Item);
                //think of a better solution, also don't forget to stop coroutine incase the job get canceled
                GameManager.GetInstance().StartCoroutine(ChangePlanToActiveState());
            } else {
                CreateHaulingJobForIngredient(job.ItemType);
            }
        }
    }

    //when plan is created the tile, that the plan was placed on stays traversable until at least 1 ingredient comes in
    //afterwards tile's traversability changes to false and plan sprite changes.
    private bool _isChangePlanToActiveStateRunning = false;
    private IEnumerator ChangePlanToActiveState() {
        if (_isChangePlanToActiveStateRunning) {
            yield break;
        }
        _isChangePlanToActiveStateRunning = true;

        if (isTraversable == true) {
            Tile t = Utils.TileAt(position);

            while (t.content.characters.Count > 0) {
                yield return null;
            }

            isTraversable = false;
            t.SetTraversability(false);
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if (_ingredients.Count == 0) {
            CreateConstructionJob();
        }

        _isChangePlanToActiveStateRunning = false;
    }

    private void DeleteJob() {
        foreach (Job job in _haulingJobs) {
            job?.DeleteJob();
        }
        _haulingJobs.Clear();

        _constructionJob?.DeleteJob();
        _constructionJob = null;
    }
}