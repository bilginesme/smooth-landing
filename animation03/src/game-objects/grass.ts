module MyGame {
    export class Grass extends Phaser.Sprite {
        private dtc:DTC;
        private grass:Phaser.Sprite;
        

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.grass = this.game.add.sprite(0, 118, 'grass');
            this.grass.anchor.setTo(0, 0);
        }

        preload() { }
        update() { }
    }
}