using System;
using System.Collections;

namespace Armiz
{
	public abstract class State
	{
		public State()
		{
			
		}

		public virtual IEnumerator Start()
		{
			yield break;
		}

		public virtual IEnumerator AlliesAttack()
		{
			yield break;
		}

		public virtual IEnumerator EnemiesAttack()
		{
			yield break;
		}
	}
}
