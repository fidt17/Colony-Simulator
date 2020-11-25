using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class PathfindingTests
    {
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
        public IEnumerator _scene_was_loaded_successfuly() {
            while (_wasGameLoaded == false) {
                yield return null;
            }
            
            Camera.main.orthographicSize = 50;
            //AStarSearch.HandleAddToClosedSet += HandleAddToClosedSet;
            //AStarSearch.HandleAddToOpenSet += HandleAddToOpenSet;
            GeneratePool();

            Assert.IsTrue(_wasGameLoaded);
        }

        private IEnumerator WaitForEnter() {
            while (!Input.GetKeyDown(KeyCode.Return)) {
                yield return null;
            }
        }

        #region Grid Coloring

        private List<GameObject> coloredNodes            = new List<GameObject>();
        
        private enum NodeType {
            Empty,
            Wall,
            Open,
            Close,
            Final
        }

        private Color GetColor(NodeType nodeType) {
            switch (nodeType) {
                case NodeType.Empty:
                    return Color.white;
                    break;
                
                case NodeType.Wall:
                    return Color.black;
                    break;
                
                case NodeType.Open:
                    return Color.yellow;
                    break;
                
                case NodeType.Close:
                    return Color.gray;
                    break;
                
                case NodeType.Final:
                    return Color.green;
                    break;
                
                default:
                    return Color.clear;
                    break;
            }
        }

        private MeshRenderer[] pool;
        
        private void GeneratePool() {
            pool = new MeshRenderer[Utils.MapSize * Utils.MapSize];
            for (int x = 0; x < Utils.MapSize; x++) {
                for (int y = 0; y < Utils.MapSize; y++) {
                    List<PathNode> list = new List<PathNode> {
                        Utils.NodeAt(x, y)
                    };
                    var obj = MeshGenerator.GetInstance().GenerateOverlapAreaOverNodes(list, GetColor(NodeType.Empty))[0];
                    pool[x + y * Utils.MapSize] = obj.GetComponent<MeshRenderer>();
                    obj.name = $"{x};{y}";
                }
            }
        }

        private void ColorNodes(List<PathNode> nodes, NodeType nodeType) {
            int mapSize = Utils.MapSize;
            foreach (PathNode node in nodes) {
                pool[node.x + node.y * mapSize].material.color = GetColor(nodeType);
            }
        }

        private void ColorNode(PathNode node, NodeType nodeType) {
            var list = new List<PathNode> {
                node
            };
            ColorNodes(list, nodeType);
        }

        private void Clear() {
            foreach (MeshRenderer mesh in pool) {
                mesh.material.color = GetColor(NodeType.Empty);
            }
        }
        
        #endregion

        private struct CN {
            public PathNode node;
            public NodeType type;

            public CN(PathNode node, NodeType type) {
                this.node = node;
                this.type = type;
            }
        }

        private List<CN> _nodesToColor = new List<CN>();
        private void HandleAddToOpenSet(PathNode node) {
            _nodesToColor.Add(new CN(node, NodeType.Open));
        }
        
        private void HandleAddToClosedSet(PathNode node) {
            _nodesToColor.Add(new CN(node, NodeType.Close));
        }

        [UnityTest]
        public IEnumerator find_path_and_color() {

            PathNode startNode = Utils.NodeAt(0, 0);
            PathNode targetNode = Utils.NodeAt(99, 99);
            
            List<PathNode> walls = new List<PathNode>();
            int x = 50;
            for (int y = 40; y < 80; y++) {
                Utils.TileAt(x, y).SetTraversability(false);
                walls.Add(Utils.NodeAt(x, y));
            }
            ColorNodes(walls, NodeType.Wall);
            
            /*
             * 3.34 ms
             * 3.33 ms
             */
            
            #region Speed Test
            
                //Normal
                int testCount = 100;
                float avgTime = 0;
                for (int i = 0; i < testCount; i++) {
                    float startTime = Time.realtimeSinceStartup;
                    Pathfinder.GetPath(startNode, targetNode);
                    avgTime += (Time.realtimeSinceStartup - startTime) / testCount;
                }
                Debug.Log($"{testCount} NORMAL iterations finished in: {avgTime * 1000} ms");
                
                //Test
                avgTime = 0;
                for (int i = 0; i < testCount; i++) {
                    float startTime = Time.realtimeSinceStartup;
                    AStarSearch.GetPath(startNode, targetNode);
                    avgTime += (Time.realtimeSinceStartup - startTime) / testCount;
                }
                Debug.Log($"{testCount} TEST iterations finished in: {avgTime * 1000} ms");
            
            #endregion
            
            AStarSearch.HandleAddToOpenSet += HandleAddToOpenSet;
            AStarSearch.HandleAddToClosedSet += HandleAddToClosedSet;
            //var path = Pathfinder.GetPath(startNode, targetNode);
            var path = AStarSearch.GetPath(startNode, targetNode);
                
            //color closed and open set
            foreach (var cn in _nodesToColor) {
                ColorNode(cn.node, cn.type);
                yield return null;
            }
            
            //color path
            foreach (var node in path) {
                ColorNode(node, NodeType.Final);
                yield return null;
            }
            
            yield return WaitForEnter();
            Clear();
        }
        
        
    }
}
