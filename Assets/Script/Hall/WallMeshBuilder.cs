using UnityEngine;
using System.Collections;

public class WallMeshBuilder
{

	public WallMeshBuilder(){

	}

	public GameObject generateWall(int width, int height){
		GameObject wall = new GameObject ();

		MeshRenderer renderer = wall.AddComponent<MeshRenderer> () as MeshRenderer;
		MeshFilter filter = wall.AddComponent<MeshFilter> () as MeshFilter;

		Mesh mesh = filter.mesh;
		mesh.vertices = generateVertices (width, height);
		mesh.normals = generateNormals (width, height);
		mesh.triangles = generateTriangles (width, height);
		mesh.uv = generateUV (mesh.vertices);
		int actual_level = ApplicationData.getLastLevel();
		if (actual_level == 1 || actual_level == 2) {
			renderer.material = Game.GetInstance ().roomMaterial.wallMaterial;
		} else if (actual_level == 3 || actual_level == 4){
			renderer.material = Game.GetInstance ().roomMaterial.wallMaterial2;
		} else if (actual_level == 5) {
			renderer.material = Game.GetInstance ().roomMaterial.wallMaterial3;
		} else {
			renderer.material = Game.GetInstance ().roomMaterial.wallMaterial4;
		}

		return wall;
	}

	private Vector3[] generateVertices(int width, int height){
		int halfv = (width + 1) * (height + 1);
		Vector3[] vs = new Vector3[halfv * 2 + 16];
		float xoffset = width / 2f;
		float yoffset = height / 2f;
		int[] triangles = new int[width * height * 2];
		for (int j=0; j<=height; j++) {
			for (int i=0; i<=width; i++) {
				int index = j*(width+1) + i;
				vs[index] = new Vector3(i - xoffset, j - yoffset, -0.1f);
				vs[index + halfv] = new Vector3(i - xoffset, j - yoffset, 0.1f);
			}
		}
		int count = vs.Length - 16;
		for (int y=0; y<2; y++) {
			for (int z=0; z<2; z++) {
				for (int x=0; x<2; x++) {
					vs[count++] = new Vector3(x*width - xoffset, y*height-yoffset, z*0.2f-0.1f);
				}
			}
		}
		for (int x=0; x<2; x++) {
			for (int y=0; y<2; y++) {
				for (int z=0; z<2; z++) {
					vs[count++] = new Vector3(x*width - xoffset, y*height-yoffset, z*0.2f-0.1f);
				}
			}
		}
		return vs;
	}

	private Vector3[] generateNormals(int width, int height){
		int halfv = (width + 1) * (height + 1);
		Vector3[] normals = new Vector3[halfv * 2 + 16];
		int[] triangles = new int[width * height * 2];
		for (int j=0; j<=height; j++) {
			for (int i=0; i<=width; i++) {
				int index = j*(width+1) + i;
				normals[index] = new Vector3(0f, 0f, -1f);
				normals[index + halfv] = new Vector3(0f, 0f, 1f);
			}
		}
		int count = normals.Length - 16;
		for (int c=0; c<4; c++) {
			normals[count++] = new Vector3(0f, 0f, -1f);
		}
		for (int c=0; c<4; c++) {
			normals[count++] = new Vector3(0f, 0f, 1f);
		}
		for (int c=0; c<4; c++) {
			normals[count++] = new Vector3(-1f, 0f, 0f);
		}
		for (int c=0; c<4; c++) {
			normals[count++] = new Vector3(1f, 0f, 0f);
		}
		return normals;
	}

	private int[] generateTriangles(int width, int height){
		int _widthpo = width + 1;
		int _heightpo = height + 1;
		int halfv = _widthpo * _heightpo;
		int[] triangles = new int[12 * width * height + 24];
		int count = 0;
		for (int j=0; j<height; j++) {
			for(int i=0; i<width; i++){
				int index = j*_widthpo+i;
				triangles[count++] = index;
				triangles[count++] = index+_widthpo;
				triangles[count++] = index+_widthpo+1;
				triangles[count++] = index;
				triangles[count++] = index+_widthpo+1;
				triangles[count++] = index+1;


				triangles[count++] = index + halfv;
				triangles[count++] = index+_widthpo+1 + halfv;
				triangles[count++] = index+_widthpo + halfv;
				triangles[count++] = index + halfv;
				triangles[count++] = index+1 + halfv;
				triangles[count++] = index+_widthpo+1 + halfv;

			}	
		}
		int index_ = halfv*2;
		{
			triangles[count++] = index_;
			triangles[count++] = index_+1;
			triangles[count++] = index_+2;

			triangles[count++] = index_+1;
			triangles[count++] = index_+3;
			triangles[count++] = index_+2;
		}
		index_ += 4;
		{
			triangles[count++] = index_;
			triangles[count++] = index_+2;
			triangles[count++] = index_+1;
			
			triangles[count++] = index_+1;
			triangles[count++] = index_+2;
			triangles[count++] = index_+3;
		}
		index_ += 4;
		{
			triangles[count++] = index_;
			triangles[count++] = index_+1;
			triangles[count++] = index_+2;
			
			triangles[count++] = index_+1;
			triangles[count++] = index_+3;
			triangles[count++] = index_+2;
		}
		index_ += 4;
		{
			triangles[count++] = index_;
			triangles[count++] = index_+2;
			triangles[count++] = index_+1;
			
			triangles[count++] = index_+1;
			triangles[count++] = index_+2;
			triangles[count++] = index_+3;
		}
		return triangles;
	}

	private Vector2[] generateUV(Vector3[] vertices){
		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i=0; i < uvs.Length-16; i++) {
			uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
		}
		for (int i=uvs.Length-16; i < uvs.Length-8; i++) {
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		}
		for (int i=uvs.Length-8; i < uvs.Length; i++) {
			uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
		}
		return uvs;
	}
}

