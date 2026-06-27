using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading;
using System.Net; 
using System.Net.Sockets; 
using System.Linq; 
using System; 
using System.IO; 
using System.Text; 
 
public class Vicinity_Check : MonoBehaviour
{
    ///////////// PROBLEM IS THAT Vicinity VARIABLE BECOMES 2 WHEN THERE IS 1 CAR NEARBY
    public float Car_InVicinity = 0;
    public string[] TagList = {"Cars"};

    public List<GameObject> ObjectsInRange = new List<GameObject>();

        public void OnTriggerEnter(Collider col){
            foreach (string TagToAdd in TagList){
                if (col.gameObject.tag == TagToAdd){
                    ObjectsInRange.Add(col.gameObject);
                    Car_InVicinity = Car_InVicinity + 1;
                }
            }
        }
            
        public void OnTriggerExit(Collider col){
            foreach (string TagToAdd in TagList){
                if (col.gameObject.tag == TagToAdd){
                    ObjectsInRange.Remove(col.gameObject);
                    Car_InVicinity = Car_InVicinity - 1;

                }
            }
        }

        

}