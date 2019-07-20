module MyGame {
    export class RockGroupA extends Phaser.Sprite {
        private dtc:DTC;
        private imgRock: Phaser.Sprite;
        private imgShell: Phaser.Sprite;
        private imgStone1: Phaser.Sprite;
        private imgStone2: Phaser.Sprite;

        private algae1: Algae;
        private algae2: Algae;
        private algae3: Algae;
        private algae4: Algae;
        private algeaSmall:Algae;

        constructor(game: Phaser.Game, x:number, y:number) {
            super(game, x, y, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);

            this.algae4 = new Algae(game, x - 32, y - 5, 'rock-group-a-algae-4', 0.65, 1.0, 8);
            this.algae4.anchor.setTo(0.5, 1.0);

            this.imgRock = this.game.add.sprite(x, y, 'rock-group-a-rock');
            this.imgRock.anchor.setTo(0.5, 1.0);

            this.algae1 = new Algae(game, x - 52, y + 1, 'rock-group-a-algae-1', 0.65, 1.0, 8);
            this.algae1.anchor.setTo(0.5, 1.0);

            this.algae2 = new Algae(game, x - 40, y + 2, 'rock-group-a-algae-2', 0.65, 1.0, 8);
            this.algae2.anchor.setTo(0.5, 1.0);

            this.algae3 = new Algae(game, x - 25, y + 3, 'rock-group-a-algae-3', 0.65, 1.0, 8);
            this.algae3.anchor.setTo(0.5, 1.0);

            this.imgShell = this.game.add.sprite(x - 42, y + 7, 'rock-group-a-shell');
            this.imgShell.anchor.setTo(0.5, 1.0);

            this.algeaSmall = new Algae(game, x - 15, y + 2, "rock-group-a-algae-small", 0.65, 1.0, 8);

            this.imgStone1 = this.game.add.sprite(x - 15, y + 11, 'rock-group-a-stone-1');
            this.imgStone1.anchor.setTo(0.5, 1.0);

            this.imgStone2 = this.game.add.sprite(this.imgStone1.x + 25, this.imgStone1.y - 6, 'rock-group-a-stone-2');
            this.imgStone2.anchor.setTo(0.5, 1.0);
        }

        preload() { }
        update() { }
    }
}