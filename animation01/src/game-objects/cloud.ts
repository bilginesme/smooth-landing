module MyGame {
    export class Cloud extends Phaser.Sprite {
        private dtc:DTC;
        private speedMin:number;
        private speedMax:number;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");
            this.dtc = new DTC();

            this.speedMin = 0.1;
            this.speedMax = 3;

            this.game.physics.enable(this);
            this.body.collideWorldBounds = false;
          
            this.reCreate();

            game.add.existing(this);
        }
        
        private reCreate() {
            this.game.physics.enable(this);

            var imgNum = this.game.rnd.between(1, this.dtc.numCloudTypes);
                    this.loadTexture('cloud-' + this.dtc.tripleDigit(imgNum));

            if (this.game.rnd.sign() == -1) {
                this.body.velocity.x = this.game.rnd.realInRange(this.speedMin, this.speedMax);
                this.x = -this.texture.width;
            }
            else {
                this.body.velocity.x = -this.game.rnd.realInRange(this.speedMin, this.speedMax);
                this.x = this.game.width + this.texture.width;
            }
            
            this.y = this.game.rnd.between(this.dtc.yMargin + 0, this.dtc.yMargin + 0.66 * this.game.height );

            this.anchor.setTo(0.5);

            if (this.game.rnd.sign() == -1)
                this.scale.x = -1;
            else
                this.scale.x = 1;
        }

        update() {
            if(this.x + this.texture.width < 0)
                this.reCreate();
            else if(this.x - this.texture.width > this.game.width)
                this.reCreate();
        }

    }

}