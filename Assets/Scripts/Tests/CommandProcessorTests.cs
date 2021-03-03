using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Pathfinding;
using UnityEngine;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

namespace Tests
{
    public class CommandProcessorTests
    {
        private Vector2Int _spawnPosition = new Vector2Int(50, 50);
        
        private Human CreateHuman() {
            return Factory.Create<Human>("human", _spawnPosition);;
        }

        private void KillCharacter(Character character) {
            character.Die();
        }

        [Test]
        public void command_processor_initialization() {
            var human = CreateHuman();
            var cp = human.AI.CommandProcessor;
            
            if (cp == null) {
                Assert.Fail();
            }
            if (cp.HasTask == true) {
                Assert.Fail();
            }
            
            KillCharacter(human);
            
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator command_processor_destroy() {
            var human = CreateHuman();
            var cp = human.AI.CommandProcessor;
            var ai = human.AI;
            
            KillCharacter(human);
            yield return new WaitForEndOfFrame();
            
            if (cp.HasTask == true) {
                Assert.Fail();
            }

            if (ai.CommandProcessor != null) {
                Assert.Fail();
            }

            if (human.AI.IsDisabled == false) {
                Assert.Fail();
            }
            
            Assert.Pass();
        }

        private IEnumerator test_add_task(Task task, CommandProcessor cp) {
            int defaultCommandCount = task.CommandsCount;
            
            //tasks count should increase by 1 and decrease by 1
            if (cp.TaskCount != 0) {
                Assert.Fail();
            }
            
            //skip 1 frame. so that command processor can start this task
            yield return null;
            
            //task should have wCommand as active command
            if (task.CurrentCommandType == null) {
                Assert.Fail();
            }
            
            //command queue count must decrease by 1
            if (task.CommandsCount != defaultCommandCount - 1) {
                Assert.Fail();
            }
        }

        [UnityTest]
        public IEnumerator add_single_task() {
            var human = CreateHuman();
            var cp = human.AI.CommandProcessor;
            Task task = new Task();
            
            yield return AddTaskAndWaitUntilFinished(task, cp);

            KillCharacter(human);
            Assert.Pass();
        }
        
        [UnityTest]
        public IEnumerator add_multiple_tasks() {
            Human human = CreateHuman();
            CommandProcessor cp = human.AI.CommandProcessor;

            int taskCount = (int) Random.Range(2, 10);
            for (int i = 0; i < taskCount; i++) {
                cp.AddTask(new Task());    
            }

            for (int i = 0; i <= taskCount; i++) {
                yield return null;
            }

            if (cp.HasTask == true) {
                Assert.Fail();
            }
            
            human.Die();
            Assert.Pass();
        }

        [Test]
        public void abort_task() {
            Human human = CreateHuman();
            Task task = new Task();
            
            _taskFinished = false;
            task.ResultHandler += OnTaskFinish;
            human.CommandProcessor.AddTask(task);
            task.AbortTask();

            if (human.CommandProcessor.HasTask == true) {
                Assert.Fail();
            }
            
            human.Die();
            Assert.Pass();
        }
        
        #region Task tests

        private bool _taskFinished;
        private void OnTaskFinish(object source, EventArgs e) {
            ((Task) source).ResultHandler -= OnTaskFinish;
            _taskFinished = true;
        }

        private IEnumerator AddTaskAndWaitUntilFinished(Task task, CommandProcessor cp) {
            _taskFinished = false;
            task.ResultHandler += OnTaskFinish;
            cp.AddTask(task);
            
            yield return test_add_task(task, cp);

            while (_taskFinished == false) {
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator wait_task() {
            Human human = CreateHuman();
            CommandProcessor cp = human.CommandProcessor;

            Task task = new WaitTask(0.1f);

            yield return AddTaskAndWaitUntilFinished(task, cp);

            if (cp.HasTask == true) {
                Assert.Fail();
            }
            
            human.Die();
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator eat_food_task() {
            Human human = CreateHuman();
            Node grassNode = Utils.NodeAt(_spawnPosition + new Vector2Int(0, 10));
            Grass grass = Factory.Create<Grass>("grass", grassNode.Position);
            float defaultHungerLevel = human.HungerComponent.HungerLevel;
            Node targetNode = SearchEngine.FindNodeNear(grassNode, human.MotionComponent.Node);
            var task = new EatFoodTask(human, targetNode, grass);

            yield return AddTaskAndWaitUntilFinished(task, human.CommandProcessor);
            yield return null;
            
            //check position
            if (human.MotionComponent.Node != targetNode) {
                Assert.Fail();
            }
            //check hunger
            if (human.HungerComponent.HungerLevel == defaultHungerLevel) {
                Assert.Fail();
            }
            //check food destroy
            if (grass.gameObject != null) {
                Assert.Fail();
            }
            
            human.Die();
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator move_task() {
            Human human = CreateHuman();
            Vector2Int targetPosition = human.MotionComponent.GridPosition + new Vector2Int(-10, 10);
            var moveTask = new MoveTask(human.MotionComponent,targetPosition);

            yield return AddTaskAndWaitUntilFinished(moveTask, human.CommandProcessor);
            
            //test position
            if (human.MotionComponent.GridPosition != targetPosition) {
                Assert.Fail();
            }
            
            human.Die();
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator haul_task() {
            Human human = CreateHuman();
            Vector2Int itemSpawnPosition = _spawnPosition + new Vector2Int(-10, 0);
            Vector2Int targetPosition = _spawnPosition    - new Vector2Int(-10, 0);
            WoodLog item = Factory.Create<WoodLog>("wood log", itemSpawnPosition);
            Task task = new HaulTask(item, targetPosition, human.MotionComponent, human.InventoryComponent);
            
            yield return AddTaskAndWaitUntilFinished(task, human.CommandProcessor);
            
            //test inventory
            if (human.InventoryComponent.HasItem(item) == true) {
                Assert.Fail();
            }
            //test item position
            if (item.Position != targetPosition) {
                Assert.Fail();
            }
            
            human.Die();
            item.Destroy();
            
            Assert.Pass();
        }

        class ItemHolder : StaticObject, IItemHolder {
            public List<Item> Items { get; } = new List<Item>();
                        
            public void ItemIn(Item item) {
                Items.Add(item);
                item.SetPosition(Position);
                item.gameObject.SetActive(false);
            }

            public Item ItemOut(Item item) {
                Items.Remove(item);
                item.gameObject.SetActive(true);
                return item;
            }
        }

        [UnityTest]
        public IEnumerator haul_to_item_holder_task() {
            Human human = CreateHuman();
            WoodLog item = Factory.Create<WoodLog>("wood log", _spawnPosition);
            Vector2Int holderPosition = _spawnPosition + new Vector2Int(0, 10);
            ItemHolder holder = Factory.Create<ItemHolder>("plank wall", holderPosition);
            Task task = new HaulToItemHolderTask(item, holder, human.MotionComponent, human.InventoryComponent);
            
            yield return AddTaskAndWaitUntilFinished(task, human.CommandProcessor);
            
            //check human position
            if ((human.MotionComponent.GridPosition - holder.Position).magnitude > 1) {
                Assert.Fail();
            }
            //check item position
            if (item.Position != holder.Position) {
                Assert.Fail();
            }
            //check holder inventory
            if (holder.Items.Contains(item) == false) {
                Assert.Fail();
            }
            //check human inventory
            if (human.InventoryComponent.HasItem(item)) {
                Assert.Fail();
            }
            
            human.Die();
            holder.Destroy();
            item.Destroy();
            
            Assert.Pass();
        }

        private class CuttableTestClass : Vegetation { }
        
        [UnityTest]
        public IEnumerator cut_task() {
            Human human = CreateHuman();
            Vector2Int vegetationSpawnPosition = _spawnPosition + new Vector2Int(0, 10);
            ICuttable vegetation = Factory.Create<CuttableTestClass>("tall tree", vegetationSpawnPosition);    
            Vector2Int targetPosition =
                SearchEngine.FindNodeNear(Utils.NodeAt(vegetationSpawnPosition), human.MotionComponent.Node).Position;
            Task task = new CutTask(human.MotionComponent, targetPosition, vegetation);
            
            yield return AddTaskAndWaitUntilFinished(task, human.CommandProcessor);
            yield return null;
            
            //check that ICuttable was destroyed
            if (((StaticObject) vegetation).gameObject != null) {
                Assert.Fail();
            }
            //check tile contents
            Tile t = Utils.TileAt(vegetationSpawnPosition);
            if (t.Contents.StaticObject == ((StaticObject) vegetation)) {
                Assert.Fail();
            }
            //check human position
            if (human.MotionComponent.GridPosition != targetPosition) {
                Debug.Log(human.MotionComponent.GridPosition + " " + targetPosition);
                Assert.Fail();
            }

            human.Die();
            Assert.Pass();
        }
        
        #endregion
    }
}
