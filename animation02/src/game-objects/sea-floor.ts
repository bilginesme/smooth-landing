module MyGame {
    export class SeaFloor extends Phaser.Sprite {
        private dtc:DTC;
        private miliSecondsForAnotherWave:number = 4000;
        private imgFront: Phaser.Sprite;
        private imgBack: Phaser.Sprite;
        private starfish:Starfish;
        private rockGroupA:RockGroupA;
        private rockGroupB:RockGroupB;
        private submarines:Submarines;
        private algae2: Algae;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.imgBack = this.game.add.sprite(0, 124, 'sea-floor-back');
            this.imgBack.anchor.setTo(0, 0);

            this.submarines = new Submarines(this.game);

            this.algae2 = new Algae(game, 150, 144, 'algae-1', 0.65, 1.0, 5);
            this.algae2.anchor.setTo(0.5, 1.0);

            this.imgFront = this.game.add.sprite(0, 127, 'sea-floor-front');
            this.imgFront.anchor.setTo(0, 0);

            this.starfish = new Starfish(this.game, 200, 155);
            //this.starfish = new Starfish(this.game, 200, 100);

            this.rockGroupA = new RockGroupA(game, 100, 154);
            this.rockGroupB = new RockGroupB(game, 440, 148);
        }

        preload() { }
        update() { }
    }
}