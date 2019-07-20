module MyGame {
	export class GameState extends Phaser.State {
		private sky: Sky;
		private sea: Sea;
		private waves: Waves;
		private volcano: Volcano;
		private clouds: Cloud[];
		private seaFloor: SeaFloor;
		private seaBackground: SeaBackground;
		private surface: Surface;

		preload() {
			this.game.stage.disableVisibilityChange = true;
		}

		create() {

			this.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;

			this.game.renderer.resize(498, 166);

			/*
				this.scale.pageAlignHorizontally = true;
				this.scale.pageAlignVertically = true;

				this.scale.setResizeCallback(function () {
					var width = window.innerWidth;
					var height = window.innerHeight;
					console.log('size: ' + width + ', ' + height);
					this.camera.setSize(width, height);
					this.game.renderer.resize(width, height);
				}, this);
			*/
    		 
			let test = this.game.add.sprite(0, 0, 'test');
			test.anchor.setTo(0, 0);

			this.sky = new Sky(this.game);
			this.clouds = new Array(4);
            for (var i = 0; i < this.clouds.length; i++) 
				this.clouds[i] = new Cloud(this.game);

			this.sea = new Sea(this.game);
			
			this.seaBackground = new SeaBackground(this.game);
			this.seaFloor = new SeaFloor(this.game);
			this.volcano = new Volcano(this.game, 330, 50);
			this.waves = new Waves(this.game);
			this.surface = new Surface(this.game);
		}
	}

}