module MyGame {
    enum PlaneStateEnum { Running, Stopped }

    export class Plane extends Phaser.Sprite {
        private dtc:DTC;
        private xInitialLeft;
        private xInitialRight;
        private secondsForNewPlane:number = 600;
        private pPlane:number = 0.25;
        private planeState:PlaneStateEnum = PlaneStateEnum.Stopped;
        private plane: Phaser.Sprite;
        private planeTrail: Phaser.Sprite;

        constructor(game: Phaser.Game) {
            super(game, -150, 0, '');   // start out of bounds
 
            this.dtc = new DTC();

            this.xInitialLeft = -50;
            this.xInitialRight = this.game.width + 50;

            this.anchor.setTo(0, 0);
            game.add.existing(this);
            game.physics.enable(this);

            this.plane = this.game.add.sprite(0,0, '');
            this.plane.anchor.setTo(0.5, 0.5);

            this.planeTrail = this.game.add.sprite(0,0, '');
            this.planeTrail.anchor.setTo(1, 0.5);

            setTimeout(() => this.startAnotherPlane(), this.secondsForNewPlane * 1000);
        }

        preload() { }
        update() { }

        private terminatePlane() {
            this.planeState = PlaneStateEnum.Stopped;
            this.plane.x = this.xInitialLeft;
            this.planeTrail.alpha = 1;
            this.planeTrail.scale.x = 0;
            this.planeTrail.scale.y = 0;

            setTimeout(() => this.startAnotherPlane(), this.secondsForNewPlane * 1000);
        }

        private startAnotherPlane() {
            console.log("Checking for new plane");

            if(this.planeState == PlaneStateEnum.Stopped) {
                if(Math.random() < this.pPlane) {
                    var imgNumPlane = this.game.rnd.between(1, this.dtc.numPlaneTypes);
                    var imgNumTrail = this.game.rnd.between(1, this.dtc.numPlaneTrailTypes);
                    this.plane.loadTexture('plane-' + this.dtc.doubleDigit(imgNumPlane));
                    var xDestination = 0;
                    var scaleDestination = 1;
                    var y1:number = this.dtc.yMargin + this.game.rnd.integerInRange(0.1 * this.game.height, 0.6 * this.game.height)
                    var y2:number = this.dtc.yMargin + this.game.rnd.integerInRange(0.1 * this.game.height, 0.6 * this.game.height)
                    var angle:number = Math.atan2(y2 - y1, this.game.width);
                    var planeTravelTimeMS = this.game.rnd.integerInRange(90000, 180000);
                    var planeTrailDisappearTimeMS = this.game.rnd.integerInRange(60000, 180000);

                    this.plane.y = y1;

                    if (this.game.rnd.sign() == -1) {
                        this.plane.scale.x = 1;
                        this.plane.x = this.xInitialLeft;
                        this.plane.rotation = angle;
                        xDestination = this.xInitialRight;
                        scaleDestination = 1;
                    }
                    else {
                        this.plane.scale.x = -1;
                        this.plane.x = this.xInitialRight;
                        this.plane.rotation = -angle;
                        xDestination = this.xInitialLeft;
                        scaleDestination = -1;
                    }

                    this.planeTrail.loadTexture('plane-trail-' + this.dtc.doubleDigit(imgNumTrail));
                    this.planeTrail.scale.x = this.plane.scale.x * 0.1;
                    this.planeTrail.scale.y = 0.1;
                    this.planeTrail.x = this.plane.x
                    this.planeTrail.y = this.plane.y;
                    this.planeTrail.rotation = this.plane.rotation;

                    var tweenMotionPlane = this.game.add.tween(this.plane).to({ x: xDestination, y: y2 }, planeTravelTimeMS, Phaser.Easing.Cubic.Out, true);
                    tweenMotionPlane.onComplete.add(function () {  }, this);

                    var tweenMotionPlaneTrail = this.game.add.tween(this.planeTrail).to({ x: xDestination, y: y2 }, planeTravelTimeMS, Phaser.Easing.Cubic.Out, true);
                    tweenMotionPlaneTrail.onComplete.add(function () {
                    }, this);

                    var tweenScalePlaneTrailPrimary = this.game.add.tween(this.planeTrail.scale).to({ x: scaleDestination, y: 1 }, planeTravelTimeMS, Phaser.Easing.Cubic.Out, true);
                    tweenScalePlaneTrailPrimary.onComplete.add(function () { 
                        var tweenFadePlaneTrailPrimary = this.game.add.tween(this.planeTrail).to({ alpha: 0 }, planeTrailDisappearTimeMS, 'Linear', true);
                        tweenFadePlaneTrailPrimary.onComplete.add(function () {
                            this.terminatePlane();
                        }, this);

                        var tweenDiffusePlaneTrailPrimary = this.game.add.tween(this.planeTrail.scale).to({ y: 2.0 }, planeTrailDisappearTimeMS, 'Linear', true);
                        tweenDiffusePlaneTrailPrimary.onComplete.add(function () {}, this);
                    }, this);

                    this.planeState = PlaneStateEnum.Running;
                    setTimeout(() => this.playPlaneSound(), 10000);
                    console.log("Started a new plane");
                }
                else {
                    console.log("Plane Not Started");
                }
            }
            else {
                
            }
            
            setTimeout(() => this.startAnotherPlane(), this.secondsForNewPlane * 1000);
        }

        private playPlaneSound() {
            var nSound = this.game.rnd.integerInRange(1, this.dtc.numPlaneSounds);
            this.game.add.audio('plane-' + this.dtc.doubleDigit(nSound), 0.5, false).play();
        }
    }
}