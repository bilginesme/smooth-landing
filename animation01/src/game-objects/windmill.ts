module MyGame {
    enum WindmillSpeedStateEnum { Increasing, Decreasing }

    export class Windmill extends Phaser.Sprite {
        private mill: Phaser.Sprite;
        private rotateTime:number = 20000;
        private rotateTimeMin:number = 5000;
        private rotateTimeMax:number = 100000;
        private windmillSpeedState:WindmillSpeedStateEnum = WindmillSpeedStateEnum.Increasing;

        constructor(game: Phaser.Game, x:number, y:number) {
            super(game, x, y, 'windmill-building');   // start out of bounds
 
            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);

            this.mill = this.game.add.sprite(443, y-34, 'windmill-mill');
            this.mill.anchor.setTo(0.5, 0.5);
            
            this.reStartRotation();
        }


        private reStartRotation() {
            if(this.windmillSpeedState == WindmillSpeedStateEnum.Increasing) {
                this.rotateTime-= 1000;
                if(this.rotateTime < this.rotateTimeMin)
                    this.rotateTime = this.rotateTimeMin;
            }
            else {
                this.rotateTime+= 1000;
                if(this.rotateTime > this.rotateTimeMax)
                    this.rotateTime = this.rotateTimeMax;
            }

            if(this.game.rnd.between(0, 10) == 1) {     // the state may change sometimes
                if(this.windmillSpeedState == WindmillSpeedStateEnum.Increasing)
                    this.windmillSpeedState = WindmillSpeedStateEnum.Decreasing;
                else
                    this.windmillSpeedState = WindmillSpeedStateEnum.Increasing;
            }

            var tween = this.game.add.tween(this.mill).to({ angle: 360 }, this.rotateTime, 'Linear', true);
            tween.onComplete.add(this.reStartRotation, this);

            //console.log("Rotate Time : " + this.rotateTime + ", State = " + this.windmillSpeedState);
        }

        preload() {
			//this.game.stage.disableVisibilityChange = true;
        }

        update() {
        }

    }
}