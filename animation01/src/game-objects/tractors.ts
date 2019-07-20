module MyGame {
    export class Tractors extends Phaser.Sprite {
        private dtc:DTC;
        private tractor:Phaser.Sprite;
        //private miliSecondsForNewTractor:number = 10000;
        private miliSecondsForNewTractor:number = 1;
        //private pNewTractor:number = 0.02;
        private pNewTractor:number = 1;
        private xStart:number;
        private xEnd:number;
        private xLimitStart:number;
        private xLimitEnd:number;
        private maxVelocity:number = 10;
        private minVelocity:number = 6;
        private isTractorOnScreen:boolean = false;
        private yUp:number;
        private yDown:number;
        private scaleTractor:number;

        constructor(game: Phaser.Game, scaleTractor:number, yUp:number, yDown:number) {
            super(game, 0, 0, '');   
 
            this.scaleTractor = scaleTractor;
            this.yUp = yUp;
            this.yDown = yDown;

            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.xStart = -0.1 * game.width;
            this.xEnd = 1.1 * game.width;

            this.xLimitStart = -0.50 * game.width;
            this.xLimitEnd = 1.50 * game.width;

            this.tractor = this.game.add.sprite(0, 0, '');
            this.tractor.anchor.setTo(1, 1);
            this.game.physics.enable(this.tractor);

            this.isTractorOnScreen = false;

            this.createNewTractor();
        }

        preload() { }
        update() {
            if(this.isTractorOnScreen && (this.tractor.x > this.xLimitEnd || this.tractor.x < this.xLimitStart)) {
                this.isTractorOnScreen = false;
                this.tractor.body.velocity.x = 0;
            }
        }

        private createNewTractor() {
            if(!this.isTractorOnScreen && Math.random() < this.pNewTractor) {
                var tractorNo:number = this.game.rnd.integerInRange(1, this.dtc.numTractorTypes);
                var yStart = this.game.rnd.integerInRange(this.yUp, this.yDown);
                var strTractorName:string = 'tractor-' + this.dtc.doubleDigit(tractorNo);

                this.tractor.loadTexture(strTractorName, 0);
                this.tractor.animations.add('tractor', null, 4, true, true);
                this.tractor.animations.play('tractor');

                this.tractor.x = this.xStart;
                this.tractor.y = yStart;
                this.tractor.scale.y = this.scaleTractor;

                if (this.game.rnd.sign() == -1) {
                    this.tractor.body.velocity.x = this.game.rnd.realInRange(this.minVelocity, this.maxVelocity);
                    this.tractor.x = this.xStart;
                    this.tractor.scale.x = 1 * this.scaleTractor;
                }
                else {
                    this.tractor.body.velocity.x = -this.game.rnd.realInRange(this.minVelocity, this.maxVelocity);
                    this.tractor.x = this.xEnd;
                    this.tractor.scale.x = -1 * this.scaleTractor;
                }

                this.isTractorOnScreen = true;

                setTimeout(() => this.playTractorSound(), 1000 * 200 / Math.abs(this.tractor.body.velocity.x));
                console.log("creating a tractor ... " + this.tractor.texture.baseTexture.source.name);
            }

            setTimeout(() => this.createNewTractor(), this.miliSecondsForNewTractor);
        }

        private playTractorSound() {
            //var nSound = this.game.rnd.integerInRange(1, this.dtc.numSubmarineSounds);
            //this.game.add.audio('submarine-' + this.dtc.doubleDigit(nSound), 0.5, false).play();
        }
    }
}