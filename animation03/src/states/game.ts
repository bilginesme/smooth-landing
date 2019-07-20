module MyGame {
	export class GameState extends Phaser.State {
		private sky: Sky;
		private clouds: Cloud[];
		private mountains:Mountains;
		private volcano:Volcano;
		private grass:Grass;

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

			this.volcano = new Volcano(this.game, this.game.width / 2, 100);
			this.mountains = new Mountains(this.game);	
			this.grass = new Grass(this.game);	
		}
	}

}