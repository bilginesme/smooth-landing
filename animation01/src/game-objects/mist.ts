module MyGame {
    export class Mist extends Phaser.Sprite {
        private dtc:DTC;
        private mist: Phaser.Sprite;

        private yMin:number;
        private yMax:number;
        private scaleFactor:number;

        private isThereAMistNow:boolean = false;
        
        //private speedMin:number = 50;
        //private speedMax:number = 50;
        private speedMin:number = 2;
        private speedMax:number = 9;

        private alphaMax:number = 1;
        private alphaMin:number = 0.5;

        private msCycle:number = 60 * 1000;
        //private msCycle:number = 6 * 1000;
        private msBetweenCycles:number = 10 * 1000;
        //private msBetweenCycles:number = 1 * 1000;

        private msCheckForAnotherMist:number = 60 * 1000;
        private pMist = 0.075;
        //private pMist = 1;

        private tweenPrimary;
        private tweenSecondary;
        private tweenTertiary;

        constructor(game: Phaser.Game, yMin:number, yMax:number, scaleFactor:number) {
            super(game, 0, 0, "");
            this.dtc = new DTC();
  
            this.yMin = yMin;
            this.yMax = yMax;
            this.scaleFactor = scaleFactor;

            this.anchor.setTo(0, 0);
            this.game.physics.enable(this);
            this.body.collideWorldBounds = false;
            game.add.existing(this);
  
            this.isThereAMistNow = false;

            this.reCreate();
        }
        
        update() {
            if(!this.isThereAMistNow)
                return;

            if(this.mist.x  < - this.mist.texture.width || this.mist.x > this.game.width + this.mist.texture.width) {
                this.isThereAMistNow = false;
                this.mist.alpha = 0;
                this.game.tweens.removeFrom(this.mist);
                this.removeChild(this.mist);
                this.game.tweens.remove(this.tweenPrimary);
                this.game.tweens.remove(this.tweenSecondary);
                this.game.tweens.remove(this.tweenTertiary);
                console.log("Removing mist");

                this.reCreate();
            }
        }

        private reCreate() {
            if(this.isThereAMistNow)
                return;

            var p = this.game.rnd.realInRange(0, 1);
            if(p > this.pMist) {
                console.log("mist not created, p = " + p);
                setTimeout(() => this.reCreate(), this.msCheckForAnotherMist);
                return;
            }

            console.log("Creating a new mist");
            this.isThereAMistNow = true;

            var imgNum = this.game.rnd.between(1, this.dtc.numMistTypes);
            this.mist = this.game.add.sprite(0, 0, 'mist-' + this.dtc.doubleDigit(imgNum));
            this.game.physics.enable(this.mist);

            if (this.game.rnd.sign() == -1) {
                this.mist.body.velocity.x = this.game.rnd.realInRange(this.speedMin, this.speedMax);
                this.mist.x = -this.mist.texture.width / 2;
            }
            else {
                this.mist.body.velocity.x = -this.game.rnd.realInRange(this.speedMin, this.speedMax);
                this.mist.x = this.game.width + this.mist.texture.width / 2;
            }
            
            console.log("x=" + this.mist.x +" , width=" + this.mist.texture.width);

            //this.mist.y = this.game.rnd.between(this.game.height, this.game.height + this.mist.texture.height / 2);
            this.mist.y = this.game.rnd.between(this.yMin, this.yMax);
            this.mist.anchor.setTo(0.5, 1);

            if (this.game.rnd.sign() == -1)
                this.mist.scale.x = -1 * this.scaleFactor;
            else
                this.mist.scale.x = 1 * this.scaleFactor;

            this.mist.scale.y = this.scaleFactor    

            this.mist.alpha = 0;

            this.addChild(this.mist);

            console.log("Mist alpha = " + this.mist.alpha);
            this.handleCycle();
        }

        private handleCycle() {
            if(!this.isThereAMistNow)
                return;

            var alphaMax = this.game.rnd.realInRange(this.alphaMin, this.alphaMax);
            //var alphaMax = 1;
            console.log("handling cycle...");

            console.log("alpha mist increasing to " + alphaMax);
            this.tweenPrimary = this.game.add.tween(this.mist).to({ alpha: alphaMax }, this.msCycle / 3, 'Linear', true);
            this.tweenPrimary.onComplete.add(function () { 
                if(this.isThereAMistNow) {
                    this.tweenSecondary = this.game.add.tween(this.mist).to({ alpha: alphaMax }, this.msCycle / 3, 'Linear', true); 
                    this.tweenSecondary.onComplete.add(function() {
                        if(this.isThereAMistNow) {
                            var alphaMin = this.alphaMin;
                        console.log("alpha mist decreasing to " + alphaMin);
                        this.tweenTertiary = this.game.add.tween(this.mist).to({ alpha: alphaMin }, this.msCycle / 3, 'Linear', true); 
                        this.tweenTertiary.onComplete.add(function() {
                            if(this.isThereAMistNow) {
                                setTimeout(() => this.handleCycle(), this.msBetweenCycles);
                            }
                        }, this);
                        }
                    }, this);
                }
                
            }, this);
        }

    }

}