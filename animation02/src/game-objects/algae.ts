module MyGame {
    export class Algae extends Phaser.Sprite {
        private scaleMinX:number;
        private scaleMinY:number;
        private swayAngle:number;

        constructor(game: Phaser.Game, x:number, y:number, textureName:string, scaleMinX:number, scaleMinY:number, swayAngle:number) {
            super(game, x, y, textureName);   
 
            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);
 
            this.scaleMinX = scaleMinX;
            this.scaleMinY = scaleMinY;
            this.swayAngle = swayAngle;

            this.shrink();
            this.turnRight();
        }

        preload() { }
        update() { }

        private shrink():void {
            var msEnlarge:number = this.game.rnd.integerInRange(2000, 4000);
            var scaleMinY:number = this.game.rnd.realInRange(this.scaleMinY, 1.0);
            var scaleMinX:number = this.game.rnd.realInRange(this.scaleMinX, 1.0);

            var tweenPrimary = this.game.add.tween(this.scale).to({ x: scaleMinX, y: scaleMinY }, msEnlarge, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 0);
                setTimeout(() => this.enlarge(), msWait);
            }, this);
        }
        private enlarge():void {
            var msShrink = this.game.rnd.integerInRange(2000, 4000);

            var tweenPrimary = this.game.add.tween(this.scale).to({ x: 1, y: 1 }, msShrink, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 0);
                setTimeout(() => this.shrink(), msWait);
            }, this);
        }

        private turnRight():void {
            var msRotate:number = this.game.rnd.integerInRange(2000, 4000);
            var angleRight:number = this.game.rnd.realInRange(0, this.swayAngle);

            var tweenPrimary = this.game.add.tween(this).to({ angle: angleRight }, msRotate, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 2000);
                setTimeout(() => this.turnLeft(), msWait);
            }, this);
        }
        private turnLeft():void {
            var msRotate:number = this.game.rnd.integerInRange(2000, 4000);
            var angleLeft:number = this.game.rnd.realInRange(-this.swayAngle, 0);

            var tweenPrimary = this.game.add.tween(this).to({ angle: angleLeft }, msRotate, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 2000);
                setTimeout(() => this.turnRight(), msWait);
            }, this);
        }
    }
}