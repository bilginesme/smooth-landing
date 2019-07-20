module MyGame {

	export class PreloaderState extends Phaser.State {
		private dtc:DTC = new DTC();

		preload() {
			this.game.load.image('logo', 'assets/logo.png');
			this.game.load.image('blank', 'assets/logo.png');
			this.game.load.image('white', 'assets/white.png');
			this.game.load.image('black', 'assets/black.png');

			for(var i=1;i<=this.dtc.numVehiclesTypesLeftCar;i++)
				this.game.load.image('car-lr-' + this.dtc.doubleDigit(i), 'assets/road/car-lr-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesRightCar;i++)
				this.game.load.image('car-rl-' + this.dtc.doubleDigit(i), 'assets/road/car-rl-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesLeftBus;i++)
				this.game.load.image('bus-lr-' + this.dtc.doubleDigit(i), 'assets/road/bus-lr-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesRightBus;i++)
				this.game.load.image('bus-rl-' + this.dtc.doubleDigit(i), 'assets/road/bus-rl-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesLeftMinibus;i++)
				this.game.load.image('minibus-lr-' + this.dtc.doubleDigit(i), 'assets/road/minibus-lr-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesRightMinibus;i++)
				this.game.load.image('minibus-rl-' + this.dtc.doubleDigit(i), 'assets/road/minibus-rl-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesLeftTruck;i++)
				this.game.load.image('truck-lr-' + this.dtc.doubleDigit(i), 'assets/road/truck-lr-' + this.dtc.doubleDigit(i) + '.png');
			for(var i=1;i<=this.dtc.numVehiclesTypesRightTruck;i++)
				this.game.load.image('truck-rl-' + this.dtc.doubleDigit(i), 'assets/road/truck-rl-' + this.dtc.doubleDigit(i) + '.png');

			this.game.load.image('distant-mountains', 'assets/distant-scene/distant-mountains.png');
			for(var i=1;i<=this.dtc.numDistantCloudTypes;i++)
				this.game.load.image('distant-clouds-' + this.dtc.doubleDigit(i), 'assets/distant-scene/distant-clouds-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numLocomotives;i++)
				this.game.load.image('locomotive-' + this.dtc.tripleDigit(i), 'assets/train/locomotives/locomotive-' + this.dtc.tripleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numRailcarsPassenger;i++)
				this.game.load.image('railcar-' + this.dtc.tripleDigit(i), 'assets/train/railcars/railcar-' + this.dtc.tripleDigit(i) + '.png');

			for(var i=601;i<=601 + this.dtc.numRailcarsRestaurant;i++)
				this.game.load.image('railcar-' + this.dtc.tripleDigit(i), 'assets/train/railcars/railcar-' + this.dtc.tripleDigit(i) + '.png');

			this.game.load.image('railroad', 'assets/train/railroad.png');
			this.game.load.image('scene-back', 'assets/train/scene-back.png');
			this.game.load.image('scene-front', 'assets/train/scene-front.png');

			this.game.load.image('windmill-building', 'assets/windmill/windmill-building.png');
			this.game.load.image('windmill-mill', 'assets/windmill/windmill-mill.png');

			for(var i=1;i<=this.dtc.numSkyTypes;i++)
				this.game.load.image('sky-' + this.dtc.doubleDigit(i), 'assets/skies/sky-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=4;i++)
				this.game.load.image('front-to-back-' + i, 'assets/background/front-to-back-' + i + '.png');

			for(var i=1;i<=this.dtc.numCloudTypes;i++)
				this.game.load.image('cloud-' + this.dtc.tripleDigit(i), 'assets/clouds/cloud-' + this.dtc.tripleDigit(i) + '.png');

			for(var i=1;i<=3;i++)
				this.game.load.image('baloon-' + i, 'assets/baloons/baloon-' + i + '.png');

			this.load.atlasJSONHash('tractor-01', './assets/tractors/tractor-01.png', './assets/tractors/tractor-01.json');

			for(var i=1;i<=this.dtc.numPlaneTypes;i++)
				this.game.load.image('plane-' + this.dtc.doubleDigit(i), 'assets/planes/plane-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numPlaneTrailTypes;i++)
				this.game.load.image('plane-trail-' + this.dtc.doubleDigit(i), 'assets/planes/plane-trail-' + this.dtc.doubleDigit(i) + '.png');

			for(var i=1;i<=this.dtc.numMistTypes;i++)
				this.game.load.image('mist-' + this.dtc.doubleDigit(i), 'assets/mist/mist-' + this.dtc.doubleDigit(i) + '.png');

			this.load.audio('train-whistle', 'assets/sound/train-whistle.wav', true);
			this.load.audio('propeller-plane-fly-by', 'assets/sound/propeller-plane-fly-by.wav', true);
			for(var i=1;i<=4;i++)
				this.load.audio('car-drive-by-' + this.dtc.doubleDigit(i), 'assets/sound/car-drive-by-' + this.dtc.doubleDigit(i) + '.wav', true);

			for(var i=1;i<=this.dtc.numPlaneSounds;i++)
				this.load.audio('plane-' + this.dtc.doubleDigit(i), 'assets/sound/plane-' + this.dtc.doubleDigit(i) + '.wav', true);

			this.load.atlasJSONHash('moon-phases', './assets/moon/moon-phases.png', './assets/moon/moon-phases.json');	
		}

		create() {
			this.game.state.start('Game');
		}

	}

}