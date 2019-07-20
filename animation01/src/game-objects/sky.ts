module MyGame {

    export class Sky extends Phaser.Sprite {
        private dtc:DTC;
        private miliSecondsForSceneChange:number = 600000;
        private miliSecondsForTween:number = 60000;
        //private miliSecondsForSceneChange:number = 10000;
        //private miliSecondsForTween:number = 5000;

        private imgPrimary: Phaser.Sprite;
        private imgSecondary: Phaser.Sprite;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.imgPrimary = this.game.add.sprite(game.width / 2, 0, '');
            this.imgPrimary.anchor.setTo(0.5, 0);
            this.imgPrimary.alpha = 0;

            this.imgSecondary = this.game.add.sprite(game.width / 2, 0, '');
            this.imgSecondary.anchor.setTo(0.5, 0);
            this.imgSecondary.alpha = 0;

            this.initSky();
        }

        preload() { }
        update() { }

        private initSky() {
            //this.game.rnd.sow([Date.now()]);

            var imgNum = this.game.rnd.between(1, this.dtc.numSkyTypes);

            this.imgPrimary.loadTexture('sky-' + this.dtc.doubleDigit(imgNum));
            this.imgPrimary.alpha = 1;
            
            if (Math.random() < 0.50) 
                this.imgPrimary.scale.x = -1;
            else 
                this.imgPrimary.scale.x = 1;

            setTimeout(() => this.changeSky(), this.miliSecondsForSceneChange);
        }

        private changeSky() {
            var imgNum = this.game.rnd.between(1, this.dtc.numSkyTypes);
            this.imgSecondary.alpha = 0;
            this.imgSecondary.loadTexture('sky-' + this.dtc.doubleDigit(imgNum));

            if (Math.random() < 0.50)
                this.imgSecondary.scale.x = -1;
            else
                this.imgSecondary.scale.x = 1;
 
            var tweenPrimary = this.game.add.tween(this.imgPrimary).to({ alpha: 0 }, this.miliSecondsForTween, 'Linear', true);
            var tweenSecondary = this.game.add.tween(this.imgSecondary).to({ alpha: 1 }, this.miliSecondsForTween, 'Linear', true);
            tweenPrimary.onComplete.add(function () { this.imgPrimary.texture = this.imgSecondary.texture; this.imgPrimary.alpha = 1; this.imgSecondary.alpha = 0; }, this);
          
            setTimeout(() => this.changeSky(), this.miliSecondsForSceneChange);
        }

    }
}