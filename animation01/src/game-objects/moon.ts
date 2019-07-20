module MyGame {

    export class Moon extends Phaser.Sprite {
        private dtc:DTC;
        private moonSpriteSheet: Phaser.Sprite;
        private numPhases:number;
        private phaseNow:number;
        
        private milisecondsPrimary:number;
        private milisecondsSecondary:number;
        private milisecondsTertiary:number;
        private milisecondsForPhaseInterval:number;
        private milisecondsForAnotherPassage:number;

        private alphaMinMoon:number = 1.0;
        private alphaMaxMoon:number = 1.0;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0.5, 0.5);
            game.add.existing(this);

            this.moonSpriteSheet = this.game.add.sprite(0, 0, 'moon-phases');
            this.moonSpriteSheet.anchor.setTo(0.5, 0.5);

            this.numPhases = 11;
            this.phaseNow = 0;
            this.moonSpriteSheet.frame = this.phaseNow;
            this.moonSpriteSheet.alpha = 0;
            
            this.milisecondsPrimary = 3 * 60 * 1000;
            this.milisecondsSecondary = 6 * 60 * 1000;
            this.milisecondsTertiary = 13 * 60 * 1000;
            this.milisecondsForPhaseInterval = 10 * 60 * 1000;
            this.milisecondsForAnotherPassage = 40 * 60 * 1000;

            /*
            this.milisecondsPrimary = 3 * 60 * 10;
            this.milisecondsSecondary = 6 * 60 * 10;
            this.milisecondsTertiary = 13 * 60 * 10;
            this.milisecondsForPhaseInterval = 2000;
            this.milisecondsForAnotherPassage = 2000;
            */
           
            setTimeout(() => this.startPassage(), this.milisecondsForAnotherPassage);
        }

        preload() {
			//this.game.stage.disableVisibilityChange = true;
        }

        update() {
        }

        private startPassage() {
            this.phaseNow = 0;
            this.nextPhase();
        }

        private nextPhase() {
            console.log("Frame : " + this.phaseNow);
            
            this.moonSpriteSheet.x = 97;
            this.moonSpriteSheet.y =  this.dtc.yMargin + 128;
            this.moonSpriteSheet.alpha = this.game.rnd.realInRange(this.alphaMinMoon, this.alphaMaxMoon);
            this.moonSpriteSheet.rotation = this.game.rnd.realInRange(-Math.PI / 3, Math.PI / 3);

            var tweenMotionPrimary = this.game.add.tween(this.moonSpriteSheet).to({ x: 137, y: this.dtc.yMargin + 79 }, this.milisecondsPrimary, 'Linear', true);
            tweenMotionPrimary.onComplete.add(function () {
                console.log("Tween 1 completed...");
                var tweenMotionSecondary = this.game.add.tween(this.moonSpriteSheet).to({ x: 247, y: this.dtc.yMargin + 57 }, this.milisecondsSecondary, 'Linear', true);
                tweenMotionSecondary.onComplete.add(function () {
                    console.log("Tween 2 completed...");
                    var tweenMotionTertiary = this.game.add.tween(this.moonSpriteSheet).to({ x: 520, y: this.dtc.yMargin + 29 }, this.milisecondsTertiary, 'Linear', true);
                    tweenMotionTertiary.onComplete.add(function () {
                        console.log("Tween 3 completed...");
                        this.isMoonVisible = false;
                        this.phaseNow++;
                        if(this.phaseNow >= this.numPhases) {
                            this.phaseNow = 0;
                            setTimeout(() => this.startPassage(), this.milisecondsForAnotherPassage);
                        }
                        else {
                            setTimeout(() => this.nextPhase(), this.milisecondsForPhaseInterval);
                        }

                        this.moonSpriteSheet.frame = this.phaseNow;
                    }, this);
                }, this);
            }, this);
   
        }

    }
}