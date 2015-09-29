using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gDestroyerClass : MonoBehaviour {

	public GameObject dustPrefab;
	public GameObject helper;

	private string destroyerState = "";
	private Vector2 enterPoint;
	private Vector2 exitPoint;
	private List<Vector3>  terrainsTransform = new List<Vector3>();
	private GameObject[]  terrainsGO;

	// Use this for initialization
	void Start () {

		enterPoint = transform.position;
		//enterPoint = new Vector2(0, 0);
		terrainsGO = GameObject.FindGameObjectsWithTag("terrain");
		foreach (GameObject terrain in terrainsGO) {
			Ferr2DT_PathTerrain pathTerrain = terrain.GetComponent<Ferr2DT_PathTerrain>();
			pathTerrain.Build();
			pathTerrain.RecreateCollider();	


		}
	}

	// Update is called once per frame
	void Update () {
		//if (destroyerState == "fly" && !divider.activeSelf) destroyerState = "";
		if (transform.position.magnitude > 10) {
			gameObject.SetActive(false);
		}	
	}

	void OnPress(bool isPressed) {
		if (destroyerState == "" && isPressed) {
			//tutorial
			gHandClass.delHand();
			destroyerState = "active";
			gHintClass.checkHint(gameObject);
		}
		if (destroyerState == "active" && !isPressed) {
			staticClass.useDestroyer = true;
			gRecHintClass.recHint(transform);
			GetComponent<Rigidbody2D>().isKinematic = false;
			//BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
			//collider.size = new Vector2(0.001F, 0.001F);
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(gHintClass.checkHint(gameObject, true));

			Vector3 diff = mousePosition - transform.position;
			//Debug.Log (diff);
			float pointBDiffC = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
			float maxDiffC = 500;
			float diffX = maxDiffC / pointBDiffC * diff.x;
			float diffY = maxDiffC / pointBDiffC * diff.y;
			
			
			GetComponent<Rigidbody2D>().AddForce( new Vector2(diffX, diffY) * 0.5F);
			//Debug.Log (diffX);
			destroyerState = "fly";
			GameObject[] terrains = GameObject.FindGameObjectsWithTag("helper terrain");
			foreach (GameObject terrainH in terrains) Destroy(terrainH);	

		}


		
	}

	void OnDrag() {
		if (destroyerState == "active") {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (gHintClass.checkHint (gameObject, true));
			Vector3 relative = transform.InverseTransformPoint (mousePosition);
			float angle = Mathf.Atan2 (relative.x, relative.y) * Mathf.Rad2Deg;
			//transform.rotation = Quaternion.Euler(0, 0, 180 - angle);
			transform.Rotate (0, 0, 270 - angle);
			//start helper collider
			

			createHelperTerrains(mousePosition);

		}

	}
	void OnTriggerStay2D(Collider2D collider) {
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "terrain") {
			if ( terrainsTransform.Contains(collider.transform.position)) return;
			terrainsTransform.Add(collider.transform.position);

			
			exitPoint = Vector3.MoveTowards(transform.position, enterPoint, -10);

			List<Vector2>  pathVerts = collider.GetComponent<Ferr2D_Path>().pathVerts;
			//pathVerts.AddRange(hit.collider.GetComponent<Ferr2D_Path>().pathVerts);
			
			List<Vector2>  posSort = new List<Vector2>();
			List<int>  pointSort = new List<int>();
			List<Vector2>  firstFigure = new List<Vector2>();
			List<Vector2>  secondFigure = new List<Vector2>();
			
			//возвращает отсортированный массив координат и точек пересечения posSort, pointSort
			getIntersectPoints((Vector2)collider.transform.position, pathVerts, ref posSort, ref pointSort);
			//если нет пересечений, то прекращаем цикл
			//if (posSort.Count == 0) break;
			int k = 0;
			while (posSort.Count >= 2) {
				//подстраховка на цикл while
				k++;
				if (k > 20) break;
				
				//возвращает 2 фигуры firstFigure, secondFigure
				bool flag = getFigures(pathVerts, posSort, ref pointSort, ref firstFigure, ref secondFigure);
				
				int t = pathVerts.Count;
				//создаем новую фигуру и 2 пыли
				if (flag) {
					pathVerts.Clear();
					
					if (getSq(firstFigure) >= getSq(secondFigure)) pathVerts.AddRange(firstFigure);
					else pathVerts.AddRange(secondFigure);
					
					//пыль start
					GameObject dust1 = Instantiate(dustPrefab, new Vector2(0, 0), Quaternion.identity) as GameObject;
					dust1.transform.parent = transform.parent;
					dust1.transform.position = posSort[0]  + new Vector2 (collider.transform.position.x, collider.transform.position.y);
					dust1.transform.localScale = new Vector3(1, 1, 1);
					GameObject dust2 = Instantiate(dustPrefab, new Vector2(0, 0), Quaternion.identity) as GameObject;
					dust2.transform.parent = transform.parent;
					dust2.transform.position = posSort[1]  + new Vector2 (collider.transform.position.x, collider.transform.position.y);
					dust2.transform.localScale = new Vector3(1, 1, 1);
					//пыль end
					
					StartCoroutine(coroutineRecreateTerrain(collider.GetComponent<Ferr2DT_PathTerrain>(), dust1, dust2));
					if (posSort.Count > 3) {
						int g = 0;
						if (pathVerts[1] == posSort[1]) g = 1;
						else g = 0;
						for (int q = 2; q < posSort.Count; q++ ) {
							pointSort[q] = pointSort[q] - pointSort[g] + 1;
							if (pointSort[q] < 0) pointSort[q] += t;
							
							
						}
					}
				}
				

				
				posSort.RemoveAt(0); posSort.RemoveAt(0);
				pointSort.RemoveAt(0); pointSort.RemoveAt(0);
				
				
			}		

			//отсоединяем все линии грута
			for (int i = 0; i < gGrootClass.terrainGrootChains.Count; i++ ) {
				gGrootClass.terrainGrootChain terr = gGrootClass.terrainGrootChains[i];
				if (terr.terrain == collider.gameObject) {
					if (!terr.terrain.GetComponent<Collider2D>().OverlapPoint(terr.chain.transform.position)) {
						terr.chain.transform.parent.SendMessage("OnMouseDown");
						gGrootClass.terrainGrootChains.Remove(terr);
						i--;
					}
				}
			}
		}
	}
	IEnumerator coroutineRecreateTerrain(Ferr2DT_PathTerrain pathTerrain, GameObject dust1, GameObject dust2) {
		yield return new WaitForSeconds(0.1F);
		pathTerrain.GetComponent<Ferr2DT_PathTerrain>().Build();
		pathTerrain.GetComponent<Ferr2DT_PathTerrain>().RecreateCollider();	
		yield return new WaitForSeconds(0.5F);
		Destroy (dust1);
		Destroy (dust2);

	}

	void createHelperTerrains (Vector3 mousePosition) {

		//удаляем helper terrains
		GameObject[] terrains = GameObject.FindGameObjectsWithTag("helper terrain");
		foreach (GameObject terrainH in terrains) Destroy(terrainH);	
		exitPoint = Vector3.MoveTowards(mousePosition, transform.position, -10);
		//ищем пересечения со всеми террейнами
		LayerMask mask = LayerMask.GetMask ("terrains");
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, mousePosition - transform.position, 10, mask); 

		//для каждого попадания:
		foreach (RaycastHit2D hit in hits) {

			List<Vector2>  pathVerts =  new List<Vector2>();
			pathVerts.AddRange(hit.collider.GetComponent<Ferr2D_Path>().pathVerts);

			List<Vector2>  posSort = new List<Vector2>();
			List<int>  pointSort = new List<int>();
			List<Vector2>  firstFigure = new List<Vector2>();
			List<Vector2>  secondFigure = new List<Vector2>();

			//возвращает отсортированный массив координат и точек пересечения posSort, pointSort
			getIntersectPoints((Vector2)hit.transform.position, pathVerts, ref posSort, ref pointSort);
			int k = 0;
			while (posSort.Count >= 2) {
				//подстраховка на цикл while
				k++;
				if (k > 20) break;

				//возвращает 2 фигуры firstFigure, secondFigure
				bool flag = getFigures(pathVerts, posSort, ref pointSort, ref firstFigure, ref secondFigure);

				int t = pathVerts.Count;
				//создаем helper terrain, если 2 фигуры нормальные
				if (flag) {
					GameObject tempTerrain = Instantiate(helper, new Vector2(0, 0), Quaternion.identity) as GameObject;
					tempTerrain.transform.position = new Vector2 (hit.collider.transform.position.x, hit.collider.transform.position.y);
					tempTerrain.transform.localScale = new Vector3(1, 1, 1);
					pathVerts.Clear();
					tempTerrain.GetComponent<Ferr2D_Path>().pathVerts.Clear();
					if (getSq(firstFigure) < getSq(secondFigure)) {
						tempTerrain.GetComponent<Ferr2D_Path>().pathVerts.AddRange(firstFigure);
						pathVerts.AddRange(secondFigure);
					} else {
						tempTerrain.GetComponent<Ferr2D_Path>().pathVerts.AddRange(secondFigure);
						pathVerts.AddRange(firstFigure);
					}
					tempTerrain.GetComponent<Ferr2DT_PathTerrain>().Build();

					if (posSort.Count > 3) {
						int g = 0;
						if (pathVerts[1] == posSort[1]) g = 1;
						else g = 0;
						for (int q = 2; q < posSort.Count; q++ ) {
							pointSort[q] = pointSort[q] - pointSort[g] + 1;
							if (pointSort[q] < 0) pointSort[q] += t;
						}
					}
				}


				
				posSort.RemoveAt(0); posSort.RemoveAt(0);
				pointSort.RemoveAt(0); pointSort.RemoveAt(0);


			}		


		}
	}


	//возвращает отсортированный массив координат и точек пересечения posSort, pointSort
	void getIntersectPoints (Vector2 terrainTrPos, List<Vector2>  pathVerts, ref List<Vector2> posSort, ref List<int> pointSort) {
		List<Vector2>  pos = new List<Vector2>();
		List<int>  point = new List<int>();
		Vector2 posTemp, posA, posB;
		posSort.Clear(); pointSort.Clear(); 
		//ищем пересечения и записываем в pos координаты, а в point номера точек
		for (int i = 0; i < pathVerts.Count; i++) {
			
			if (i == pathVerts.Count - 1) {
				posA = pathVerts[0];
				posB = pathVerts[i];
			} else {
				posA = pathVerts[i + 1];
				posB = pathVerts[i];
			}
			posTemp = lineIntersectPos(enterPoint - terrainTrPos, exitPoint - terrainTrPos, posA, posB);
			if (posTemp.x != 10000) {
				if (Mathf.Abs(posTemp.x - posA.x) <= 0.02F && Mathf.Abs(posTemp.y - posA.y) <= 0.02F) posTemp = posA;
				if (Mathf.Abs(posTemp.x - posB.x) <= 0.02F && Mathf.Abs(posTemp.y - posB.y) <= 0.02F) posTemp = posB;
				pos.Add(posTemp);
				point.Add(i);
			}
		}
		
		//sorting
		float minLength = 1000;
		int j = -1;
		for (int i = 0; i < pos.Count; i++ ) {
			for (int y = 0; y < pos.Count; y++ ) {
				if ((pos[y] - (enterPoint - terrainTrPos)).magnitude < minLength) {
					minLength = (pos[y] - (enterPoint - terrainTrPos)).magnitude;
					j = y;
				}
			}
			minLength = 1000;
			posSort.Add(pos[j]);
			pos[j] = new Vector2(1000, 1000);
			pointSort.Add(point[j]);
		}
	}

	//возвращает 2 фигуры firstFigure, secondFigure
	bool getFigures (List<Vector2>  pathVerts, List<Vector2> posSort, ref List<int> pointSort, ref List<Vector2> firstFigure, ref List<Vector2> secondFigure) {
		//обнуление
		firstFigure.Clear(); secondFigure.Clear();
		bool flag = true;
		int i = -1;
		int k = 0;
		while (flag){	
			if (i == -1) {
				firstFigure.Add(posSort[0]);
				firstFigure.Add(posSort[1]);
				i = pointSort[1] + 1;
			}
			if (i >= pathVerts.Count) i = 0;
			if (i == pointSort[0]) flag = false;
			if (pathVerts[i] != posSort[0] && pathVerts[i] != posSort[1]) {
				firstFigure.Add(pathVerts[i]);
			}
			i ++;
			if (k > pathVerts.Count) {
				//for (int h = 0 ; h< pathVerts.Count; h++) Debug.Log("pathVerts: "+ h+" h: " + pathVerts[h]);
				//flag = false;
				return false;
			}
			k++;
			
		}

		flag = true;
		i = -1;
		k = 0;
		while (flag){
			if (i == -1) {
				secondFigure.Add(posSort[1]);
				secondFigure.Add(posSort[0]);
				i = pointSort[0] + 1;
			}
			if (i >= pathVerts.Count) i = 0;
			if (i == pointSort[1]) flag = false;
			if (pathVerts[i] != posSort[0] && pathVerts[i] != posSort[1]) {
				secondFigure.Add(pathVerts[i]);
			}
			i ++;
			if (k > pathVerts.Count) {
				return false;
				//flag = false;
			}
			k++;
		}
		if (firstFigure.Count < 3 || secondFigure.Count < 3) return false;


		return true;

	}
	/// <summary>
	/// Checks if a line between p0 to p1 intersects a line between p2 and p3
	///
	/// returns the point of intersection
	/// </summary>
	public static Vector2 lineIntersectPos(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float s1_x, s1_y, s2_x, s2_y;
		s1_x = p1.x - p0.x;
		s1_y = p1.y - p0.y;
		s2_x = p3.x - p2.x;
		s2_y = p3.y - p2.y;
		float s, t;
		s = (-s1_y * (p0.x - p2.x) + s1_x * (p0.y - p2.y)) / (-s2_x * s1_y + s1_x * s2_y);
		t = (s2_x * (p0.y - p2.y) - s2_y * (p0.x - p2.x)) / (-s2_x * s1_y + s1_x * s2_y);
		if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
		{
			// Collision detected
			return new Vector2(p0.x + (t * s1_x), p0.y + (t * s1_y));
		}
		return new Vector2(10000, 10000); // No collision
	}

	// Расчет площади многоугольника через сумму площадей трапеций 
	public static float getSq (List<Vector2>  figure) {
		//Debug.Log("Figure");
		int n = figure.Count;
		float s = 0;
		float res = 0;
		for (int i = 0; i < n; i++) {
			if (i == 0) {
				s = figure[i].x*(figure[n-1].y - figure[i+1].y); //если i == 0, то y[i-1] заменяем на y[n-1]
				res += s;
			}	else
				if (i == n-1) {
				s = figure[i].x*(figure[i-1].y - figure[0].y); // если i == n-1, то y[i+1] заменяем на y[0]
					res += s;
				} else {
				s = figure[i].x*(figure[i-1].y - figure[i+1].y);
					res += s;
				}
			//Debug.Log(figure[i]);
		}
		//Debug.Log("sq: " + Mathf.Abs(res/2));
		return Mathf.Abs(res/2);
	}
}
