module MyGame {
    enum TrainStateEnum { Running, Stopped }

    export class Railway extends Phaser.Sprite {
        private dtc:DTC;
        //private secondsForNewTrain:number = 120;
        //private pTrain:number = 0.2;

        private secondsForNewTrain:number = 1;
        private pTrain:number = 1;

        private trainState:TrainStateEnum = TrainStateEnum.Stopped;
        private railroad: Phaser.Sprite;
        private bgFront: Phaser.Sprite;
        private train: Phaser.Sprite;
        private bgBack: Phaser.Sprite;
        private maxRailcarWidth:number = 0;

        private trains: number[][][];
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
                    var trainType = 0;  // Passanger = 0, Cargo = 1
                    this.direction = this.game.rnd.sign();    // if minus, right-to-left

                    var xStart = 0;
                    if(this.direction > 0)
                        xStart = -10;
                    else
                        xStart = this.game.width + 10;

                    var trainArrayNo = this.game.rnd.integerInRange(0, this.trains[trainType].length - 1);

                    //trainArrayNo = 9; // for DEVELOPMENT PURPOSES

                    var velocity = this.game.rnd.realInRange(this.trains[trainType][trainArrayNo][0], this.trains[trainType][trainArrayNo][1]);
                    console.log("Speed is " + velocity);
                    var numTotalCars = this.trains[trainType][trainArrayNo].length - 2;   // the first two elements are min and max speed
                    this.maxRailcarWidth = 0;

                    for(var i=0;i<numTotalCars;i++) {
                        var vehicleNo:number = 0;
                        var strVehicleType = "";

                        if(i==0) 
                            strVehicleType = "locomotive";
                        else 
                            strVehicleType = "railcar";
                        
                        vehicleNo = this.trains[trainType][trainArrayNo][i + 2];  // the first two elements are min and max speed

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
                    this.game.add.audio('train-whistle', 0.02, false).play();
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

            this.trains = [];
            this.trains[0] = [];

            // the first two elements are min and max speed
            this.trains[0][0] =  [40, 60, 3, 8, 8, 8, 8, 8, 8];
            this.trains[0][1] =  [40, 60, 3, 23, 23, 23, 23, 23, 24, 24];
            this.trains[0][2] =  [50, 70, 3, 22, 22, 22, 601, 22, 22, 22];
            this.trains[0][3] =  [50, 70, 9, 1, 1, 1, 1, 1, 1];
            this.trains[0][4] =  [50, 70, 9, 14, 14, 14, 14, 12, 14];

            this.trains[0][5] =  [50, 80, 14, 5, 5, 5, 5, 5, 5, 5];
            this.trains[0][6] =  [30, 50, 13, 12, 13, 12, 12, 12, 11, 11];
            this.trains[0][7] =  [60, 90, 28, 5, 5, 5, 5, 5];
            this.trains[0][8] =  [50, 70, 17, 25, 25, 25, 25, 25, 503, 503];
            this.trains[0][9] =  [40, 70, 16, 9, 9, 9, 9, 9];
        }
    }

}