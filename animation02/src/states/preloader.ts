module MyGame {

	export class PreloaderState extends Phaser.State {
		private dtc:DTC = new DTC();

		preload() {
			this.game.load.image('logo', 'assets/logo.png');
			this.game.load.image('blank', 'assets/logo.png');
			this.game.load.image('white', 'assets/white.png');
 
			this.game.load.image('sea-floor-front', 'assets/sea-floor/sea-floor-front.png');
			this.game.load.image('sea-floor-back', 'assets/sea-floor/sea-floor-back.png');

			this.game.load.image('starfish-test', 'assets/sea-floor/starfish-test.png');	//delete this afterwards

			for(var i=1;i<=this.dtc.numStarfishLegs;i++)
				this.game.load.image('starfish-leg-' + i, 'assets/sea-floor/starfish/starfish-leg-' + i + '.png');

			for(var i=1;i<=2;i++)
				this.game.load.image('background-' + i, 'assets/background/background-' + i + '.png');
 
			this.game.load.image('test', 'assets/background/test.png');

			for(var i=1;i<=this.dtc.numSkyTypes;i++)
				this.game.load.image('sky-' + this.dtc.doubleDigit(i), 'assets/skies/sky-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numBoatTypes;i++)
				this.game.load.image('boat-' + this.dtc.doubleDigit(i), 'assets/boats/boat-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numSubmarineTypes;i++)
				this.game.load.image('submarine-' + this.dtc.doubleDigit(i), 'assets/submarines/submarine-' + this.dtc.doubleDigit(i) + '.png');
			this.game.load.image('torchlight', 'assets/submarines/torchlight.png');
			this.game.load.image('bubbles', 'assets/submarines/bubbles.png');

			for(var i=1;i<=this.dtc.numSeaTypes;i++)
				this.game.load.image('sea-' + this.dtc.doubleDigit(i), 'assets/sea/sea-' + this.dtc.doubleDigit(i) + '.png');
			
			for(var i=1;i<=this.dtc.numCloudTypes;i++)
				this.game.load.image('cloud-' + this.dtc.doubleDigit(i), 'assets/clouds/cloud-' + this.dtc.doubleDigit(i) + '.png');

			this.game.load.image('wave', 'assets/sea/wave.png');

			this.game.load.image('algae-1', 'assets/sea-floor/algae-1.png');

			this.game.load.image('rock-group-a-rock', 'assets/sea-floor/rock-group-a/rock.png');
			this.game.load.image('rock-group-a-shell', 'assets/sea-floor/rock-group-a/shell.png');
			this.game.load.image('rock-group-a-stone-1', 'assets/sea-floor/rock-group-a/stone-1.png');
			this.game.load.image('rock-group-a-stone-2', 'assets/sea-floor/rock-group-a/stone-2.png');
			this.game.load.image('rock-group-a-algae-small', 'assets/sea-floor/rock-group-a/algae-small.png');
			this.game.load.image('rock-group-a-algae-1', 'assets/sea-floor/rock-group-a/algae-1.png');
			this.game.load.image('rock-group-a-algae-2', 'assets/sea-floor/rock-group-a/algae-2.png');
			this.game.load.image('rock-group-a-algae-3', 'assets/sea-floor/rock-group-a/algae-3.png');
			this.game.load.image('rock-group-a-algae-4', 'assets/sea-floor/rock-group-a/algae-4.png');


			this.game.load.image('rock-group-b-algae-1', 'assets/sea-floor/rock-group-b/algae-1.png');
			this.game.load.image('rock-group-b-algae-purple', 'assets/sea-floor/rock-group-b/algae-purple.png');
			this.game.load.image('rock-group-b-algae-small', 'assets/sea-floor/rock-group-b/algae-small.png');
			this.game.load.image('rock-group-b-rock-1', 'assets/sea-floor/rock-group-b/rock-1.png');
			this.game.load.image('rock-group-b-rock-2', 'assets/sea-floor/rock-group-b/rock-2.png');

			this.game.load.image('volcano', 'assets/volcano/volcano.png');
			this.game.load.image('volcano-smoke', 'assets/volcano/volcano-smoke.png');

			this.game.load.image('underwater-mountains', 'assets/sea-background/underwater-mountains.png');
			this.game.load.image('shipwreck', 'assets/sea-background/shipwreck.png');

			this.load.audio('boat-horn', 'assets/sound/boat-horn.wav', true);

			for(var i=1;i<=this.dtc.numSubmarineSounds;i++)
				this.load.audio('submarine-' + this.dtc.doubleDigit(i), 'assets/sound/submarine-' + this.dtc.doubleDigit(i) + '.wav', true);
		}

		create() {
			this.game.state.start('Game');
		}

	}

}