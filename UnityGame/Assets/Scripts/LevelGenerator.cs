using UnityEngine;
using System.Collections;

/*
Карта.
Карта генерируется из 12 кусков, размерностью 7,5х7,5 метра, образуя собой вытянутую единое поле. Каждый кусок стыкуется с другим,
 образуя собой непрерывную локацию (для прототипа подойдёт одна локация - переходы на другие не нужны).
Таким образом мы получим вытянутую локацию 30х22,5 метра.

*/

public class LevelGenerator : MonoBehaviour 
{
	const float groundUnitSize = 7.5f;
	const int groundWidth = 4;
	const int groundHeight = 3;
	const int z = 1;

	[SerializeField] GameObject groundPrefab;

	private void Start()
	{
		Vector3 leftUp = new Vector3(-groundWidth*groundUnitSize/2, -groundHeight*groundUnitSize/2, 0);
		for (int i = 0; i < groundWidth; i++)
		{
			for (int j = 0; j < groundHeight; j++)
			{
				var g = Instantiate(groundPrefab, leftUp + new Vector3(i*groundUnitSize, j*groundUnitSize, z), Quaternion.identity) as GameObject;
				g.transform.SetParent(this.transform);
				g.name = string.Format("ground[{0}][{1}]",i,j); 
			}
		}
	}
}
