module MyGame {
    enum TrainStateEnum { Running, Stopped }

    export class Railway extends Phaser.Sprite {
        private dtc:DTC;
        //private secondsForNewTrain:number = 120;
        //private pTrain:number = 0.3;

        private secondsForNewTrain:number = 1;
        private pTrain:number = 1;

        private trainState:TrainStateEnum = TrainStateEnum.Stopped;
        private railroad: Phaser.Sprite;
        private bgFront: Phaser.Sprite;
        private train: Phaser.Sprite;
        private bgBack: Phaser.Sprite;
        private maxRailcarWidth:number = 0;

        private trainArrays: number[][];
        private direction:number;

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   

            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.bgBack = this.game.add.sprite(0, game.height - 4, 'scene-back');
            this.bgBack.anchor.setTo(0, 1);

            this.railroad = this.game.add.sprite(0, game.height, 'railroad');
            this.railroad.anchor.setTo(0, 1);

            this.train = this.game.add.sprite(0, game.height - this.railroad.texture.height, '');
            this.train.anchor.setTo(0, 1);

            this.bgFront = this.game.add.sprite(0, game.height, 'scene-front');
            this.bgFront.anchor.setTo(0, 1);

            this.constructTrainArrays();

            setTimeout(() => this.startAnotherTrain(), this.secondsForNewTrain * 1000);
            setTimeout(() => this.checkState(), 1000);
        }

        preload() {}
        update() {}

        private checkState() {
            if(this.trainState == TrainStateEnum.Running) {
                for(var i=0;i<this.train.children.length;i++) {

                    if(this.direction > 0) {
                        if(this.train.children[i].x > this.game.width + this.maxRailcarWidth) 
                            this.train.removeChildAt(i);
                    }
                    else {
                        if(this.train.children[i].x < -this.maxRailcarWidth) 
                        this.train.removeChildAt(i);
                    }

                    if(this.train.children.length == 0)
                        this.trainState = TrainStateEnum.Stopped;
                }
            }
            
            setTimeout(() => this.checkState(), 1000);
        }

        private startAnotherTrain() {
            console.log("Checking for new train");

            if(this.trainState == TrainStateEnum.Stopped) {
                if(Math.random() < this.pTrain) {
                    this.direction = this.game.rnd.sign();    // if minus, right-to-left

                    var xStart = 0;
                    if(this.direction > 0)
                        xStart = -10;
                    else
                        xStart = this.game.width + 10;

                    var trainArrayNo = this.game.rnd.integerInRange(0, this.trainArrays.length - 1);

                    trainArrayNo = 5;

                    var velocity = this.game.rnd.realInRange(this.trainArrays[trainArrayNo][0], this.trainArrays[trainArrayNo][1]);
                    console.log("Speed is " + velocity);
                    var numTotalCars = this.trainArrays[trainArrayNo].length - 2;   // the first two elements are min and max speed
                    this.maxRailcarWidth = 0;

                    for(var i=0;i<numTotalCars;i++) {
                        var vehicleNo:number = 0;
                        var strVehicleType = "";

                        if(i==0) 
                            strVehicleType = "locomotive";
                        else 
                            strVehicleType = "railcar";
                        
                        vehicleNo = this.trainArrays[trainArrayNo][i + 2];  // the first two elements are min and max speed

                        var newVehicle = new Phaser.Sprite(this.game, xStart, 0, strVehicleType + '-' + this.dtc.tripleDigit(vehicleNo));
                        newVehicle.anchor.setTo(1, 1);
                        this.game.physics.enable(newVehicle);

                        newVehicle.body.velocity.x = velocity * this.direction;
                        newVehicle.scale.x = this.direction;

                        this.train.addChild(newVehicle);

                        if(newVehicle.texture.width > this.maxRailcarWidth)
                            this.maxRailcarWidth = newVehicle.texture.width;

                        xStart = xStart - this.direction * newVehicle.texture.width;
                    }
                    
                    this.trainState = TrainStateEnum.Running;
                    this.game.add.audio('train-whistle', 0.1, false).play();
                    console.log("Started");
                }
                else {
                    console.log("Not Started");
                }
            }
            else {
                
            }
            
            setTimeout(() => this.startAnotherTrain(), this.secondsForNewTrain * 1000);
        }

        private constructTrainArrays() {
            this.trainArrays = [];

            // the first two elements are min and max speed
            this.trainArrays[0] =  [40, 60, 3, 8, 8, 8, 8, 8, 8];
            this.trainArrays[1] =  [40, 60, 3, 23, 23, 23, 23, 23, 24, 24];
            this.trainArrays[2] =  [50, 70, 3, 22, 22, 22, 601, 22, 22, 22];
            
            this.trainArrays[3] =  [50, 70, 13, 17, 17, 17, 16, 16, 17, 17, 17];
            this.trainArrays[4] =  [50, 70, 9, 1, 1, 1, 1, 1, 1];
            this.trainArrays[5] =  [50, 70, 9, 14, 14, 14, 14, 12, 14];

        }
    }

}