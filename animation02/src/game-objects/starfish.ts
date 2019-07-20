module MyGame {
    export class Starfish extends Phaser.Sprite {
        private dtc:DTC;
        private imgTest: Phaser.Sprite;
        private imgLegs: Phaser.Sprite[];

        private xMin:number = 100;
        private xMax:number = 300;
        private yMin:number = 145;
        private yMax:number = 165;

        private isMoving:boolean = false;

        constructor(game: Phaser.Game, x:number, y:number) {
            super(game, x, y, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.imgLegs = new Array(this.dtc.numStarfishLegs);
            var centersImgLegs:Phaser.Point[] = new Array(this.dtc.numStarfishLegs);
            centersImgLegs[0] = new Phaser.Point(0.07, 0.25);
            centersImgLegs[1] = new Phaser.Point(0.00, 0.95);
            centersImgLegs[2] = new Phaser.Point(0.75, 0.80);
            centersImgLegs[3] = new Phaser.Point(0.90, 0.25);
            centersImgLegs[4] = new Phaser.Point(0.85, 0.02);

            for(var i=0;i<this.dtc.numStarfishLegs;i++) {
                this.imgLegs[i] = this.game.add.sprite(x, y, 'starfish-leg-' + (i + 1));
                this.imgLegs[i].anchor.setTo(centersImgLegs[i].x, centersImgLegs[i].y)

                //this.imgLegs[i].scale = new Phaser.Point(3, 3);
                this.shrink(i);
            }

            //this.imgTest = this.game.add.sprite(200, 155, 'starfish-test');
            //this.imgTest.anchor.setTo(0.5, 0.5);

            this.checkForAnotherMove();
        }

        private shrink(indexLeg:number):void {
            var msEnlarge:number = this.game.rnd.integerInRange(900, 1500);
            var scaleMin:number = this.game.rnd.realInRange(0.65, 1.0);

            var tweenPrimary = this.game.add.tween(this.imgLegs[indexLeg].scale).to({ x: scaleMin, y: scaleMin }, msEnlarge, Phaser.Easing.Linear.None, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(500, 3000);
                setTimeout(() => this.enlarge(indexLeg), msWait);
            }, this);

        }
        private enlarge(indexLeg:number):void {
            var msShrink = this.game.rnd.integerInRange(800, 1500);

            var tweenPrimary = this.game.add.tween(this.imgLegs[indexLeg].scale).to({ x: 1, y: 1 }, msShrink, Phaser.Easing.Linear.None, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(500, 3000);
                setTimeout(() => this.shrink(indexLeg), msWait);
            }, this);
        }

        private startMovement() {
            this.isMoving = true;
            var xDestination: number = this.game.rnd.integerInRange(this.xMin, this.xMax);
            var yDestination: number = this.game.rnd.integerInRange(this.yMin, this.yMax);
            var distance = Math.sqrt(Math.pow((xDestination - this.imgLegs[0].x), 2) + Math.pow(yDestination - this.imgLegs[0].y, 2));
            var speed:number = 1.4;
            var msTrip:number = 1000 * (distance / speed);

            for(var i=0;i<this.dtc.numStarfishLegs;i++) {
                var tweenMotion = this.game.add.tween(this.imgLegs[i]).to({ x: xDestination, y: yDestination }, msTrip, Phaser.Easing.Linear.None, true);
                tweenMotion.onComplete.add(function () {
                    this.isMoving = false;
                }, this);
            }
        }

        private checkForAnotherMove() {
            if(!this.isMoving)
                this.startMovement();
            
            setTimeout(() => this.checkForAnotherMove(), 3000);
        }


        preload() { }
        update() { }
    }
}