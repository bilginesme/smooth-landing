module MyGame {
    enum BaloonStateEnum { Running, Stopped }

    export class Baloon extends Phaser.Sprite {
        private dtc:DTC;
        private xInitialLeft;
        private xInitialRight;
        private speedMin:number = 4;
        private speedMax:number = 10;
        private secondsForNewBaloon:number = 250;
        private baloonState:BaloonStateEnum = BaloonStateEnum.Stopped;
        
        constructor(game: Phaser.Game) {
            super(game, -200, 0, 'baloon-1');   // start out of bounds
 
            this.dtc = new DTC();

            this.xInitialLeft = -100;
            this.xInitialRight = this.game.width + 100;

            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);

            // Physics
            game.physics.enable(this);
            this.body.collideWorldBounds = false;
            this.body.setCircle(20);
            
            setTimeout(() => this.startAnotherBaloon(), this.secondsForNewBaloon * 1000);
            setTimeout(() => this.checkState(), 1000);
        }

        preload() {
			//this.game.stage.disableVisibilityChange = true;
        }

        update() {
        }


        checkState() {
            if(this.baloonState == BaloonStateEnum.Running) {
                if(this.x > this.xInitialRight || this.x < this.xInitialLeft) {                    
                    this.baloonState = BaloonStateEnum.Stopped;
                    this.x = this.xInitialLeft;
                    this.body.velocity.x = 0;
                }
            }
            
            setTimeout(() => this.checkState(), 1000);
        }

        startAnotherBaloon() {
            console.log("Checking");

            if(this.baloonState == BaloonStateEnum.Stopped) {
                if(Math.random() > 0.90) {
                    var imgNum = this.game.rnd.between(1, 3);
                    this.loadTexture('baloon-' + imgNum);

                    if (this.game.rnd.sign() == -1) {
                        this.body.velocity.x = this.game.rnd.between(this.speedMin, this.speedMax);
                        this.x = this.xInitialLeft;
                    }
                    else {
                        this.body.velocity.x = -this.game.rnd.between(this.speedMin, this.speedMax);
                        this.x = this.xInitialRight;
                    }
                    this.y = this.texture.height + this.game.rnd.between(this.dtc.yMargin + 5, this.dtc.yMargin + 30);

                    if (this.game.rnd.sign() == -1)
                        this.scale.x = -1;
                    else
                        this.scale.x = 1;

                    this.baloonState = BaloonStateEnum.Running;
                    console.log("Started");
                }
                else {
                    console.log("Not Started");
                }
            }
            else {
                
            }
            
            setTimeout(() => this.startAnotherBaloon(), this.secondsForNewBaloon * 1000);
        }

    }

}