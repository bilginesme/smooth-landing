module MyGame {
    export class SeaBackground extends Phaser.Sprite {
        private dtc:DTC;
        private fadingElementMountains:FadingElement;
        private fadingElementShipwreck:FadingElement;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, "");   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.fadingElementMountains = new FadingElement(game, game.width / 2, 145, 'underwater-mountains', 0.3, 1.0, 3 * 60 * 1000, 1 * 60 * 1000);
            this.fadingElementMountains = new FadingElement(game, 290, 132, 'shipwreck', 0.1, 1.0, 3.5 * 60 * 1000, 1.5 * 60 * 1000);
        }

        preload() { }
        update() { }
    }
}