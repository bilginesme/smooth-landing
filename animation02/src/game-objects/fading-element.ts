module MyGame {
    export class FadingElement extends Phaser.Sprite {
        private alphaMin:number;
        private alphaMax:number;
        private msCycle:number;
        private msBetweenCycles:number;

        constructor(game: Phaser.Game, x:number, y:number, textureName:string, alphaMin:number, alphaMax:number, msCycle:number, msBetweenCycles:number) {
            super(game, x, y, textureName);   
 
            this.anchor.setTo(0.5, 1.0);
            game.add.existing(this);
 
            this.alphaMin = alphaMin;
            this.alphaMax = alphaMax;
            this.msCycle = msCycle;
            this.msBetweenCycles = msBetweenCycles;

            this.alpha = this.alphaMin;

            this.handleCycle();
        }

        preload() { }
        update() { }

        private handleCycle() {
            var alphaMax = this.game.rnd.realInRange(this.alphaMin, this.alphaMax);
            var tween = this.game.add.tween(this).to({ alpha: alphaMax }, this.msCycle / 3, 'Linear', true);
            tween.onComplete.add(function () { 
                var tweenSecondary = this.game.add.tween(this).to({ alpha: alphaMax }, this.msCycle / 3, 'Linear', true); 
                tweenSecondary.onComplete.add(function() {
                    var alphaMin = this.alphaMin;
                    var tweenTertiary = this.game.add.tween(this).to({ alpha: alphaMin }, this.msCycle / 3, 'Linear', true); 
                    tweenTertiary.onComplete.add(function() {
                        setTimeout(() => this.handleCycle(), this.msBetweenCycles);
                    }, this);
                }, this);
            }, this);
        }
    }
}