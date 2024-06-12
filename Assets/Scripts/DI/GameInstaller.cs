using Utils;
using Zenject;

public class GameInstaller : MonoInstaller {
	public EnemySpawner Spawner;
	public LaserSpawner Laser;
	public InputHandler InputHandler;
	public UIManager uI;
	public MenuManager menu;
	public GameManager gameManager;
	
	public override void InstallBindings() {
		

		Container.Bind<PlayerController>().FromComponentInNewPrefabResource("player").AsSingle();
		Container.Bind<InputHandler>().FromInstance(InputHandler).AsSingle();
		Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
		Container.Bind<UIManager>().FromInstance(uI).AsSingle();
		Container.Bind<MenuManager>().FromInstance(menu).AsSingle();
		Container.Bind<EncryptedStorage>().AsSingle().WithArguments("userdata.txt", "123456789");
	
		Container.Bind<EnemySpawner>().FromInstance(Spawner).AsSingle();
		Container.Bind<LaserSpawner>().FromInstance(Laser).AsSingle();

		Container.BindMemoryPool<EnemyController, EnemyPool>()
				 .WithInitialSize(10)
				 .FromComponentInNewPrefabResource("Alien_mk1")
				 .UnderTransformGroup("Aliens");
		Container.BindMemoryPool<LaserController, LaserControllerPool>()
			.WithInitialSize(10)
			.FromComponentInNewPrefabResource("shot")
			.UnderTransformGroup("Lasers");

		Container.BindMemoryPool<Vfx, VfxPool>()
			.WithInitialSize(5)
			.FromComponentInNewPrefabResource("explo2a_0")
			.UnderTransformGroup("vfx");
		

	}
}
