using System;
using System.Collections;

namespace Armiz
{
	public abstract class State
	{
		protected GameController gameController;

		public State(GameController _gameController)
		{
			gameController = _gameController;
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
