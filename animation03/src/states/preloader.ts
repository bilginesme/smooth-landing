module MyGame {

	export class PreloaderState extends Phaser.State {
		private dtc:DTC = new DTC();

		preload() {
			this.game.load.image('logo', 'assets/logo.png');
			this.game.load.image('blank', 'assets/logo.png');
			this.game.load.image('white', 'assets/white.png');
 
			this.load.atlasJSONHash('rhino-spritesheet', './assets/sprites/rhino-spritesheet.png', './assets/sprites/rhino-spritesheet.json');
			
			this.game.load.image('mountains', 'assets/mountains/mountains.png');
			this.game.load.image('grass', 'assets/grass/grass.png');

			this.game.load.image('volcano', 'assets/volcano/volcano.png');
			this.game.load.image('volcano-smoke', 'assets/volcano/volcano-smoke.png');

			for(var i=1;i<=2;i++)
				this.game.load.image('background-' + i, 'assets/background/background-' + i + '.png');
 
			this.game.load.image('test', 'assets/background/test.png');

			for(var i=1;i<=this.dtc.numSkyTypes;i++)
				this.game.load.image('sky-' + this.dtc.doubleDigit(i), 'assets/skies/sky-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numCloudTypes;i++)
				this.game.load.image('cloud-' + this.dtc.doubleDigit(i), 'assets/clouds/cloud-' + this.dtc.doubleDigit(i) + '.png');

			this.load.audio('boat-horn', 'assets/sound/boat-horn.wav', true);
		}

		create() {
			this.game.state.start('Game');
		}

	}

}