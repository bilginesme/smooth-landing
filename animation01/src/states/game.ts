module MyGame {
	export class GameState extends Phaser.State {
		private dtc:DTC;
		private railway: Railway;
		private baloon: Baloon;
		private windmill: Windmill;
		private plane: Plane;
		private distantScene: DistantScene;
		private moon: Phaser.Sprite;
		private sky: Sky;
		private road: Road;
		private clouds: Cloud[];
		private mountains:Mountains;
		private tractorsFront:Tractors;
		private tractorsBack:Tractors;
		private mistBack:Mist;
		private mistMiddle:Mist;
		private mistFront:Mist;
		private mistFrontMost:Mist;

		preload() {
			this.game.stage.disableVisibilityChange = true;
		}

		create() {
			this.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
			this.game.renderer.resize(500, 200);

			this.dtc = new DTC();

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
    		 
			let imgWhite = this.game.add.sprite(0, 0, 'white');
			imgWhite.anchor.setTo(0, 0);

			this.sky = new Sky(this.game);
			this.moon = new Moon(this.game);
			this.plane = new Plane(this.game);
			this.distantScene = new DistantScene(this.game);

			this.clouds = new Array(6);
            for (var i = 0; i < this.clouds.length; i++) 
				this.clouds[i] = new Cloud(this.game);

			this.mountains = new Mountains(this.game);
			//let frontToBack4 = this.game.add.sprite(0, 94, 'front-to-back-4');
			//frontToBack4.anchor.setTo(0, 0);

			let frontToBack3 = this.game.add.sprite(0, this.dtc.yMargin + 108, 'front-to-back-3');
			frontToBack3.anchor.setTo(0, 0);

			this.baloon = new Baloon(this.game);
			this.windmill = new Windmill(this.game, 443, this.dtc.yMargin + 131);

			this.mistBack = new Mist(this.game, this.game.height * 0.9, this.game.height * 1, 0.3);

			let frontToBack2 = this.game.add.sprite(0, this.dtc.yMargin + 114, 'front-to-back-2');
			frontToBack2.anchor.setTo(0, 0);
			
			//this.tractorsBack = new Tractors(this.game, 0.6, 140, 148);	// 140 up

			this.mistMiddle = new Mist(this.game, this.game.height * 1.0, this.game.height * 1.15, 0.4);

			let frontToBack1 = this.game.add.sprite(0, this.dtc.yMargin + 126, 'front-to-back-1');
			frontToBack1.anchor.setTo(0, 0);

			//this.tractorsFront = new Tractors(this.game, 1, 145, 155);

			this.road = new Road(this.game);
			this.mistFront = new Mist(this.game, this.game.height, this.game.height * 1.5, 1);

			this.railway = new Railway(this.game);
			//this.mistFrontMost = new Mist(this.game);
		}

		update() { 
		}

	 
	}

}