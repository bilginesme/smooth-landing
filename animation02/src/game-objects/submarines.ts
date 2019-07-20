module MyGame {
    export class Submarines extends Phaser.Sprite {
        private dtc:DTC;
        private submarine:Phaser.Sprite;
        private torchlight:Phaser.Sprite;
        private bubbles:Phaser.Sprite;
        private miliSecondsForNewSubmarine:number = 10000;
        private pNewSubmarine:number = 0.01;
        private pTorchlight:number = 0.35;
        private xStart:number;
        private xEnd:number;
        private xLimitStart:number;
        private xLimitEnd:number;
        private maxVelocity:number = 10;
        private minVelocity:number = 6;
        private swayAngle:number = 16;
        private isSubmarineOnScreen:boolean = false;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.xStart = -0.25 * game.width;
            this.xEnd = 1.25 * game.width;

            this.xLimitStart = -0.50 * game.width;
            this.xLimitEnd = 1.50 * game.width;

            this.submarine = this.game.add.sprite(0, 0, '');
            this.submarine.anchor.setTo(0, 0.5);
            this.game.physics.enable(this.submarine);

            this.torchlight = this.game.add.sprite(0, 0, 'torchlight');
            this.torchlight.anchor.setTo(0, 0.5);
            this.torchlight.alpha = 0;
            this.game.physics.enable(this.torchlight);

            this.bubbles = this.game.add.sprite(0, 0, '');
            this.bubbles.anchor.setTo(0, 0);
            this.bubbles.alpha = 1;
            this.game.physics.enable(this.bubbles);

            this.isSubmarineOnScreen = false;

            this.createNewSubmarine();
            this.turnRight();
        }

        preload() { }
        update() {
            if(this.isSubmarineOnScreen && (this.submarine.x > this.xLimitEnd || this.submarine.x < this.xLimitStart)) {
                this.isSubmarineOnScreen = false;
                this.submarine.body.velocity.x = 0;
                this.torchlight.body.velocity.x = 0;
                this.bubbles.removeChildren();
            }
        }

        private createNewSubmarine() {
            if(!this.isSubmarineOnScreen && Math.random() < this.pNewSubmarine) {
                var submarineNo:number = this.game.rnd.integerInRange(1, this.dtc.numSubmarineTypes);
                var yStart = this.game.rnd.integerInRange(85, 125);
                var strSubmarineName:string = 'submarine-' + this.dtc.doubleDigit(submarineNo);

                this.submarine.loadTexture(strSubmarineName);
                this.submarine.x = this.xStart;
                this.submarine.y = yStart;

                if (this.game.rnd.sign() == -1) {
                    this.submarine.body.velocity.x = this.game.rnd.realInRange(this.minVelocity, this.maxVelocity);
                    this.submarine.x = this.xStart;
                    this.submarine.scale.x = 1;
                    this.torchlight.x = this.submarine.x + this.submarine.texture.width;
                }
                else {
                    this.submarine.body.velocity.x = -this.game.rnd.realInRange(this.minVelocity, this.maxVelocity);
                    this.submarine.x = this.xEnd;
                    this.submarine.scale.x = -1;
                    this.torchlight.x = this.submarine.x - this.submarine.texture.width;
                }

                this.torchlight.scale.x = this.submarine.scale.x;
                this.torchlight.y = this.submarine.y;
                this.torchlight.body.velocity.x = this.submarine.body.velocity.x;

                if(this.game.rnd.realInRange(0,1) < this.pTorchlight)
                    this.torchlight.alpha = this.game.rnd.realInRange(0.1, 1.0);
                else
                    this.torchlight.alpha = 0;

                this.isSubmarineOnScreen = true;

                this.createBubble();

                setTimeout(() => this.playSubmarineSound(), 1000 * 200 / Math.abs(this.submarine.body.velocity.x));
                console.log("creating a submarine ... " + this.submarine.texture.baseTexture.source.name);
            }

            setTimeout(() => this.createNewSubmarine(), this.miliSecondsForNewSubmarine);
        }

        private createBubble() {
            if(!this.isSubmarineOnScreen)
                return;

            var bubble = new Phaser.Sprite(this.game, this.submarine.x, this.submarine.y, "bubbles");

            if(this.submarine.scale.x > 0)
                bubble.x = this.submarine.x - 10;
            else
                bubble.x = this.submarine.x + 10;

            bubble.scale.x = this.submarine.scale.x;
            bubble.y = this.submarine.y;
            this.game.physics.enable(bubble);
            bubble.body.velocity.x = this.submarine.body.velocity.x;
            bubble.anchor.setTo(0.5, 1.0);
            bubble.alpha = 1;
            bubble.scale.x = 0;
            bubble.scale.y = 0;
            this.bubbles.addChild(bubble);
            this.enlargeBubble(bubble);
        }

        private turnRight():void {
            var msRotate:number = this.game.rnd.integerInRange(2500, 5000);
            var angleRight:number = this.game.rnd.realInRange(0, this.swayAngle);

            var tweenPrimary = this.game.add.tween(this.torchlight).to({ angle: angleRight }, msRotate, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 5000);
                setTimeout(() => this.turnLeft(), msWait);
            }, this);
        }
        private turnLeft():void {
            var msRotate:number = this.game.rnd.integerInRange(2500, 5000);
            var angleLeft:number = this.game.rnd.realInRange(-this.swayAngle, 0);

            var tweenPrimary = this.game.add.tween(this.torchlight).to({ angle: angleLeft }, msRotate, Phaser.Easing.Sinusoidal.InOut, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 5000);
                setTimeout(() => this.turnRight(), msWait);
            }, this);
        }

        private enlargeBubble(bubble:Phaser.Sprite):void {
            var msEnlarge = this.game.rnd.integerInRange(4000, 8000);

            var tweenPrimary = this.game.add.tween(bubble.scale).to({ x: 1, y: 1 }, msEnlarge, Phaser.Easing.Linear.None, true);
            tweenPrimary.onComplete.add(function () { 
                var msWait = this.game.rnd.integerInRange(0, 0);
                setTimeout(() => this.disappearBubble(bubble), msWait);
            }, this);
        }
        private disappearBubble(bubble:Phaser.Sprite):void {
            var msDisappear:number = this.game.rnd.integerInRange(5000, 10000);

            var tweenPrimary = this.game.add.tween(bubble).to({ alpha: 0 }, msDisappear, Phaser.Easing.Linear.None, true);
            tweenPrimary.onComplete.add(function () { 
            }, this);

            var tweenSlowDown = this.game.add.tween(bubble.body.velocity).to({ x: 0 }, msDisappear, Phaser.Easing.Linear.None, true);

            var msWait = this.game.rnd.integerInRange(1000, 2000);
            setTimeout(() => this.createBubble(), msWait);
        }

        private playSubmarineSound() {
            var nSound = this.game.rnd.integerInRange(1, this.dtc.numSubmarineSounds);
            this.game.add.audio('submarine-' + this.dtc.doubleDigit(nSound), 0.5, false).play();
        }
    }
}