using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class VegetationTests {
        private Vector2Int _spawnPosition = new Vector2Int(50, 50);

        #region Spawn Tests

        private bool check_staticObject_initialization(StaticObject staticObject) {
            //null check
            if (staticObject is null) {
                Assert.IsTrue(false);
            }
            
            //data check
            if (staticObject.Data is null) {
                Assert.IsTrue(false);
            }
            
            //position check
            if (staticObject.Position != _spawnPosition) {
                Assert.IsTrue(false);
            }

            Tile t = Utils.TileAt(staticObject.Position);
            //tile contents check
            if (t.content.StaticObject != staticObject) {
                Assert.IsTrue(false);
            }
            
            //tile traversability check
            if (t.isTraversable != staticObject.IsTraversable) {
                Assert.IsTrue(false);
            }
            
            //node traversability check
            if (Utils.NodeAt(staticObject.Position).isTraversable != staticObject.IsTraversable) {
                Assert.IsTrue(false);
            }
            
            //gameobject check
            if (staticObject.gameObject is null) {
                Assert.IsTrue(false);
            }
            
            //gameobject position check
            if (staticObject.gameObject.transform.position != Utils.ToVector3(_spawnPosition)) {
                Assert.IsTrue(false);
            }
            
            return true;
        }
        
        [Test]
        public void spawn_vegetation_test() {
            Vegetation vegetation;
            
            vegetation = Factory.Create<Tree>("tall tree", _spawnPosition);
            if (check_staticObject_initialization(vegetation) == false) {
                Assert.IsTrue(false);
            }
            vegetation.Destroy();
            
            vegetation = Factory.Create<Grass>("grass", _spawnPosition);
            if (check_staticObject_initialization(vegetation) == false) {
                Assert.IsTrue(false);
            }
            vegetation.Destroy();

            Assert.IsTrue(true);
        }
        
        #endregion

        #region Destroy Tests

        private bool _wasOnDestroyEventRaised;

        private void StaticObjectDestroyHandler(object source, System.EventArgs e) {
            _wasOnDestroyEventRaised = true;
        }
        
        private bool check_staticObject_destroy(StaticObject staticObject) {
            //check that gameObject is destroyed
            if (staticObject.gameObject != null) {
                return false;
            }
            
            //check that OnDestroy event was raised
            if (_wasOnDestroyEventRaised == false) {
                return false;
            }
            
            //Check tile content
            if (Utils.TileAt(staticObject.Position).content.StaticObject == staticObject) {
                return false;
            }

            return true;
        }
        
        [UnityTest]
        public IEnumerator destroy_vegetation_test() {
            Vegetation vegetation;
            
            vegetation = Factory.Create<Tree>("tall tree", _spawnPosition);
            _wasOnDestroyEventRaised = false;
            vegetation.OnDestroyed += StaticObjectDestroyHandler;
            vegetation.Destroy();
            yield return new WaitForEndOfFrame();
            if (check_staticObject_destroy(vegetation) == false) {
                Assert.IsTrue(false);
            }
            
            vegetation = Factory.Create<Grass>("grass", _spawnPosition);
            _wasOnDestroyEventRaised = false;
            vegetation.OnDestroyed += StaticObjectDestroyHandler;
            vegetation.Destroy();
            yield return new WaitForEndOfFrame();
            if (check_staticObject_destroy(vegetation) == false) {
                Assert.IsTrue(false);
            }
            
            Assert.IsTrue(true);
        }

        #endregion


        #region Harvest & Cut Tests


        #endregion
    }
}
