using System;
using System.Collections.Generic;

namespace Armiz
{
	[Serializable]
	public class FightersSaveData
	{
		public int enemyLevel;
		public float enemyHealth;
		public List<float> alliesLevel;
		public List<float> alliesHealth;
		public List<aVector3> alliesPosition;

		public static FightersSaveData NullData()
		{
			var fighter = new Fighter();
			var data = new FightersSaveData();

			data.enemyLevel = 1;
			data.enemyHealth = fighter.GetTotalHealth_Enemy(1);

			data.alliesLevel = new List<float>();
			data.alliesLevel.Add(1);

			data.alliesHealth = new List<float>();
			data.alliesHealth.Add(fighter.GetTotalHealth_Ally(1));

			data.alliesPosition = new List<aVector3>();

			return data;
		}
	}

	[Serializable]
	public class aVector3
	{
		public float X = 0;
		public float Y = 0;
		public float Z = 0;

		public aVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}
