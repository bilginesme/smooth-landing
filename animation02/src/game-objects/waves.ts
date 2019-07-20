module MyGame {
    export class Waves extends Phaser.Sprite {
        private dtc:DTC;
        private miliSecondsForAnotherWave:number = 4000;
        private imgWave: Phaser.Sprite;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.imgWave = this.game.add.sprite(game.width / 2, 39, 'wave');
            this.imgWave.alpha = 1;
            this.imgWave.anchor.setTo(0.5, 0);

            this.createWave();
        }

        preload() { }
        update() { }

        private createWave() {
            var scaleMinimum:number = this.game.rnd.realInRange(0.75, 0.97);
            var msHalfWave = this.miliSecondsForAnotherWave / 2;

            var tweenScalePrimary = this.game.add.tween(this.imgWave.scale).to({ x: 1, y: scaleMinimum }, msHalfWave, Phaser.Easing.Sinusoidal.InOut, true);
            tweenScalePrimary.onComplete.add(function () { 
                var tweenScaleSecondary = this.game.add.tween(this.imgWave.scale).to({ x: 1, y: 1 }, msHalfWave, Phaser.Easing.Sinusoidal.InOut, true);
            }, this);

            setTimeout(() => this.createWave(), this.miliSecondsForAnotherWave);
        }
    }
}