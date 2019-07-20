module MyGame {
    export class RockGroupB extends Phaser.Sprite {
        private dtc:DTC;
        private imgRock1: Phaser.Sprite;
        private imgRock2: Phaser.Sprite;

        private algae1: Algae;

        private algae3: Algae;
        private algae4: Algae;

        constructor(game: Phaser.Game, x:number, y:number) {
            super(game, x, y, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);

            this.algae1 = new Algae(game, x - 52, y - 2, 'rock-group-b-algae-1', 0.65, 1.0, 5);
            this.algae1.anchor.setTo(0.5, 1.0);

          
            this.algae3 = new Algae(game, x + 5, y - 3, 'rock-group-b-algae-purple', 0.75, 1.0, 8);
            this.algae3.anchor.setTo(0.5, 1.0);

            this.imgRock1 = this.game.add.sprite(x - 15, y + 11, 'rock-group-b-rock-1');
            this.imgRock1.anchor.setTo(0.5, 1.0);
            
            this.algae4 = new Algae(game, x - 25, y + 3, 'rock-group-b-algae-small', 0.60, 1.0, 8);
            this.algae4.anchor.setTo(0.5, 1.0);

            this.imgRock2 = this.game.add.sprite(this.imgRock1.x - 10, this.imgRock1.y + 6, 'rock-group-b-rock-2');
            this.imgRock2.anchor.setTo(0.5, 1.0);
        }

        preload() { }
        update() { }
    }
}