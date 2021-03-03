using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
	public class ConstructionTests {

		private Vector2Int _spawnPosition = new Vector2Int(50, 50);
		string[] constructionPlansToTest = new string[] {
			"wall"
		};
		
		private ConstructionPlan create_construction_plan(string dataName) {
			ConstructionPlan plan = Factory.CreateConstructionPlan("plank wall", _spawnPosition);
			
			//check data + check all fields
			if (plan.Data is null) {
				Assert.Fail();
			}

			if (plan.Data.construction is null) {
				Assert.Fail();
			}

			if (plan.Data.ingredients          == null
			    || plan.Data.ingredients.Count == 0) {
				Assert.Fail();
			}
			
			//check position
			if (plan.Position != _spawnPosition) {
				Assert.Fail();
			}
			
			//check traversability
			if (plan.IsTraversable == false) {
				Assert.Fail();
			}
			
			//check ingredients
			if (plan.Ingredients.Count != plan.Data.ingredients.Count) {
				Assert.Fail();
			}
			
			//check gameobject + position
			if (plan.gameObject == null) {
				Assert.Fail();
			}
			
			//check tile content
			if (Utils.TileAt(plan.Position).Contents.ConstructionPlan != plan) {
				Assert.Fail();
			}			
			
			//check that tile doesn't have a stockpile
			if (Utils.TileAt(plan.Position).Contents.StockpilePart != null) {
				Assert.Fail();
			}
			
			//check that if there was an item on construction plan
			//haul job to remove the item was created and other jobs were not created
			if (Utils.TileAt(plan.Position).Contents.HasItem == true) {
				bool hasHaulJob = false;
				foreach (Job job in JobSystem.GetInstance().AllJobs) {
					if (job.GetType() == typeof(HaulToItemHolderJob)) {
						Assert.Fail();
					}

					if (job.GetType() == typeof(HaulJob)) {
						hasHaulJob = true;
					}
				}

				if (hasHaulJob == false) {
					Assert.Fail();
				}
			} else if (Utils.TileAt(plan.Position).Contents.StaticObject is ICuttable) {

				bool hasCutJob = false;
				foreach (Job job in JobSystem.GetInstance().AllJobs) {
					if (job.GetType() == typeof(HaulToItemHolderJob)) {
						Assert.Fail();
					}

					if (job.GetType() == typeof(CutJob)) {
						hasCutJob = true;
					}
				}

				if (hasCutJob == false) {
					Assert.Fail();
				}
			}

			return plan;
		}

		private IEnumerator cancel_construction_plan(ConstructionPlan plan) {
			plan.CancelPlan();
			yield return null;
			
			if (plan.WasPlanCanceledCorrectly() == false) {
				Assert.Fail();
			}
		}
		
		[UnityTest]
		public IEnumerator create_construction_plan() {
			foreach (string plan in constructionPlansToTest) {
				//yield return cancel_construction_plan(create_construction_plan(plan));
			}

			Item item = Factory.Create<WoodLog>("wood log", _spawnPosition);
			foreach (string plan in constructionPlansToTest) {
				yield return cancel_construction_plan(create_construction_plan(plan));
			}
			item.Destroy();

			Grass grass = Factory.Create<Grass>("grass", _spawnPosition);
			foreach (string plan in constructionPlansToTest) {
				yield return cancel_construction_plan(create_construction_plan(plan));
			}
			grass.Destroy();
			
			Assert.Pass();
		}
		
	}
}