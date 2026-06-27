using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Car_Path : MonoBehaviour {

	public Color LineColor;

	private List<Transform> nodes = new List<Transform>();

	void OnDrawGizmosSelected(){ // instead of update function -> watch in editor
		Gizmos.color = LineColor;

		Transform[] Pathtransforms = transform.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();

		//loop through array
		for(int i = 0; i < Pathtransforms.Length; i++){
			if(Pathtransforms[i] != transform){
				nodes.Add(Pathtransforms[i]);

			}
		}

		for(int i=0; i < nodes.Count; i++){
			Vector3 currentnode = nodes[i].position;
			Vector3 previousnode = Vector3.zero;

			if (i>0) {
				previousnode = nodes[i-1].position; //stick to last node instead of -1
			}
			else if(i == 0 && nodes.Count > 1){ //having at least two nodes
				previousnode = nodes[nodes.Count - 1].position;
			}
		
			Gizmos.DrawLine(previousnode,currentnode);
			Gizmos.DrawWireSphere(currentnode, 0.3f);

		}



	}




}
