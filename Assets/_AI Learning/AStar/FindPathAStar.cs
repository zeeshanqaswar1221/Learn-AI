using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astar.BasicAStar
{
    public class FindPathAStar : MonoBehaviour
    {
        
        [Serializable]
        public class PathMarker:IComparable{

            public MapLocation location;
            public GameObject marker;

            public float G;
            public float H;
            public float F;

            public PathMarker parent;

            public PathMarker(float g, float h, float f, GameObject marker, MapLocation location, PathMarker p){
                G = g;
                H = h;
                F = f;
                parent = p;
                this.marker = marker;
                this.location = location;
            }

            public int CompareTo(object obj)
            {
                var temp = (PathMarker) obj;
                if (this.F > temp.F)
                {
                    return 1;
                }
                if (this.F < temp.F)
                {
                    return -1;
                } 

                return 0;
            }

            public override bool Equals(object obj)
            {
                if(obj != null || this.GetType().Equals(obj.GetType())){ 
                    return this.location == ((PathMarker)obj).location;
                }

                return false;
            }

        }

        public Maze maze;
        public Material openMaterial;
        public Material closeMaterial;

        public List<PathMarker> opened = new List<PathMarker>(), closed = new List<PathMarker>();

        public GameObject goal;
        public GameObject start;
        public GameObject path;

        public PathMarker startNode, endNode, lastNode;

        public bool done;


        private void Start() {
            Invoke("BeginSearch", 0.5f);
        }

        int loopSafety = 0;
        private void BeginSearch(){

            done = false;
            RemoveAllMarkers();

            List<MapLocation> locations = new List<MapLocation>();

            for (int z = 0; z < maze.depth; z++)
            {
                for (int x = 0; x < maze.width; x++)
                {
                    if (maze.map[x,z] != 1)
                    {
                        locations.Add(new MapLocation(x,z));
                    }
                }
            }

            locations.Shuffle();

            Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0f, locations[0].z * maze.scale);
            startNode = new PathMarker(0,0,0, Instantiate(start, startLocation, Quaternion.identity), new MapLocation(locations[0].x, locations[0].z),null);


            Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0f, locations[1].z * maze.scale);
            endNode = new PathMarker(0,0,0, Instantiate(goal, goalLocation, Quaternion.identity), new MapLocation(locations[1].x, locations[1].z),null);

            opened.Clear();
            closed.Clear();
            opened.Add(startNode);
            lastNode = startNode;

            StartCoroutine( ExploreNodes());
            
        }

        IEnumerator ExploreNodes(){

            while(opened.Count != 0 && !done && loopSafety < 5000){
                opened.Sort();
                yield return DoSearch(opened[0]);
                loopSafety++;
            }
            
            yield return DrawPath();
        }


        IEnumerator DrawPath()
        {
            RemoveAllMarkers();
            
            PathMarker pathNode = lastNode;
            while (pathNode != null)
            {
                Vector3 newPos = new Vector3(pathNode.location.x *  maze.scale, 0f, pathNode.location.z * maze.scale);
                Instantiate(pathNode.marker, newPos, Quaternion.identity).GetComponent<Renderer>().sharedMaterial = openMaterial;
                // print($"Node: {pathNode.location.x} {pathNode.location.z}");
                pathNode = pathNode.parent;
            }
            
            yield return null;
        }

        private bool ContainsInOpenAndClose(MapLocation node)
        {
            bool result = false;
            foreach (var openedNodes in opened)
            {
                if (openedNodes.location.x == node.x && openedNodes.location.z == node.z)
                {
                    result = true;
                }
            }

            foreach (var closedNodes in closed)
            {
                if (closedNodes.location.x == node.x && closedNodes.location.z == node.z)
                {
                    result = true;
                }
            }
            
            return result;
        }

        IEnumerator DoSearch(PathMarker node){

            opened.Remove(node);
            lastNode = node;
            node.marker.GetComponent<Renderer>().sharedMaterial = closeMaterial;
            closed.Add(node);

            List<MapLocation> rawLocations = new List<MapLocation>();
            
            rawLocations.Add(node.location + maze.directions[0]);
            rawLocations.Add(node.location + maze.directions[1]);
            rawLocations.Add(node.location + maze.directions[2]);
            rawLocations.Add(node.location + maze.directions[3]);

            // Remove Walls
            List<MapLocation> refinedLocations = new List<MapLocation>();
            foreach (var unKnownNode in rawLocations)
            {
                if (maze.map[unKnownNode.x,unKnownNode.z] != 1)
                {
                    refinedLocations.Add(unKnownNode);
                }
            }

            foreach (var tNode in refinedLocations)
            {
                if (!ContainsInOpenAndClose(tNode))
                {
                    float g = Mathf.Sqrt(Mathf.Pow(node.location.x - tNode.x, 2) + Mathf.Pow(node.location.z - tNode.z, 2));
                    float h = Mathf.Sqrt(Mathf.Pow(endNode.location.x - tNode.x, 2) + Mathf.Pow(endNode.location.z - tNode.z, 2));
                    float f = g+h;
                    Vector3 pos = new Vector3(tNode.x * maze.scale, 0f, tNode.z * maze.scale);
                    
                    Transform go = Instantiate(path, pos , Quaternion.identity).transform;
                    go.GetChild(0).GetComponent<TextMesh>().text = $"G: {g}";
                    go.GetChild(1).GetComponent<TextMesh>().text = $"H: {h}";
                    go.GetChild(2).GetComponent<TextMesh>().text = $"F: {f}";
                    go.name = $"Path {tNode.x} , {tNode.z}";
                    
                    var marker = new PathMarker(g, h, f, go.gameObject, tNode, node);
                    
                    opened.Add(marker);
                    
                    if (endNode.location.x == tNode.x && endNode.location.z == tNode.z)
                    {
                        done = true;
                        print("PathFound");
                        lastNode = marker;
                        break;
                    }
                    
                    yield return new WaitForSeconds(1f); 
                }
            }

        } 

        private void RemoveAllMarkers(){
            GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

            for (int i = 0; i < markers.Length; i++)
            {
                Destroy(markers[i]);
            }
        }

    }
}

