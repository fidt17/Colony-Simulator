using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
	public class CharacterTests {
		private Vector2Int _spawnPosition = new Vector2Int(50, 50);

		#region Spawn

		private bool check_character_initialization(Character character) {
			//null test
			if (character is null) {
				return false;
			}

			//data test
			if (character.Data is null) {
				return false;
			}

			//GameObject test
			if (character.gameObject == null) {
				return false;
			}

			#region Component Tests

			//Selectable component
			if (character.SelectableComponent.CheckInitialization() == false) {
				return false;
			}

			//Motion component
			if (character.MotionComponent.CheckInitialization(_spawnPosition) == false) {
				return false;
			}

			//Hunger component
			if (character.HungerComponent.CheckInitialization() == false) {
				return false;
			}

			//AI component
			if (character.AI.CheckInitialization() == false) {
				return false;
			}

			#endregion

			//Tile test
			if (Utils.TileAt(_spawnPosition).Contents.Characters.Contains(character) == false) {
				return false;
			}

			return true;
		}

		[Test]
		public void spawn_human_test() {
			Human human = Factory.Create<Human>("human", _spawnPosition);

			//Base character test
			if (check_character_initialization(human) == false) {
				Assert.Fail();
			}

			#region Unique components

			//Motion animator
			if (human.MotionAnimator.CheckInitialization() == false) {
				Assert.Fail();
			}

			//Job handler component
			if (human.JobHandlerComponent.CheckInitialization() == false) {
				Assert.Fail();
			}

			//Inventory component
			if (human.InventoryComponent.CheckInitialization() == false) {
				Assert.Fail();
			}

			#endregion

			human.Die();
			Assert.Pass();
		}

		[Test]
		public void spawn_rabbit_test() {
			Rabbit rabbit = Factory.Create<Rabbit>("rabbit", _spawnPosition);

			//Base character test
			if (check_character_initialization(rabbit) == false) {
				Assert.Fail();
			}

			//Motion animator
			if (rabbit.MotionAnimator.CheckInitialization() == false) {
				Assert.Fail();
			}

			rabbit.Die();
			Assert.Pass();
		}

		#endregion

		#region Death

		public bool check_character_death(Character character) {
			//Verify that all tile doesn't have information about the character anymore
			if (Utils.TileAt(_spawnPosition).Contents.Characters.Contains(character)) {
				return false;
			}

			//Verify that all character components are disabled
			List<CharacterComponent> components = new List<CharacterComponent>() {
				//character.SelectableComponent,
				character.MotionComponent,
				character.HungerComponent
			};

			foreach (CharacterComponent component in components) {
				if (component.IsDisabled == false) {
					return false;
				}
			}

			//Verify that character AI controller disables

			//Verify that gameObject is destroyed.
			if (character.gameObject != null) {
				return false;
			}

			return true;
		}

		[UnityTest]
		public IEnumerator kill_human_test() {
			Human human = Factory.Create<Human>("human", _spawnPosition);
			human.Die();
			yield return new WaitForEndOfFrame();

			//Common character checks
			if (check_character_death(human) == false) {
				Assert.Fail();
			}

			//Verify that all components are disabled.
			List<CharacterComponent> components = new List<CharacterComponent>() {
				human.MotionAnimator,
				human.JobHandlerComponent,
				human.InventoryComponent,
			};

			foreach (CharacterComponent component in components) {
				if (component.IsDisabled == false) {
					Assert.Fail();
				}
			}

			Assert.Pass();
		}

		[UnityTest]
		public IEnumerator kill_rabbit_test() {
			Rabbit rabbit = Factory.Create<Rabbit>("rabbit", _spawnPosition);
			rabbit.Die();
			yield return new WaitForEndOfFrame();

			//Common character checks
			if (check_character_death(rabbit) == false) {
				Assert.Fail();
			}

			//Verify that all components are disabled.
			List<CharacterComponent> components = new List<CharacterComponent>() {
				rabbit.MotionAnimator,
			};

			foreach (CharacterComponent component in components) {
				if (component.IsDisabled == false) {
					Assert.Fail();
				}
			}

			Assert.Pass();
		}

		#endregion

		#region Selection

		[Test]
		public void human_on_select_must_switch_to_move_command_input_state() {
			Human human = Factory.Create<Human>("human", _spawnPosition);
			human.Select();

			if (CommandInputStateMachine.currentCommandState.GetType() != typeof(MoveCommandInputState)) {
				Assert.Fail();
			}
			
			human.Die();
			Assert.Pass();
		}

		#endregion
	}
}