using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using NaughtyAttributes;
using Debug = UnityEngine.Debug;


[ExecuteInEditMode]
public class LightSource : MonoBehaviour {

	private Mesh mesh;
	private MeshFilter meshFilter;

	public int rayCount = 64;
	public float overfill = 1;
	
	public float range;
	public bool dirty = true;
	public bool isDynamic = false;

	[Range(0, 360)]public float angle = 180;

	// Use this for initialization
	void Start () { meshFilter = GetComponent<MeshFilter>(); }

	// todo: color variable to pass into shader !? 

	public LayerMask layers = new LayerMask(){value = ~0}; 
	
	private bool willRender = true;
	private void OnWillRenderObject() { willRender = true; }

	[ReadOnly][SerializeField]private bool hitPlayer;
	
	// Update is called once per frame
	void LateUpdate () {

		if (isDynamic && transform.hasChanged) {
			dirty = true;
		}
		
		if (dirty && willRender) {
			CalculateLights();
			dirty = false;
		}
		
		if (mesh != null) {
			meshFilter.mesh = mesh;
		}

		willRender = false;

	}
	List<Vector3> vertices = new List<Vector3>();
	List<int> indices = new List<int>();
	List<Vector2> uvs = new List<Vector2>();
	
	[Button]
	void CalculateLights() {
		//var sw = new Stopwatch();
		//sw.Start();
		if(mesh == null) mesh = new Mesh();
		mesh.Clear();

		vertices.Clear();
		indices.Clear();
		uvs.Clear();
		
		vertices.Add(Vector3.zero);
		
		Vector2 dir = Quaternion.Euler(0, 0, -angle / 2) * transform.right;
		uvs.Add(new Vector2(0, 0));
		hitPlayer = false;
		for (int i = 0; i < rayCount; ++i) {
			var hit = Physics2D.Raycast(transform.position, dir, range, layers);
			
			if(hit.collider != null){
				hitPlayer |= hit.collider.gameObject.CompareTag("Player");
			}
			
			var point = hit.collider != null ? hit.point + dir * overfill : ((Vector2)transform.position + dir * range);
			vertices.Add(transform.InverseTransformPoint(point));

			var distance_n = hit.collider != null ? (hit.distance / range)  : 1;
			
			uvs.Add(new Vector2(distance_n, 0));
			
			if (i > 0) {
				indices.AddRange(new []{0, (i), (i+1)});
			}
			
			if (hit.collider != null) {
				// todo: maybe later add better corner detection !? 
			}
			
			
			dir = Quaternion.Euler(0, 0, angle / rayCount) * dir;
		}
		if(angle > 350)
		indices.AddRange(new []{0, (vertices.Count-1), (1)});

		
		
		mesh.SetVertices(vertices);
		mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
		mesh.SetUVs(0, uvs.ToList());
		//sw.Stop();
		//Debug.Log(sw.ElapsedMilliseconds);
	}
}
