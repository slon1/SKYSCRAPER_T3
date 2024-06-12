using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Vfx : MonoBehaviour {
	private int timeout = 2333;
	private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
	private CancellationToken token;
	public Animator animator;

	public Action<Vfx> OnDespawned;

	private void OnDestroy() {
		animator = null;
		cancellationTokenSource.Cancel();
		OnDespawned?.Invoke(this);
	}

	public async void Play() {
		token = cancellationTokenSource.Token;

		animator.SetTrigger("Play");
		try {
			await Task.Delay(timeout, token);
		}
		catch (Exception) {
			return;
		}
		finally {
			OnDespawned?.Invoke(this);
		}
	}
}
