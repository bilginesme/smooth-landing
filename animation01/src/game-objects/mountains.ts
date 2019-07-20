module MyGame {

    export class Mountains extends Phaser.Sprite {
        private dtc:DTC;
        private mountains: Phaser.Sprite;
        private msBetweenAppearences:number = 60 * 1000;
        private msForMountainVisible:number = 5 * 60 * 1000;
        private pMountain:number = 0.20;
        private alphaMin:number = 0.5;
        private alphaMax:number = 0.7;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.mountains = this.game.add.sprite(0, this.dtc.yMargin + 94, 'front-to-back-4');
            this.mountains.anchor.setTo(0, 0);
            this.mountains.alpha = 0;

            setTimeout(() => this.changeScene(), this.msBetweenAppearences);
        }

        preload() {
			//this.game.stage.disableVisibilityChange = true;
        }

        update() {
        }

        private changeScene() {
            var p = this.game.rnd.realInRange(0, 1);
            console.log("p_mountains = " + p);

            if(p >= 0 && p < this.pMountain) {
                var alphaMax = this.game.rnd.realInRange(this.alphaMin, this.alphaMax);
                var tween = this.game.add.tween(this.mountains).to({ alpha: alphaMax }, this.msForMountainVisible / 3, 'Linear', true);
                tween.onComplete.add(function () { 
                    var tweenSecondary = this.game.add.tween(this.mountains).to({ alpha: alphaMax }, this.msForMountainVisible / 3, 'Linear', true); 
                    tweenSecondary.onComplete.add(function() {
                        var tweenTertiary = this.game.add.tween(this.mountains).to({ alpha: 0 }, this.msForMountainVisible / 3, 'Linear', true); 
                        tweenTertiary.onComplete.add(function() {
                            setTimeout(() => this.changeScene(), this.msBetweenAppearences);
                        }, this);
                    }, this);
                }, this);

                console.log("Mountains");
            }
            else {
                this.mountains.alpha = 0;
                setTimeout(() => this.changeScene(), this.msBetweenAppearences);
            }
        }
    }
}