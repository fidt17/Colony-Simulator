using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests {
	public class _SceneLoaderForUnitTests {
		private bool _wasGameLoaded = false;

		[OneTimeSetUp]
		public void LoadScene() {
			Debug.Log("Loading test scene...");
			SceneManager.LoadScene("Unit Testing Scene");
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			_wasGameLoaded = true;
		}

		[UnityTest]
		public IEnumerator scene_was_loaded_successfuly() {
			while (_wasGameLoaded == false) {
				yield return null;
			}

			Assert.IsTrue(_wasGameLoaded);
		}
	}
}