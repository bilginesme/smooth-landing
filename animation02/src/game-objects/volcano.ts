module MyGame {
    export class Volcano extends Phaser.Sprite {
        private dtc:DTC;
        private msForAnotherSmoke:number = 300000;
        private msSmokeDurationMin:number = 30000;
        private msSmokeDurationMax:number = 60000;
        private volcano: Phaser.Sprite;
        private volcanoSmoke: Phaser.Sprite;

        constructor(game: Phaser.Game, x:number, y:number) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.volcanoSmoke = this.game.add.sprite(x, y - 45, 'volcano-smoke');
            this.volcanoSmoke.anchor.setTo(0, 0.3);
            this.volcanoSmoke.scale.x = 0;
            this.volcanoSmoke.scale.y = 0;

            this.volcano = this.game.add.sprite(x, y, 'volcano');
            this.volcano.anchor.setTo(0.5, 1);

            this.createSmoke();
        }

        preload() { }
        update() { }

        private createSmoke() {
            var scaleMaxY:number = this.game.rnd.realInRange(0.5, 1.0);
            var scaleMaxX:number = this.game.rnd.realInRange(0.5, 1.0) * this.game.rnd.sign();
            var msSmokeDuration = this.game.rnd.integerInRange(this.msSmokeDurationMin, this.msSmokeDurationMax);
            var msSmokeDurationHalf = msSmokeDuration / 2;

            var tweenScalePrimary = this.game.add.tween(this.volcanoSmoke.scale).to({ x: scaleMaxX, y: scaleMaxY }, msSmokeDurationHalf, Phaser.Easing.Linear.None, true);
            tweenScalePrimary.onComplete.add(function () { 
                var tweenScaleSecondary = this.game.add.tween(this.volcanoSmoke).to({ alpha: 0 }, msSmokeDurationHalf, Phaser.Easing.Linear.None, true);
                tweenScaleSecondary.onComplete.add(function() {
                    this.volcanoSmoke.scale.x = 0;
                    this.volcanoSmoke.scale.y = 0;
                    this.volcanoSmoke.alpha = 1;
                    setTimeout(() => this.createSmoke(), this.msForAnotherSmoke);
                }, this);
            }, this);
        }
    }
}