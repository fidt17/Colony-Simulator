using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ConstructionPlan : StaticObject, IItemHolder {

    public new ConstructionScriptableObject Data { get; protected set; }
    
    public List<Item>       Items       { get; private set; } = new List<Item>();
    public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();

    private List<Job> _jobs = new List<Job>();
    
    private Coroutine _waitUntilCharacterMovesFromTheTile;
    private bool _wasConstructionCanceled = false;

    public void Build() {
        Destroy();
        Construction newConstruction = Factory.Create<Construction>(Data.construction.dataName, Position);
        //TODO: MOVE ME SOMEWHERE ELSE. find a better place for this check
        if (Data.construction is WallScriptableObject data)
        {
            data.ApplyCorrectWallSprite(newConstruction);
        }
    }
    
    public void SetData(ConstructionScriptableObject data, Vector2Int position) {
        Data = data;
        Position = position;
        IsTraversable = true;
        ConfigureIngredients();
    } 

    public override void SetGameObject(GameObject gameObject) {
        base.SetGameObject(gameObject);
        Utils.TileAt(Position).Contents.SetConstructionPlan(this);
        CheckCurrentTileContents();
    }

    public override void Destroy() {
        GameObject.Destroy(gameObject);
        Utils.TileAt(Position).Contents.RemoveConstructionPlan();
    }

    public void ItemIn(Item item) {
        Items.Add(item);
        item.SetPosition(Position);
        item.gameObject.SetActive(false);
    }

    public Item ItemOut(Item item) {
        if (Items.Contains(item)) {
            Items.Remove(item);
            item.gameObject.SetActive(true);
            return item;
        }
        return null;
    }

    private void ConfigureIngredients() {
        foreach(var dataIngredient in Data.ingredients) {
            Ingredient newIngredient = new Ingredient();
            newIngredient.itemName = dataIngredient.itemName;
            newIngredient.count = dataIngredient.count;
            Ingredients.Add(newIngredient);
        }
    }

    private void CreateConstructionJob() => JobSystem.GetInstance().AddJob(new BuildJob(this));

    private void CheckCurrentTileContents() {
        Tile currentTile = Utils.TileAt(Position.x, Position.y);
        //Removing any existing stockpiles from the tile
        StockpileCreator.RemoveStockpileFromTile(currentTile);

        //there must be no other items or vegetation on the tile
        if (currentTile.Contents.HasItem) {
            RemoveItemFromTileUnderConstructionPlan();
            return;
        } 
        
        if (currentTile.Contents.StaticObject is ICuttable vegetation) {
            Job job = new CutJob(vegetation, Position);
            job.JobResultHandler += OnTileClearJobFinish;
            _jobs.Add(job);
            JobSystem.GetInstance().AddJob(job);
            return;
        }
          
        foreach (var ingredient in Ingredients) {
            for (int i = ingredient.count; i > 0; i--) {
                CreateHaulingJobForIngredient(SearchEngine.GetTypeDerivativeOf<Item>(ingredient.itemName));
            }
        }
    }
    
    private void RemoveItemFromTileUnderConstructionPlan() {
        //find closest free spot
        bool RequirementsFunction(Tile t) => (t is null) ? false : !t.Contents.HasItem;
        Tile haulToTile = SearchEngine.FindClosestTileWhere(Position, RequirementsFunction);
        
        if (haulToTile != null) {
            Job job = new HaulJob(Utils.TileAt(Position).Contents.Item, haulToTile.position);
            _jobs.Add(job);
            JobSystem.GetInstance().AddJob(job);
            job.JobResultHandler += OnTileClearJobFinish;
        } else {
            Debug.LogError("No empty tile to move item to was found. Implement wait function to try again some time later. P: " + Position);
        }
    }

    private void OnTileClearJobFinish(object source, EventArgs e) {
        ((Job) source).JobResultHandler -= HandleHaulJobResult;
        _jobs.Remove((Job) source);
        if (_wasConstructionCanceled == false) {
            CheckCurrentTileContents();
        }
    }

    private void CreateHaulingJobForIngredient(Type ingredientType) {
        HaulToItemHolderJob job = new HaulToItemHolderJob(ingredientType, this);
        JobSystem.GetInstance().AddJob(job);
        _jobs.Add(job);
        job.JobResultHandler += HandleHaulJobResult;
    }

    private void HandleHaulJobResult(object source, EventArgs e) {
        if (_wasConstructionCanceled) {
            return;
        }

        if (source is HaulToItemHolderJob) {
            HaulToItemHolderJob job = source as HaulToItemHolderJob;
            bool result = (e as Job.JobResultEventArgs).result;
            if (result == true) {
                job.JobResultHandler -= HandleHaulJobResult;
                _jobs.Remove(job);
                CalculateNewIngredientsValues((source as HaulToItemHolderJob).Item);
                //think of a better solution, also don't forget to stop coroutine incase the job get canceled
                _waitUntilCharacterMovesFromTheTile = GameManager.GetInstance().StartCoroutine(ChangePlanToActiveState());
            } else {
                CreateHaulingJobForIngredient(job.ItemType);
            }
        }
    }
    
    //TODO: item count
    private void CalculateNewIngredientsValues(Item newItem) {
        for (int i = Ingredients.Count - 1; i >= 0; i--) {
            Ingredients[i].count--;
            if (Ingredients[i].count <= 0) {
                Ingredients.RemoveAt(i);
            }
        }
    }

    //when plan is created, the tile, that the plan was placed on, stays traversable until at least 1 ingredient comes in
    //afterwards tile's traversability changes to false and plan sprite changes.
    private bool _isChangePlanToActiveStateRunning = false;
    private IEnumerator ChangePlanToActiveState() {
        if (_isChangePlanToActiveStateRunning) {
            yield break;
        }
        _isChangePlanToActiveStateRunning = true;

        if (IsTraversable == true) {
            Tile t = Utils.TileAt(Position);

            while (t.Contents.Characters.Count > 0) {
                yield return null;
            }

            IsTraversable = false;
            PutOnTile();
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if (Ingredients.Count == 0) {
            CreateConstructionJob();
        }

        _isChangePlanToActiveStateRunning = false;
    }

    public void CancelPlan() {
        _wasConstructionCanceled = true;
        for (int i = _jobs.Count - 1; i >= 0; i--) {
            _jobs[i].DeleteJob();
        }
        _jobs.Clear();

        if (_waitUntilCharacterMovesFromTheTile != null) {
            GameManager.GetInstance().StopCoroutine(_waitUntilCharacterMovesFromTheTile);
        }
        Destroy();
    }
    
    #region For Testing

    public bool WasPlanCanceledCorrectly() {
        if (_wasConstructionCanceled == false) {
            return false;
        }

        if (_jobs.Count != 0) {
            return false;
        }

        if (gameObject != null) {
            return false;
        }

        return true;
    }
    
    #endregion
}