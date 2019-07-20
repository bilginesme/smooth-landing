module MyGame {
    export class Surface extends Phaser.Sprite {
        private dtc:DTC;
        private boat:Phaser.Sprite;
        private maxBoats:number = 1;
        private miliSecondsForNewBoat:number = 10000;
        private pNewBoat = 0.1;
        private xStart:number;
        private yStart:number;
        private xEnd:number;
        private maxVelocityBoat:number = 10;
        private minVelocityBoat:number = 5;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.xStart = -0.25 * game.width;
            this.xEnd = 1.25 * game.width;
            this.yStart = 39;

            this.boat = this.game.add.sprite(0, this.yStart, '');
            this.boat.anchor.setTo(0.5, 1.0);

            this.createNewBoat();
        }

        preload() { }
        update() {
            for(var i=0;i<this.boat.children.length;i++) {
                var b = this.boat.children[i];
                if(b.x > this.xEnd || b.x < this.xStart) {
                    this.boat.removeChildAt(i);
                    console.log("Removing boat...");
                }
            }
        }

        private createNewBoat() {
            var numTotalVehicles:number = this.boat.children.length;

            if(numTotalVehicles < this.maxBoats && Math.random() < this.pNewBoat) {
                var boatNo:number = 0;
                boatNo = this.game.rnd.integerInRange(1, this.dtc.numBoatTypes)
                var strBoatName:string = 'boat-' + this.dtc.doubleDigit(boatNo);
                var newBoat = new Phaser.Sprite(this.game, this.xStart, 0, strBoatName);
                newBoat.anchor.setTo(0.5, 1);
                this.game.physics.enable(newBoat);

                if (this.game.rnd.sign() == -1) {
                    newBoat.body.velocity.x = this.game.rnd.realInRange(this.minVelocityBoat, this.maxVelocityBoat);
                    newBoat.x = this.xStart;
                    newBoat.scale.x = 1;
                }
                else {
                    newBoat.body.velocity.x = -this.game.rnd.realInRange(this.minVelocityBoat, this.maxVelocityBoat);
                    newBoat.x = this.xEnd;
                    newBoat.scale.x = -1;
                }
                this.boat.addChild(newBoat);

                setTimeout(() => this.playBoatSound(), 1000 * 100 / Math.abs(newBoat.body.velocity.x));
                console.log("creating a boat... " + newBoat.texture.baseTexture.source.name);
            }

            setTimeout(() => this.createNewBoat(), this.miliSecondsForNewBoat);
        }

        private playBoatSound() {
            this.game.add.audio('boat-horn', 0.2, false).play();
        }
    }
}