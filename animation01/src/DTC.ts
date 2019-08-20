module MyGame {
    export enum DirectionEnum  { NoDirection = 0, LeftToRight = 1, RightToLeft = 2 }
    
    export class DTC {
        //public yMargin:number = 74;         // offset after shift to Widescreen mode
        public yMargin:number = 34;
        public numSkyTypes:number = 30;
        public numCloudTypes:number = 121;
        public numDistantCloudTypes:number = 17;
        public numVehiclesTypesLeftCar:number = 28;
        public numVehiclesTypesRightCar:number = 28;

        public numVehiclesTypesLeftBus:number = 9;
        public numVehiclesTypesRightBus:number = 9;

        public numVehiclesTypesLeftMinibus:number = 18;
        public numVehiclesTypesRightMinibus:number = 18;

        public numVehiclesTypesLeftTruck:number = 14;
        public numVehiclesTypesRightTruck:number = 14;

        public numLocomotives:number = 43;
        //public numRailcars:number = 6;    not needed anymore
        public numRailcarsPassenger = 25;    // 1-300
        public numRailcarsMail = 2;         // 301-500
        public numRailcarsCouchettes = 9;   // 501-600
        public numRailcarsRestaurant = 2;   // 601-700
        public numRailcarsCargo = 0;        // 701-999

        public numMaxCarsIncludingLocomotive:number = 10;
        public numPlaneTypes:number = 1;
        public numPlaneTrailTypes:number = 5;
        public numPlaneSounds:number = 3;

        public numTractorTypes:number = 1;
        public numMistTypes:number = 5;

        public truncateString(str:string, nChars:number):string {
            var result = "";
            var suffix = "..."
            
            if(str.length <= nChars) {
                result = str;
            }
            else {
                var strTruncated = str.substring(0, nChars - suffix.length);
                result = strTruncated + suffix;
            }
    
            return result;
        }
       
        public doubleDigit(n:number):string {
            var strResult = "";

            if(n < 100 && n > 0) {
                if(n < 10) 
                    strResult = "0" + n;
                else
                    strResult = n.toString();
            }

            return strResult;
        }

        public tripleDigit(n:number):string {
            var strResult = "";

            if(n > 0 && n < 1000) {
                if(n < 10) 
                    strResult = "00" + n;
                else if(n < 100) 
                    strResult = "0" + n;
                else
                    strResult = n.toString();
            }

            return strResult;
        }
      
    }
}
