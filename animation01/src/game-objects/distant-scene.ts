module MyGame {

    export class DistantScene extends Phaser.Sprite {
        private dtc:DTC;
        private clouds: Phaser.Sprite;
        private mountain: Phaser.Sprite;
        private miliSecondsForSceneChange:number = 360000;
        private pMountain:number = 0.00;    // disable the mountain for now 2018-11-17
        private pClouds:number = 0.20;

        private alphaMinClouds:number = 0.5;
        private alphaMinMountain:number = 0.5;
        private alphaMaxClouds:number = 0.7;
        private alphaMaxMountain:number = 0.7;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
            this.dtc = new DTC();

            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);

            this.clouds = this.game.add.sprite(0, 0, '');
            this.clouds.anchor.setTo(0, 0);
            this.clouds.alpha = 0;

            this.mountain = this.game.add.sprite(240, 115, 'distant-mountains');
            this.mountain.anchor.setTo(0.5, 1);
            this.mountain.alpha = 0;

            setTimeout(() => this.changeScene(), this.miliSecondsForSceneChange);
        }

        preload() {
			//this.game.stage.disableVisibilityChange = true;
        }

        update() {
        }

        private changeScene() {
            var p = this.game.rnd.realInRange(0, 1);
            console.log("p = " + p);

            if(p >= 0 && p < this.pMountain) {
                var alphaMax = this.game.rnd.realInRange(this.alphaMinMountain, this.alphaMaxMountain);
                var tween = this.game.add.tween(this.mountain).to({ alpha: alphaMax }, this.miliSecondsForSceneChange / 3, 'Linear', true);
                tween.onComplete.add(function () { 
                    var tweenSecondary = this.game.add.tween(this.mountain).to({ alpha: alphaMax }, this.miliSecondsForSceneChange / 3, 'Linear', true); 
                    tweenSecondary.onComplete.add(function() {
                        this.game.add.tween(this.mountain).to({ alpha: 0 }, this.miliSecondsForSceneChange / 3, 'Linear', true); 
                    }, this);
                }, this);

                this.clouds.alpha = 0;
                console.log("Mountain");
            }
            else if( p >= this.pMountain && p < (this.pClouds + this.pMountain)) {
                var imgNum = this.game.rnd.between(1, this.dtc.numDistantCloudTypes);
                this.clouds.loadTexture('distant-clouds-' + this.dtc.doubleDigit(imgNum));

                if (this.game.rnd.sign() == -1)
                    this.clouds.scale.x = -1;
                else
                    this.clouds.scale.x = 1;

                var alphaMax = this.game.rnd.realInRange(this.alphaMinClouds, this.alphaMaxClouds);
                var tween = this.game.add.tween(this.clouds).to({ alpha: alphaMax }, this.miliSecondsForSceneChange / 3, 'Linear', true);
                tween.onComplete.add(function () { 
                    var tweenSecondary = this.game.add.tween(this.clouds).to({ alpha: alphaMax }, this.miliSecondsForSceneChange / 3, 'Linear', true); 
                    tweenSecondary.onComplete.add(function() {
                        this.game.add.tween(this.clouds).to({ alpha: 0 }, this.miliSecondsForSceneChange / 3, 'Linear', true); 
                    }, this);
                }, this);
                this.mountain.alpha = 0;
                console.log("Clouds");
            }
            else {
                this.clouds.alpha = 0;
                this.mountain.alpha = 0;
            }

            setTimeout(() => this.changeScene(), this.miliSecondsForSceneChange);
        }
    }
}