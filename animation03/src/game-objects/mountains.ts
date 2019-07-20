module MyGame {
    export class Mountains extends Phaser.Sprite {
        private dtc:DTC;
        private bushes:Phaser.Sprite;
        private rhino:Rhino;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.bushes = this.game.add.sprite(0, 63, 'mountains');
            this.bushes.anchor.setTo(0, 0);

            //this.rhino = new Rhino(this.game, new Phaser.Point(0, 0));
            //this.rhino.giveLife();
        }

        preload() { }
        update() { }
    }
}