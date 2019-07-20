module MyGame {
    export class Road extends Phaser.Sprite {
        private dtc:DTC;
        private roadLeftToRight:Phaser.Sprite;
        private roadRightToLeft:Phaser.Sprite;
        private maxVehicles:number = 3;
        private miliSecondsForNewVehicles:number = 1000;
        private pNewVehicle = 0.010;
        private xStart:number;
        private yStart:number;
        private xEnd:number;
        private maxVelocityCar:number = 80;
        private minVelocityCar:number = 40;
        private list: Array<string> = new Array<string>();

        constructor(game: Phaser.Game) {
            super(game, 0, 0, '');   
 
            this.dtc = new DTC();

            this.anchor.setTo(0, 0);
            game.add.existing(this);

            this.xStart = -0.25 * game.width;
            this.xEnd = 1.25 * game.width;
            this.yStart = game.height - 6;

            this.roadRightToLeft = this.game.add.sprite(0, this.yStart, '');
            this.roadRightToLeft.anchor.setTo(0, 0);

            this.roadLeftToRight = this.game.add.sprite(0, this.yStart, '');
            this.roadLeftToRight.anchor.setTo(0, 0);

            this.createNewVehicle();
        }

        preload() { }
        update() {
            for(var i=0;i<this.roadLeftToRight.children.length;i++) {
                var vehicle = this.roadLeftToRight.children[i];
                if(vehicle.x > this.xEnd) {
                    this.roadLeftToRight.removeChildAt(i);
                    console.log("Removing vehicle...");
                }
            }

            for(var i=0;i<this.roadRightToLeft.children.length;i++) {
                var vehicle = this.roadRightToLeft.children[i];
                if(vehicle.x < this.xStart) {
                    this.roadRightToLeft.removeChildAt(i);
                    console.log("Removing vehicle...");
                }
            }
        }

        private createNewVehicle() {
            var numTotalVehicles:number = this.roadLeftToRight.children.length + this.roadRightToLeft.children.length;

            if(numTotalVehicles == 0)
                this.list = new Array<string>();

            if(numTotalVehicles < this.maxVehicles && Math.random() < this.pNewVehicle) {
                var vehicleNo:number = 0;
                var strVehicleType = "";
                var nType = this.game.rnd.integerInRange(1, 4);
                var direction:DirectionEnum;
                if (this.game.rnd.sign() == -1) 
                    direction = DirectionEnum.LeftToRight;
                else 
                    direction = DirectionEnum.RightToLeft;
                    
                var maxVexhiclesToChoseFrom:number = 0;
                if(nType == 1) {
                    strVehicleType = "car";
                    if(direction == DirectionEnum.LeftToRight)
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesLeftCar;
                    else
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesRightCar;
                }
                else if(nType == 2) {
                    strVehicleType = "bus";
                    if(direction == DirectionEnum.LeftToRight)
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesLeftBus;
                    else
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesRightBus;
                }
                else if(nType == 3) {
                    strVehicleType = "minibus";
                    if(direction == DirectionEnum.LeftToRight)
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesLeftMinibus;
                    else
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesRightMinibus;
                }
                else if(nType == 4) {
                    strVehicleType = "truck";
                    if(direction == DirectionEnum.LeftToRight)
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesLeftTruck;
                    else
                        maxVexhiclesToChoseFrom = this.dtc.numVehiclesTypesRightTruck;
                }
                
                vehicleNo = this.game.rnd.integerInRange(1, maxVexhiclesToChoseFrom)

                var isVehicleAppropriate:boolean = true;
                var strDirectionSuffix:string;
                if(direction == DirectionEnum.LeftToRight)
                    strDirectionSuffix = "lr";
                else
                    strDirectionSuffix = "rl";

                var strVehicleName:string = strVehicleType + '-' + strDirectionSuffix + '-' + this.dtc.doubleDigit(vehicleNo);
                for(var i=0;i<this.list.length;i++) {
                    if(this.list[i].localeCompare(strVehicleName) == 0) {
                        isVehicleAppropriate = false;
                        break;
                    }   
                }

                if(isVehicleAppropriate) {
                    var newVehicle = new Phaser.Sprite(this.game, this.xStart, 0, strVehicleName);
                    newVehicle.anchor.setTo(0.5, 1);
                    this.game.physics.enable(newVehicle);
    
                    if (direction == DirectionEnum.LeftToRight) {
                        newVehicle.body.velocity.x = this.game.rnd.realInRange(this.minVelocityCar, this.maxVelocityCar);
                        newVehicle.x = this.xStart;
                        newVehicle.scale.x = 1;
                        this.roadLeftToRight.addChild(newVehicle);
                    }
                    else {
                        newVehicle.body.velocity.x = -this.game.rnd.realInRange(this.minVelocityCar, this.maxVelocityCar);
                        newVehicle.x = this.xEnd;
                        newVehicle.scale.x = -1;
                        this.roadRightToLeft.addChild(newVehicle);
                    }

                    this.list.push(newVehicle.texture.baseTexture.source.name);
                    setTimeout(() => this.playCarPassingBaySound(), 1000 * 100 / Math.abs(newVehicle.body.velocity.x));
                    console.log("creating a vehicle, numVehicles = " + this.children.length + " ---> " + newVehicle.texture.baseTexture.source.name);
                }
            }

            setTimeout(() => this.createNewVehicle(), this.miliSecondsForNewVehicles);
        }

        private playCarPassingBaySound() {
            var nSound = this.game.rnd.integerInRange(1, 4);
            this.game.add.audio('car-drive-by-' + this.dtc.doubleDigit(nSound), 0.016, false).play();
        }
    }
}