using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
	public class ItemTests {
		private Vector2Int _spawnPosition = new Vector2Int(50, 50);

		#region Spawn Tests

		private bool check_item_initialization<T>(T item, bool positionMustDiffer) where T : Item {
			//check data
			if (item.Data is null) {
				return false;
			}

			//check position
			//if item was spawned in available position
			if (positionMustDiffer == false) {
				if (item.Position != _spawnPosition) {
					Assert.Fail();
				}

				if (item.gameObject.transform.position != Utils.ToVector3(_spawnPosition)) {
					Assert.Fail();
				}
			}
			else {
				if (item.Position == _spawnPosition) {
					Assert.Fail();
				}
			}

			//check tile content
			if (Utils.TileAt(item.Position).Contents.Item != item) {
				Assert.Fail();
			}

			//check region content
			RegionContent rc = Utils.NodeAt(item.Position).subregion.content;
			if (rc.Get<T>().Contains(item) == false) {
				Assert.Fail();
			}

			//check stockpile manager lists
			if (Utils.TileAt(item.Position).Contents.StockpilePart is null) {
				if (StockpileManager.GetInstance().otherItems.Contains(item) == false) {
					Assert.Fail();
				}
			}
			else {
				if (StockpileManager.GetInstance().stockpileItems.Contains(item) == false) {
					Assert.Fail();
				}
			}

			return true;
		}

		[Test]
		public void spawn_item_on_traversable_tile() {
			WoodLog item = Factory.Create<WoodLog>("wood log", _spawnPosition);

			if (check_item_initialization<WoodLog>(item, false) == false) {
				Assert.Fail();
			}

			item.Destroy();
			Assert.Pass();
		}

		[Test]
		public void spawn_item_on_not_traversable_tile() {
			Tile t = Utils.TileAt(_spawnPosition);
			t.SetTraversability(false);
			WoodLog item = Factory.Create<WoodLog>("wood log", _spawnPosition);

			if (check_item_initialization<WoodLog>(item, true) == false) {
				Assert.Fail();
			}

			item.Destroy();
			t.SetTraversability(true);
			Assert.Pass();
		}

		[Test]
		public void spawn_item_on_tile_with_item() {
			WoodLog item1 = Factory.Create<WoodLog>("wood log", _spawnPosition);
			WoodLog item2 = Factory.Create<WoodLog>("wood log", _spawnPosition);

			if (check_item_initialization<WoodLog>(item2, true) == false) {
				Assert.Fail();
			}

			item1.Destroy();
			item2.Destroy();
			Assert.Pass();
		}

		#endregion

		#region Destroy Tests

		private bool _OnDestroyedWasCalled;

		private bool check_item_destroy<T>(T item) where T : Item {
			//gameObject must be destroyed
			if (item.gameObject != null) {
				return false;
			}

			//check that event was rised
			if (_OnDestroyedWasCalled == false) {
				return false;
			}

			//check tile contents
			if (Utils.TileAt(item.Position).Contents.Item == item) {
				return false;
			}

			//check region contents
			if (Utils.NodeAt(item.Position).subregion.content.Get<T>().Contains(item)) {
				return false;
			}

			//check stockpile manager lists
			if (StockpileManager.GetInstance().otherItems.Contains(item)
			    || StockpileManager.GetInstance().stockpileItems.Contains(item)) {
				return false;
			}

			return true;
		}

		[UnityTest]
		public IEnumerator test_item_destroy() {
			WoodLog item = Factory.Create<WoodLog>("wood log", _spawnPosition);
			_OnDestroyedWasCalled =  false;
			item.OnDestroyed      += OnItemDestroy;

			item.Destroy();
			yield return new WaitForEndOfFrame();


			if (check_item_destroy<WoodLog>(item) == false) {
				Assert.Fail();
			}

			Assert.Pass();
		}

		private void OnItemDestroy(object source, System.EventArgs e) {
			_OnDestroyedWasCalled = true;
		}

		#endregion
	}
}